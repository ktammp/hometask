using BLL.Services;

namespace Tests.BLLServiceTests;

public class TicketServiceTest
{
    private readonly TicketService _ticketService;

    public TicketServiceTest(TicketService ticketService)
    {
        _ticketService = ticketService;
    }
}