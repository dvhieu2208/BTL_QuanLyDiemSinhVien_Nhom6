using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.Interfaces
{
    internal interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(string id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(string id);
    }
}
