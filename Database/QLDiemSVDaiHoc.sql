/* =====================================================================
   CƠ SỞ DỮ LIỆU QUẢN LÝ ĐIỂM SINH VIÊN - NHÓM 6
   Chạy toàn bộ file này trong SQL Server Management Studio (F5).
   Script có thể chạy lại nhiều lần: các bảng cũ sẽ bị xóa và tạo lại.
   ===================================================================== */
IF DB_ID(N'QuanLyDiemSVDaiHoc') IS NULL
    CREATE DATABASE QuanLyDiemSVDaiHoc;
GO
USE QuanLyDiemSVDaiHoc;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/* ---------- Xóa bảng cũ (theo thứ tự khóa ngoại) ---------- */
IF OBJECT_ID(N'Diem', N'U') IS NOT NULL DROP TABLE Diem;
IF OBJECT_ID(N'ThoiKhoaBieu', N'U') IS NOT NULL DROP TABLE ThoiKhoaBieu;
IF OBJECT_ID(N'SinhVien', N'U') IS NOT NULL DROP TABLE SinhVien;
IF OBJECT_ID(N'Lop', N'U') IS NOT NULL DROP TABLE Lop;
IF OBJECT_ID(N'Khoa_ChuyenNganh', N'U') IS NOT NULL DROP TABLE Khoa_ChuyenNganh;
IF OBJECT_ID(N'TaiKhoan', N'U') IS NOT NULL DROP TABLE TaiKhoan;
IF OBJECT_ID(N'PhongHoc', N'U') IS NOT NULL DROP TABLE PhongHoc;
IF OBJECT_ID(N'HeDaoTao', N'U') IS NOT NULL DROP TABLE HeDaoTao;
IF OBJECT_ID(N'Que', N'U') IS NOT NULL DROP TABLE Que;
IF OBJECT_ID(N'DanToc', N'U') IS NOT NULL DROP TABLE DanToc;
IF OBJECT_ID(N'ChucVu', N'U') IS NOT NULL DROP TABLE ChucVu;
IF OBJECT_ID(N'MonHoc', N'U') IS NOT NULL DROP TABLE MonHoc;
IF OBJECT_ID(N'Khoa', N'U') IS NOT NULL DROP TABLE Khoa;
GO

/* ---------- Xóa thủ tục / hàm cũ để chạy lại script không bị lỗi "đã tồn tại" ---------- */
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql + N'DROP PROCEDURE ' + QUOTENAME(name) + N';' FROM sys.procedures WHERE name LIKE 'sp[_]%';
SELECT @sql = @sql + N'DROP FUNCTION ' + QUOTENAME(name) + N';' FROM sys.objects WHERE name = 'fn_XepLoai';
EXEC sp_executesql @sql;
GO

/* ============================ BẢNG ============================ */
CREATE TABLE Khoa (
    MaKhoa    VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenKhoa   NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15),
    DiaChi    NVARCHAR(200),
    Website   VARCHAR(100)
);
GO

CREATE TABLE MonHoc (
    MaMon  VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    DVHT   INT           NOT NULL DEFAULT 3 CHECK (DVHT > 0)
);
GO

