using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;
using Xunit;

namespace Tests.ControllerTests;

public class HomeControllerTests
{
    [Fact]
    public async Task CreateTicket_ValidData_RedirectsToIndex()
    {
        var ticketServiceMock = new Mock<ITicketService>();
        //ticketServiceMock.Setup(x => x.CreateTicket(It.IsAny<TicketDTO>())).ReturnsAsync(true);

        var controller = new HomeController(ticketServiceMock.Object);
        var ticket = new TicketDTO()
        {
            Description = "New test ticket",
            Deadline = DateTime.Now.AddHours(20)
        };
        
        var result = await controller.CreateTicket(ticket);
        
        Assert.IsType<RedirectToActionResult>(result);
    }
}