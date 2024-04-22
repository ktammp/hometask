using IntegrationTests.Helpers;
using Microsoft.Net.Http.Headers;
using Xunit.Abstractions;

namespace IntegrationTests;

public class TicketControllerIntegrationTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    
    private readonly HttpClient _client;
    private readonly Guid _notResolvedTicketGuid; //This is the ID of the ticket inserted into the database using CustomWebApplicationFactory
    private readonly Guid _resolvedTicketGuid; //This is the ID of the resolved ticket inserted into the database using CustomWebApplicationFactory

    public TicketControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _resolvedTicketGuid = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-111d7497ede0", "D");
        _notResolvedTicketGuid = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-377d7497ede0", "D");
        _client = factory.CreateClient();
    }

    
    [Theory]
    [InlineData("/Tickets")]
    public async Task Get_EndPointsReturnsSuccessForClient(string url)
    {
        
        var response = await _client.GetAsync(url);

        //Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }

    public async Task<HttpRequestMessage> SetupEditTicketPostData(string description)
    {
        
        var initResponse = await _client.GetAsync($"Tickets/Edit/{_notResolvedTicketGuid}");
        var (fieldValue, cookieValue) = await AntiForgeryTokenController.ExtractAntiForgeryValues(initResponse);

        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"Tickets/Edit/{_notResolvedTicketGuid}");
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
    public async Task Edit_WhenCalled_ReturnsEditTicketForm()
    {
        //Act
        var response = await _client.GetAsync($"Tickets/Edit/{_notResolvedTicketGuid}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Muuda pöördumist", responseString);
    }
    
    
    [Fact]
    public async Task Edit_SentCorrectModel_ReturnsToIndexViewWithUpdatedTicket()
    {
        //Arrange
        var postRequest = await SetupEditTicketPostData("This is a description");
        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        
        // Assert
        Assert.Contains("Home Page - WebApp", responseString);
        Assert.DoesNotContain("Testing Testing", responseString);
        Assert.Contains("This is a description", responseString);
        Assert.Contains("27.04.2022 12:15", responseString);
    }
    [Fact]
    public async Task Edit_SentIncorrectModel_ReturnsSameViewWithErrorMessages()
    {
        //Arrange
        HttpRequestMessage postRequest = await SetupEditTicketPostData("");
        
        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("The Description field is required.", responseString);
    }
    
    [Fact]
    public async Task Delete_WhenCalled_ReturnsDeleteTicketForm()
    {
        //Act
        var response = await _client.GetAsync($"Tickets/Delete/{_resolvedTicketGuid}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Kustuta pöördumine", responseString);
    }
    

    [Fact]
    public async Task Delete_WhenExecuted_ReturnsToResolvedTicketIndex()
    {
        var resolvedTickets = await _client.GetAsync("Tickets");
        resolvedTickets.EnsureSuccessStatusCode();
        var text = await resolvedTickets.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Resolved ticket", text);
        
        
        var initResponse = await _client.GetAsync($"Tickets/Delete/{_resolvedTicketGuid}");
        
        var (fieldValue, cookieValue) = await AntiForgeryTokenController.ExtractAntiForgeryValues(initResponse);

        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"Tickets/Delete/{_resolvedTicketGuid}");
        postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenController.AntiForgeryCookieName, cookieValue).ToString());
        
        var formModel = new Dictionary<string, string>
        {
            { AntiForgeryTokenController.AntiForgeryFieldName, fieldValue },
        };

        postRequest.Content = new FormUrlEncodedContent(formModel);
        

        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Kõik lahendatud pöördumised", responseString);
        Assert.DoesNotContain("Resolved ticket", responseString);
    }
}