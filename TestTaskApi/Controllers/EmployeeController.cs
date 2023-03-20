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
[Route("/api/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IMapper mapper, IEmployeeService employeeService)
    {
        _mapper = mapper;
        _employeeService = employeeService;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetEmployeeByGuidAsync([FromQuery] string guid)
    {
        var wasParsed = Guid.TryParse(guid, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var result = await _employeeService.GetEmployeeByGuidAsync(parsedGuid);

        if (!result.IsSuccess) return StatusCode((int)result.StatusCode, result.Message);

        var mappedResult = _mapper.Map<EmployeeResponse>(result.Value);

        return Ok(mappedResult);
    }

    [HttpPost]
    [Route("create-employee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateNewEmployeeAsync([FromBody] EmployeeRequest employeeRequest)
    {
        var mappedEmployee = _mapper.Map<Employee>(employeeRequest);

        var result = await _employeeService.CreateNewEmployeeAsync(mappedEmployee);

        return !result.IsSuccess
            ? StatusCode((int)result.StatusCode, result.Message)
            : Ok(result.Value);
    }

    [HttpGet]
    [Route("get-all-employees")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllEmployeesAsync()
    {
        var result = await _employeeService.GetAllEmployeesAsync();

        if (!result.IsSuccess) return StatusCode((int)result.StatusCode, result.Message);

        var mappedResponse = _mapper.Map<IEnumerable<EmployeeResponse>>(result.Value);

        return Ok(mappedResponse);
    }

    [HttpPatch]
    [Route("modify")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PatchUserAsync(JsonPatchDocument<EmployeeRequest> employeeRequest,
        [FromQuery] string userId)
    {
        var wasParsed = Guid.TryParse(userId, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var employeeResult = await _employeeService.GetEmployeeByGuidAsync(parsedGuid);

        if (!employeeResult.IsSuccess) return NotFound(employeeResult.Message);

        var requestDto = _mapper.Map<EmployeeRequest>(employeeResult.Value);

        try
        {
            employeeRequest.ApplyTo(requestDto);
        }
        catch (Exception e)
        {
            Log.Error("An error occured, because there were a malformed patch request. {Exception}, {Message}",
                e.GetType().FullName, e.Message);

            return BadRequest(e.Message);
        }

        var entity = _mapper.Map<Employee>(requestDto);

        var patchResult = await _employeeService.PatchEmployeeAsync(parsedGuid, entity);


        if (!patchResult.IsSuccess) return StatusCode((int)patchResult.StatusCode, patchResult.Message);

        var mappedResult = _mapper.Map<EmployeeResponse>(patchResult.Value);

        return Ok(mappedResult);
    }

    [HttpDelete]
    [Route("remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveEmployee([FromQuery] string employeeId)
    {
        var wasParsed = Guid.TryParse(employeeId, out var parsedGuid);

        if (!wasParsed)
            return BadRequest("Incorrect guid was provided");

        var result = await _employeeService.RemoveEmployeeAsync(parsedGuid);

        return !result.IsSuccess
            ? StatusCode((int)result.StatusCode, result.Message)
            : Ok();
    }
}