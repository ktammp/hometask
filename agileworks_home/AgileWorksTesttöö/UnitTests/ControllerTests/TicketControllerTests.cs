using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace UnitTests.ControllerTests;

public class TicketControllerTests
{
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly TicketsController _controller;
    private readonly Guid _rndGuid;

    public TicketControllerTests()
    {
        
        _ticketServiceMock = new Mock<ITicketService>();
        _controller = new TicketsController(_ticketServiceMock.Object);
        _rndGuid = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-377d7497ede0", "D");


    }
    
    [Fact]
    public async Task Index_should_return_index_or_default_view()
    {

        _ticketServiceMock.Setup(c => c.GetAllResolvedTicketsAsync())
            .ReturnsAsync(() => new List<TicketDTO>());


        var result = await _controller.Index() as ViewResult;

        var viewName = result!.ViewName;

        Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Index");
    }
    
    [Fact]
    public async Task Edit_should_return_view_with_existing_request()
    {
        var ticket = new TicketDTO();
        _ticketServiceMock.Setup(c => c.GetTicketByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => ticket);

        var result = await _controller.Edit(_rndGuid) as ViewResult;
        var viewName = result!.ViewName;

        Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Edit");
        Assert.Equal(ticket, result.Model);
    }

    [Fact]
    public async Task Edit_should_return_OkResult()
    {
        
        var ticket = new TicketDTO();
        _ticketServiceMock.Setup(c => c.GetTicketByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => ticket);
      

        var data = await _controller.Edit(ticket.Id, ticket);
        Assert.IsType<RedirectToActionResult>(data);
    }

    
    [Fact]
    public async Task Delete_should_return_view_with_existing_request()
    {
        var ticket = new TicketDTO();
        _ticketServiceMock.Setup(c => c.GetTicketByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => ticket);

        var result = await _controller.Delete(_rndGuid) as ViewResult;
        var viewName = result!.ViewName;

        Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Delete");
        Assert.Equal(ticket, result.Model);
    }

    [Fact]
    public async Task Delete_should_return_OkResult()
    {
        var data = await _controller.DeleteConfirmed(_rndGuid);
        Assert.IsType<RedirectToActionResult>(data);
    }
}