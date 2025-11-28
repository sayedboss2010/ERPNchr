using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using WEB.Models;

namespace WEB.Areas.Account.Controllers;

//[AreaAuthentication]
[Area("Account")]
public class HomeController : Controller
{
    public IActionResult Index(long UserID)
    {
        //ViewBag.HR_Employees_ID = "0";

        //try
        //{

        //    var max_Row = db.HR_Employees.Select(a => a.EmpCode).Max();
        //    int Count = int.Parse(max_Row.ToString()) + 1;
        //    ViewBag.MaxCode = Count;

        //}
        //catch (Exception)
        //{

        //}
        //if (ViewBag.MaxCode == null)
        //    ViewBag.MaxCode = "1000";
        ////var CurrentBranchDept_list = (from emp in db.HR_Departments
        ////                              join bd in db.HR_Branch_Department on emp.ID equals bd.Department_ID
        ////                              where emp.IsActive == true
        ////                              && emp.Deleted_UserId == null
        ////                              select new CustomOptionLongId
        ////                              {
        ////                                  DisplayText = emp.Name_AR,
        ////                                  Value = bd.ID
        ////                              }).Distinct().OrderBy(a => a.DisplayText).ToList();
        //ViewBag.CurrentBranchDept_ID = new SelectList(CurrentBranchDept_list, "Value", "DisplayText");
        //ViewBag.CurrentFunctional_Degree_ID = new SelectList(db.HR_Functional_Degree, "ID", "Name_AR");
        //ViewBag.CurrentJob_ID = new SelectList(db.HR_Jobs, "ID", "Title_AR");
        return View();
    }
    public IActionResult TestTable()
    {
        return View();
    }
}