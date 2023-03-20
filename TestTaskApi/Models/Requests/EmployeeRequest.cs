namespace TestTaskApi.Models.Requests;

public class EmployeeRequest
{
    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitleRequest> JobTitles { get; set; }
}