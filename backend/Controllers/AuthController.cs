using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, IAuthService service) : ControllerBase
{
	private readonly ILogger<AuthController> _logger = logger;

	private readonly IAuthService _service = service;

	private readonly ApiResponse _apiResponse = new();

	[HttpPost("sign_in", Name = "AuthController_SignIn")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> SignIn([FromBody] AuthData authData)
	{
		_logger.LogInformation("Executing AuthController class - SignIn method");

		try
		{
			if (authData == null)
			{
				_logger.LogError("AuthController class - SignIn method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Datos no recibidos.";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError("AuthController class - SignIn method - Invalida data");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Se enviaron datos no válidos.";

				return BadRequest(_apiResponse);
			}

			PersonDto? personDto = await _service.SignIn(authData);

			if (personDto == null)
			{
				_logger.LogError("AuthController class - SignIn method - Data not found");

				_apiResponse.StatusCode = HttpStatusCode.NotFound;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Registro no encontrado o eliminado.";

				return NotFound(_apiResponse);
			}

			if (!personDto.User.Roles.Any(role => role.Id == authData.IdRole))
			{
				_logger.LogError("AuthController class - SignIn method - User and role don't match");

				_apiResponse.StatusCode = HttpStatusCode.Conflict;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Usuario no tiene el rol seleccionado.";

				return Conflict(_apiResponse);
			}

			await _service.SetSession(personDto.User.Id, true);

			_logger.LogInformation("Sign In successfully.");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = personDto;

			return _apiResponse;
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"AuthController class - SignIn method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("SignIn:"))
			{
				_logger.LogError("AuthController class - SignIn method - Session already active");

				_apiResponse.StatusCode = HttpStatusCode.Forbidden;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "Ya tiene una sesión iniciada.";

				return new ObjectResult(_apiResponse)
				{
					StatusCode = (int)HttpStatusCode.Forbidden
				};
			}

			_logger.LogError($"AuthController class - SignIn method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving AuthController class - SignIn method");
		}
	}

	[HttpGet("sign_out/{idPerson}", Name = "AuthController_SignOut")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse>> SignOut(int idPerson)
	{
		_logger.LogInformation("Executing AuthController class - SignOut method");

		try
		{
			if (idPerson == 0)
			{
				_logger.LogError("AuthController class - SignOut method - No data recieved.");

				_apiResponse.StatusCode = HttpStatusCode.BadRequest;
				_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
				_apiResponse.ErrorMessage = "ID no válido.";

				return BadRequest(_apiResponse);
			}

			int idUser = await _service.SignOut(idPerson);

			await _service.SetSession(idUser, false);

			_logger.LogInformation("Sign Out successfully.");

			_apiResponse.StatusCode = HttpStatusCode.OK;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.Data = null;

			return _apiResponse;
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"AuthController class - SignOut method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.RequestTimeout;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Tiempo de espera excedido";

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"AuthController class - SignOut method - {ex.ToString()}");

			_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
			_apiResponse.StatusMessage = _apiResponse.StatusCode.ToString();
			_apiResponse.ErrorMessage = "Falla interna, acción no completada.";

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation("Leaving AuthController class - SignOut method");
		}
	}
}
