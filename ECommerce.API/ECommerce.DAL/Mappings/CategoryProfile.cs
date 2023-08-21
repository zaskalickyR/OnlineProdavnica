using AutoMapper;
using ECommerce.DAL.DTO.Category.DataOut;
using ECommerce.DAL.DTO.Product.DataOut;
using ECommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDataOut>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
