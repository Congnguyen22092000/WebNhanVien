using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNhanVien.Helpers;
using WebNhanVien.Models;

namespace WebNhanVien.Controllers
{

    public class StaffController : Controller
    {
        float LastStaffId = 6;
        
        public SessionHelper SessionHelper = new SessionHelper();

        /*public class NhanVienModel
        {
            public NhanVien nv { get; set; }

            public List<NhanVien> DsNhanVien { get; set; }
        }*/
        // GET: Staff
        public NhanVien nv = new NhanVien();
        public List<NhanVien> DsNhanVien = new List<NhanVien>();
        public StaffController() {


            /*GetNhanVienAll();*/
            /*GetNhanVien();*/
            TaoDsNhanVien();


        }
        /*public NhanVienModel GetNhanVienAll()
        {
            NhanVienModel nvAll = new NhanVienModel();
            nvAll.nv = GetNhanVien();
            nvAll.DsNhanVien = TaoDsNhanVien();
            return nvAll;
        }*/

        public NhanVien GetNhanVien()
        {
            return nv;
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


        

        [HttpPost]
        public IActionResult CreateNV(NhanVien newItem = null)
        {

            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
                LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
                if (this.LastStaffId % 10 == 0)
                {
                    newItem.maNhanVien = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2) + "0";
                }
                else
                {
                    newItem.maNhanVien = "NV-" + (this.LastStaffId / 10000).ToString().Substring(2);
                }
                newItem.hoTen = XuLyTen(newItem.hoTen);
                if (!IsDuplicatedStaff(newItem))
                { 
                    DsNhanVien.Add(newItem);
                    LastStaffId += 1;
                }
                
                HttpContext.Session.SetString("LastStaffId", JsonConvert.SerializeObject(LastStaffId));
                HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));

            }
            return Redirect("/staff/index");
        }
        [HttpGet]
        public IActionResult CreateNV()
        {
            /*LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
            
            ViewBag.StaffListJson = HttpContext.Session.GetString("StaffList");*/
            return View();
            /*return Redirect("/staff/index");*/
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
                if (DsNhanVien[i].maNhanVien == newItem.maNhanVien)
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
                if (nv.maNhanVien == MNV) result = nv;
            }

            return result;
        }

        [HttpPost]
        public bool Delete(string maNhanVien)
        {
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                LastStaffId = JsonConvert.DeserializeObject<float>(HttpContext.Session.GetString("LastStaffId"));
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            for (int i = 0; i < DsNhanVien.Count; i++)
            {
                if (DsNhanVien[i].maNhanVien == maNhanVien)
                {
                    DsNhanVien.RemoveAt(i);
                    /*LastStaffId -= 1;
                    HttpContext.Session.SetString("LastStaffId", JsonConvert.SerializeObject(LastStaffId));*/
                    HttpContext.Session.SetString("StaffList", JsonConvert.SerializeObject(DsNhanVien));
                }
            }
            return true;
        }


    
        public string XuLyTen(string name)
        {
            string result = "";
            if (name != "" && name != null)
            {
                List<char> arrName = new List<char>(name.ToLower().Trim().ToCharArray());
                for (int i = 0; i < arrName.Count; i++)
                {
                    if (arrName[i] == ' ')
                    {
                        int _i = i + 1;
                        if (arrName[_i] == ' ')
                        {
                            arrName.RemoveAt(i);
                        }
                    }
                }
                arrName[0] = char.ToUpper(arrName[0]);
                for (int i = 0; i < arrName.Count; i++)
                {
                    if (arrName[i] == ' ')
                    {
                        int _i = i + 1;
                        arrName[_i] = char.ToUpper(arrName[_i]);
                    }
                }

                for (int i = 0; i < arrName.Count; i++)
                {
                    result += arrName[i].ToString();
                }
            }
            return result;
        }

        public bool IsDuplicatedStaff(NhanVien pnv)
        {
            pnv.hoTen = XuLyTen(pnv.hoTen);
            bool daTonTai = false;
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            foreach (NhanVien nv in DsNhanVien)
            {
                if (nv.hoTen == pnv.hoTen)
                {
                    if (DateTime.Compare(nv.ngaySinh, pnv.ngaySinh) == 0)
                    {
                        daTonTai = true;
                        break;

                    }
                    else { daTonTai = false; }
                }
                else { daTonTai = false; }
            }
            return daTonTai;
        }
        public bool EditValidate(NhanVien pnv)
        {
            bool daTonTai = false;
            byte[] json;
            if (HttpContext.Session.TryGetValue("StaffList", out json))
            {
                DsNhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(HttpContext.Session.GetString("StaffList"));
            }
            pnv.hoTen = XuLyTen(pnv.hoTen);
            for (int i = 0; i < DsNhanVien.Count; i++)
            {
                if (DsNhanVien[i].hoTen == pnv.hoTen && DateTime.Compare(DsNhanVien[i].ngaySinh, pnv.ngaySinh) == 0 && DsNhanVien[i].maNhanVien != pnv.maNhanVien)
                {
                    daTonTai = true;
                    break;
                }

            }
            return daTonTai;
        }

        [HttpGet]
        public ActionResult test()
        {

            return View();
        }


    }
}
