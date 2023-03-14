using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToDo.ModelBinder;

namespace ToDo.Dtos;

public class ToDoItemPostWithFilesDto
{
    // [ModelBinder(BinderType=typeof(FormDataJsonBinder))]
    // public ToDoItemPostDto ToDoItemPostDto { get; set; }
    public string Name { get; set; }
    public bool Enable { get; set; }
    [Range(1, 5)]
    public int Orders { get; set; }
    public IFormFileCollection? Files { get; set; }
}