using System.ComponentModel.DataAnnotations;

namespace TestTaskApi.Models.Entities;

public class Employee
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitle> JobTitles { get; set; }
}