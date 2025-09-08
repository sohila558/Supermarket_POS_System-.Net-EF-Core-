using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySuperMarket.DTOs;
using MySuperMarket.Models;
using MySuperMarket.Repository.GenericRepository;

namespace MySuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : GenericController<CustomerDTO, CustomerDTO, CustomerDTO, CustomerDTO, Customer>
    {
        public CustomersController(IMapper mapper, IRepository<Customer> repository) : base(mapper, repository)
        {
        }


    }
}
