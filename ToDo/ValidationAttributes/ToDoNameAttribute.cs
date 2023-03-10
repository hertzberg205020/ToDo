using AutoMapper.Configuration;
using System.ComponentModel.DataAnnotations;
using ToDo.Dtos;
using ToDo.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ToDo.ValidationAttributes;

public class ToDoNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 解析IOC服務
        var ctx = validationContext.GetRequiredService<ToDoDbContext>();
        if (value == null)
        {
            return new ValidationResult("事項名稱不可為空");
        }
        var name = value.ToString();
        var dto = validationContext.ObjectInstance;
        bool isDuplicate = true;
        var query = ctx.ToDoList.Where(a => a.Name == name);


        // 如果是Put的話，要排除自己
        if (dto is ToDoItemPutDto putDto)
        {
            query = query.Where(a => a.ToDoId != putDto.ToDoId);
        }
        
        isDuplicate = query.Any();
        return isDuplicate ? 
            new ValidationResult("事項名稱不可重複") :
            ValidationResult.Success;
    }
}