CREATE TABLE ChucVu (
    MaChucVu  VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenChucVu NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE DanToc (
    MaDanToc  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenDanToc NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Que (
    MaQue  VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenQue NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE HeDaoTao (
    MaHeDT  VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenHeDT NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE PhongHoc (
    MaPhong  VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenPhong NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Khoa_ChuyenNganh (
    MaChuyenNganh  VARCHAR(10)   NOT NULL PRIMARY KEY,
    MaKhoa         VARCHAR(10)   NOT NULL,
    TenChuyenNganh NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_KCN_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);
GO

/* Yêu cầu 1: Sĩ số ban đầu = 0 (DEFAULT 0, không cho âm) */
CREATE TABLE Lop (
    MaLop   VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenLop  NVARCHAR(100) NOT NULL,
    MaKhoa  VARCHAR(10)   NOT NULL,
    KhoaHoc VARCHAR(20),
    SiSo    INT NOT NULL CONSTRAINT DF_Lop_SiSo DEFAULT 0 CONSTRAINT CK_Lop_SiSo CHECK (SiSo >= 0),
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);
GO

CREATE TABLE SinhVien (
    MaSV          VARCHAR(10)   NOT NULL PRIMARY KEY,
    TenSV         NVARCHAR(100) NOT NULL,
    MaKhoa        VARCHAR(10),
    MaLop         VARCHAR(10),
    NgaySinh      DATE,
    GioiTinh      NVARCHAR(5),
    MaQue         VARCHAR(10),
    MaDanToc      VARCHAR(10),
    MaChuyenNganh VARCHAR(10),
    MaHeDT        VARCHAR(10),
    MaChucVu      VARCHAR(10),
    CONSTRAINT FK_SV_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa),
    CONSTRAINT FK_SV_Lop  FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    CONSTRAINT FK_SV_Que FOREIGN KEY (MaQue) REFERENCES Que(MaQue),
    CONSTRAINT FK_SV_DanToc FOREIGN KEY (MaDanToc) REFERENCES DanToc(MaDanToc),
    CONSTRAINT FK_SV_ChuyenNganh FOREIGN KEY (MaChuyenNganh) REFERENCES Khoa_ChuyenNganh(MaChuyenNganh),
    CONSTRAINT FK_SV_HeDT FOREIGN KEY (MaHeDT) REFERENCES HeDaoTao(MaHeDT),
    CONSTRAINT FK_SV_ChucVu FOREIGN KEY (MaChucVu) REFERENCES ChucVu(MaChucVu)
);
GO

CREATE TABLE TaiKhoan (
    TenDangNhap  VARCHAR(50)   NOT NULL PRIMARY KEY,
    MatKhau      VARCHAR(255)  NOT NULL,
    TrangThai    BIT           NOT NULL DEFAULT 1,
    CauHoiBaoMat NVARCHAR(200) NULL,
    DapAnBaoMat  VARCHAR(255)  NULL
);
GO

/* Khóa chính có thêm LanThi để lưu được điểm thi lần 1, lần 2 */
CREATE TABLE Diem (
    MaSV   VARCHAR(10) NOT NULL,
    MaLop  VARCHAR(10) NOT NULL,
    MaMon  VARCHAR(10) NOT NULL,
    HocKy  INT         NOT NULL,
    LanThi INT         NOT NULL DEFAULT 1 CHECK (LanThi BETWEEN 1 AND 3),
    Diem   DECIMAL(4,2) CHECK (Diem BETWEEN 0 AND 10),
    CONSTRAINT PK_Diem PRIMARY KEY (MaSV, MaLop, MaMon, LanThi),
    CONSTRAINT FK_Diem_SV FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    CONSTRAINT FK_Diem_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    CONSTRAINT FK_Diem_Mon FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon)
);
GO

CREATE TABLE ThoiKhoaBieu (
    MaLop   VARCHAR(10) NOT NULL,
    MaMon   VARCHAR(10) NOT NULL,
    HocKy   INT NOT NULL CHECK (HocKy BETWEEN 1 AND 10),
    ThuHoc  INT NOT NULL CHECK (ThuHoc BETWEEN 2 AND 8),   -- 8 = Chủ nhật
    CaHoc   INT NOT NULL CHECK (CaHoc BETWEEN 1 AND 4),
    MaPhong VARCHAR(10),
    CONSTRAINT PK_TKB PRIMARY KEY (MaLop, MaMon),
    CONSTRAINT FK_TKB_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    CONSTRAINT FK_TKB_Mon FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    CONSTRAINT FK_TKB_Phong FOREIGN KEY (MaPhong) REFERENCES PhongHoc(MaPhong)
);
GO

/* ===================== TRIGGER: YÊU CẦU 3 =====================
   Mỗi khi thêm / sửa (đổi lớp) / xóa sinh viên thì tính lại Sĩ số
   của các lớp bị ảnh hưởng. */
CREATE TRIGGER trg_SinhVien_CapNhatSiSo
ON SinhVien
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE l
    SET SiSo = (SELECT COUNT(*) FROM SinhVien s WHERE s.MaLop = l.MaLop)
    FROM Lop l
    WHERE l.MaLop IN (SELECT MaLop FROM inserted
                      UNION
                      SELECT MaLop FROM deleted);
END
GO

/* ===================== HÀM DÙNG CHUNG ===================== */
CREATE OR ALTER FUNCTION fn_XepLoai (@Diem DECIMAL(4,2))
RETURNS NVARCHAR(20)
AS
BEGIN
    RETURN CASE
        WHEN @Diem IS NULL THEN N''
        WHEN @Diem >= 9 THEN N'Xuất sắc'
        WHEN @Diem >= 8 THEN N'Giỏi'
        WHEN @Diem >= 7 THEN N'Khá'
        WHEN @Diem >= 5 THEN N'Trung bình'
        WHEN @Diem >= 4 THEN N'Yếu'
        ELSE N'Kém'
    END;
END
GO

/* ===================== THỦ TỤC: DANH MỤC ===================== */
-- Yêu cầu 2: chọn khoa -> chỉ hiện các lớp thuộc khoa đó
CREATE PROCEDURE sp_Lop_LayTheoKhoa
    @MaKhoa VARCHAR(10) = NULL
AS
BEGIN
    SELECT l.MaLop, l.TenLop, l.MaKhoa, k.TenKhoa, l.KhoaHoc, l.SiSo
    FROM Lop l JOIN Khoa k ON k.MaKhoa = l.MaKhoa
    WHERE @MaKhoa IS NULL OR l.MaKhoa = @MaKhoa
    ORDER BY l.MaLop;
END
GO

-- Yêu cầu 4: chọn khoa -> chỉ hiện chuyên ngành của khoa đó
CREATE PROCEDURE sp_ChuyenNganh_LayTheoKhoa
    @MaKhoa VARCHAR(10) = NULL
AS
BEGIN
    SELECT MaChuyenNganh, TenChuyenNganh, MaKhoa
    FROM Khoa_ChuyenNganh
    WHERE @MaKhoa IS NULL OR MaKhoa = @MaKhoa
    ORDER BY TenChuyenNganh;
END
GO

/* ===================== THỦ TỤC: SINH VIÊN ===================== */
-- Yêu cầu 3 + 6: lọc theo lớp / tìm kiếm theo quê, chuyên ngành, khoa
CREATE PROCEDURE sp_SinhVien_TimKiem
    @MaKhoa        VARCHAR(10)   = NULL,
    @MaLop         VARCHAR(10)   = NULL,
    @MaChuyenNganh VARCHAR(10)   = NULL,
    @MaQue         VARCHAR(10)   = NULL,
    @TuKhoa        NVARCHAR(100) = NULL
AS
BEGIN
    SELECT sv.MaSV, sv.TenSV, sv.NgaySinh, sv.GioiTinh,
           sv.MaLop, l.TenLop, sv.MaKhoa, k.TenKhoa,
           sv.MaChuyenNganh, cn.TenChuyenNganh,
           sv.MaQue, q.TenQue, dt.TenDanToc, h.TenHeDT, cv.TenChucVu
    FROM SinhVien sv
    LEFT JOIN Lop l               ON l.MaLop = sv.MaLop
    LEFT JOIN Khoa k              ON k.MaKhoa = sv.MaKhoa
    LEFT JOIN Khoa_ChuyenNganh cn ON cn.MaChuyenNganh = sv.MaChuyenNganh
    LEFT JOIN Que q               ON q.MaQue = sv.MaQue
    LEFT JOIN DanToc dt           ON dt.MaDanToc = sv.MaDanToc
    LEFT JOIN HeDaoTao h          ON h.MaHeDT = sv.MaHeDT
    LEFT JOIN ChucVu cv           ON cv.MaChucVu = sv.MaChucVu
    WHERE (@MaKhoa IS NULL OR sv.MaKhoa = @MaKhoa)
      AND (@MaLop IS NULL OR sv.MaLop = @MaLop)
      AND (@MaChuyenNganh IS NULL OR sv.MaChuyenNganh = @MaChuyenNganh)
      AND (@MaQue IS NULL OR sv.MaQue = @MaQue)
      AND (@TuKhoa IS NULL OR sv.MaSV LIKE '%' + @TuKhoa + '%' OR sv.TenSV LIKE N'%' + @TuKhoa + N'%'
           OR q.TenQue LIKE N'%' + @TuKhoa + N'%')   -- gõ tên quê vào ô tìm kiếm cũng tìm được
    ORDER BY sv.MaLop, sv.MaSV;
END
GO

CREATE PROCEDURE sp_SinhVien_Them
    @MaSV VARCHAR(10), @TenSV NVARCHAR(100), @MaKhoa VARCHAR(10), @MaLop VARCHAR(10),
    @NgaySinh DATE, @GioiTinh NVARCHAR(5), @MaQue VARCHAR(10), @MaDanToc VARCHAR(10),
    @MaChuyenNganh VARCHAR(10), @MaHeDT VARCHAR(10), @MaChucVu VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM SinhVien WHERE MaSV = @MaSV)
    BEGIN RAISERROR(N'Mã sinh viên đã tồn tại.', 16, 1); RETURN; END
    IF NOT EXISTS (SELECT 1 FROM Lop WHERE MaLop = @MaLop AND MaKhoa = @MaKhoa)
    BEGIN RAISERROR(N'Lớp không thuộc khoa đã chọn.', 16, 1); RETURN; END
    IF @MaChuyenNganh IS NOT NULL AND NOT EXISTS
        (SELECT 1 FROM Khoa_ChuyenNganh WHERE MaChuyenNganh = @MaChuyenNganh AND MaKhoa = @MaKhoa)
    BEGIN RAISERROR(N'Chuyên ngành không thuộc khoa đã chọn.', 16, 1); RETURN; END

    INSERT INTO SinhVien (MaSV, TenSV, MaKhoa, MaLop, NgaySinh, GioiTinh, MaQue,
                          MaDanToc, MaChuyenNganh, MaHeDT, MaChucVu)
    VALUES (@MaSV, @TenSV, @MaKhoa, @MaLop, @NgaySinh, @GioiTinh, @MaQue,
            @MaDanToc, @MaChuyenNganh, @MaHeDT, @MaChucVu);
END
GO

CREATE PROCEDURE sp_SinhVien_Sua
    @MaSV VARCHAR(10), @TenSV NVARCHAR(100), @MaKhoa VARCHAR(10), @MaLop VARCHAR(10),
    @NgaySinh DATE, @GioiTinh NVARCHAR(5), @MaQue VARCHAR(10), @MaDanToc VARCHAR(10),
    @MaChuyenNganh VARCHAR(10), @MaHeDT VARCHAR(10), @MaChucVu VARCHAR(10)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Lop WHERE MaLop = @MaLop AND MaKhoa = @MaKhoa)
    BEGIN RAISERROR(N'Lớp không thuộc khoa đã chọn.', 16, 1); RETURN; END
    IF @MaChuyenNganh IS NOT NULL AND NOT EXISTS
        (SELECT 1 FROM Khoa_ChuyenNganh WHERE MaChuyenNganh = @MaChuyenNganh AND MaKhoa = @MaKhoa)
    BEGIN RAISERROR(N'Chuyên ngành không thuộc khoa đã chọn.', 16, 1); RETURN; END

    UPDATE SinhVien
    SET TenSV = @TenSV, MaKhoa = @MaKhoa, MaLop = @MaLop, NgaySinh = @NgaySinh,
        GioiTinh = @GioiTinh, MaQue = @MaQue, MaDanToc = @MaDanToc,
        MaChuyenNganh = @MaChuyenNganh, MaHeDT = @MaHeDT, MaChucVu = @MaChucVu
    WHERE MaSV = @MaSV;
END
GO

CREATE  PROCEDURE sp_SinhVien_Xoa
    @MaSV VARCHAR(10)
AS
BEGIN
    BEGIN TRAN;
        DELETE FROM Diem WHERE MaSV = @MaSV;
        DELETE FROM SinhVien WHERE MaSV = @MaSV;
    COMMIT;
END
GO

CREATE PROCEDURE sp_SinhVien_LayThongTin
    @MaSV VARCHAR(10)
AS
BEGIN
    SELECT sv.MaSV, sv.TenSV, sv.NgaySinh, sv.GioiTinh, sv.MaLop, l.TenLop,
           sv.MaKhoa, k.TenKhoa, cn.TenChuyenNganh
    FROM SinhVien sv
    LEFT JOIN Lop l ON l.MaLop = sv.MaLop
    LEFT JOIN Khoa k ON k.MaKhoa = sv.MaKhoa
    LEFT JOIN Khoa_ChuyenNganh cn ON cn.MaChuyenNganh = sv.MaChuyenNganh
    WHERE sv.MaSV = @MaSV;
END
GO

/* ===================== THỦ TỤC: LỚP ===================== */
CREATE PROCEDURE sp_Lop_Them
    @MaLop VARCHAR(10), @TenLop NVARCHAR(100), @MaKhoa VARCHAR(10), @KhoaHoc VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Lop WHERE MaLop = @MaLop)
    BEGIN RAISERROR(N'Mã lớp đã tồn tại.', 16, 1); RETURN; END
    -- Yêu cầu 1: lớp mới luôn có Sĩ số = 0
    INSERT INTO Lop (MaLop, TenLop, MaKhoa, KhoaHoc, SiSo)
    VALUES (@MaLop, @TenLop, @MaKhoa, @KhoaHoc, 0);
