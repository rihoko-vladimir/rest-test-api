using System.Net;
using TestTaskApi.Interfaces.Repositories;
using TestTaskApi.Interfaces.Services;
using TestTaskApi.Models;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<Employee>> GetEmployeeByGuidAsync(Guid userId)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByGuidAsync(userId);

            return employee is not null
                ? Result.Success(employee)
                : Result.Error<Employee>("Employee wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return Result.Error<Employee>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Guid>> CreateNewEmployeeAsync(Employee employee)
    {
        try
        {
            var employeeId = await _employeeRepository.CreateNewEmployeeAsync(employee);

            return Result.Success(employeeId);
        }
        catch (Exception e)
        {
            return Result.Error<Guid>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Employee>> PatchEmployeeAsync(Guid userId, Employee employee)
    {
        try
        {
            var patchedEmployee = await _employeeRepository.PatchEmployeeAsync(userId, employee);

            return patchedEmployee is not null
                ? Result.Success(patchedEmployee)
                : Result.Error<Employee>("Employee wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return Result.Error<Employee>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            return Result.Success(employees);
        }
        catch (Exception e)
        {
            return Result.Error<IEnumerable<Employee>>(e, HttpStatusCode.InternalServerError);
        }
    }
}