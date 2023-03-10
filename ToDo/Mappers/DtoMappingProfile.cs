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


        #region Post用的DTO轉換為Entity

        CreateMap<UploadFilePostDto, UploadFile>();
        CreateMap<ToDoItemPostDto, ToDoItem>();

        #endregion

        #region Put用的DTO轉換為Entity

        CreateMap<ToDoItemPutDto, ToDoItem>()
            .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UpdateEmployeeId, opt => opt.MapFrom(src => Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019")));

        #endregion
    }
}