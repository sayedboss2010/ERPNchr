namespace VM.ViewModels;

public class UserCredentialsVm
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }

    public int DepartmentId { get; set; }

    public bool IsDeleted { get; set; }
    public bool? IsChange { get; set; }

}