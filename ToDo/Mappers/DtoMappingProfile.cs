using AutoMapper;
using ToDo.Dtos;
using ToDo.Models;

namespace ToDo.Mappers;

public class DtoMappingProfile: Profile
{
    public DtoMappingProfile()
    {
        CreateMap<ToDoItem, ToDoItemSelectDto>()
            .ForMember(dest => dest.InsertEmployeeName,opt => opt.MapFrom(src => $"{src.InsertEmployee.Name}({src.InsertEmployeeId})"))
            .ForMember(dest => dest.UpdateEmployeeName, opt => opt.MapFrom(src => $"{src.UpdateEmployee.Name}({src.UpdateEmployeeId})"))
            .ReverseMap();

        CreateMap<UploadFile, UploadFileDto>();
    }
}