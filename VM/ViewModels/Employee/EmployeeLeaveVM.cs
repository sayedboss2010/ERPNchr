using EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels;

public class EmployeeLeaveVM
{
    public int EmployeeUserType { get; set; }

    public int? EmployeeTypeId { get; set; }
    public string EmployeeTypeName{ get; set; }
    public long? Id { get; set; }
    public string AttachmentPath { get; set; } = null!;
    public long? EmployeeId { get; set; }
    public long? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? DepartmentName { get; set; }
    public int? DepartmentID { get; set; }

    public byte? LeaveTypeId { get; set; }
    public int RegularRemainingAfter { get; set; }
    public int CasualRemainingAfter { get; set; }

    public decimal CasualTotalDays { get; set; }
    public int? CasualRemainingDays { get; set; }
    /// <summary>
    /// ايام العارضة المستخدمة
    /// </summary>
    public decimal CasualUsedDays { get; set; }

    [Display(Name = "نوع الإجازة")]
    public string? LeaveTypeName { get; set; }  // سنوية / مرضية / طارئة

    [Display(Name = "تاريخ البداية")]
    [DataType(DataType.Date)]
    public DateOnly? StartDate { get; set; }

    [Display(Name = "تاريخ النهاية")]
    [DataType(DataType.Date)]
    public DateOnly? EndDate { get; set; }

    [Display(Name = "سبب الإجازة")]
    public string? Reason { get; set; }


    public bool? IsActive { get; set; }

    public int? CreatedUserId { get; set; }

    [Display(Name = "تاريخ التسجيل")]

    public DateOnly? CreatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? DeletedUserId { get; set; }

    public DateOnly? DeletedDate { get; set; }

    public decimal? TotalDays { get; set; } // الاجازة المستحقة عن السنة

    public int? ActualDays { get; set; } // بعد استبعاد الجمعة
    public decimal? UsedDays { get; set; }    // اللي استخدمه قبل كده
    public int? RemainingBefore { get; set; } // قبل الطلب
    
    public bool? FinalApproval { get; set; } // موافقة المدير

    public decimal? UsedDaysMonth { get; set; } // ايام اجازات الشهر
    public decimal? DaysFromBalance { get; set; } // عدد الايام من الرصيد
    public int? DeductedDays { get; set; } // خصم من المرتب اليوم بيوم
    public long? LeaveBalanceID { get; set; } // HR_Employee_LeaveBalance كود جدول 
    public bool? DirectManagerApproval { get; set; }   // موافقة المدير المباشر
    public bool? DepartmentManagerApproval { get; set; } // موافقة مدير الادارة
    public int? LeaveDays { get; set; }


}