END
GO

CREATE PROCEDURE sp_Lop_Sua
    @MaLop VARCHAR(10), @TenLop NVARCHAR(100), @MaKhoa VARCHAR(10), @KhoaHoc VARCHAR(20)
AS
BEGIN
    IF @MaKhoa <> (SELECT MaKhoa FROM Lop WHERE MaLop = @MaLop)
       AND EXISTS (SELECT 1 FROM SinhVien WHERE MaLop = @MaLop)
    BEGIN RAISERROR(N'Lớp đã có sinh viên, không thể chuyển sang khoa khác.', 16, 1); RETURN; END
    UPDATE Lop SET TenLop = @TenLop, MaKhoa = @MaKhoa, KhoaHoc = @KhoaHoc
    WHERE MaLop = @MaLop;
END
GO

CREATE PROCEDURE sp_Lop_Xoa
    @MaLop VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM SinhVien WHERE MaLop = @MaLop)
    BEGIN RAISERROR(N'Lớp đang có sinh viên, không thể xóa.', 16, 1); RETURN; END
    BEGIN TRAN;
        DELETE FROM Diem WHERE MaLop = @MaLop;
        DELETE FROM ThoiKhoaBieu WHERE MaLop = @MaLop;
        DELETE FROM Lop WHERE MaLop = @MaLop;
    COMMIT;
END
GO

/* ===================== THỦ TỤC: MÔN HỌC ===================== */
CREATE PROCEDURE sp_MonHoc_Them
    @MaMon VARCHAR(10), @TenMon NVARCHAR(100), @DVHT INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM MonHoc WHERE MaMon = @MaMon)
    BEGIN RAISERROR(N'Mã môn đã tồn tại.', 16, 1); RETURN; END
    INSERT INTO MonHoc (MaMon, TenMon, DVHT) VALUES (@MaMon, @TenMon, @DVHT);
END
GO

CREATE PROCEDURE sp_MonHoc_Sua
    @MaMon VARCHAR(10), @TenMon NVARCHAR(100), @DVHT INT
AS
BEGIN
    UPDATE MonHoc SET TenMon = @TenMon, DVHT = @DVHT WHERE MaMon = @MaMon;
END
GO

CREATE PROCEDURE sp_MonHoc_Xoa
    @MaMon VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Diem WHERE MaMon = @MaMon)
    BEGIN RAISERROR(N'Môn học đã có điểm, không thể xóa.', 16, 1); RETURN; END
    BEGIN TRAN;
        DELETE FROM ThoiKhoaBieu WHERE MaMon = @MaMon;
        DELETE FROM MonHoc WHERE MaMon = @MaMon;
    COMMIT;
END
GO

/* ===================== THỦ TỤC: THỜI KHÓA BIỂU (LỚP HỌC PHẦN) ===================== */
CREATE PROCEDURE sp_TKB_LayDanhSach
    @HocKy INT = NULL, @MaLop VARCHAR(10) = NULL, @MaMon VARCHAR(10) = NULL,
    @MaKhoa VARCHAR(10) = NULL,
    @TuKhoa NVARCHAR(100) = NULL
AS
BEGIN
    SELECT t.MaLop, l.TenLop, t.MaMon, m.TenMon, m.DVHT, t.HocKy, t.ThuHoc,
           CASE WHEN t.ThuHoc = 8 THEN N'Chủ nhật' ELSE N'Thứ ' + CAST(t.ThuHoc AS NVARCHAR(2)) END AS TenThu,
           t.CaHoc, t.MaPhong, p.TenPhong,
           (SELECT COUNT(*) FROM Diem d WHERE d.MaLop = t.MaLop AND d.MaMon = t.MaMon AND d.LanThi = 1) AS SoDiem,
           l.SiSo
    FROM ThoiKhoaBieu t
    JOIN Lop l ON l.MaLop = t.MaLop
    JOIN MonHoc m ON m.MaMon = t.MaMon
    LEFT JOIN PhongHoc p ON p.MaPhong = t.MaPhong
    WHERE (@HocKy IS NULL OR t.HocKy = @HocKy)
      AND (@MaLop IS NULL OR t.MaLop = @MaLop)
      AND (@MaMon IS NULL OR t.MaMon = @MaMon)
      AND (@MaKhoa IS NULL OR l.MaKhoa = @MaKhoa)
      AND (@TuKhoa IS NULL OR m.TenMon LIKE N'%' + @TuKhoa + N'%' OR t.MaMon LIKE '%' + @TuKhoa + '%'
           OR l.TenLop LIKE N'%' + @TuKhoa + N'%')
    ORDER BY t.HocKy, t.MaLop, t.ThuHoc, t.CaHoc;
END
GO

CREATE PROCEDURE sp_TKB_Them
    @MaLop VARCHAR(10), @MaMon VARCHAR(10), @HocKy INT, @ThuHoc INT, @CaHoc INT, @MaPhong VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM ThoiKhoaBieu WHERE MaLop = @MaLop AND MaMon = @MaMon)
    BEGIN RAISERROR(N'Lớp này đã có môn học này trong thời khóa biểu.', 16, 1); RETURN; END
    IF EXISTS (SELECT 1 FROM ThoiKhoaBieu WHERE MaLop = @MaLop AND HocKy = @HocKy
               AND ThuHoc = @ThuHoc AND CaHoc = @CaHoc)
    BEGIN RAISERROR(N'Lớp đã có môn khác học vào thứ/ca này.', 16, 1); RETURN; END
    INSERT INTO ThoiKhoaBieu (MaLop, MaMon, HocKy, ThuHoc, CaHoc, MaPhong)
    VALUES (@MaLop, @MaMon, @HocKy, @ThuHoc, @CaHoc, @MaPhong);
END
GO

CREATE PROCEDURE sp_TKB_Sua
    @MaLop VARCHAR(10), @MaMon VARCHAR(10), @HocKy INT, @ThuHoc INT, @CaHoc INT, @MaPhong VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM ThoiKhoaBieu WHERE MaLop = @MaLop AND HocKy = @HocKy
               AND ThuHoc = @ThuHoc AND CaHoc = @CaHoc AND MaMon <> @MaMon)
    BEGIN RAISERROR(N'Lớp đã có môn khác học vào thứ/ca này.', 16, 1); RETURN; END
    UPDATE ThoiKhoaBieu
    SET HocKy = @HocKy, ThuHoc = @ThuHoc, CaHoc = @CaHoc, MaPhong = @MaPhong
    WHERE MaLop = @MaLop AND MaMon = @MaMon;
END
GO

