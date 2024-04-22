using AutoMapper;
using BLL.Contracts;
using BLL.DTO;
using BLL.Services;
using DAL.EF;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.ServiceTests;

public class ITicketServiceMock
{


    [Test]
    public void GetAllTickets_order_by_deadline()
    {
        var tickets = new List<Domain.Ticket>
        {
            new Ticket { Description = "First input", Deadline = DateTime.Now.AddHours(10) },
            new Ticket { Description = "Second input", Deadline = DateTime.Now.AddHours(15) },
            new Ticket { Description = "Third input", Deadline = DateTime.Now.AddHours(20) }
        };

        var mockSet = new Mock<DbSet<Ticket>>();
        mockSet.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(tickets.AsQueryable().Provider);
        mockSet.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(tickets.AsQueryable().Expression);
        mockSet.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(tickets.AsQueryable().ElementType);
        mockSet.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(tickets.AsQueryable().GetEnumerator());

        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(m => m.Tickets).Returns(mockSet.Object);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<List<TicketDTO>>(It.IsAny<List<Domain.Ticket>>()))
            .Returns((List<Domain.Ticket> source) => 
                source.Select(b => new TicketDTO { Description = b.Description, Deadline = b.Deadline }).ToList());

        var service = new TicketService(mockMapper.Object, mockContext.Object);

        // Act
        var result = service.GetAllNotResolvedTicketsAsync().Result.ToList();

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("First input", result[0].Description);
        Assert.AreEqual("Second input", result[1].Description);
        Assert.AreEqual("Third input", result[2].Description);
    }
    

    
}