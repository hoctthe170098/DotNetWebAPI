using WebAPI1.Models;

namespace WebAPI1.Services
{
    public interface IProductRespository
    {
        List<ProductModel> GetAll(string? search,double? from, double? to,string? sortBy,int page=1);
    }
}
