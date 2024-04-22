using System.Collections;
using AutoMapper;
using BLL.Contracts;
using BLL.DTO;
using BLL.Helpers;
using BLL.Services;
using DAL.EF;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.ServiceTests;

[TestFixture]
public class TicketServiceTests
{
    
    [SetUp]
    public void Setup()
    {
        var _mapperMock = new Mock<IMapper>();
        var _dbContextMock = new Mock<AppDbContext>();

        // ... (Previous AllTickets initialization and DbContext mock setup)

       // _service = new TicketService( _mapperMock.Object, _dbContextMock.Object);

 
    }

   
}


   
    
