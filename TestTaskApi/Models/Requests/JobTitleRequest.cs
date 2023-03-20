using System.Text.Json.Serialization;

namespace TestTaskApi.Models.Requests;

public class JobTitleRequest
{
    [JsonIgnore] public Guid Id { get; set; }

    public string JobTitleName { get; set; }

    public int Grade { get; set; }

    public List<EmployeeRequest> Employees { get; set; }
}