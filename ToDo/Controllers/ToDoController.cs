using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Dtos;
using ToDo.Models;
using ToDo.Parameters;
using ToDo.Services;
using ToDo.Services.Impl;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToDoItemService _service;

        public ToDoController(ToDoDbContext context, IMapper mapper, IToDoItemService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// 查詢全部備忘錄訊息
        /// </summary>
        /// <returns></returns>
        // GET: api/TodoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemSelectDto>>> Get([FromQuery] TodoParameter parameter)
        {
             var res = await _service.GetToDoItemsAsync(parameter);


            // return _mapper.Map<List<ToDoItemSelectDto>>(result);

            if (res.Count <= 0)
            {
                return NotFound();
            }

            
            return Ok(res);
        }

        [HttpGet("{toDoItemId}")]
        public async Task<ActionResult<ToDoItemSelectDto>> Get(Guid toDoItemId)
        {

            #region 這樣寫太累了

            // var result = (from a in _context.ToDoList
            //     join b in _context.Employees on a.InsertEmployeeId equals b.EmployeeId
            //     join c in _context.Employees on a.UpdateEmployeeId equals c.EmployeeId
            //     where a.ToDoId == toDoItemId
            //     select new ToDoItemSelectDto
            //     {
            //         Enable = a.Enable,
            //         InsertEmployeeName = b.Name,
            //         InsertTime = a.InsertTime,
            //         Name = a.Name,
            //         Orders = a.Orders,
            //         TodoId = a.ToDoId,
            //         UpdateEmployeeName = c.Name,
            //         UpdateTime = a.UpdateTime,
            //         UploadFiles =  (from d in _context.UploadFiles
            //             where d.ToDoId == a.ToDoId
            //             select new UploadFileDto
            //             {
            //                 Name = d.Name,
            //                 Src = d.Src,
            //                 UploadFileId = d.UploadFileId,
            //                 ToDoId = d.ToDoId
            //             }).ToList()
            //     }).FirstOrDefault();

            #endregion

            var result = await _service.GetOneAsync(toDoItemId);

            if (result == null)
            {
                return NotFound($"找不到id={toDoItemId} 資源");
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<ToDoItemSelectDto> Post([FromBody] ToDoItemPostDto item)
        {
            // 絕對不要用entity物件接資料後直接進行資料新增
            // 要守護資料的安全性，請先使用DTO來接收傳入的資料
            // 新增的時候有設置外鍵關聯的話直接就可以用導航屬性關聯起來
            // insert到資料庫的話會一起在2個table新增
            var insertItem = new ToDoItem()
            {
                // Name = item.Name,
                // Enable = item.Enable,
                // Orders = item.Orders,
                InsertEmployeeId = Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019"),
                UpdateEmployeeId = Guid.Parse("f3c7567a-085b-40e9-8ad7-e0822bc7156a"),
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                // UploadFiles = item.UploadFiles.Select(a => new UploadFile()
                // {
                //     Name = a.Name,
                //     Src = a.Src
                // }).ToList()
            };


            #region 使用EF Core所提供的映射功能

            _context.ToDoList.Add(insertItem).CurrentValues.SetValues(item);
            _context.SaveChanges();

            foreach (var temp in item.UploadFiles)
            {
                _context.UploadFiles.Add(new UploadFile{ToDoId = insertItem.ToDoId }).CurrentValues.SetValues(temp);
            }

            _context.SaveChanges();

            #endregion

            return CreatedAtAction(nameof(Get), new {toDoItemId = insertItem.ToDoId}, item);
        }


        [HttpPost("withoutFK")]
        public ActionResult PostNoFk(ToDoItemPostDto value)
        {
            var toDoItem = new ToDoItem()
            {
                Name = value.Name,
                Enable = value.Enable,
                Orders = value.Orders,
                InsertEmployeeId = Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019"),
                UpdateEmployeeId = Guid.Parse("f3c7567a-085b-40e9-8ad7-e0822bc7156a"),
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,

            };
            _context.ToDoList.Add(toDoItem);
            _context.SaveChanges();

            var uploadFiles = value.UploadFiles.Select(a => new UploadFile()
            {
                Name = a.Name,
                Src = a.Src,
                ToDoId = toDoItem.ToDoId
            }).ToList();
            _context.UploadFiles.AddRange(uploadFiles);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("autoMapper")]
        public async Task<ActionResult<ToDoItemSelectDto>> PostAutoMapper([FromBody] ToDoItemPostDto item)
        {

            var id = await _service.AddAsync(item);

            return CreatedAtAction(nameof(Get), new { toDoItemId = id }, item);
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <para>
        /// RESTFul規範中: PUT方法所對應的路由要帶有資料的Id
        /// </para>
        /// <param name="toDoItemId"></param>
        /// <param name="value"></param>
        [HttpPut("{toDoItemId}")]
        public async Task<IActionResult> Put(Guid toDoItemId, ToDoItemPutDto value)
        {
            if (toDoItemId != value.ToDoId)
            {
                return BadRequest();
            }
            var row = await _service.UpdateAsync(toDoItemId, value);

            if (row == 0)
            {
                return NotFound();
            }
            // 狀態碼: 204，伺服器成功處理請求，沒有返回任何內容
            return NoContent();
        }

        [HttpPut("autoMapper/{toDoItemId}")]
        public void PutAutoMapper(Guid toDoItemId, ToDoItemPutDto value)
        {
            // 不要直接將傳過的值上傳到資料庫中
            // Find(): 參數只能放Primary key
            var target = _context.ToDoList.Find(toDoItemId);
            if (target == null)
            {
                return;
            }

            // 使用AutoMapper來做映射
            // 第一個參數放使用者傳來的值
            // 第二個參數放要更新的目標
            _mapper.Map(value, target);

            _context.SaveChanges();
        }

        [HttpPut("EfCore/{toDoItemId}")]
        public void PutEfCoreEnhance(Guid toDoItemId, ToDoItemPutDto value)
        {
            // 不要直接將傳過的值上傳到資料庫中
            // Find(): 參數只能放Primary key
            var target = _context.ToDoList.Find(toDoItemId);
            if (target == null)
            {
                return;
            }

            #region 系統賦值

            target.UpdateTime = DateTime.Now;
            target.UpdateEmployeeId = Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019");

            #endregion

            #region 上傳的數值

            // target.Name = value.Name;
            // target.Orders = value.Orders;
            // target.Enable = value.Enable;
            _context.ToDoList.Update(target).CurrentValues.SetValues(value);

            #endregion

            _context.SaveChanges();
        }

        [HttpDelete("noFK/{toDoId}")]
        public IActionResult DeleteNoFk(Guid toDoId)
        {
            var child = _context.UploadFiles.Where(e => e.ToDoId == toDoId).ToList();
            _context.UploadFiles.RemoveRange(child);
            _context.SaveChanges();

            var target = _context.ToDoList
                .Include(e => e.UploadFiles)
                .SingleOrDefault(e => e.ToDoId == toDoId);
            if (target == null)
            {
                return NotFound();
            }

            _context.ToDoList.Remove(target);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{toDoId}")]
        public async Task<IActionResult> Delete(Guid toDoId)
        {
            var row = await _service.DeleteAsync(toDoId);
            
            if (row == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("list/{IdList}")]
        public IActionResult DeleteList(string IdList)
        {
            List<Guid>? deleteList = JsonSerializer.Deserialize<List<Guid>>(IdList);
            if (deleteList == null)
            {
                return BadRequest();
            }

            var target = _context.ToDoList
                .Include(e => e.UploadFiles)
                .Where(e => deleteList.Contains(e.ToDoId));
            if (!target.Any())
            {
                return NotFound("找不到要刪除的資源");
            }
            _context.ToDoList.RemoveRange(target);
            _context.SaveChanges();
            return NoContent();
            // var idList = IdList.Split(',').Select(e => Guid.Parse(e)).ToList();
            // var child = _context.UploadFiles.Where(e => idList.Contains(e.ToDoId)).ToList();
            // _context.UploadFiles.RemoveRange(child);
            // _context.SaveChanges();
            //
            // var target = _context.ToDoList
            //     .Include(e => e.UploadFiles)
            //     .Where(e => idList.Contains(e.ToDoId))
            //     .ToList();
            // if (target == null)
            // {
            //     return NotFound();
            // }
            //
            // _context.ToDoList.RemoveRange(target);
            // _context.SaveChanges();
            // return NoContent();
        }


        /// <summary>
        /// convert toDoItem to ToDoItemSelectDto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static ToDoItemSelectDto Map(ToDoItem item)
        {
            List<UploadFileDto> uploadFileDtoList = new List<UploadFileDto>();

            foreach (var uploadFile in item.UploadFiles)
            {
                uploadFileDtoList.Add(new UploadFileDto()
                {
                    Name = uploadFile.Name,
                    Src = uploadFile.Src,
                    UploadFileId = uploadFile.UploadFileId,
                    ToDoId = uploadFile.ToDoId
                });
            }
            return new ToDoItemSelectDto()
            {
                Enable = item.Enable,
                InsertEmployeeName = item.InsertEmployee.Name,
                InsertTime = item.InsertTime,
                Name = item.Name,
                Orders = item.Orders,
                TodoId = item.ToDoId,
                UpdateEmployeeName = item.UpdateEmployee.Name,
                UpdateTime = item.UpdateTime,
                UploadFiles = uploadFileDtoList,
            };
        }
    }
}
