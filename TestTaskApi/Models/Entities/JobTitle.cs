using System.ComponentModel.DataAnnotations;

namespace TestTaskApi.Models.Entities;

public class JobTitle
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<Employee> Employees { get; set; }
}