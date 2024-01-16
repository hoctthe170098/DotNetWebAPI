using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Data;
using WebAPI1.Services;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product2Controller : ControllerBase
    {
        private readonly IProductRespository _res;

        public Product2Controller(IProductRespository res)
        {
            _res = res;
        }
        [HttpGet]
        public IActionResult GetAllProduct(string? search,double? from, double? to,string? sortBy,int page=1) {   
            try
            {
                var result = _res.GetAll(search,from,to,sortBy,page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can not get products!");
            }
        }
    }
}
