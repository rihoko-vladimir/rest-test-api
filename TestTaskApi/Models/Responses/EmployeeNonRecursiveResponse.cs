namespace TestTaskApi.Models.Responses;

public class EmployeeNonRecursiveResponse
{
    public Guid Id { get; set; }

    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }
}