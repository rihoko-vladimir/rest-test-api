namespace TestTaskApi.Models.Responses;

public class EmployeeResponse
{
    public Guid Id { get; set; }

    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitleNonRecursiveResponse> JobTitles { get; set; }
}