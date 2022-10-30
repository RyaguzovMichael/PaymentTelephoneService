using System.Text.Json.Serialization;

namespace PaymentTelephoneServices.API.Models;

public class ResponseVm
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("error")]
    public string? Error { get; set; }
    [JsonPropertyName("errorCode")]
    public ErrorCodes ErrorCode { get; set; }
}
