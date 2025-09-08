using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySuperMarket.DTOs;
using MySuperMarket.Repository.InvoiceRepository;

namespace MySuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        protected readonly IMapper _mapper;

        private readonly IRepoInvoices _invoicesRepo;

        public InvoicesController(IRepoInvoices invoicesRepo, IMapper mapper)
        {
            _invoicesRepo = invoicesRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddInvoiceDTO invoice)
        {
            await _invoicesRepo.AddAsync(invoice);

            return Ok();
        }

        [HttpGet("GetAllAsync")]
        public async Task<List<AddInvoiceDTO>> GetAllAsync(AddInvoiceDTO dto)
        {
            return _mapper.Map<List<AddInvoiceDTO>>(await _invoicesRepo.GetAllAsync(dto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(_mapper.Map<AddInvoiceDTO>(await _invoicesRepo.GetByIdASync(id)));
        }

        [HttpPut("Refunded")]
        public async Task<IActionResult> RefundAsync(int id)
        {
            var entity = await _invoicesRepo.RefundAsync(id);

            if (!entity)
            {
                return BadRequest("Not found!");
            }

            return Ok(entity);
        }

        [HttpPut("RefundPartial")]
        public async Task<IActionResult> RefundPartialAsync(RefundedInvoiceDTO dto)
        {
            var entity = await _invoicesRepo.RefundPartialAsync(dto);

            if (!entity)
            {
                return BadRequest("Not found!");
            }

            return Ok(entity);
        }
    }
}
