using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal abstract class QuanLyCoBanBLL<T> where T : IKiemTraHopLe
    {
        protected readonly IRepository<T> repo;

        protected QuanLyCoBanBLL(IRepository<T> repository)
        {
            repo = repository;
        }

        public List<T> LayTatCa() { return repo.GetAll(); }
        public T LayTheoMa(string ma) { return repo.GetById(ma); }

        protected virtual bool KiemTraNghiepVu(T doiTuong, bool themMoi, out string loi)
        {
            loi = null;
            return true;
        }

        public bool Them(T doiTuong, out string loi)
        {
            if (!doiTuong.KiemTra(out loi)) return false;
            if (!KiemTraNghiepVu(doiTuong, true, out loi)) return false;
            return ThucHien(() => repo.Add(doiTuong), "Không thêm được dữ liệu", out loi);
        }

        public bool Sua(T doiTuong, out string loi)
        {
            if (!doiTuong.KiemTra(out loi)) return false;
            if (!KiemTraNghiepVu(doiTuong, false, out loi)) return false;
            return ThucHien(() => repo.Update(doiTuong), "Không tìm thấy dữ liệu cần sửa", out loi);
        }

        public bool Xoa(string ma, out string loi)
        {
            if (string.IsNullOrWhiteSpace(ma)) { loi = "Chưa chọn dòng cần xóa"; return false; }
            return ThucHien(() => repo.Delete(ma), "Không tìm thấy dữ liệu cần xóa", out loi);
        }

        /// <summary>Chạy thao tác CSDL, chuyển lỗi SQL thành thông báo dễ hiểu.</summary>
        protected static bool ThucHien(Func<bool> thaoTac, string loiKhiKhongCoDong, out string loi)
        {
            try
            {
                bool ok = thaoTac();
                loi = ok ? null : loiKhiKhongCoDong;
                return ok;
            }
            catch (SqlException ex)
            {
                loi = XuLyLoi.DichLoiSql(ex);
                return false;
            }
        }
    }
    internal static class XuLyLoi
    {
        public static string DichLoiSql(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601: return "Mã đã tồn tại trong hệ thống.";
                case 547: return "Dữ liệu đang được sử dụng ở bảng khác hoặc không hợp lệ (ràng buộc khóa ngoại/CHECK).";
                case 2:
                case 53:
                case 18456: return "Không kết nối được SQL Server. Kiểm tra chuỗi kết nối trong App.config.\n" + ex.Message;
                default: return ex.Message;
            }
        }
    }
}
