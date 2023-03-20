using TestTaskApi.Models.Entities;

namespace TestTaskApi.Interfaces.Repositories;

public interface IEmployeeRepository
{
    public Task<Employee?> GetEmployeeByGuidAsync(Guid userId);

    public Task<Guid> CreateNewEmployeeAsync(Employee employee);

    public Task<Employee?> PatchEmployeeAsync(Guid userId, Employee employee);

    public Task<IEnumerable<Employee>> GetAllEmployeesAsync();

    public Task<bool> RemoveEmployeeAsync(Guid employeeId);
}