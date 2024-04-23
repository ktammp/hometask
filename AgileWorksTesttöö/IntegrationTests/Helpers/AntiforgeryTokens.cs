using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace IntegrationTests.Helpers;

public class AntiforgeryTokens
{

    // Gets or sets the name of the cookie to use.
    [JsonProperty("cookieName")]
    [JsonPropertyName("cookieName")]
    public string CookieName { get; set; } = string.Empty;


    // Gets or sets the value to use for the antiforgery token HTTP cookie.
    [JsonProperty("cookieValue")]
    [JsonPropertyName("cookieValue")]
    public string CookieValue { get; set; } = string.Empty;

    // Gets or sets the name of the form parameter to use for the antiforgery token.
    [JsonProperty("formFieldName")]
    [JsonPropertyName("formFieldName")]
    public string FormFieldName { get; set; } = string.Empty;


    //Gets or sets the name of the HTTP request header to use for the antiforgery token.
    [JsonProperty("headerName")]
    [JsonPropertyName("headerName")]
    public string HeaderName { get; set; } = string.Empty;
    
    
    // Gets or sets the value to use for the antiforgery token for forms and/or HTTP request headers.
    [JsonProperty("requestToken")]
    [JsonPropertyName("requestToken")]
    public string RequestToken { get; set; } = string.Empty;
}
