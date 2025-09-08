using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperMarket.DTOs;
using MySuperMarket.Models;
using MySuperMarket.Repository.GenericRepository;
namespace MySuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<DTOg, DTOgi, DTOpo, DTOpu, Model> : ControllerBase
        where DTOg : class
        where DTOgi : class
        where DTOpo : class
        where DTOpu : IPUT
        where Model : class

    {
        protected readonly IMapper _mapper;
        protected readonly IRepository<Model> _repository;

        public GenericController(IMapper mapper, IRepository<Model> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        [HttpGet("GetAllASync")]
        public async Task<IEnumerable<DTOg>> GetAllASync()
        {
            return _mapper.Map<IEnumerable<DTOg>>(await _repository.GetAllASync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.GetByIdASync(id);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<DTOgi>(entity);
            return Ok(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOpo dTOpo)
        {
            var entity = _mapper.Map<Model>(dTOpo);
            await _repository.AddASync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity });
        }
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] DTOpu dTOpu)
        {
            var entity = await _repository.GetByIdASync(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity = _mapper.Map<Model>(dTOpu);
            await _repository.UpdateASync(entity);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdASync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await _repository.DeleteASync(entity);
            return NoContent();
        }
    }
}
