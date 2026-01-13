namespace VM.ViewModels;

public class LogInUserVm
{
    public long Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public int? UserTypeId { get; set; }

    public string AuthKey { get; set; }

    public int Sts { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int? BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public bool? IsChange { get; set; }

}