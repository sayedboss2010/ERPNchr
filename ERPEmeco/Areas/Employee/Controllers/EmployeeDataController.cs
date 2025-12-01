using DAL.Classes.DroupDowen;
using DAL.Classes.Employee;
using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using VM.ViewModels.Employee;


namespace ERPNchr.Areas.Employee.Controllers;

//[AreaAuthentication]
[Area("Employee")]
public class EmployeeDataController : Controller
{
    private readonly AppDbContext db = new AppDbContext();
    private readonly IWebHostEnvironment _env;
    public EmployeeDataController(IWebHostEnvironment env)
    {
        _env = env;
    }
    public IActionResult IndexALL()
    {
        return View();
    }
    public IActionResult AddEdite(long EmployeeID = 0)
    {
        //var userId = int.Parse(Request.Cookies.FirstOrDefault(c => c.Key == "UserId").Value);
        //var materials = new SelectList(MaterialsDal.GetAllMaterials(userId), "Id", "MaterialNameAr");

       // var colors = new SelectList(ColorDal.list(userId), "Id", "ColorName");

        //ViewBag.Materials = materials;
        //ViewBag.colors = colors;

        //int pageSize = 10;

        var objlst = EmployeeDataDal.Find(EmployeeID);
        var ListHrDepartment = new SelectList(DroupDowenDal.ListHrDepartment(), "Id", "NameAr");
        ViewBag.ListHrDepartment = ListHrDepartment;
        var ListHrBranch = new SelectList(DroupDowenDal.ListHrBranch(), "Id", "NameAr");

        ViewBag.ListHrBranch = ListHrBranch;
        var ListHrJobs = new SelectList(DroupDowenDal.ListHrJobs(), "Id", "TitleAr");
       
        ViewBag.ListHrJobs = ListHrJobs;
        var ListHrEmployeesTypes = new SelectList(DroupDowenDal.ListHrEmployeesTypes(), "Id", "EmployeeTypeNameAr");
        ViewBag.ListHrEmployeesTypes = ListHrEmployeesTypes;
        //ViewBag.TotalResults = objlst.TotalResults;
        //ViewBag.CurrentPage = objlst.CurrentPage;
        //ViewBag.TotalPages = objlst.TotalPages;
        //ViewBag.Search = searchStr;


        //bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //if (isAjax)
        //{
        //    return PartialView("_PartialAllCompanys", objlst.Items);
        //}

        return View(objlst);


    }

    [HttpPost]
    public async Task<ActionResult> AddEdite(HrEmployee model, IFormFile NID_Path)
    {
       
        try
        {
            long HR_Employees_ID;

            // ===== إنشاء موظف جديد =====
            if (model.Id == 0)
            {
                HR_Employees_ID = db.Database
                    .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_Employees_SEQ")
                    .AsEnumerable()
                    .First();

                model.Id = HR_Employees_ID;

                // معالجة ملف NID
                if (NID_Path != null && NID_Path.Length > 0)
                {
                    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string uploadFolder = Path.Combine(webRoot, "uploads", "NID_Path");



                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);
                    // اسم ملف فريد
                    string fileName = $"{Guid.NewGuid()}_{NID_Path.FileName}";
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NID_Path.CopyTo(stream);
                    }

                    // المسار الذي يُخزن في الداتابيز
                    model.NidPath = $"/uploads/NID_Path/{fileName}"; ;
                }

                model.IsActive = model.IsActive; // من الفيو
                //model.CreatedDate = DateTime.Now.Date;
                //model.CreatedUserId = int.Parse(Session["User_ID"].ToString());

                db.HrEmployees.Add(model);
                await db.SaveChangesAsync();
            }
            else
            {
                // ===== تعديل موظف موجود =====
                var existingEmployee = db.HrEmployees.Find(model.Id);
                if (existingEmployee == null)
                    return NotFound();

                // تحديث البيانات الأساسية
                existingEmployee.EmpCode = model.EmpCode;
                existingEmployee.NameAr = model.NameAr;
                existingEmployee.Nid = model.Nid;
                existingEmployee.Email = model.Email;
                existingEmployee.PhoneNumber = model.PhoneNumber;
                existingEmployee.AddressAr = model.AddressAr;
                existingEmployee.IsActive = model.IsActive;
                existingEmployee.EmployeeTypeId = model.EmployeeTypeId; // نوع الموظف
                existingEmployee.BranchId = model.BranchId;
                existingEmployee.DepartmentId = model.DepartmentId;
                existingEmployee.CurrentJobId = model.CurrentJobId;
                existingEmployee.AppointmentDate = model.AppointmentDate;
                existingEmployee.CurrentSalary = model.CurrentSalary;
            
                existingEmployee.Isbank = model.Isbank;
                existingEmployee.InsuranceNumber = model.InsuranceNumber;
                existingEmployee.DateIn = model.DateIn;
                existingEmployee.DateOut = model.DateOut;

                // معالجة ملف NID جديد
                if (NID_Path != null && NID_Path.Length > 0)
                {
                    // مسار حفظ الملفات داخل wwwroot
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "NID_Path");

                    // إنشاء المجلد إذا لم يكن موجوداً
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Path.GetFileName(NID_Path.FileName);
                    string fullPath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await NID_Path.CopyToAsync(stream);
                    }

                    // حفظ المسار النسبي في قاعدة البيانات
                    existingEmployee.NidPath = Path.Combine("Uploads", "NID_Path", fileName).Replace("\\", "/");
                }

                // تحديث السجل في قاعدة البيانات باستخدام EF Core
                db.HrEmployees.Update(existingEmployee);
                await db.SaveChangesAsync();
            }

          

            return RedirectToAction("AddEdite", "EmployeeData", new { area = "Employee", EmployeeID = model.Id });
        }
        catch (Exception ex)
        {
            ViewBag.Message = "حدث خطأ أثناء الحفظ: " + ex.Message;
            //FillDroup(model);
            return View(model);
        }
    m}


}
