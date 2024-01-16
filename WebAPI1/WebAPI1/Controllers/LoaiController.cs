using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Data;
using WebAPI1.Models;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LoaiController(MyDbContext context) 
        {
            _context=context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var dsLoai = _context.Loais.ToList();
                return Ok(dsLoai);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetAll(int id)
        {
            var loai = _context.Loais.Where(l=>l.MaLoai==id).FirstOrDefault();
            if (loai != null)
            {
                return Ok(loai);
            }
            else
            {
                return NotFound();
            }         
        }
        [HttpPost]
        [Authorize]
        public IActionResult CreateNew(LoaiModel model)
        {
            try
            {
                var loai = new Loai
                {
                    TenLoai = model.TenLoai
                };
                _context.Add(loai);
                _context.SaveChanges(); 
                return StatusCode(StatusCodes.Status201Created,loai);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLoaiById(int id,LoaiModel model)
        {
            var loai = _context.Loais.Where(l => l.MaLoai == id).FirstOrDefault();
            if (loai != null)
            {
                loai.TenLoai = model.TenLoai;
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var loai = _context.Loais.Where(l => l.MaLoai == id).FirstOrDefault();
            if (loai != null)
            {
                _context.Loais.Remove(loai);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
