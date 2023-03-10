using System.ComponentModel.DataAnnotations;
using ToDo.Dtos;

namespace ToDo.ValidationAttributes;

public class StartTimeBeforeEndTimeAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var postDto = (ToDoItemPostDto)validationContext.ObjectInstance;
        if (postDto.StartTime == null || postDto.EndTime == null)
        {
            return ValidationResult.Success;
        }

        return postDto.StartTime > postDto.EndTime ? 
            new ValidationResult("開始時間不可大於結束時間", new string[]{"time"}) :
            ValidationResult.Success;
    }
}