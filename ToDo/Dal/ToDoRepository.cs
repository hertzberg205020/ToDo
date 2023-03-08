using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ToDo.Dtos;
using ToDo.Models;

namespace ToDo.Dal
{
    public class ToDoRepository
    {
        private readonly ToDoDbContext _context;
        private readonly IMapper _mapper;

        public SqlConnection DbConnection => (SqlConnection)_context.Database.GetDbConnection();

        public ToDoRepository(ToDoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ToDoItemSelectDto> GetToDoItemByIdAsync(Guid id)
        {
            var todoItem = await _context.ToDoList
                .Include(x => x.InsertEmployee)
                .Include(x => x.UpdateEmployee)
                .Include(x => x.UploadFiles)
                .FirstOrDefaultAsync(x => x.TodoId == id);
            return _mapper.Map<ToDoItemSelectDto>(todoItem);
        }


        /// <summary>
        /// 使用原生SQL語句
        /// </summary>
        /// <param name="sql">原生SQL語法，使用 @參數化 變數</param>
        /// <param name="parameters">參數化變數值得集合</param>
        /// <returns></returns>

        public async Task<List<IDictionary<string, object>>> QueryByDapper(string sql, DynamicParameters parameters)
        {
            SqlConnection connection = DbConnection;
            await connection.OpenAsync();
            var res = await connection.QueryAsync(sql, parameters);

            List<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            foreach (var item in res)
            {
                if (item is IDictionary<string, object> data) ret.Add(data);
            }

            return ret;
        }

    }
}
