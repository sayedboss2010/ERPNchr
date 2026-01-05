using EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ERPNchr.Areas.Attendance.Controllers
{
    [Area("Attendance")]

    public class AttendanceReportController : Controller
    {
        //  https://localhost:7028/Attendance/AttendanceReport/Index
        private readonly AppDbContext _context = new AppDbContext();

        public IActionResult IndexAll()
        {
            return View();
        }


        public IActionResult Index()
        {
            var targetDate = DateTime.Now.Date;
            
            // قراءة الكوكيز
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;
            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;
            int? branchCookieId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;
            int? branchDeptId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var data = new List<EmployeeAttendanceVM>();

            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_GetEmployeeAttendance", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TargetDate", targetDate));
                    command.Parameters.Add(new SqlParameter("@BranchDeptID", branchDeptId));

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var checkIn = reader["FirstInMorning"] != DBNull.Value ? TimeOnly.Parse(reader["FirstInMorning"].ToString()) : (TimeOnly?)null;
                            var checkOut = reader["LastOutEvening"] != DBNull.Value ? TimeOnly.Parse(reader["LastOutEvening"].ToString()) : (TimeOnly?)null;

                            TimeOnly officeStart = new TimeOnly(8, 0, 0);
                            TimeSpan? delay = (checkIn.HasValue && checkIn > officeStart) ? checkIn - officeStart : null;

                            data.Add(new EmployeeAttendanceVM
                            {
                                jopName = reader["JobTitle"].ToString(),
                                EmployeeName = reader["Name_AR"].ToString(),
                                EmpCode = reader["EmpCode"].ToString(),
                                Date = DateOnly.FromDateTime(targetDate),
                                CheckIn = checkIn,
                                CheckOut = checkOut,
                                Delay = delay,
                                Status = reader["MorningStatus"].ToString(),
                                Reason = reader["Reason"].ToString()
                            });
                        }
                    }
                }
            }
            return View(data);
        }



        //public IActionResult Index()
        //{
        //    TimeOnly officeStart = new TimeOnly(8, 0, 0);
        //    var today = DateOnly.FromDateTime(DateTime.Now);

        //    var data = (from e in _context.HrEmployees
        //                    // Left join مع حضور الموظف
        //                join a in _context.HrEmployeeAttendances
        //                    on e.Id equals a.EmployeeId into attendances
        //                from a in attendances.DefaultIfEmpty()
        //                    // Left join مع جدول الإجازات
        //                join l in _context.HrEmployeeLeaves
        //                    on e.Id equals l.EmployeeId into leaves
        //                from l in leaves.DefaultIfEmpty()
        //                    // شرط اليوم الحالي سواء حضور أو إجازة
        //                where (a != null && a.ModeDate == today)
        //                      || (l != null && today >= l.StartDate && today <= l.EndDate)
        //                group new { a, l } by new { e.NameAr, e.EmpCode, Date = today } into g
        //                let checkIn = g.Where(x => x.a != null && x.a.MoveCodeId == 1)
        //                               .Select(x => (TimeOnly?)x.a.MoveTime)
        //                               .Min()
        //                let checkOut = g.Where(x => x.a != null && x.a.MoveCodeId == 2)
        //                                .Select(x => (TimeOnly?)x.a.MoveTime)
        //                                .Max()
        //                let leave = g.Where(x => x.l != null).FirstOrDefault()
        //                select new EmployeeAttendanceVM
        //                {
        //                    EmployeeName = g.Key.NameAr,
        //                    EmpCode = g.Key.EmpCode,
        //                    Date = g.Key.Date,
        //                    CheckIn = checkIn,
        //                    CheckOut = checkOut,
        //                    Delay = (checkIn.HasValue && checkIn > officeStart) ? checkIn - officeStart : null,
        //                    Status = leave != null ? "إجازة" :
        //                             (checkIn.HasValue ? "حاضر" : "غائب")
        //                })
        //                .OrderBy(x => x.EmployeeName)
        //                .ThenBy(x => x.Date)
        //                .ToList();

        //    return View(data);
        //}


    }
}
