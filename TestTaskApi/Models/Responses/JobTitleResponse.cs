namespace TestTaskApi.Models.Responses;

public class JobTitleResponse
{
    public Guid Id { get; set; }
    
    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<EmployeeNonRecursiveResponse> Employees { get; set; }
}