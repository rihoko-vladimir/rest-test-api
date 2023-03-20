using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TestTaskApi.Interfaces.Services;
using TestTaskApi.Models.Entities;
using TestTaskApi.Models.Requests;
using TestTaskApi.Models.Responses;

namespace TestTaskApi.Controllers;

[ApiController]
[Route("api/job")]
public class JobController : ControllerBase
{
    private readonly IJobInformationService _jobInformationService;
    private readonly IMapper _mapper;

    public JobController(IJobInformationService jobInformationService, IMapper mapper)
    {
        _jobInformationService = jobInformationService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(JobTitleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetEmployeeByGuidAsync([FromQuery] string guid)
    {
        var wasParsed = Guid.TryParse(guid, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var result = await _jobInformationService.GetJobInfoAsync(parsedGuid);

        if (!result.IsSuccess) return StatusCode((int)result.StatusCode, result.Message);

        var mappedResult = _mapper.Map<JobTitleResponse>(result.Value);

        return Ok(mappedResult);
    }

    [HttpPost]
    [Route("create-job")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateNewEmployeeAsync([FromBody] JobTitleRequest jobRequest)
    {
        var mappedJobTitle = _mapper.Map<JobTitle>(jobRequest);

        var result = await _jobInformationService.CreateNewJobAsync(mappedJobTitle);

        return !result.IsSuccess
            ? StatusCode((int)result.StatusCode, result.Message)
            : Ok(result.Value);
    }

    [HttpGet]
    [Route("get-all-jobs")]
    [ProducesResponseType(typeof(IEnumerable<JobTitleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllEmployeesAsync()
    {
        var result = await _jobInformationService.GetAllJobsAsync();

        if (!result.IsSuccess) return StatusCode((int)result.StatusCode, result.Message);

        var mappedResponse = _mapper.Map<IEnumerable<JobTitleResponse>>(result.Value);

        return Ok(mappedResponse);
    }

    [HttpPatch]
    [Route("modify")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PatchUserAsync(JsonPatchDocument<JobTitleRequest> jobRequest,
        [FromQuery] string jobId)
    {
        var wasParsed = Guid.TryParse(jobId, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var jobResult = await _jobInformationService.GetJobInfoAsync(parsedGuid);

        if (!jobResult.IsSuccess) return NotFound(jobResult.Message);

        var requestDto = _mapper.Map<JobTitleRequest>(jobResult.Value);

        try
        {
            jobRequest.ApplyTo(requestDto);
        }
        catch (Exception e)
        {
            Log.Error("An error occured, because there were a malformed patch request. {Exception}, {Message}",
                e.GetType().FullName, e.Message);
            return BadRequest(e.Message);
        }

        var entity = _mapper.Map<JobTitle>(requestDto);

        var patchResult = await _jobInformationService.PatchJobTitleAsync(parsedGuid, entity);

        return !patchResult.IsSuccess
            ? StatusCode((int)patchResult.StatusCode, patchResult.Message)
            : Ok(patchResult.Value);
    }

    [HttpDelete]
    [Route("remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveJob([FromQuery] string jobId)
    {
        var wasParsed = Guid.TryParse(jobId, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var result = await _jobInformationService.RemoveJob(parsedGuid);

        return !result.IsSuccess
            ? StatusCode((int)result.StatusCode, result.Message)
            : Ok();
    }
}