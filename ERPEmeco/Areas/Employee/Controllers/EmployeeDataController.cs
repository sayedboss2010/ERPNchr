using DAL.Classes.DroupDowen;
using DAL.Classes.Employee;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Web;


namespace ERPNchr.Areas.Employee.Controllers;

//[AreaAuthentication]
[Area("Employee")]
public class EmployeeDataController : Controller
{
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
        var ListHrJobs = new SelectList(DroupDowenDal.ListHrJobs(), "Id", "TitleAr");
        ViewBag.ListHrJobs = ListHrJobs;
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
    public async Task<ActionResult> AddEdite(HrEmployee hR_Employees, IFormFile NID_Path)
    {

        //var Chek_Nid = db.HR_Employees.Where(a => a.IsActive == true && a.Deleted_UserId == null && a.NID == hR_Employees.NID).Count();
        //var Chek_name = db.HR_Employees.Where(a => a.IsActive == true && a.Deleted_UserId == null && a.Name_AR == hR_Employees.Name_AR).Count();
        //if (Chek_name == 0)
        //{
        //    //if (Chek_Nid == 0)
        //    //{
        //    long HR_Employees_ID = db.Database.SqlQuery<long>("SELECT NEXT VALUE FOR dbo.HR_Employees_SEQ").Single();

        //    try
        //    {
        //        var max_Row = db.HR_Employees.Select(a => a.EmpCode).Max();
        //        int Count = int.Parse(max_Row.ToString()) + 1;
        //        ViewBag.MaxCode = Count;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    if (ViewBag.MaxCode == null)
        //        ViewBag.MaxCode = "1000";
        //    if (NID_Path != null)
        //    {
        //        string path = Server.MapPath("~/Uploads/NID_Path/");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        string fileName = Path.GetFileName(NID_Path.FileName);
        //        NID_Path.SaveAs(path + fileName);
        //        hR_Employees.NID_Path = "Uploads/NID_Path/" + fileName;
        //    }

        //    hR_Employees.ID = HR_Employees_ID;

        //    hR_Employees.IsActive = true;
        //    hR_Employees.Created_Date = DateTime.Now;
        //    hR_Employees.Created_UserId = int.Parse(Session["User_ID"].ToString());
        //    // if (ModelState.IsValid)
        //    // {
        //    db.HR_Employees.Add(hR_Employees);
        //    await db.SaveChangesAsync();


        //    if (hR_Employees.Current_Salary != null)
        //    {
        //        //        public Nullable<int> Year { get; set; }
        //        // public Nullable<int> Month { get; set; }
        //        // السنة والشهر لسه
        //        long _HR_Employee_Salary_History_ID = db.Database.SqlQuery<long>("SELECT NEXT VALUE FOR dbo.HR_Employee_Salary_History_SEQ").Single();
        //        HR_Employee_Salary_History hR_Salary_History = new HR_Employee_Salary_History();

        //        hR_Salary_History.ID = _HR_Employee_Salary_History_ID;
        //        hR_Salary_History.HR_Employee_ID = hR_Employees.ID;
        //        hR_Salary_History.Salary = hR_Employees.Current_Salary;
        //        hR_Salary_History.Year = hR_Employees.Year;
        //        hR_Salary_History.Month = hR_Employees.Month;
        //        hR_Salary_History.IS_Active = true;
        //        hR_Salary_History.IS_Deleted = false;
        //        hR_Salary_History.User_Creation_Date = DateTime.Now;
        //        hR_Salary_History.User_Creation_Id = int.Parse(Session["User_ID"].ToString());
        //        db.HR_Employee_Salary_History.Add(hR_Salary_History);
        //        await db.SaveChangesAsync();
        //    }

        //    if (hR_Employees.Insurance_Number != null)
        //    {
        //        try
        //        {
        //            // السنة والشهر لسه
        //            long _HR_Employee_Insurance_History_ID = db.Database.SqlQuery<long>("SELECT NEXT VALUE FOR dbo.HR_Employee_Insurance_History_SEQ").Single();
        //            HR_Employee_Insurance_History HR_Insurance_History = new HR_Employee_Insurance_History();

        //            HR_Insurance_History.ID = _HR_Employee_Insurance_History_ID;
        //            HR_Insurance_History.HR_Employees_ID = HR_Employees_ID;
        //            HR_Insurance_History.Date_In = hR_Employees.Date_In;
        //            HR_Insurance_History.Date_Out = hR_Employees.Date_Out;
        //            HR_Insurance_History.IS_Active = true;
        //            HR_Insurance_History.IS_Deleted = false;
        //            HR_Insurance_History.User_Creation_Date = DateTime.Now;
        //            HR_Insurance_History.User_Creation_Id = int.Parse(Session["User_ID"].ToString());
        //            db.HR_Employee_Insurance_History.Add(HR_Insurance_History);
        //            await db.SaveChangesAsync();

        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }

        //    //}
        //    //else
        //    //{
        //    //    ViewBag.Message = "الرقم القومي موجود مسبقا ";
        //    //}
        //}
        //else
        //{
        //    try
        //    {
        //        var max_Row = db.HR_Employees.Select(a => a.EmpCode).Max();
        //        int Count = int.Parse(max_Row.ToString()) + 1;
        //        ViewBag.MaxCode = Count;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    if (ViewBag.MaxCode == null)
        //        ViewBag.MaxCode = "1000";
        //    ViewBag.Message = "الاسم موجود مسبقا";
        //    FillDroup(hR_Employees);
        //    return View(hR_Employees);
        //}

        //FillDroup(hR_Employees);
        return RedirectToAction("AddEdite","EmployeeData",new { area = "Employee", EmployeeID = hR_Employees.Id });

    }

}
