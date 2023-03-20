using TestTaskApi.Models;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Interfaces.Services;

public interface IEmployeeService
{
    public Task<Result<Employee>> GetEmployeeByGuidAsync(Guid userId);

    public Task<Result<Guid>> CreateNewEmployeeAsync(Employee employee);

    public Task<Result<Employee>> PatchEmployeeAsync(Guid userId, Employee employee);

    public Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync();

    public Task<Result> RemoveEmployeeAsync(Guid employeeId);
}