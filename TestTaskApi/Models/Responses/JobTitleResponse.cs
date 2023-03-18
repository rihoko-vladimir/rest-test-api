namespace TestTaskApi.Models.Responses;

public class JobTitleResponse
{
    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<EmployeeResponse> Employees { get; set; }
}