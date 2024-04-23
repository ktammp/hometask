using AutoMapper;
using BLL.DTO;

namespace BLL.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Domain.Ticket, TicketDTO>().ReverseMap();
    }
}