using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.RoleOption;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Controllers;

[Route("api/role_option")]
[ApiController]
public class RoleOptionController(ILogger<RoleOptionController> logger, IRoleOptionService service) : ControllerBase
{
	private readonly ILogger<RoleOptionController> _logger = logger;

	private readonly IRoleOptionService _service = service;

	private readonly ApiResponse _apiResponse = new ApiResponse();

	[HttpPost(Name = "RoleOptionController_Create")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<RoleOption>> Create([FromBody] RoleOptionCreateDto roleOptionCreateDto)
	{
		_logger.LogInformation("Executing RoleOptionController class - Create method");

		try
		{
			if (roleOptionCreateDto == null)
			{
				_logger.LogError("RoleOptionController class - Create method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Datos no recibidos.";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError("RoleOptionController class - Create method - Invalida data");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Se enviaron datos no válidos.";

				return BadRequest(_apiResponse);
			}

			RoleOptionDto roleOptionDto = await _service.Create(roleOptionCreateDto);

			_logger.LogInformation("Data created successfully.");

			_apiResponse.StatusCode = HttpStatusCode.Created;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleOptionDto;

			return CreatedAtRoute("RoleOptionController_GetById", new
			{
				id = roleOptionDto.Id
			}, _apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleOptionController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleOptionController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleOptionController class - Create method");
		}
	}

	[HttpGet(Name = "RoleOptionController_GetAll")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? status)
	{
		_logger.LogInformation("Executing RoleOptionController class - GetAll method");

		try
		{
			Expression<Func<RoleOption, bool>>? filter = null;

			if (status != null)
			{
				filter = role => role.Status == status;
			}

			IEnumerable<RoleOptionDto> personsDto = await _service.GetAll(filter);

			_logger.LogInformation("RoleOptionController class - GetAll method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personsDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleOptionController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleOptionController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleOptionController class - GetAll method");
		}
	}

	[HttpGet("{id}", Name = "RoleOptionController_GetById")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetById([FromRoute] int id)
	{
		_logger.LogInformation("Executing RoleOptionController class - GetById method");

		try
		{
			if (id <= 0)
			{
				_logger.LogError("RoleOptionController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID no válido.";

				return BadRequest(_apiResponse);
			}

			RoleOptionDto? roleOptionDto = await _service.GetOne(roleOption => roleOption.Id == id);

			if (roleOptionDto == null)
			{
				_logger.LogError("RoleOptionController class - GetById method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Registro no encontrado.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation("RoleOptionController class - GetById method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleOptionDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleOptionController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleOptionController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleOptionController class - GetById method");
		}
	}

	[HttpPut(Name = "RoleOptionController_Update")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Update([FromBody] RoleOptionUpdateDto roleOptionUpdateDto)
	{
		_logger.LogInformation("Executing RoleOptionController class - Update method");

		try
		{
			if (roleOptionUpdateDto == null)
			{
				_logger.LogError("RoleOptionController class - Update method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Datos no recibidos.";

				return BadRequest(_apiResponse);
			}

			RoleOptionDto? roleOptionUpdatedDto = await _service.Update(roleOptionUpdateDto);

			if (roleOptionUpdatedDto == null)
			{
				_logger.LogError($"RoleOptionController class - Update method - Not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Not found";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"RoleOptionController class - Update method - Update successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = roleOptionUpdatedDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleOptionController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleOptionController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleOptionController class - Update method");
		}
	}

	[HttpDelete("{id}", Name = "RoleOptionController_Delete")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Delete([FromRoute] int id)
	{
		_logger.LogInformation("Executing RoleOptionController class - Delete method");

		try
		{
			if (id == 0)
			{
				_logger.LogError("RoleOptionController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID no válido.";

				return BadRequest(_apiResponse);
			}

			bool deleteSuccess = await _service.Delete(id);

			if (!deleteSuccess)
			{
				_logger.LogError("RoleOptionController class - Delete method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Registro no encontrado.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"RoleOptionController class - Delete method - Deleted successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"RoleOptionController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"RoleOptionController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = ex.Message;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving RoleOptionController class - Delete method");
		}
	}
}
