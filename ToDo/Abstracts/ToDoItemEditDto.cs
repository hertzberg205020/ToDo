using System.ComponentModel.DataAnnotations;
using ToDo.Dtos;
using ToDo.Models;

namespace ToDo.Abstracts;

public abstract class ToDoItemEditDto: IValidatableObject
{
    public string Name { get; set; }
    public bool Enable { get; set; }
    [Range(1, 5)]
    public int Orders { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<UploadFilePostDto> UploadFiles { get; set; } = new List<UploadFilePostDto>();
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // 解析IOC服務
        var ctx = validationContext.GetRequiredService<ToDoDbContext>();

        // var dto = validationContext.ObjectInstance;
        bool isDuplicate = true;
        var query = ctx.ToDoList.Where(a => a.Name == Name);


        // 如果是Put的話，要排除自己
        if (this is ToDoItemPutDto putDto)
        {
            query = query.Where(a => a.ToDoId != putDto.ToDoId);
        }

        isDuplicate = query.Any();
        if (isDuplicate)
        {
            yield return new ValidationResult("事項名稱不可重複", new[] { "name" });
        }

        if (StartTime > EndTime)
        {
            yield return new ValidationResult("開始時間不可大於結束時間", new string[] { "time" });
        }

    }
}