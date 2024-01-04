using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using WebAPI1.Models;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public static List<Product> Products = new List<Product>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var product = Products.SingleOrDefault(p => p.MaHangHoa == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            var product = new Product
            {
                MaHangHoa = Guid.NewGuid(),
                Name = productVM.Name,
                DonGia = productVM.DonGia,
            };
            Products.Add(product);
            return Ok(new
            {
                Success = true,
                Data = product
            });
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, Product productEdit)
        {
            try
            {
                var product = Products.SingleOrDefault(p => p.MaHangHoa == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                if (id != productEdit.MaHangHoa.ToString())
                {
                    return BadRequest();
                }
                product.Name = productEdit.Name;
                product.DonGia = productEdit.DonGia;
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var product = Products.SingleOrDefault(p => p.MaHangHoa == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                Products.Remove(product);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

