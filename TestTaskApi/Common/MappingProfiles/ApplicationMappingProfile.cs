using AutoMapper;
using TestTaskApi.Models.Entities;
using TestTaskApi.Models.Requests;
using TestTaskApi.Models.Responses;

namespace TestTaskApi.Common.MappingProfiles;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<EmployeeRequest, Employee>()
            .ReverseMap();
        CreateMap<Employee, EmployeeResponse>()
            .ReverseMap();
        CreateMap<JobTitleRequest, JobTitle>()
            .ReverseMap();
        CreateMap<JobTitle, JobTitleResponse>()
            .ReverseMap();
        CreateMap<Employee, EmployeeNonRecursiveResponse>()
            .ReverseMap();
        CreateMap<JobTitle, JobTitleNonRecursiveResponse>()
            .ReverseMap();
    }
}