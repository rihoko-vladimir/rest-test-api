namespace TestTaskApi.Models.Entities;

public class Employee
{
    public Guid Id { get; } = Guid.NewGuid();

    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitle> JobTitles { get; set; }
}