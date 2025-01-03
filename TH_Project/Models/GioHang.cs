﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TH_Project.Data;

namespace TH_Project.Models
{
    public class GioHang
    {
        QLBANSACHEntities2 db = new QLBANSACHEntities2();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public Double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public Double dThanhTien
        {
            get { return  iSoLuong * dDonGia ; }
        }

        public GioHang(int MaSach)
        {
            iMaSach = MaSach;
            SACH sach = db.SACHes.Single(n => n.Masach == iMaSach);
            sTenSach = sach.Tensach;
            sAnhBia = sach.Anhbia;
            dDonGia = double.Parse(sach.Giaban.ToString());
            iSoLuong = 1;
        }
    }
}