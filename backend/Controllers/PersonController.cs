using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Controllers;

[Route("api/person")]
[ApiController]
public class PersonController(ILogger<PersonController> logger, IPersonService service) : ControllerBase
{
	private readonly ILogger<PersonController> _logger = logger;

	private readonly IPersonService _service = service;

	private readonly ApiResponse _apiResponse = new ApiResponse();

	[HttpPost(Name = "PersonController_Create")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status207MultiStatus)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Create([FromBody] ICollection<PersonCreateDto> personsCreateDto)
	{
		_logger.LogInformation("Executing PersonController class - Create method");

		try
		{
			if (personsCreateDto == null)

				if (personsCreateDto == null || personsCreateDto.Count == 0)
				{
					_logger.LogError("PersonController class - Create method - No data recieved.");

					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
					_apiResponse.ErrorMessage = "Datos no recibidos o lista vacía.";

					return BadRequest(_apiResponse);
				}

			if (personsCreateDto.Any(p => p.User == null))
			{
				_logger.LogError("PersonController class - Create method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Uno de los elementos no tiene usuario.";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError("PersonController class - Create method - Invalida data");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Se enviaron datos no válidos.";

				return BadRequest(_apiResponse);
			}

			RowsChanged rowsChanged = await _service.Create(personsCreateDto);

			_logger.LogInformation("Data created successfully.");

			_apiResponse.StatusCode = HttpStatusCode.MultiStatus;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = rowsChanged;

			return StatusCode(StatusCodes.Status207MultiStatus, _apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - Create method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - Create method");
		}
	}

	[HttpGet(Name = "PersonController_GetAll")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetAll()
	{
		_logger.LogInformation("Executing PersonController class - GetAll method");

		try
		{
			IEnumerable<PersonDto> personsDto = await _service.GetAll();

			_logger.LogInformation("PersonController class - GetAll method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personsDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - GetAll method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - GetAll method");
		}
	}

	[HttpGet("count", Name = "PersonController_GetCount")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetCount()
	{
		_logger.LogInformation("Executing PersonController class - GetCount method");

		try
		{
			IEnumerable<PersonCount> personCount = await _service.GetCount();

			_logger.LogInformation("PersonController class - GetCount method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personCount;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - GetCount method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - GetCount method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - GetCount method");
		}
	}

	[HttpGet("{id}", Name = "PersonController_GetById")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> GetById([FromRoute] int id)
	{
		_logger.LogInformation("Executing PersonController class - GetById method");

		try
		{
			if (id <= 0)
			{
				_logger.LogError("PersonController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID no válido.";

				return BadRequest(_apiResponse);
			}

			PersonDto? personDto = await _service.GetOne(person => person.Id == id);

			if (personDto == null)
			{
				_logger.LogError("PersonController class - GetById method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Registro no encontrado.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation("PersonController class - GetById method - Data retrieved");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - GetById method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - GetById method");
		}
	}

	[HttpPut(Name = "PersonController_Update")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Update([FromBody] PersonUpdateDto personUpdateDto)
	{
		_logger.LogInformation("Executing PersonController class - Update method");

		try
		{
			if (personUpdateDto == null)
			{
				_logger.LogError("PersonController class - Update method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Datos no recibidos.";

				return BadRequest(_apiResponse);
			}

			PersonDto? personUpdatedDto = await _service.Update(personUpdateDto);

			if (personUpdatedDto == null)
			{
				_logger.LogError($"PersonController class - Update method - Not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Not found";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"PersonController class - Update method - Update successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personUpdatedDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - Update method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - Update method");
		}
	}

	[HttpDelete("{id}", Name = "PersonController_Delete")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> Delete([FromRoute] int id)
	{
		_logger.LogInformation("Executing PersonController class - Delete method");

		try
		{
			if (id == 0)
			{
				_logger.LogError("PersonController class - GetById method - ID not valid");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID no válido.";

				return BadRequest(_apiResponse);
			}

			bool deleteSuccess = await _service.Delete(id);

			if (!deleteSuccess)
			{
				_logger.LogError("PersonController class - Delete method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Registro no encontrado.";

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"PersonController class - Delete method - Deleted successfully");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"PersonController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"PersonController class - Delete method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving PersonController class - Delete method");
		}
	}
}
