using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNhanVien.Models;

namespace WebNhanVien.Controllers
{

    public class StaffController : Controller
    {
        float LastStaffId=6;

        // GET: Staff
        public List<NhanVien> DsNhanVien = new List<NhanVien>();
        public StaffController() {

           
            TaoDsNhanVien();
            
        }
        public List<NhanVien> TaoDsNhanVien()
        {

            DsNhanVien.Add(new NhanVien("NV-0001", "Nguyễn Văn Công", DateTime.Parse("7/6/2000"), "09715869331", "Nhân viên"));
            DsNhanVien.Add(new NhanVien("NV-0002", "Nguyễn Văn Ánh", DateTime.Parse("8/6/1999"), "09715869331", "Nhân viên"));
            DsNhanVien.Add(new NhanVien("NV-0003", "Lê Thị Huệ", DateTime.Parse("6/3/2000"), "09715869331", "Nhân viên"));
            DsNhanVien.Add(new NhanVien("NV-0004", "Trần Văn Chung", DateTime.Parse("4/5/2000"), "09715869331", "Nhân viên"));
            DsNhanVien.Add(new NhanVien("NV-0005", "Bùi Tiến Đạt", DateTime.Parse("12/11/2000"), "09715869331", "Nhân viên"));
           
            /*HttpContext.Session.SetObjectAsJson("listNV", DsNhanVien);*/
            return DsNhanVien;
        }

        [HttpGet]
        public ActionResult Index()
        {

            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
                LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
            }
            else
            {

                HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                HttpContext.Session.SetString("LastStaffId", JsonConvert.SerializeObject(LastStaffId));
            }

            return View(DsNhanVien);

        }
        [HttpPost]
        public ActionResult Index(string key = "")
        {

            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
                LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
            }
            else
            {

                HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                HttpContext.Session.SetString("LastStaffId", JsonConvert.SerializeObject(LastStaffId));
            }
                HttpContext.Session.SetString("StaffListTemp", JsonConvert.SerializeObject(DsNhanVien));
            if (key != "")
            {
                List<NhanVien> dsTimKiem = new List<NhanVien>();
                foreach (NhanVien nv in DsNhanVien)
                {
                    if (nv.hoTen.ToLower().Contains(key.ToLower()) || nv.soDT.ToLower().Contains(key.ToLower()))
                    {
                        dsTimKiem.Add(nv);
                    }
                }
                HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(dsTimKiem));
                return View(dsTimKiem);
                

            }

            return Redirect("/Staff/Index");

        }

        public ActionResult ReUp()
        {
            DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffListTemp"));
            HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
            return Redirect("/staff/index");
        }



        [HttpPost]
        public ActionResult Create(NhanVien newItem = null)
        {

            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
                LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
                if (this.LastStaffId % 10 == 0)
                {
                    newItem.maNV = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2) + "0";
                }
                else
                {
                    newItem.maNV = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2);
                }
                LastStaffId += 1;
                
                DsNhanVien.Add(newItem);
                HttpContext.Session.SetString("LastStaffId", JsonConvert.SerializeObject(LastStaffId));
                HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));

            }
            return Redirect("/staff/index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
            if (this.LastStaffId % 10 == 0)
            {
                ViewBag.LastStaffId = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2) + "0";
            }
            else
            {
                ViewBag.LastStaffId = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(NhanVien newItem = null)
        {
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            for (int i = 0; i < DsNhanVien.Count; i++)
            {
                if (DsNhanVien[i].maNV == newItem.maNV)
                {
                    DsNhanVien[i] = newItem;
                    HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                }
            }
            return Redirect("/staff/index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            return View(GetMNV(id));
        }
        public NhanVien GetMNV(string MNV)
        {
            NhanVien result = null;
            DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            foreach (NhanVien nv in DsNhanVien)
            {
                if (nv.maNV == MNV) result = nv;
            }

            return result;
        }

        public ActionResult Delete(string id)
        {
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            for (int i = 0; i < DsNhanVien.Count; i++)
            {
                if (DsNhanVien[i].maNV == id)
                {
                    DsNhanVien.RemoveAt(i);
                    HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                }
            }
            return Redirect("/staff/index");
        }

        
        /*[HttpPost]
        public bool Delete(string maNhanVien)
        {
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            for (int i = 0; i < DsNhanVien.Count; i++)
            {
                if (DsNhanVien[i].maNV == maNhanVien)
                {
                    DsNhanVien.RemoveAt(i);
                    HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                }
            }
            return true;
        }*/
    }
}
