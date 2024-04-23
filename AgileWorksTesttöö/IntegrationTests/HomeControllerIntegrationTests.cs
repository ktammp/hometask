using IntegrationTests.Helpers;
using Microsoft.Net.Http.Headers;

namespace IntegrationTests;

public class HomeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly Guid _notResolvedTicketGuid; //This is the ID of the resolved ticket inserted into the database using CustomWebApplicationFactory


    public HomeControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _notResolvedTicketGuid = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-377d7497ede0", "D");
        _client = factory.CreateClient();
    }

    
    [Theory]
    [InlineData("/")]
    [InlineData("/Home")]
    [InlineData("/Home/CreateTicket")]
    public async Task Get_EndPointsReturnsSuccessForClient(string url)
    {
        
        var response = await _client.GetAsync(url);

        //Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }

    private async Task<HttpRequestMessage> SetupTicketPostData(string description)
    {
        
        var initResponse = await _client.GetAsync("/Home/CreateTicket");
        var (fieldValue, cookieValue) = await AntiForgeryTokenController.ExtractAntiForgeryValues(initResponse);

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Home/CreateTicket");
        postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenController.AntiForgeryCookieName, cookieValue).ToString());
        
        var formModel = new Dictionary<string, string>
        {
            { AntiForgeryTokenController.AntiForgeryFieldName, fieldValue },
            { "Description", description },
            { "Deadline", "2022-04-27 12:15" }
        };

        postRequest.Content = new FormUrlEncodedContent(formModel);

        return postRequest;

    }
    [Fact]
    public async Task Ticket_WhenMarkedResolved_IsDeletedFromHomePage()
    {
        var index = await _client.GetAsync("/");
        index.EnsureSuccessStatusCode();
        var text = await index.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Contains("Testing Testing", text);
        
      
        var (fieldValue, cookieValue) = await AntiForgeryTokenController.ExtractAntiForgeryValues(index);

        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Home/MarkAsResolved/{_notResolvedTicketGuid}");
        postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenController.AntiForgeryCookieName, cookieValue).ToString());

        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.DoesNotContain("Testing Testing", responseString);
    }
    
    [Fact]
    public async Task CreateTicket_WhenCalled_ReturnsCreateForm()
    {
        //Act
        var response = await _client.GetAsync("/Home/CreateTicket");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Lisa uus pöördumine", responseString);
    }
    
    
    [Fact]
    public async Task CreateTicket_SentWrongModel_ReturnsSameViewWithErrorMessages()
    {
        //Arrange
        HttpRequestMessage postRequest = await SetupTicketPostData("");
        
        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("The Description field is required.", responseString);
    }
    
    
    [Fact]
    public async Task CreateTicket_SentCorrectModel_ReturnsToIndexViewWithTicket()
    {
        //Arrange
        var postRequest = await SetupTicketPostData("This is a description");
        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        
        // Assert
        Assert.Contains("Home Page - WebApp", responseString);
        Assert.Contains("This is a description", responseString);
        Assert.Contains("27.04.2022 12:15", responseString);

    }
    
   
}