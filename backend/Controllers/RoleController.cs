using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Role;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Controllers;

[Route("api/role")]
[ApiController]
public class RoleController : ControllerBase
{
	private readonly ILogger<RoleController> _logger;

	private readonly IRoleService _service;

	private ApiResponse _apiResponse;

	public RoleController(ILogger<RoleController> logger, IRoleService service)
	{
		_logger = logger;
		_service = service;
		_apiResponse = new ApiResponse();
	}

	[HttpPost(Name = "RoleController_Create")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<Role>> Create([FromBody] RoleCreateDto roleCreateDto)
	{
		_logger.LogInformation("Executing RoleController class - Create method");

		try
		{
			if (roleCreateDto == null)
			{
				_logger.LogError("RoleController class - Create method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "No data received";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError("RoleController class - Create method - Invalida data");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Invalid data";

				return BadRequest(_apiResponse);
			}

			RoleDto roleDto = await _service.Create(roleCreateDto);

			_logger.LogInformation("Data created successfully.");

			_apiResponse.StatusCode = HttpStatusCode.Created;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleDto;

			return CreatedAtRoute("RoleController_GetById", new
			{
				id = roleDto.Id
			}, _apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Timeout";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleController class - Create method");
		}
	}

	[HttpGet(Name = "RoleController_GetAll")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetAll()
	{
		_logger.LogInformation("Executing RoleController class - GetAll method");

		try
		{
			IEnumerable<RoleDto> personsDto = await _service.GetAll();

			_logger.LogInformation("RoleController class - GetAll method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personsDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Timeout";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleController class - GetAll method");
		}
	}

	[HttpGet("{id}", Name = "RoleController_GetById")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetById([FromRoute] int id)
	{
		_logger.LogInformation("Executing RoleController class - GetById method");

		try
		{
			if (id <= 0)
			{
				_logger.LogError("RoleController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID not valid.";

				return BadRequest(_apiResponse);
			}

			RoleDto? roleDto = await _service.GetOne(role => role.Id == id);

			if (roleDto == null)
			{
				_logger.LogError("RoleController class - GetById method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Data not found.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation("RoleController class - GetById method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Timeout";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleController class - GetById method");
		}
	}

	[HttpPut(Name = "RoleController_Update")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Update([FromBody] RoleDto roleDto)
	{
		_logger.LogInformation("Executing RoleController class - Update method");

		try
		{
			if (roleDto == null)
			{
				_logger.LogError("RoleController class - Update method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "No data received";

				return BadRequest(_apiResponse);
			}

			RoleDto? roleUpdatedDto = await _service.Update(roleDto);

			if (roleUpdatedDto == null)
			{
				_logger.LogError($"RoleController class - Update method - Not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Not found";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"RoleController class - Update method - Update successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleUpdatedDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Timeout";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleController class - Update method");
		}
	}

	[HttpDelete("{id}", Name = "RoleController_Delete")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Delete([FromRoute] int id)
	{
		_logger.LogInformation("Executing RoleController class - Delete method");

		try
		{
			if (id == 0)
			{
				_logger.LogError("RoleController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID not valid.";

				return BadRequest(_apiResponse);
			}

			bool deleteSuccess = await _service.Delete(id);

			if (!deleteSuccess)
			{
				_logger.LogError("RoleController class - Delete method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Data not found.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"RoleController class - Delete method - Deleted successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Timeout";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleController class - Delete method");
		}
	}
}
