namespace WebAPI1.Models
{
    public class ProductVM
    {
        public string Name { get; set; }
        public double DonGia { get; set; }
    }
    public class Product:ProductVM
    {
        public Guid MaHangHoa { get; set; }
    }
}
