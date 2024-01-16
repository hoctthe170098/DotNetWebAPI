using WebAPI1.Data;
using WebAPI1.Models;

namespace WebAPI1.Services
{
    public class LoaiRepositoryInMemory : ILoaiRepository
    {
        static List<LoaiVM> loais = new List<LoaiVM>
        {
            new LoaiVM{MaLoai=1,TenLoai="Tivi"},
            new LoaiVM{MaLoai=2,TenLoai="Tulanh"},
            new LoaiVM{MaLoai=3,TenLoai="DieuHoa"},
            new LoaiVM{MaLoai=4,TenLoai="MayGiat"},
        };

        public LoaiVM Add(LoaiModel loai)
        {
            var _loai = new LoaiVM
            {
                MaLoai = loais.Max(l => l.MaLoai) + 1,
                TenLoai = loai.TenLoai
            };
            loais.Add(_loai);
            return _loai;
        }

        public void Delete(int id)
        {
            var _loai = loais.FirstOrDefault(l => l.MaLoai == id);
            if (_loai != null)
            {
               loais.Remove(_loai); 
            }
        }

        public List<LoaiVM> GetAll()
        {
            return loais;
        }

        public LoaiVM GetById(int id)
        {
            return loais.FirstOrDefault(l => l.MaLoai == id);
        }

        public void Update(LoaiVM loai)
        {
            var _loai=loais.FirstOrDefault(l=>l.MaLoai==loai.MaLoai);
            if (_loai != null)
            {
                _loai.TenLoai = loai.TenLoai;
            }
        }
    }
}
