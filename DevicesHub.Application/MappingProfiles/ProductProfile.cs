
using AutoMapper;
using DevicesHub.Application.ViewModels;
using DevicesHub.Domain.Models;

namespace DevicesHub.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, Product>().ReverseMap();
        }
    }
}
