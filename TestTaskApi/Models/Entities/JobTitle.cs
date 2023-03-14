namespace TestTaskApi.Models.Entities;

public class JobTitle
{
    public Guid Id { get; set; }

    public string JobTitleName { get; set; }

    public List<Employee> Employees { get; set; }
}