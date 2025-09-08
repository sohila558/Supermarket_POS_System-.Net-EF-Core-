using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySuperMarket.DTOs;
using MySuperMarket.Models;
using MySuperMarket.Repository.GenericRepository;

namespace MySuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : GenericController<ProductDTO, ProductDTO, ProductDTO, ProductDTO, Product>
    {
        public ProductsController(IMapper mapper, IRepository<Product> repository) : base(mapper, repository)
        {
        }
    }
}
