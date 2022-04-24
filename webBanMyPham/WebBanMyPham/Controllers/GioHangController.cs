using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedBanQuanAo.Models;

namespace WedBanQuanAo.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/

        public ActionResult GioHang()
        {
            if (Session["HoaDon"] == null)
                return RedirectToAction("MessageEmpty");
            List<HOADON> lstHD = LayHoaDon();

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();

            return View(lstHD);
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<HOADON> lstGioHang = Session["HoaDon"] as List<HOADON>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(sp => sp.Sp.SoLuong);
            }
            return tsl;
        }

        private double TongThanhTien()
        {
            double ttt = 0;
            List<HOADON> lstGioHang = Session["HoaDon"] as List<HOADON>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.Sp.Gia * sp.Sp.SoLuong);
            }
            return ttt;
        }

        public ActionResult MessageEmpty()
        {
            return View();
        }

        public List<HOADON> LayHoaDon()
        {
            List<HOADON> lstHoaDon = Session["HoaDon"] as List<HOADON>;
            if (lstHoaDon == null)
            {
                //nếu lstGioHang chưa tồn tại thì khởi tạo
                lstHoaDon = new List<HOADON>();
                Session["HoaDon"] = lstHoaDon;
            }
            return lstHoaDon;
        }

        DatabaseDataContext db = new DatabaseDataContext();

        public ActionResult ThemGioHang(string ms, string strURL)
        {
            List<HOADON> lstHoaDon = LayHoaDon();
            HOADON hd = lstHoaDon.Find(sp => sp.Sp.Id == ms);

            if (hd == null)
            {
                hd = new HOADON();

                // get sp
                SANPHAM sp_ = db.SANPHAMs.Join(db.DONGIAs,
                                    sp => sp.ID,
                                    donGia => donGia.ID_SP,
                                    (sp, donGia) => new SANPHAM
                                    {
                                        Id = sp.ID,
                                        TenSP = sp.TENSP,
                                        SoLuong = int.Parse(sp.SOLUONG.Value.ToString()),
                                        HinhAnh = sp.HINHANH,
                                        Gia = double.Parse(donGia.GIA.Value.ToString())
                                    }).Single(sp => sp.Id == ms);

                hd.Sp = new SANPHAM();
                hd.Sp.Id = sp_.ma;
                hd.Sp.TenSP = sp_.TenSP;
                hd.Sp.Gia = sp_.Gia;
                hd.Sp.HinhAnh = sp_.HinhAnh;
                hd.Sp.SoLuong = 1;

                lstHoaDon.Add(hd);
            }
            else
            {
                hd.Sp.SoLuong++;
            }

            return Redirect(strURL);
        }

        public ActionResult XoaGioHang_All()
        {
            //lấy giỏ hàng
            List<HoaDon> lstGioHang = LayHoaDon();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(string MaSP)
        {
            List<HoaDon> lstGioHang = LayHoaDon();
            HoaDon hd = lstGioHang.Single(sp => sp.Sp.Id == MaSP);
            //nếu có thì tiến hành xóa
            if (hd != null)
            {
                lstGioHang.RemoveAll(sp => sp.Sp.Id == MaSP);
                return RedirectToAction("GioHang");
            }
            //nếu giỏ hàng rỗng
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(string MaSP, FormCollection f)
        {
            List<HoaDon> lstGioHang = LayHoaDon();
            HoaDon sp = lstGioHang.Single(s => s.Sp.Id == MaSP);
            //nếu có thì tiến hành cập nhật
            if (sp != null)
            {
                sp.Sp.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult DatHang()
        {
            // xử lí theo quy trình
            return RedirectToAction("TTGioHang", "ThongTinGioHang");
            /*
            // check người dùng 
            if (Session["ThongTinNguoiDung"] == null)
                return RedirectToAction("DangNhap", "Auth");

            List<HoaDon> lstGioHang = LayHoaDon(); // lấy lst hóa đơn 

            // get mã hóa đơn
            string maHD = db.fn_autoIDHD();

            var ttND = Session["ThongTinNguoiDung"] as ThongTinNguoiDung;

            lstGioHang.ForEach(gh =>
            {
                List<sp_AddHDResult> rs = db.sp_AddHD(maHD, ttND.HoTen, ttND.Tk.Username, gh.Sp.TenSP, gh.Sp.SoLuong).ToList();

                if (rs[0].Message != "SUCCESS")
                {
                    ViewBag.Info = "ERR";
                    ViewBag.Message = "Đã xảy ra lỗi vui lòng thử lại";
                    RedirectToAction("GioHang");
                }
            });
            Session["IDHD"] = maHD;

            // xuất hóa đơn
            return RedirectToAction("HoaDon");
            */


        }

        public ActionResult HoaDon()
        {
            if (Session["HoaDon"] == null)
                return RedirectToAction("MessageEmpty");
            List<HoaDon> lstHD = LayHoaDon();
            var ttND = Session["ThongTinNguoiDung"] as ThongTinNguoiDung;

            lstHD.ForEach(hd =>
            {
                hd.TenKH = ttND.HoTen;
            });

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();

            return View(lstHD);
        }
    }
}
