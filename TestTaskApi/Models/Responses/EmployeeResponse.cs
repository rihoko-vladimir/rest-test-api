namespace TestTaskApi.Models.Responses;

public class EmployeeResponse
{
    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitleResponse> JobTitles { get; set; }
}