CREATE PROCEDURE sp_TKB_Xoa
    @MaLop VARCHAR(10), @MaMon VARCHAR(10)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Diem WHERE MaLop = @MaLop AND MaMon = @MaMon)
    BEGIN RAISERROR(N'Lớp học phần đã có điểm, không thể xóa.', 16, 1); RETURN; END
    DELETE FROM ThoiKhoaBieu WHERE MaLop = @MaLop AND MaMon = @MaMon;
END
GO

-- Yêu cầu 5: chọn lớp + học kỳ -> chỉ hiện các môn theo thời khóa biểu của lớp đó
CREATE  PROCEDURE sp_TKB_LayMonTheoLopHocKy
    @MaLop VARCHAR(10), @HocKy INT
AS
BEGIN
    SELECT t.MaMon, m.TenMon, m.DVHT
    FROM ThoiKhoaBieu t JOIN MonHoc m ON m.MaMon = t.MaMon
    WHERE t.MaLop = @MaLop AND t.HocKy = @HocKy
    ORDER BY m.TenMon;
END
GO

/* ===================== THỦ TỤC: ĐIỂM ===================== */
-- Yêu cầu 5: toàn bộ sinh viên của lớp + điểm (nếu có) để nhập trực tiếp trên lưới
CREATE  PROCEDURE sp_Diem_LayDanhSachNhap
    @MaLop VARCHAR(10), @HocKy INT, @MaMon VARCHAR(10), @LanThi INT
AS
BEGIN
    SELECT sv.MaSV, sv.TenSV, sv.NgaySinh, d.Diem,
           dbo.fn_XepLoai(d.Diem) AS XepLoai
    FROM SinhVien sv
    LEFT JOIN Diem d ON d.MaSV = sv.MaSV AND d.MaLop = @MaLop
                    AND d.MaMon = @MaMon AND d.LanThi = @LanThi
    WHERE sv.MaLop = @MaLop
    ORDER BY sv.MaSV;
END
GO

-- Thêm mới hoặc cập nhật điểm (upsert)
CREATE PROCEDURE sp_Diem_Luu
    @MaSV VARCHAR(10), @MaLop VARCHAR(10), @MaMon VARCHAR(10),
    @HocKy INT, @LanThi INT, @Diem DECIMAL(4,2)
AS
BEGIN
    IF @LanThi > 1 AND NOT EXISTS (SELECT 1 FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop
                                   AND MaMon = @MaMon AND LanThi = @LanThi - 1)
    BEGIN RAISERROR(N'Sinh viên %s chưa có điểm lần thi trước.', 16, 1, @MaSV); RETURN; END

    IF EXISTS (SELECT 1 FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop AND MaMon = @MaMon AND LanThi = @LanThi)
        UPDATE Diem SET Diem = @Diem, HocKy = @HocKy
        WHERE MaSV = @MaSV AND MaLop = @MaLop AND MaMon = @MaMon AND LanThi = @LanThi;
    ELSE
        INSERT INTO Diem (MaSV, MaLop, MaMon, HocKy, LanThi, Diem)
        VALUES (@MaSV, @MaLop, @MaMon, @HocKy, @LanThi, @Diem);
END
GO

CREATE PROCEDURE sp_Diem_Xoa
    @MaSV VARCHAR(10), @MaLop VARCHAR(10), @MaMon VARCHAR(10), @LanThi INT
AS
BEGIN
    DELETE FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop AND MaMon = @MaMon AND LanThi >= @LanThi;
END
GO

/* ===================== THỦ TỤC: BÁO CÁO ===================== */
-- Yêu cầu 7: danh sách điểm theo lớp, môn, lần thi
CREATE PROCEDURE sp_BaoCao_DiemTheoLopMon
    @MaLop VARCHAR(10), @MaMon VARCHAR(10), @LanThi INT
AS
BEGIN
    SELECT l.MaLop, l.TenLop, m.MaMon, m.TenMon,
           ISNULL((SELECT TOP 1 HocKy FROM ThoiKhoaBieu t WHERE t.MaLop = l.MaLop AND t.MaMon = m.MaMon), 0) AS HocKy,
           @LanThi AS LanThi,
           CAST(ROW_NUMBER() OVER (ORDER BY sv.MaSV) AS INT) AS STT,
           sv.MaSV, sv.TenSV, sv.NgaySinh, d.Diem,
           dbo.fn_XepLoai(d.Diem) AS XepLoai
    FROM SinhVien sv
    JOIN Lop l ON l.MaLop = sv.MaLop
    CROSS JOIN MonHoc m
    LEFT JOIN Diem d ON d.MaSV = sv.MaSV AND d.MaLop = l.MaLop AND d.MaMon = m.MaMon AND d.LanThi = @LanThi
    WHERE sv.MaLop = @MaLop AND m.MaMon = @MaMon
    ORDER BY sv.MaSV;
END
GO

-- Yêu cầu 8: bảng điểm các môn theo mã sinh viên
CREATE PROCEDURE sp_BaoCao_BangDiemSV
    @MaSV VARCHAR(10), @HocKy INT = NULL
AS
BEGIN
    SELECT sv.MaSV, sv.TenSV, sv.NgaySinh, l.TenLop, k.TenKhoa, cn.TenChuyenNganh,
           d.HocKy, m.MaMon, m.TenMon, m.DVHT, d.LanThi, d.Diem,
           dbo.fn_XepLoai(d.Diem) AS XepLoai
    FROM Diem d
    JOIN SinhVien sv ON sv.MaSV = d.MaSV
    JOIN MonHoc m ON m.MaMon = d.MaMon
    LEFT JOIN Lop l ON l.MaLop = sv.MaLop
    LEFT JOIN Khoa k ON k.MaKhoa = sv.MaKhoa
    LEFT JOIN Khoa_ChuyenNganh cn ON cn.MaChuyenNganh = sv.MaChuyenNganh
    WHERE d.MaSV = @MaSV AND (@HocKy IS NULL OR d.HocKy = @HocKy)
    ORDER BY d.HocKy, m.TenMon, d.LanThi;
END
GO

-- Yêu cầu 9: thời khóa biểu của một lớp trong một học kỳ
CREATE PROCEDURE sp_BaoCao_ThoiKhoaBieu
    @MaLop VARCHAR(10), @HocKy INT
AS
BEGIN
    SELECT l.MaLop, l.TenLop, t.HocKy, t.ThuHoc,
           CASE WHEN t.ThuHoc = 8 THEN N'Chủ nhật' ELSE N'Thứ ' + CAST(t.ThuHoc AS NVARCHAR(2)) END AS TenThu,
           t.CaHoc,
           CASE t.CaHoc WHEN 1 THEN N'Tiết 1-3 (7h00)' WHEN 2 THEN N'Tiết 4-6 (9h35)'
                        WHEN 3 THEN N'Tiết 7-9 (13h00)' ELSE N'Tiết 10-12 (15h35)' END AS TietHoc,
           m.MaMon, m.TenMon, m.DVHT, ISNULL(p.TenPhong, t.MaPhong) AS TenPhong
    FROM ThoiKhoaBieu t
    JOIN Lop l ON l.MaLop = t.MaLop
    JOIN MonHoc m ON m.MaMon = t.MaMon
    LEFT JOIN PhongHoc p ON p.MaPhong = t.MaPhong
    WHERE t.MaLop = @MaLop AND t.HocKy = @HocKy
    ORDER BY t.ThuHoc, t.CaHoc;
END
GO

/* ===================== THỦ TỤC: THỐNG KÊ ===================== */
CREATE PROCEDURE sp_ThongKe_SVTheoKhoa
AS
BEGIN
    SELECT k.TenKhoa, l.MaLop, l.TenLop, l.SiSo,
           (SELECT COUNT(*) FROM SinhVien s WHERE s.MaLop = l.MaLop AND s.GioiTinh = N'Nam') AS SoNam,
           (SELECT COUNT(*) FROM SinhVien s WHERE s.MaLop = l.MaLop AND s.GioiTinh = N'Nữ') AS SoNu
    FROM Lop l JOIN Khoa k ON k.MaKhoa = l.MaKhoa
    ORDER BY k.TenKhoa, l.MaLop;
