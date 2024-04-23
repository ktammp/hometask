using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace UnitTests.ControllerTests;

public class HomeControllerTests
{
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        
        _ticketServiceMock = new Mock<ITicketService>();
        _controller = new HomeController(_ticketServiceMock.Object);

    }
    
    [Fact]
    public async Task Index_should_return_index_or_default_view()
    {

        _ticketServiceMock.Setup(c => c.GetAllNotResolvedTicketsAsync())
            .ReturnsAsync(() => new List<TicketDTO>());


        var result = await _controller.Index() as ViewResult;

        var viewName = result!.ViewName;

        Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Index");
    }
    
    [Fact]
    public async Task CreateTicket_ValidData_RedirectsToIndex()
    {
        
        var ticket = new TicketDTO()
        {
            Description = "New test ticket",
            Deadline = DateTime.Now.AddHours(20)
        };
        
        var result = await _controller.CreateTicket(ticket);
        
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task MarkAsResolved_RedirectsToIndex()
    {
        var ticketId = Guid.NewGuid();
        var result = await _controller.MarkAsResolved(ticketId) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result!.ActionName!);
    }
    
    [Fact]
    public void CreateTicket_should_return_CreateTicket_view()
    {
        var result = _controller.CreateTicket() as ViewResult;
        var viewName = result!.ViewName;

        Assert.True(string.IsNullOrEmpty(viewName) || viewName == "CreateTicket");
    }
  
}