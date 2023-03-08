using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Dtos;
using ToDo.Models;
using ToDo.Parameters;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;
        private readonly IMapper _mapper;

        public ToDoController(ToDoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 查詢全部備忘錄訊息
        /// </summary>
        /// <returns></returns>
        // GET: api/TodoList
        [HttpGet]
        public ActionResult<IEnumerable<ToDoItemSelectDto>> Get([FromQuery] TodoParameter parameter)
        {
             var result = _context
                .ToDoList
                .Include(a => a.UpdateEmployee)
                .Include(a => a.InsertEmployee)
                .Include(a => a.UploadFiles)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                // result = result.Where(a => a.Name.IndexOf(name) > -1);
                result = result.Where(a => a.Name.Contains(parameter.Name));
            }

            if (parameter.Enable != null)
            {
                result = result.Where(a => a.Enable == parameter.Enable);
            }

            if (parameter.InsertTime != null)
            {
                result = result.Where(a => a.InsertTime == parameter.InsertTime);
            }

            if (parameter.MinOrder != null && parameter.MaxOrder != null)
            {
                result = result.Where(a => a.Orders >= parameter.MinOrder && a.Orders <= parameter.MaxOrder);
            }


            // return _mapper.Map<List<ToDoItemSelectDto>>(result);

            // 型別轉換請放在最後
            return Ok(result.ToList().Select(a => Map(a)));
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoItemSelectDto> Get(Guid id)
        {
            var result = (from a in _context.ToDoList
                join b in _context.Employees on a.InsertEmployeeId equals b.EmployeeId
                join c in _context.Employees on a.UpdateEmployeeId equals c.EmployeeId
                where a.TodoId == id
                select new ToDoItemSelectDto
                {
                    Enable = a.Enable,
                    InsertEmployeeName = b.Name,
                    InsertTime = a.InsertTime,
                    Name = a.Name,
                    Orders = a.Orders,
                    TodoId = a.TodoId,
                    UpdateEmployeeName = c.Name,
                }).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }
            return result;
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
                TodoId = item.TodoId,
                UpdateEmployeeName = item.UpdateEmployee.Name,
                UpdateTime = item.UpdateTime,
                UploadFiles = uploadFileDtoList,
            };
        }
    }
}
