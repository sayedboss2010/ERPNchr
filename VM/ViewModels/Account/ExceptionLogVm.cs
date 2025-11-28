namespace VM.ViewModels;

public class ExceptionLogVm
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Exception { get; set; }

    public string ExceptionType { get; set; }
    public DateTime? ExceptionTime { get; set; }
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
}