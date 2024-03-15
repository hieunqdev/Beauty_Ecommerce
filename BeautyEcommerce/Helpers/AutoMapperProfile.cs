using AutoMapper;
using BeautyEcommerce.Data;
using BeautyEcommerce.ViewModels;

namespace BeautyEcommerce.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<RegisterVM, KhachHang>();
        }
    }
}
