using AutoMapper;
using Employees.App.Models;
using Employees.Models;

namespace Employees.App
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();

            CreateMap<Employee, ManagerDto>()
                .ForMember(dto => dto.ManagedEmployeesCount,
                            dest => dest.MapFrom(em => em.ManagedEmployees.Count));

            CreateMap<Employee, EmployeeFullInfoDto>();

            CreateMap<Employee, ListEmployeesDto>();
        }
    }
}
