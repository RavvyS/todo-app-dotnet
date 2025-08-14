using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<CreateTodoDto, TodoItem>();
            CreateMap<TodoItem, TodoResponseDto>();
            CreateMap<UpdateTodoDto, TodoItem>();
        }
    }
}