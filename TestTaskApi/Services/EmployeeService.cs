using System.Net;
using Serilog;
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

            if (employee is not null) return Result.Success(employee);

            Log.Information("Employee wasn't found - {Id}", userId);

            return Result.Error<Employee>("Employee wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

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
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<Guid>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Employee>> PatchEmployeeAsync(Guid userId, Employee employee)
    {
        try
        {
            var patchedEmployee = await _employeeRepository.PatchEmployeeAsync(userId, employee);

            if (patchedEmployee is not null) return Result.Success(patchedEmployee);

            Log.Information("Employee wasn't found - {Id}", userId);

            return Result.Error<Employee>("Employee wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

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
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<IEnumerable<Employee>>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result> RemoveEmployeeAsync(Guid employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByGuidAsync(employeeId);

            if (employee is null)
            {
                Log.Information("Employee wasn't found - {Id}", employeeId);

                return Result.Error("Employee wasn't found", HttpStatusCode.NotFound);
            }

            var wasRemoved = await _employeeRepository.RemoveEmployeeAsync(employeeId);

            if (wasRemoved)
            {
                return Result.Success();
            }

            Log.Error("Server error occured");

            return Result.Error("Unknown error occured", HttpStatusCode.InternalServerError);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error(e, HttpStatusCode.InternalServerError);
        }
    }
}