namespace TestTaskApi.Models.Requests;

public class JobTitleRequest
{
    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<EmployeeRequest> Employees { get; set; }
}