
using AutoMapper;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.ViewModels;

namespace DevicesHub.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductVM, Product>().ReverseMap();
        }
    }
}
