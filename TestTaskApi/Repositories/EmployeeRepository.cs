using Microsoft.EntityFrameworkCore;
using TestTaskApi.Interfaces.Repositories;
using TestTaskApi.Models.DbContext;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetEmployeeByGuidAsync(Guid userId)
    {
        var employee = await _context.Employees
            .Include(employee1 => employee1.JobTitles)
            .FirstOrDefaultAsync(
                employee1 => employee1.Id.Equals(userId));

        return employee;
    }

    public async Task<Guid> CreateNewEmployeeAsync(Employee employee)
    {
        var newEmployee = await _context.Employees.AddAsync(employee);

        await _context.SaveChangesAsync();

        return newEmployee.Entity.Id;
    }

    public async Task<Employee?> PatchEmployeeAsync(Guid userId, Employee employee)
    {
        var employeeInDatabase =
            await _context
                .Employees
                .Include(employee1 => employee1.JobTitles)
                .FirstOrDefaultAsync(dbEmployee => dbEmployee.Id.Equals(userId));

        if (employeeInDatabase is null) return null;

        employeeInDatabase.NameAndSurname = employee.NameAndSurname;
        employeeInDatabase.DateOfBirth = employee.DateOfBirth;
        employeeInDatabase.JobTitles = employee.JobTitles;

        await _context.SaveChangesAsync();

        var savedEmployee =
            await _context.Employees.FirstOrDefaultAsync(dbEmployee => dbEmployee.Id.Equals(employeeInDatabase.Id));

        return savedEmployee;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        var employees = await _context
            .Employees
            .Include(employee1 => employee1.JobTitles)
            .ToListAsync();

        return employees;
    }
}