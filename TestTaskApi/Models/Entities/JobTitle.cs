namespace TestTaskApi.Models.Entities;

public class JobTitle
{
    public Guid Id { get; } = Guid.NewGuid();

    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<Employee> Employees { get; set; }
}