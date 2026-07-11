using AutoMapper;
using HRPayroll.Domain.Entities;
using HRPayroll.Domain.ValueObjects;
using HRPayroll.Application.DTOs;

namespace HRPayroll.Application.Mappings;

/// <summary>
/// AutoMapper profile for Employee entity and DTO conversions.
/// </summary>
public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DesignationTitle, opt => opt.MapFrom(src => src.Designation.Title));

        CreateMap<Employee, EmployeeListDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DesignationTitle, opt => opt.MapFrom(src => src.Designation.Title));

        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Designation, opt => opt.Ignore());

        CreateMap<UpdateEmployeeDto, Employee>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Designation, opt => opt.Ignore());
    }
}
