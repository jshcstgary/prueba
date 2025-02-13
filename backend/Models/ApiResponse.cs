using System.Net;

namespace PruebaViamaticaBackend.Models;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public string StatusMessage { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public object? Data { get; set; } = null!;
}
