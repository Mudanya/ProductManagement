using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace UltimateApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {   
            CreateMap<Company, CompanyDTO>()
                .ForMember(x=>x.FullAddress, 
                opt => opt.MapFrom(c=> string.Join(" ", c.Address, c.Country)));
            CreateMap<Employee, EmployeeDto >();
            CreateMap<CompanyCreateDto, Company>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();
            CreateMap<CompanyUpdateDto, Company>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
