using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo.Models
{
    public class SanPham
    {
        string MASP, TENSP, SIZE, HINHANH, MALSP;
        int DONGIA, SOLUONG;
        public string masp
        {
            get { return masp; }
            set { masp = value; }
        }

        public string hinhanh
        {
            get { return hinhanh; }
            set { hinhanh = value; }
        }

        public string size
        {
            get { return SIZE; }
            set { SIZE = value; }
        }

        public string malsp
        {
            get { return MALSP; }
            set { MALSP = value; }
        }

        public int dongia
        {
            get { return DONGIA; }
            set { DONGIA = value; }
        }
        public int soluong
        {
            get { return SOLUONG; }
            set { SOLUONG = value; }
        }
    }
}