using System.Text.Json.Serialization;

namespace TestTaskApi.Models.Requests;

public class EmployeeRequest
{
    [JsonIgnore] public Guid Id { get; set; }

    public string NameAndSurname { get; set; }

    public DateTime DateOfBirth { get; set; }

    public List<JobTitleRequest> JobTitles { get; set; }
}