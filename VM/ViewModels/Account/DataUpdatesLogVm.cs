namespace VM.ViewModels;

public class DataUpdatesLogVm
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Operation { get; set; }

    public string TableName { get; set; }

    public int? RawId { get; set; }

    public DateTime UpdateTime { get; set; }

    public string Description { get; set; }
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
}