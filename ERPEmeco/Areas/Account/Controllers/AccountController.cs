using DAL.Classes.Account;
using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using VM.ViewModels;
using VM.ViewModels.Employee;
using static Azure.Core.HttpHeader;

namespace WEB.Areas.Account.Controllers;

[Area("Account")]
[AllowAnonymous]
public class AccountController : Controller
{
    public IActionResult LogIn()
    {
        ViewBag.ErrorMessage = "";
        ViewBag.error = "none";

        return View();
    }

    [HttpPost]
    public IActionResult LogIn(string userName, string password)
    {
        // Here you would typically call a service or repository to verify the user credentials.
        // For now, we will just return a view with the provided credentials for demonstration purposes.

        // In a real application, you would validate the credentials against a database or other data source.
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Username and password are required.");
            return View();
        }


        //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
        //model.Password = hashedPassword;
       


        var credentialsVm = new UserCredentialsVm
        {
            UserName = userName,
            Password = password
        };

        var user = LoginDal.VerifyUserCredentials(credentialsVm);
        if (user.Sts == 1)
        {
            CookieOptions option = new CookieOptions();
            option.IsEssential = true;
            Response.Cookies.Append("UserId", user.Id.ToString(), option);
            Response.Cookies.Append("AuthKey", user.AuthKey, option);
            Response.Cookies.Append("UserType", user.UserTypeId.ToString(), option);
            Response.Cookies.Append("UserName", user.Email, option);
            Response.Cookies.Append("FullName", user.FullName, option);
            Response.Cookies.Append("DepartmentID", user.DepartmentId.ToString(), option);
            Response.Cookies.Append("BranchID", user.BranchId.ToString(), option);
            Response.Cookies.Append("Email", user.Email, option);
            Response.Cookies.Append("IsChange", user.IsChange.ToString(), option);
            if (user.IsChange == true)
            {
                return RedirectToAction("Index", "Home", new { area = "Account", EmployeeID = user.Id });
            }
            else
            {
                return RedirectToAction("ChangePass", "Account", new { area = "Account", EmployeeID = user.Id
                    , Email= user.Email,
                    NameAr= user.FullName
                });

            }
        }

        ViewBag.ErrorMessage = "1";
        ViewBag.error = "block";

        return View();
    }

    public IActionResult LogOut()
    {
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        return RedirectToAction("LogIn", "Account", new { area = "Account" });
    }

    //public IActionResult ChangePass(long EmployeeId, string Email,string NameAr)
    //{

    //    ViewBag.EmployeeId = EmployeeId;
    //    ViewBag.Email = Email;
    //    ViewBag.NameAr = NameAr;
    //    ViewBag.ErrorMessage = "";
    //    ViewBag.error = "none";

    //    return View();
    //}

    [HttpGet]
    public IActionResult ChangePass(long employeeID)
    {
        var user = _context.HrEmployees.FirstOrDefault(x => x.Id == employeeID);

        if (user == null)
            return NotFound();

        var model = new ChangePassVM
        {
            EmployeeId = user.Id,
            //Email = user.Email,

        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePass(ChangePassVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.HrEmployees.FirstOrDefault(x => x.Id == model.EmployeeId);

        if (user == null)
        {
            ModelState.AddModelError("", "الموظف غير موجود");
            return View(model);
        }

        // ✅ التأكد من الإيميل
        if (user.Email != model.Email)
        {
            ModelState.AddModelError("Email", "البريد الإلكتروني غير مطابق");
            return View(model);
        }

        // 🔐 تشفير كلمة المرور (مثال)
        user.Password = model.NewPassword;
        user.IsChange = true;

        _context.SaveChanges();

        TempData["Success"] = "تم تغيير كلمة المرور بنجاح";
        return RedirectToAction("ChangePass", new { employeeID = model.EmployeeId });
    }


    private readonly AppDbContext _context = new AppDbContext();

    //[HttpPost]
    //public IActionResult ChangePass(long EmployeeId, string Email, string NameAr, string Password)
    //{
    //    // تحقق من وجود مأمورية تتداخل مع نفس الفترة
    //    bool exists = _context.HrEmployees
    //         .Any(p => p.Id == EmployeeId
    //                   && p.Email== Email);
    //    if (exists)
    //    {
    //        var employee = _context.HrEmployees.Find(EmployeeId);
    //        employee.Password = Password;
    //        _context.SaveChangesAsync();

    //    }
    //    return RedirectToAction("LogIn", "Account", new { area = "Account" });

    //}

}