END
GO

CREATE PROCEDURE sp_ThongKe_DiemTBTheoMon
    @MaLop VARCHAR(10) = NULL
AS
BEGIN
    SELECT m.TenMon, CAST(AVG(d.Diem) AS DECIMAL(4,2)) AS DiemTB, COUNT(*) AS SoLuot
    FROM Diem d JOIN MonHoc m ON m.MaMon = d.MaMon
    WHERE d.LanThi = 1 AND (@MaLop IS NULL OR d.MaLop = @MaLop)
    GROUP BY m.TenMon
    ORDER BY m.TenMon;
END
GO

CREATE PROCEDURE sp_ThongKe_XepLoai
    @MaLop VARCHAR(10) = NULL
AS
BEGIN
    SELECT dbo.fn_XepLoai(Diem) AS KetQua, COUNT(*) AS SoLuong, MIN(Diem) AS DiemThap
    FROM Diem
    WHERE LanThi = 1 AND (@MaLop IS NULL OR MaLop = @MaLop)
    GROUP BY dbo.fn_XepLoai(Diem)
    ORDER BY MIN(Diem) DESC;
END
GO

/* ===================== DỮ LIỆU MẪU ===================== */
INSERT INTO Khoa (MaKhoa, TenKhoa, DienThoai, DiaChi, Website) VALUES
('CNTT', N'Công nghệ thông tin', '0241234567', N'Nhà A1', 'https://cntt.example.edu.vn'),
('KT',   N'Kinh tế',             '0241234568', N'Nhà B2', 'https://kinhte.example.edu.vn');
GO

INSERT INTO MonHoc (MaMon, TenMon, DVHT) VALUES
('CSDL',  N'Cơ sở dữ liệu', 3),
('JAVA',  N'Lập trình Java', 3),
('CTDL',  N'Cấu trúc dữ liệu và giải thuật', 3),
('WEB',   N'Lập trình Web', 3),
('CNPM',  N'Công nghệ phần mềm', 3),
('HDH',   N'Hệ điều hành', 3),
('MMT',   N'Mạng máy tính', 3),
('OOP',   N'Lập trình hướng đối tượng', 3),
('TA1',   N'Tiếng Anh chuyên ngành 1', 2),
('TA2',   N'Tiếng Anh chuyên ngành 2', 2),
('THDC',  N'Tin học đại cương', 3),
('PTTK',  N'Phân tích và thiết kế hệ thống', 3),
('AI',    N'Trí tuệ nhân tạo', 3),
('LTNC',  N'Lập trình nâng cao', 3),
('KTLT',  N'Kỹ thuật lập trình', 3);
GO

INSERT INTO ChucVu (MaChucVu, TenChucVu) VALUES
('SV', N'Sinh viên'), ('LCT', N'Lớp trưởng'), ('LCP', N'Lớp phó');
GO

INSERT INTO DanToc (MaDanToc, TenDanToc) VALUES
('DT01', N'Kinh'), ('DT02', N'Tày'), ('DT03', N'Mường'), ('DT04', N'Thái');
GO

INSERT INTO Que (MaQue, TenQue) VALUES
('HN', N'Hà Nội'), ('HP', N'Hải Phòng'), ('NB', N'Ninh Bình'),
('ND', N'Nam Định'), ('TB', N'Thái Bình'), ('LS', N'Lạng Sơn');
GO

INSERT INTO HeDaoTao (MaHeDT, TenHeDT) VALUES
('CQ', N'Chính quy'), ('CLC', N'Chất lượng cao');
GO

INSERT INTO PhongHoc (MaPhong, TenPhong) VALUES
('A101', N'Phòng A101'), ('A102', N'Phòng A102'), ('A103', N'Phòng A103'),
('B201', N'Phòng B201'), ('B202', N'Phòng B202'),
('Lab01', N'Phòng Máy 01'), ('Lab02', N'Phòng Máy 02');
GO

INSERT INTO Khoa_ChuyenNganh (MaChuyenNganh, MaKhoa, TenChuyenNganh) VALUES
('KTPM', 'CNTT', N'Kỹ thuật phần mềm'),
('HTTT', 'CNTT', N'Hệ thống thông tin'),
('KHMT', 'CNTT', N'Khoa học máy tính'),
('QTKD', 'KT',   N'Quản trị kinh doanh'),
('KTOAN','KT',   N'Kế toán');
GO

/* Sĩ số khởi tạo = 0, trigger sẽ tự tính lại khi thêm sinh viên */
INSERT INTO Lop (MaLop, TenLop, MaKhoa, KhoaHoc, SiSo) VALUES
('CNTT01', N'Công nghệ thông tin 01', 'CNTT', 'K2025', 0),
('CNTT02', N'Công nghệ thông tin 02', 'CNTT', 'K2025', 0),
('CNTT03', N'Công nghệ thông tin 03', 'CNTT', 'K2025', 0),
('CNTT04', N'Công nghệ thông tin 04', 'CNTT', 'K2026', 0),
('CNTT05', N'Công nghệ thông tin 05', 'CNTT', 'K2026', 0),
('KT01',   N'Kinh tế 01',             'KT',   'K2025', 0);
GO

