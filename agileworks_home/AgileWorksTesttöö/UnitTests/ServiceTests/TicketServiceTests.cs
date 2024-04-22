using AutoMapper;
using BLL.DTO;
using BLL.Services;
using Domain;


namespace UnitTests.ServiceTests;

public class TicketServiceTests : InMemoryDb
{
    private readonly TicketService _ticketService;
    private readonly Guid _rndGuid;

    public TicketServiceTests()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TicketDTO, Domain.Ticket>().ReverseMap();
        }));
        _ticketService = new TicketService(mapper, DbContext);
        _rndGuid = Guid.ParseExact("bddd91db-6d50-4bdd-b13b-377d7497ede0", "D");

    }

    [Fact]
    public async Task GetAllNotResolvedTicketsAsync_should_return_list_of_tickets()
    {
        await SeedTicketsAsync();

        var res = await _ticketService.GetAllNotResolvedTicketsAsync();

        Assert.NotNull(res);
        Assert.Equal(9, res.Count());
    }
    
    [Fact]
    public async Task GetAllResolvedTicketsAsync_should_return_list_of_tickets_where_resolved_is_true()
    {
        await SeedTicketsAsync();

        var res = await _ticketService.GetAllResolvedTicketsAsync();

        Assert.NotNull(res);
        Assert.Equal(5, res.Count());
    }
    
    [Fact]
    public async Task GetTicketByIdAsync_should_return_null_for_missing_ticket()
    {
        
        var res = await _ticketService.GetTicketByIdAsync(Guid.NewGuid());

        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetTicketByIdAsync_should_return_model_for_existing_ticketDto()
    {
        await SeedTicketsAsync();
       
        var result = await _ticketService.GetTicketByIdAsync(_rndGuid);

        Assert.NotNull(result);
        Assert.Equal(_rndGuid, result.Id);
    }
    
    [Fact]
    public async Task CreateAsync_should_return_created_ticketDto()
    {
        var ticketDto = new TicketDTO()
        {
            Description = "New ticket description",
            Deadline = DateTime.Now.AddHours(30)
        };

        var createTicket = await _ticketService.CreateTicketAsync(ticketDto);

        Assert.NotNull(createTicket);
        Assert.Equal("New ticket description", createTicket.Description);
    }
    [Fact]
    public async Task UpdateTicketAsync_should_return_updated_ticketDto()
    {
        await SeedTicketsAsync();
        var ticket = await _ticketService.GetTicketByIdAsync(_rndGuid);
        DbContext.ChangeTracker.Clear();
        
        ticket.Description = "Updated description";
        var res = await _ticketService.UpdateTicketAsync(ticket); 

        Assert.Equal("Updated description", res.Description);
    }
    [Fact]
    public async Task MarkTicketResolvedAsync_should_return_updated_ticketDto()
    {
        await SeedTicketsAsync();
        
        var result = await _ticketService.MarkTicketResolvedAsync(_rndGuid);

        Assert.True(result.Resolved);
    }
    [Fact]
    public async Task DeleteTicketAsync_should_return_true()
    {
        await SeedTicketsAsync();
        
        var result = await _ticketService.DeleteTicketAsync(_rndGuid);

        Assert.True(result);
    }
    
    
    private async Task SeedTicketsAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
        for (int i = 0; i < 8; i++)
        {
            DbContext.Tickets.Add(new Ticket()
            {
                Description = "Ticket no " + i,
                Deadline = DateTime.Now.AddHours(i)
            });
        }
        for (int i = 0; i < 5; i++)
        {
            DbContext.Tickets.Add(new Ticket()
            {
                Description = "Ticket no " + 8 + i,
                Deadline = DateTime.Now.AddHours(-i),
                Resolved = true
            });
        }
        DbContext.Tickets.Add(new Ticket()
        {
            Id = _rndGuid,
            Description = "Ticket with random guid",
            Deadline = DateTime.Now.AddHours(10)
        });

       

        await DbContext.SaveChangesAsync();
    }
}