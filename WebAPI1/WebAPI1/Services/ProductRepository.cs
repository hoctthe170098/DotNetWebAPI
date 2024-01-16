using Microsoft.EntityFrameworkCore;
using WebAPI1.Data;
using WebAPI1.Models;

namespace WebAPI1.Services
{
    public class ProductRepository : IProductRespository
    {
        private readonly MyDbContext _context;
        private static int PAGE_SIZE{get;set;}=2;
        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }
        public List<ProductModel> GetAll(string? search, double? from, double? to, string? sortBy,int page=1)
        {
            var allProducts = _context.HangHoas.Include(p=>p.Loai).AsQueryable();
            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(p => p.TenHH.Contains(search));
            }           
            if(from.HasValue)
            {
                allProducts=allProducts.Where(p => p.DonGia >= from);
            }
            if (to.HasValue)
            {
                allProducts=allProducts.Where(p => p.DonGia <= to);
            }
            #endregion
            #region Sorting
            //Default sort by Name
            allProducts = allProducts.OrderBy(p => p.TenHH);
            if(!string.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "tenhh_desc": 
                        allProducts = allProducts.OrderByDescending(p => p.TenHH);
                        break;
                    case "Gia_asc": 
                        allProducts = allProducts.OrderBy(p => p.DonGia);
                        break;
                    case "Gia_desc":
                        allProducts = allProducts.OrderByDescending(p => p.DonGia);
                        break;
                }
            }
            #endregion
            /*            #region Paging
                        allProducts=allProducts.Skip((page-1)*PAGE_SIZE).Take(PAGE_SIZE);
                        #endregion
                        var result = allProducts.Select(p => new ProductModel
                        {
                            MaHangHoa= p.MaHH,
                            DonGia= p.DonGia,
                            Name=p.TenHH,
                            TenLoai=p.Loai.TenLoai
                        });
                        return result.ToList();*/
            var result = PaginatedList<HangHoa>.Create(allProducts, page, PAGE_SIZE);
            return result.Select(p => new ProductModel
            {
                MaHangHoa = p.MaHH,
                DonGia = p.DonGia,
                Name = p.TenHH,
                TenLoai = p.Loai?.TenLoai
            }).ToList();
        }
    }
}