INSERT INTO SinhVien (MaSV, TenSV, MaKhoa, MaLop, NgaySinh, GioiTinh, MaQue, MaDanToc, MaChuyenNganh, MaHeDT, MaChucVu) VALUES
('SV001', N'Nguyễn Văn An', 'CNTT', 'CNTT01', '2005-02-15', N'Nam', 'HN', 'DT01', 'KTPM', 'CQ', 'LCT'),
('SV002', N'Trần Thị Minh Anh', 'CNTT', 'CNTT01', '2005-08-21', N'Nữ', 'HP', 'DT01', 'HTTT', 'CQ', 'LCP'),
('SV003', N'Lê Hoàng Nam', 'CNTT', 'CNTT01', '2004-11-02', N'Nam', 'NB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV004', N'Phạm Ngọc Mai', 'CNTT', 'CNTT01', '2005-04-18', N'Nữ', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV005', N'Đỗ Đức Anh', 'CNTT', 'CNTT01', '2005-01-27', N'Nam', 'ND', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV006', N'Hoàng Thị Lan', 'CNTT', 'CNTT01', '2005-07-09', N'Nữ', 'TB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV007', N'Vũ Minh Đức', 'CNTT', 'CNTT01', '2004-12-11', N'Nam', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV008', N'Bùi Thị Hương', 'CNTT', 'CNTT01', '2005-03-25', N'Nữ', 'HP', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV009', N'Đặng Quốc Việt', 'CNTT', 'CNTT01', '2005-09-14', N'Nam', 'NB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV010', N'Nguyễn Thị Ngọc', 'CNTT', 'CNTT01', '2005-06-30', N'Nữ', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV011', N'Trịnh Văn Bình', 'CNTT', 'CNTT02', '2005-02-07', N'Nam', 'ND', 'DT02', 'HTTT', 'CQ', 'LCT'),
('SV012', N'Phan Thị Thu', 'CNTT', 'CNTT02', '2005-10-19', N'Nữ', 'TB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV013', N'Nguyễn Đức Long', 'CNTT', 'CNTT02', '2004-05-23', N'Nam', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV014', N'Lý Thị Hoa', 'CNTT', 'CNTT02', '2005-11-05', N'Nữ', 'LS', 'DT02', 'HTTT', 'CQ', 'SV'),
('SV015', N'Trần Quốc Huy', 'CNTT', 'CNTT02', '2005-01-16', N'Nam', 'HP', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV016', N'Phạm Thị Linh', 'CNTT', 'CNTT02', '2005-08-03', N'Nữ', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV017', N'Đinh Văn Khánh', 'CNTT', 'CNTT02', '2004-09-28', N'Nam', 'NB', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV018', N'Mai Thị Phương', 'CNTT', 'CNTT02', '2005-12-20', N'Nữ', 'ND', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV019', N'Ngô Tuấn Kiệt', 'CNTT', 'CNTT02', '2005-04-12', N'Nam', 'TB', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV020', N'Võ Thị Thanh', 'CNTT', 'CNTT02', '2005-07-26', N'Nữ', 'HN', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV021', N'Nguyễn Hoàng Sơn', 'CNTT', 'CNTT03', '2004-03-09', N'Nam', 'HP', 'DT01', 'KHMT', 'CQ', 'LCT'),
('SV022', N'Trần Ngọc Hà', 'CNTT', 'CNTT03', '2005-05-17', N'Nữ', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV023', N'Lê Văn Dũng', 'CNTT', 'CNTT03', '2005-10-08', N'Nam', 'ND', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV024', N'Hoàng Thị Yến', 'CNTT', 'CNTT03', '2005-02-28', N'Nữ', 'TB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV025', N'Phạm Minh Quân', 'CNTT', 'CNTT03', '2004-12-03', N'Nam', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV026', N'Đỗ Thị Ngân', 'CNTT', 'CNTT03', '2005-06-14', N'Nữ', 'HP', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV027', N'Vũ Quốc Khải', 'CNTT', 'CNTT03', '2005-09-22', N'Nam', 'NB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV028', N'Bùi Minh Châu', 'CNTT', 'CNTT03', '2005-01-05', N'Nữ', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV029', N'Đặng Văn Tùng', 'CNTT', 'CNTT03', '2004-08-16', N'Nam', 'ND', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV030', N'Nguyễn Thùy Linh', 'CNTT', 'CNTT03', '2005-11-27', N'Nữ', 'TB', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV031', N'Phạm Quốc Bảo', 'CNTT', 'CNTT04', '2005-03-18', N'Nam', 'HN', 'DT01', 'KTPM', 'CLC', 'LCT'),
('SV032', N'Trần Thị Hạnh', 'CNTT', 'CNTT04', '2005-07-31', N'Nữ', 'HP', 'DT01', 'HTTT', 'CLC', 'SV'),
('SV033', N'Lê Minh Hoàng', 'CNTT', 'CNTT04', '2004-10-13', N'Nam', 'NB', 'DT01', 'KHMT', 'CLC', 'SV'),
('SV034', N'Nguyễn Khánh Ly', 'CNTT', 'CNTT04', '2005-05-06', N'Nữ', 'HN', 'DT01', 'KTPM', 'CLC', 'SV'),
('SV035', N'Hoàng Văn Thành', 'CNTT', 'CNTT04', '2005-12-09', N'Nam', 'ND', 'DT01', 'HTTT', 'CLC', 'SV'),
('SV036', N'Phan Ngọc Ánh', 'CNTT', 'CNTT04', '2005-02-19', N'Nữ', 'TB', 'DT01', 'KHMT', 'CLC', 'SV'),
('SV037', N'Đỗ Minh Khang', 'CNTT', 'CNTT04', '2004-06-25', N'Nam', 'HN', 'DT01', 'KTPM', 'CLC', 'SV'),
('SV038', N'Vũ Thị Hằng', 'CNTT', 'CNTT04', '2005-09-07', N'Nữ', 'HP', 'DT01', 'HTTT', 'CLC', 'SV'),
('SV039', N'Bùi Quốc Trung', 'CNTT', 'CNTT04', '2005-01-29', N'Nam', 'ND', 'DT01', 'KHMT', 'CLC', 'SV'),
('SV040', N'Trịnh Thị Mai', 'CNTT', 'CNTT04', '2005-04-04', N'Nữ', 'HN', 'DT01', 'KTPM', 'CLC', 'SV'),
('SV041', N'Nguyễn Thành Đạt', 'CNTT', 'CNTT05', '2004-11-18', N'Nam', 'TB', 'DT01', 'HTTT', 'CQ', 'LCT'),
('SV042', N'Trần Thị Kim Oanh', 'CNTT', 'CNTT05', '2005-08-12', N'Nữ', 'HN', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV043', N'Lê Quốc Tuấn', 'CNTT', 'CNTT05', '2005-03-03', N'Nam', 'HP', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV044', N'Phạm Thị Như Quỳnh', 'CNTT', 'CNTT05', '2005-10-24', N'Nữ', 'ND', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV045', N'Hoàng Minh Tâm', 'CNTT', 'CNTT05', '2004-07-15', N'Nam', 'HN', 'DT01', 'KHMT', 'CQ', 'SV'),
('SV046', N'Nguyễn Thị Vân', 'CNTT', 'CNTT05', '2005-05-29', N'Nữ', 'TB', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV047', N'Đặng Minh Nhật', 'CNTT', 'CNTT05', '2005-09-03', N'Nam', 'NB', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV048', N'Lý Ngọc Trâm', 'CNTT', 'CNTT05', '2005-01-12', N'Nữ', 'LS', 'DT02', 'KHMT', 'CQ', 'SV'),
('SV049', N'Võ Minh Phúc', 'CNTT', 'CNTT05', '2004-12-28', N'Nam', 'HN', 'DT01', 'KTPM', 'CQ', 'SV'),
('SV050', N'Mai Quốc Anh', 'CNTT', 'CNTT05', '2005-06-08', N'Nam', 'HP', 'DT01', 'HTTT', 'CQ', 'SV'),
('SV051', N'Đoàn Thị Hồng', 'KT', 'KT01', '2005-03-14', N'Nữ', 'HN', 'DT01', 'KTOAN', 'CQ', 'LCT'),
('SV052', N'Cao Văn Lực', 'KT', 'KT01', '2005-07-02', N'Nam', 'ND', 'DT03', 'QTKD', 'CQ', 'SV'),
('SV053', N'Tạ Thị Minh', 'KT', 'KT01', '2004-12-19', N'Nữ', 'TB', 'DT01', 'KTOAN', 'CQ', 'SV'),
('SV054', N'Lương Văn Hải', 'KT', 'KT01', '2005-05-25', N'Nam', 'LS', 'DT04', 'QTKD', 'CQ', 'SV'),
('SV055', N'Hà Thu Trang', 'KT', 'KT01', '2005-09-30', N'Nữ', 'HP', 'DT01', 'QTKD', 'CQ', 'SV');
GO

/* Tài khoản admin (mật khẩu đã băm PBKDF2 - giữ nguyên mật khẩu cũ của nhóm) */
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TrangThai, CauHoiBaoMat, DapAnBaoMat) VALUES
('admin', 'PBKDF2$100000$xlmzdtT0uqezhC9BG04wMw==$N0+e9SwUn9GHL//xp/twVaQluhPm7x6KT2uZUuVPZLg=', 1,
 N'Tên viết tắt của trường bạn là gì?',
 'PBKDF2$100000$+edF5gsLvflHZj/JQLdQhA==$B1ZLhb39foRQU7ojgMNw5MKdcYtNFflrieD/tGIHjO0=');
GO

INSERT INTO ThoiKhoaBieu (MaLop, MaMon, HocKy, ThuHoc, CaHoc, MaPhong) VALUES
('CNTT01','THDC',1,2,1,'A101'),
('CNTT01','KTLT',1,3,3,'A102'),
('CNTT01','CTDL',1,4,1,'A103'),
('CNTT01','OOP',1,5,3,'B201'),
('CNTT01','TA1',1,6,1,'B202'),
('CNTT01','CSDL',2,2,1,'A101'),
('CNTT01','JAVA',2,3,3,'A102'),
('CNTT01','WEB',2,4,1,'A103'),
('CNTT01','TA2',2,5,3,'B201'),
('CNTT01','HDH',2,6,1,'B202'),
('CNTT01','MMT',3,2,1,'A101'),
('CNTT01','CNPM',3,3,3,'A102'),
('CNTT01','PTTK',3,4,1,'A103'),
('CNTT01','AI',3,5,3,'B201'),
('CNTT01','LTNC',3,6,1,'B202'),
('CNTT02','THDC',1,3,2,'A103'),
('CNTT02','KTLT',1,4,4,'B201'),
('CNTT02','CTDL',1,5,2,'B202'),
('CNTT02','OOP',1,6,4,'Lab01'),
('CNTT02','TA1',1,7,2,'Lab02'),
('CNTT02','CSDL',2,3,2,'A103'),
('CNTT02','JAVA',2,4,4,'B201'),
('CNTT02','WEB',2,5,2,'B202'),
('CNTT02','TA2',2,6,4,'Lab01'),
('CNTT02','HDH',2,7,2,'Lab02'),
('CNTT02','MMT',3,3,2,'A103'),
('CNTT02','CNPM',3,4,4,'B201'),
('CNTT02','PTTK',3,5,2,'B202'),
('CNTT02','AI',3,6,4,'Lab01'),
('CNTT02','LTNC',3,7,2,'Lab02'),
('CNTT03','THDC',1,4,3,'B202'),
('CNTT03','KTLT',1,5,1,'Lab01'),
('CNTT03','CTDL',1,6,3,'Lab02'),
('CNTT03','OOP',1,7,1,'A101'),
('CNTT03','TA1',1,2,3,'A102'),
('CNTT03','CSDL',2,4,3,'B202'),
('CNTT03','JAVA',2,5,1,'Lab01'),
('CNTT03','WEB',2,6,3,'Lab02'),
('CNTT03','TA2',2,7,1,'A101'),
('CNTT03','HDH',2,2,3,'A102'),
('CNTT03','MMT',3,4,3,'B202'),
('CNTT03','CNPM',3,5,1,'Lab01'),
('CNTT03','PTTK',3,6,3,'Lab02'),
('CNTT03','AI',3,7,1,'A101'),
('CNTT03','LTNC',3,2,3,'A102'),
('CNTT04','THDC',1,5,4,'Lab02'),
('CNTT04','KTLT',1,6,2,'A101'),
('CNTT04','CTDL',1,7,4,'A102'),
('CNTT04','OOP',1,2,2,'A103'),
('CNTT04','TA1',1,3,4,'B201'),
('CNTT04','CSDL',2,5,4,'Lab02'),
('CNTT04','JAVA',2,6,2,'A101'),
('CNTT04','WEB',2,7,4,'A102'),
('CNTT04','TA2',2,2,2,'A103'),
('CNTT04','HDH',2,3,4,'B201'),
('CNTT04','MMT',3,5,4,'Lab02'),
('CNTT04','CNPM',3,6,2,'A101'),
('CNTT04','PTTK',3,7,4,'A102'),
('CNTT04','AI',3,2,2,'A103'),
('CNTT04','LTNC',3,3,4,'B201'),
('CNTT05','THDC',1,6,1,'A102'),
('CNTT05','KTLT',1,7,3,'A103'),
('CNTT05','CTDL',1,2,1,'B201'),
('CNTT05','OOP',1,3,3,'B202'),
('CNTT05','TA1',1,4,1,'Lab01'),
('CNTT05','CSDL',2,6,1,'A102'),
('CNTT05','JAVA',2,7,3,'A103'),
('CNTT05','WEB',2,2,1,'B201'),
('CNTT05','TA2',2,3,3,'B202'),
('CNTT05','HDH',2,4,1,'Lab01'),
('CNTT05','MMT',3,6,1,'A102'),
('CNTT05','CNPM',3,7,3,'A103'),
('CNTT05','PTTK',3,2,1,'B201'),
('CNTT05','AI',3,3,3,'B202'),
('CNTT05','LTNC',3,4,1,'Lab01'),
('KT01','THDC',1,2,1,'B201'),
('KT01','TA1',1,3,2,'B202'),
('KT01','KTLT',1,4,3,'B201'),
('KT01','TA2',2,2,1,'B201'),
('KT01','CSDL',2,3,2,'B202');
GO

INSERT INTO Diem (MaSV, MaLop, MaMon, HocKy, LanThi, Diem) VALUES
('SV001','CNTT01','THDC',1,1,8.0),
('SV002','CNTT01','THDC',1,1,4.0),
('SV003','CNTT01','THDC',1,1,6.0),
('SV004','CNTT01','THDC',1,1,7.0),
('SV005','CNTT01','THDC',1,1,9.5),
('SV006','CNTT01','THDC',1,1,7.0),
('SV007','CNTT01','THDC',1,1,4.5),
('SV008','CNTT01','THDC',1,1,7.5),
('SV009','CNTT01','THDC',1,1,5.0),
('SV010','CNTT01','THDC',1,1,9.0),
('SV001','CNTT01','KTLT',1,1,6.5),
('SV002','CNTT01','KTLT',1,1,10.0),
('SV003','CNTT01','KTLT',1,1,7.0),
('SV004','CNTT01','KTLT',1,1,5.5),
('SV005','CNTT01','KTLT',1,1,4.5),
('SV006','CNTT01','KTLT',1,1,6.5),
('SV007','CNTT01','KTLT',1,1,7.5),
('SV008','CNTT01','KTLT',1,1,9.0),
('SV009','CNTT01','KTLT',1,1,7.5),
('SV010','CNTT01','KTLT',1,1,6.0),
('SV001','CNTT01','CTDL',1,1,8.0),
('SV002','CNTT01','CTDL',1,1,4.5),
('SV003','CNTT01','CTDL',1,1,8.0),
('SV004','CNTT01','CTDL',1,1,6.0),
('SV005','CNTT01','CTDL',1,1,9.5),
('SV006','CNTT01','CTDL',1,1,8.5),
('SV007','CNTT01','CTDL',1,1,7.5),
('SV008','CNTT01','CTDL',1,1,5.5),
('SV009','CNTT01','CTDL',1,1,8.0),
('SV010','CNTT01','CTDL',1,1,7.5),
('SV001','CNTT01','OOP',1,1,6.0),
('SV002','CNTT01','OOP',1,1,6.0),
('SV003','CNTT01','OOP',1,1,9.0),
('SV004','CNTT01','OOP',1,1,6.5),
('SV005','CNTT01','OOP',1,1,7.5),
('SV006','CNTT01','OOP',1,1,9.0),
('SV007','CNTT01','OOP',1,1,4.5),
('SV008','CNTT01','OOP',1,1,10.0),
('SV009','CNTT01','OOP',1,1,8.5),
('SV010','CNTT01','OOP',1,1,6.0),
('SV001','CNTT01','TA1',1,1,5.5),
('SV002','CNTT01','TA1',1,1,5.0),
('SV003','CNTT01','TA1',1,1,8.5),
('SV004','CNTT01','TA1',1,1,3.0),
('SV004','CNTT01','TA1',1,2,6.0),
('SV005','CNTT01','TA1',1,1,8.5),
('SV006','CNTT01','TA1',1,1,7.5),
('SV007','CNTT01','TA1',1,1,6.0),
('SV008','CNTT01','TA1',1,1,6.0),
('SV009','CNTT01','TA1',1,1,6.5),
('SV010','CNTT01','TA1',1,1,6.5),
('SV001','CNTT01','CSDL',2,1,8.5),
('SV002','CNTT01','CSDL',2,1,5.5),
('SV003','CNTT01','CSDL',2,1,9.0),
('SV004','CNTT01','CSDL',2,1,8.5),
('SV005','CNTT01','CSDL',2,1,6.5),
('SV006','CNTT01','CSDL',2,1,8.0),
('SV007','CNTT01','CSDL',2,1,6.0),
('SV008','CNTT01','CSDL',2,1,7.0),
('SV009','CNTT01','CSDL',2,1,8.0),
('SV010','CNTT01','CSDL',2,1,10.0),
('SV001','CNTT01','JAVA',2,1,7.0),
('SV002','CNTT01','JAVA',2,1,7.0),
('SV003','CNTT01','JAVA',2,1,7.0),
('SV004','CNTT01','JAVA',2,1,6.5),
('SV005','CNTT01','JAVA',2,1,5.5),
('SV006','CNTT01','JAVA',2,1,10.0),
('SV007','CNTT01','JAVA',2,1,6.0),
('SV008','CNTT01','JAVA',2,1,6.0),
('SV009','CNTT01','JAVA',2,1,6.0),
('SV010','CNTT01','JAVA',2,1,8.0),
('SV001','CNTT01','WEB',2,1,10.0),
('SV002','CNTT01','WEB',2,1,10.0),
('SV003','CNTT01','WEB',2,1,8.5),
('SV004','CNTT01','WEB',2,1,7.5),
('SV005','CNTT01','WEB',2,1,7.0),
('SV006','CNTT01','WEB',2,1,5.5),
('SV007','CNTT01','WEB',2,1,8.0),
('SV008','CNTT01','WEB',2,1,7.0),
('SV009','CNTT01','WEB',2,1,10.0),
('SV010','CNTT01','WEB',2,1,6.5),
('SV001','CNTT01','TA2',2,1,5.5),
('SV002','CNTT01','TA2',2,1,4.0),
('SV003','CNTT01','TA2',2,1,4.0),
('SV004','CNTT01','TA2',2,1,8.0),
('SV005','CNTT01','TA2',2,1,7.5),
('SV006','CNTT01','TA2',2,1,5.0),
('SV007','CNTT01','TA2',2,1,4.5),
('SV008','CNTT01','TA2',2,1,7.5),
('SV009','CNTT01','TA2',2,1,6.5),
('SV010','CNTT01','TA2',2,1,7.5),
('SV001','CNTT01','HDH',2,1,7.0),
('SV002','CNTT01','HDH',2,1,5.5),
('SV003','CNTT01','HDH',2,1,4.0),
('SV004','CNTT01','HDH',2,1,7.0),
('SV005','CNTT01','HDH',2,1,7.0),
('SV006','CNTT01','HDH',2,1,7.5),
('SV007','CNTT01','HDH',2,1,8.5),
('SV008','CNTT01','HDH',2,1,8.5),
('SV009','CNTT01','HDH',2,1,6.0),
('SV010','CNTT01','HDH',2,1,8.0),
('SV011','CNTT02','THDC',1,1,3.5),
('SV011','CNTT02','THDC',1,2,6.5),
('SV012','CNTT02','THDC',1,1,8.0),
('SV013','CNTT02','THDC',1,1,7.0),
('SV014','CNTT02','THDC',1,1,8.5),
('SV015','CNTT02','THDC',1,1,8.5),
('SV016','CNTT02','THDC',1,1,5.5),
('SV017','CNTT02','THDC',1,1,7.0),
('SV018','CNTT02','THDC',1,1,9.0),
('SV019','CNTT02','THDC',1,1,6.0),
('SV020','CNTT02','THDC',1,1,7.0),
('SV011','CNTT02','KTLT',1,1,6.0),
('SV012','CNTT02','KTLT',1,1,8.0),
('SV013','CNTT02','KTLT',1,1,7.5),
('SV014','CNTT02','KTLT',1,1,5.5),
('SV015','CNTT02','KTLT',1,1,4.5),
('SV016','CNTT02','KTLT',1,1,3.5),
('SV016','CNTT02','KTLT',1,2,6.5),
('SV017','CNTT02','KTLT',1,1,8.5),
('SV018','CNTT02','KTLT',1,1,6.5),
('SV019','CNTT02','KTLT',1,1,6.0),
('SV020','CNTT02','KTLT',1,1,6.0),
('SV011','CNTT02','CTDL',1,1,5.5),
('SV012','CNTT02','CTDL',1,1,8.0),
('SV013','CNTT02','CTDL',1,1,8.0),
('SV014','CNTT02','CTDL',1,1,3.5),
('SV014','CNTT02','CTDL',1,2,6.5),
('SV015','CNTT02','CTDL',1,1,7.0),
('SV016','CNTT02','CTDL',1,1,7.0),
('SV017','CNTT02','CTDL',1,1,6.5),
('SV018','CNTT02','CTDL',1,1,7.0),
('SV019','CNTT02','CTDL',1,1,9.5),
('SV020','CNTT02','CTDL',1,1,7.5),
('SV011','CNTT02','OOP',1,1,9.0),
('SV012','CNTT02','OOP',1,1,8.5),
('SV013','CNTT02','OOP',1,1,4.5),
('SV014','CNTT02','OOP',1,1,10.0),
('SV015','CNTT02','OOP',1,1,7.5),
('SV016','CNTT02','OOP',1,1,6.5),
('SV017','CNTT02','OOP',1,1,8.5),
('SV018','CNTT02','OOP',1,1,10.0),
('SV019','CNTT02','OOP',1,1,5.5),
('SV020','CNTT02','OOP',1,1,6.0),
('SV011','CNTT02','TA1',1,1,7.5),
('SV012','CNTT02','TA1',1,1,4.0),
('SV013','CNTT02','TA1',1,1,5.0),
('SV014','CNTT02','TA1',1,1,7.0),
('SV015','CNTT02','TA1',1,1,8.0),
('SV016','CNTT02','TA1',1,1,8.0),
('SV017','CNTT02','TA1',1,1,8.5),
('SV018','CNTT02','TA1',1,1,9.0),
('SV019','CNTT02','TA1',1,1,6.5),
('SV020','CNTT02','TA1',1,1,6.5),
('SV021','CNTT03','THDC',1,1,6.5),
('SV022','CNTT03','THDC',1,1,6.5),
('SV023','CNTT03','THDC',1,1,9.0),
('SV024','CNTT03','THDC',1,1,8.5),
('SV025','CNTT03','THDC',1,1,3.5),
('SV025','CNTT03','THDC',1,2,6.5),
('SV026','CNTT03','THDC',1,1,8.0),
('SV027','CNTT03','THDC',1,1,5.5),
('SV028','CNTT03','THDC',1,1,5.0),
('SV029','CNTT03','THDC',1,1,9.0),
('SV030','CNTT03','THDC',1,1,9.0),
('SV021','CNTT03','KTLT',1,1,9.0),
('SV022','CNTT03','KTLT',1,1,8.0),
('SV023','CNTT03','KTLT',1,1,10.0),
('SV024','CNTT03','KTLT',1,1,5.0),
('SV025','CNTT03','KTLT',1,1,8.5),
('SV026','CNTT03','KTLT',1,1,6.0),
('SV027','CNTT03','KTLT',1,1,6.0),
('SV028','CNTT03','KTLT',1,1,4.5),
('SV029','CNTT03','KTLT',1,1,9.5),
('SV030','CNTT03','KTLT',1,1,10.0),
('SV021','CNTT03','CTDL',1,1,8.5),
('SV022','CNTT03','CTDL',1,1,6.0),
('SV023','CNTT03','CTDL',1,1,7.5),
('SV024','CNTT03','CTDL',1,1,5.5),
('SV025','CNTT03','CTDL',1,1,6.5),
('SV026','CNTT03','CTDL',1,1,9.5),
('SV027','CNTT03','CTDL',1,1,9.5),
('SV028','CNTT03','CTDL',1,1,8.0),
('SV029','CNTT03','CTDL',1,1,8.5),
('SV030','CNTT03','CTDL',1,1,7.0),
('SV021','CNTT03','OOP',1,1,7.5),
('SV022','CNTT03','OOP',1,1,7.0),
('SV023','CNTT03','OOP',1,1,8.0),
('SV024','CNTT03','OOP',1,1,6.0),
('SV025','CNTT03','OOP',1,1,8.5),
('SV026','CNTT03','OOP',1,1,6.5),
('SV027','CNTT03','OOP',1,1,7.5),
('SV028','CNTT03','OOP',1,1,7.0),
('SV029','CNTT03','OOP',1,1,9.5),
('SV030','CNTT03','OOP',1,1,8.5),
('SV021','CNTT03','TA1',1,1,5.5),
('SV022','CNTT03','TA1',1,1,5.5),
('SV023','CNTT03','TA1',1,1,8.0),
('SV024','CNTT03','TA1',1,1,7.0),
('SV025','CNTT03','TA1',1,1,6.5),
('SV026','CNTT03','TA1',1,1,7.5),
('SV027','CNTT03','TA1',1,1,9.5),
('SV028','CNTT03','TA1',1,1,7.0),
('SV029','CNTT03','TA1',1,1,9.5),
('SV030','CNTT03','TA1',1,1,6.0);
GO

-- Kiểm tra sĩ số đã được trigger cập nhật
SELECT MaLop, TenLop, SiSo FROM Lop;
GO
