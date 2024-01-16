using WebAPI1.Models;

namespace WebAPI1.Services
{
    public interface ILoaiRepository
    {
        List<LoaiVM>GetAll();
        LoaiVM GetById(int id);
        LoaiVM Add(LoaiModel loai);
        void Update(LoaiVM loai);
        void Delete(int id);
    }
}
