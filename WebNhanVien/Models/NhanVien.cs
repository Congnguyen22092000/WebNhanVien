using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNhanVien.Models
{

    public class NhanVien
    {
        public NhanVien(string maNhanVien, string hoTen, DateTime ngaySinh, string soDT, string chucVu)
        {
            this.maNhanVien = maNhanVien;
            this.hoTen = hoTen;
            this.ngaySinh = ngaySinh;
            this.soDT = soDT;
            this.chucVu = chucVu;
        }
     
        public string maNhanVien { get; set; }

        public string hoTen { get; set; }
      
      
        public DateTime ngaySinh { get; set; }
       
        public string soDT { get; set; }
       
        public string chucVu { get; set; }

        public NhanVien() { }


        


    }
    
}