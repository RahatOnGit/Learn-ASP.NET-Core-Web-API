using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
           
            CreateMap<Student, StudentDTO>().
            ForMember(des=>des.Address, 
            opt=>opt.MapFrom(src=> string.IsNullOrEmpty(src.Address) ? "Noaddress" : src.Address))
            .ReverseMap(); 


        }
    }
}
