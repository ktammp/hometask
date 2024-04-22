using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;


namespace Tests.ControllerTests;

public class HomeControllerTests
{
    [Test]
    public async Task CreateTicket_ValidData_RedirectsToIndex()
    {
        var ticketServiceMock = new Mock<ITicketService>();
        ticketServiceMock.Setup(x => x.CreateTicketAsync (It.IsAny<TicketDTO>())).ReturnsAsync(true);

        var controller = new HomeController(ticketServiceMock.Object);
        var ticket = new TicketDTO()
        {
            Description = "New test ticket",
            Deadline = DateTime.Now.AddHours(20)
        };
        
        var result = await controller.CreateTicket(ticket);
        
        Assert.IsInstanceOf<RedirectToActionResult>(result);
    }

    [Test]
    public async Task MarkAsResolved_RedirectsToIndex()
    {
        var ticketServiceMock = new Mock<ITicketService>();
        var controller = new HomeController(ticketServiceMock.Object);
        var ticketId = Guid.NewGuid();
        var result = await controller.MarkAsResolved(ticketId) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.That(result!.ActionName!, Is.EqualTo("Index"));
    }
    [Test]
    public async Task CreateTicket_InvalidData_ReturnsViewWithTicket()
    {
        var ticketServiceMock = new Mock<ITicketService>();
        var controller = new HomeController(ticketServiceMock.Object);
        var ticket = new TicketDTO();
        


        var result = await controller.CreateTicket(ticket) as ViewResult;

        Assert.That(result!.Model, Is.EqualTo(ticket));
    }
    
}