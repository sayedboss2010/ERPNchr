using DAL.Classes.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            return RedirectToAction("Index", "Home", new { area = "Account" });
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
}