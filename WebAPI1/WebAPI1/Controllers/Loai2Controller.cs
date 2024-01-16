using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Models;
using WebAPI1.Services;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Loai2Controller : ControllerBase
    {
        private readonly ILoaiRepository _LoaiRepository;

        public Loai2Controller(ILoaiRepository loaiRepository) 
        {
            _LoaiRepository=loaiRepository;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            try
            {
                return Ok(_LoaiRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var data= _LoaiRepository.GetById(id);
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id,LoaiVM loai)
        {
            if (id != loai.MaLoai)
            {
                return BadRequest();
            }
            try
            {
                _LoaiRepository.Update(loai);
                return NoContent();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _LoaiRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        public IActionResult Add(LoaiModel loai)
        {
            try
            {
                return Ok(_LoaiRepository.Add(loai));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
