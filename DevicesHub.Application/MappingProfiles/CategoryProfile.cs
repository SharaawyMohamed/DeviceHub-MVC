using AutoMapper;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.ViewModels;


namespace DevicesHub.Application.MappingProfiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();
        }
    }
}
