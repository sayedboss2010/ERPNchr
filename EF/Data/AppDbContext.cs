using System;
using System.Collections.Generic;
using EF.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AUserLogin> AUserLogins { get; set; }

    public virtual DbSet<DataUpdatesLogTb> DataUpdatesLogTbs { get; set; }

    public virtual DbSet<ExceptionLogTb> ExceptionLogTbs { get; set; }

    public virtual DbSet<HrBranch> HrBranches { get; set; }

    public virtual DbSet<HrBranchDepartment> HrBranchDepartments { get; set; }

    public virtual DbSet<HrDepartment> HrDepartments { get; set; }

    public virtual DbSet<HrEmployee> HrEmployees { get; set; }

    public virtual DbSet<HrEmployeeAttendance> HrEmployeeAttendances { get; set; }

    public virtual DbSet<HrEmployeeLateness> HrEmployeeLatenesses { get; set; }

    public virtual DbSet<HrEmployeeLeaf> HrEmployeeLeaves { get; set; }

    public virtual DbSet<HrEmployeeLeaveBalance> HrEmployeeLeaveBalances { get; set; }

    public virtual DbSet<HrJob> HrJobs { get; set; }

    public virtual DbSet<HrJobGrade> HrJobGrades { get; set; }

    public virtual DbSet<HrLeaveType> HrLeaveTypes { get; set; }

    public virtual DbSet<HrMachineIp> HrMachineIps { get; set; }

    public virtual DbSet<HrMachineMove> HrMachineMoves { get; set; }

    public virtual DbSet<LogInHistoryTb> LogInHistoryTbs { get; set; }

    public virtual DbSet<PrGroup> PrGroups { get; set; }

    public virtual DbSet<PrGroupModuleMenu> PrGroupModuleMenus { get; set; }

    public virtual DbSet<PrMenu> PrMenus { get; set; }

    public virtual DbSet<PrModule> PrModules { get; set; }

    public virtual DbSet<PrSystem> PrSystems { get; set; }

    public virtual DbSet<PrUser> PrUsers { get; set; }

    public virtual DbSet<PrUserGroup> PrUserGroups { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-R5ET6G3;Database=HR_Nchr;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AUserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_A_User_Login");

            entity.ToTable("A__User_Login");

            entity.Property(e => e.AccessToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LogOutDate)
                .HasColumnType("datetime")
                .HasColumnName("LogOut_Date");
            entity.Property(e => e.LoginDate)
                .HasColumnType("datetime")
                .HasColumnName("Login_Date");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
        });

        modelBuilder.Entity<DataUpdatesLogTb>(entity =>
        {
            entity.ToTable("DataUpdatesLogTb");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Operation).HasMaxLength(50);
            entity.Property(e => e.TableName).HasMaxLength(200);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.DataUpdatesLogTbs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_DataUpdatesLogTb_UserTb");
        });

        modelBuilder.Entity<ExceptionLogTb>(entity =>
        {
            entity.ToTable("ExceptionLogTb");

            entity.Property(e => e.ExceptionTime).HasColumnType("datetime");
            entity.Property(e => e.ExceptionType).HasMaxLength(1000);

            entity.HasOne(d => d.User).WithMany(p => p.ExceptionLogTbs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ExceptionLogTb_PR_User");
        });

        modelBuilder.Entity<HrBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Branches");

            entity.ToTable("HR_Branches", tb => tb.HasComment("الفروع"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CompanyId).HasColumnName("Company_ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.NameAr)
                .HasMaxLength(100)
                .HasColumnName("Name_AR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<HrBranchDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Branch_Department");

            entity.ToTable("HR_Branch_Department", tb => tb.HasComment("إدارات الفرع"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BranchId).HasColumnName("Branch_ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");

            entity.HasOne(d => d.Branch).WithMany(p => p.HrBranchDepartments)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Branch_Department_Branches");

            entity.HasOne(d => d.Department).WithMany(p => p.HrBranchDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Branch_Department_Departments");
        });

        modelBuilder.Entity<HrDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Departments");

            entity.ToTable("HR_Departments", tb => tb.HasComment("الإدارات"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.NameAr)
                .HasMaxLength(100)
                .HasColumnName("Name_AR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<HrEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employees");

            entity.ToTable("HR_Employees", tb => tb.HasComment("الموظفين"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AddressAr)
                .HasMaxLength(100)
                .HasColumnName("Address_AR");
            entity.Property(e => e.AddressEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Address_EN");
            entity.Property(e => e.AppointmentDate).HasComment("تاريخ التعيين");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.CurrentBranchDeptId).HasColumnName("CurrentBranchDept_ID");
            entity.Property(e => e.CurrentFunctionalDegreeId).HasColumnName("CurrentFunctional_Degree_ID");
            entity.Property(e => e.CurrentJobId).HasColumnName("CurrentJob_ID");
            entity.Property(e => e.CurrentSalary)
                .HasComment("ملوش لازمه")
                .HasColumnType("money")
                .HasColumnName("Current_Salary");
            entity.Property(e => e.DateIn)
                .HasComment("تاريخ دخول للتأمين")
                .HasColumnName("Date_In");
            entity.Property(e => e.DateOut)
                .HasComment("تاريخ الخروج من التأمين")
                .HasColumnName("Date_Out");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpCode).HasComment("كود الموظف");
            entity.Property(e => e.EmpCodeNew).HasMaxLength(50);
            entity.Property(e => e.HrJobGradesId).HasColumnName("HR_JobGradesID");
            entity.Property(e => e.InsuranceNumber)
                .HasMaxLength(50)
                .HasComment("الرقم التأميني")
                .HasColumnName("Insurance_Number");
            entity.Property(e => e.Isbank)
                .HasDefaultValue(false)
                .HasColumnName("ISBank");
            entity.Property(e => e.Mobile).HasMaxLength(11);
            entity.Property(e => e.NameAr)
                .HasMaxLength(100)
                .HasColumnName("Name_AR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_EN");
            entity.Property(e => e.Nid)
                .HasMaxLength(14)
                .HasComment("الرقم القومي")
                .HasColumnName("NID");
            entity.Property(e => e.NidPath)
                .HasMaxLength(500)
                .HasColumnName("NID_Path");
            entity.Property(e => e.PhoneNumber).HasMaxLength(12);
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");

            entity.HasOne(d => d.CurrentBranchDept).WithMany(p => p.HrEmployees)
                .HasForeignKey(d => d.CurrentBranchDeptId)
                .HasConstraintName("FK_Employees_Branch_Department");

            entity.HasOne(d => d.CurrentJob).WithMany(p => p.HrEmployees)
                .HasForeignKey(d => d.CurrentJobId)
                .HasConstraintName("FK_Employees_Jobs");
        });

        modelBuilder.Entity<HrEmployeeAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee_Attendance");

            entity.ToTable("HR_Employee_Attendance", tb => tb.HasComment("������ ���������"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.MachineId).HasColumnName("Machine_ID");
            entity.Property(e => e.MoveCodeId).HasColumnName("MoveCode_ID");

            entity.HasOne(d => d.Employee).WithMany(p => p.HrEmployeeAttendances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Employee_Attendance_Employees");

            entity.HasOne(d => d.Machine).WithMany(p => p.HrEmployeeAttendances)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_Employee_Attendance_Machine_IP");

            entity.HasOne(d => d.MoveCode).WithMany(p => p.HrEmployeeAttendances)
                .HasForeignKey(d => d.MoveCodeId)
                .HasConstraintName("FK_Employee_Attendance_MachineMoves");
        });

        modelBuilder.Entity<HrEmployeeLateness>(entity =>
        {
            entity.ToTable("HR_Employee_Lateness", tb => tb.HasComment("تاخيرات الموظفين"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.HrEmployeesId).HasColumnName("HR_Employees_ID");
            entity.Property(e => e.HrLatenessTypeId).HasColumnName("HR_Lateness_Type_ID");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_Active");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IS_Deleted");
            entity.Property(e => e.UserCreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("User_Creation_Date");
            entity.Property(e => e.UserCreationId).HasColumnName("User_Creation_Id");
            entity.Property(e => e.UserDeletionDate)
                .HasColumnType("datetime")
                .HasColumnName("User_Deletion_Date");
            entity.Property(e => e.UserDeletionId).HasColumnName("User_Deletion_Id");
            entity.Property(e => e.UserUpdationDate)
                .HasColumnType("datetime")
                .HasColumnName("User_Updation_Date");
            entity.Property(e => e.UserUpdationId).HasColumnName("User_Updation_Id");

            entity.HasOne(d => d.HrEmployees).WithMany(p => p.HrEmployeeLatenesses)
                .HasForeignKey(d => d.HrEmployeesId)
                .HasConstraintName("FK_HR_Employee_Lateness_HR_Employees1");
        });

        modelBuilder.Entity<HrEmployeeLeaf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee_Leaves");

            entity.ToTable("HR_Employee_Leaves", tb => tb.HasComment("أجازات الموظف"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AttachmentPath).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.DepartmentManagerApproval).HasComment("موافقة مدير الادارة");
            entity.Property(e => e.DirectManagerApproval).HasComment("موافقة المدير المباشر");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.HrEmployeeLeaveBalanceId).HasColumnName("HR_Employee_LeaveBalanceID");
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveType_ID");
            entity.Property(e => e.Reason)
                .HasMaxLength(300)
                .HasComment("السبب");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");

            entity.HasOne(d => d.Employee).WithMany(p => p.HrEmployeeLeaves)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Employee_Leaves_Employees");

            entity.HasOne(d => d.HrEmployeeLeaveBalance).WithMany(p => p.HrEmployeeLeaves)
                .HasForeignKey(d => d.HrEmployeeLeaveBalanceId)
                .HasConstraintName("FK_HR_Employee_Leaves_HR_Employee_LeaveBalance");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.HrEmployeeLeaves)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_Employee_Leaves_LeaveTypes");
        });

        modelBuilder.Entity<HrEmployeeLeaveBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HR_Emplo__3214EC272A44A34F");

            entity.ToTable("HR_Employee_LeaveBalance", tb => tb.HasComment("رصيد الإجازات السنوي (اعتيادي + عارضة)"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnnualRemainingDays).HasComment("باقي الايام العارضة");
            entity.Property(e => e.AnnualTotalDays)
                .HasDefaultValue(7m)
                .HasComment("إجمالي الإجازات العارضة في السنة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.AnnualUsedDays)
                .HasComment("ايام العارضة المستخدمة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CasualRemainingDays).HasComment("باقي الايام العارضة");
            entity.Property(e => e.CasualTotalDays)
                .HasDefaultValue(7m)
                .HasComment("إجمالي الإجازات العارضة في السنة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CasualUsedDays)
                .HasComment("ايام العارضة المستخدمة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.EmployeeId)
                .HasComment("رقم الموظف")
                .HasColumnName("Employee_ID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAnnualBalanceCalculated).HasComment("تم احتساب الإجازة على باقي الرصيد الاعتيادي (0 = لا، 1 = نعم)");
            entity.Property(e => e.TotalDays)
                .HasDefaultValue(30m)
                .HasComment("إجمالي الإجازات الاعتيادية في السنة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
            entity.Property(e => e.UsedDays)
                .HasComment("الايام المستخدمه فى السنة")
                .HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Employee).WithMany(p => p.HrEmployeeLeaveBalances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveBalance_Employee");
        });

        modelBuilder.Entity<HrJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Jobs");

            entity.ToTable("HR_Jobs", tb => tb.HasComment("الوظائف"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.MaxSalary).HasColumnType("money");
            entity.Property(e => e.MinSalary).HasColumnType("money");
            entity.Property(e => e.TitleAr)
                .HasMaxLength(100)
                .HasColumnName("Title_AR");
            entity.Property(e => e.TitleEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Title_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<HrJobGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HR_JobGr__3214EC271A6C9895");

            entity.ToTable("HR_JobGrades");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NameAr)
                .HasMaxLength(200)
                .HasColumnName("Name_AR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(200)
                .HasColumnName("Name_EN");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<HrLeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LeaveTypes");

            entity.ToTable("HR_LeaveTypes", tb => tb.HasComment("أنواع الأجازات"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.NameAr)
                .HasMaxLength(100)
                .HasColumnName("Name_AR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<HrMachineIp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Machine_IP");

            entity.ToTable("HR_Machine_IP", tb => tb.HasComment("������� ������"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BranchId).HasColumnName("Branch_ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.MachineIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MachineIP");
            entity.Property(e => e.MachineNameAr)
                .HasMaxLength(50)
                .HasColumnName("MachineName_AR");
            entity.Property(e => e.MachineNameEn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MachineName_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");

            entity.HasOne(d => d.Branch).WithMany(p => p.HrMachineIps)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Machine_IP_Branches");
        });

        modelBuilder.Entity<HrMachineMove>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MachineMoves");

            entity.ToTable("HR_MachineMoves", tb => tb.HasComment("��� ���� ������"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_UserId");
            entity.Property(e => e.DeletedDate).HasColumnName("Deleted_Date");
            entity.Property(e => e.DeletedUserId).HasColumnName("Deleted_UserId");
            entity.Property(e => e.MoveNameAr)
                .HasMaxLength(50)
                .HasColumnName("MoveName_AR");
            entity.Property(e => e.MoveNameEn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MoveName_EN");
            entity.Property(e => e.UpdatedDate).HasColumnName("Updated_Date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("Updated_UserId");
        });

        modelBuilder.Entity<LogInHistoryTb>(entity =>
        {
            entity.ToTable("LogInHistoryTb");

            entity.Property(e => e.LogInDate).HasColumnType("datetime");
            entity.Property(e => e.LogOutDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.LogInHistoryTbs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_LogInHistoryTb_PR_User");
        });

        modelBuilder.Entity<PrGroup>(entity =>
        {
            entity.ToTable("PR_Group");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GroupName).HasMaxLength(150);
            entity.Property(e => e.GroupNameEn)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("GroupName_En");
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.OrderBy).HasColumnName("Order_BY");
            entity.Property(e => e.PrApplicationCategoryId).HasColumnName("PR_ApplicationCategoryId");
            entity.Property(e => e.PrApplicationId).HasColumnName("PR_ApplicationId");
        });

        modelBuilder.Entity<PrGroupModuleMenu>(entity =>
        {
            entity.ToTable("PR_GroupModuleMenu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsActive).HasColumnName("IS_Active");
            entity.Property(e => e.OrderBy).HasColumnName("Order_BY");
            entity.Property(e => e.PrGroupId).HasColumnName("PR_GroupId");
            entity.Property(e => e.PrMenuId).HasColumnName("PR_MenuId");
            entity.Property(e => e.PrModuleId).HasColumnName("PR_ModuleId");

            entity.HasOne(d => d.PrGroup).WithMany(p => p.PrGroupModuleMenus)
                .HasForeignKey(d => d.PrGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PR_GroupModuleMenu_PR_Group");

            entity.HasOne(d => d.PrMenu).WithMany(p => p.PrGroupModuleMenus)
                .HasForeignKey(d => d.PrMenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PR_GroupModuleMenu_PR_Menu");

            entity.HasOne(d => d.PrModule).WithMany(p => p.PrGroupModuleMenus)
                .HasForeignKey(d => d.PrModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PR_GroupModuleMenu_PR_Module");
        });

        modelBuilder.Entity<PrMenu>(entity =>
        {
            entity.ToTable("PR_Menu");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.GroupId).HasColumnName("Group_Id");
            entity.Property(e => e.MenuTitle).HasMaxLength(150);
            entity.Property(e => e.MenuTitleEn)
                .HasMaxLength(150)
                .HasColumnName("MenuTitle_En");
            entity.Property(e => e.MenuUrl)
                .HasMaxLength(250)
                .HasColumnName("MenuURL");
            entity.Property(e => e.PrApplicationCategoryId).HasColumnName("PR_ApplicationCategoryId");
            entity.Property(e => e.PrApplicationId).HasColumnName("PR_ApplicationId");
            entity.Property(e => e.PrMenuId).HasColumnName("PR_MenuId");
            entity.Property(e => e.PrModuleId).HasColumnName("PR_ModuleId");
        });

        modelBuilder.Entity<PrModule>(entity =>
        {
            entity.ToTable("PR_Module");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ModuleDescription).HasMaxLength(250);
            entity.Property(e => e.ModuleName).HasMaxLength(150);
            entity.Property(e => e.ModuleNameEn)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ModuleName_En");
            entity.Property(e => e.PrApplicationCategoryId).HasColumnName("PR_ApplicationCategoryId");
            entity.Property(e => e.PrApplicationId).HasColumnName("PR_ApplicationId");
        });

        modelBuilder.Entity<PrSystem>(entity =>
        {
            entity.ToTable("PR_System");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.SystemNameAr)
                .HasMaxLength(150)
                .HasColumnName("SystemNameAR");
            entity.Property(e => e.SystemNameEn)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PrUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("PR_User");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EmployeesId).HasColumnName("Employees_ID");
            entity.Property(e => e.FullName).HasColumnName("Full_Name");
            entity.Property(e => e.SectorId).HasColumnName("Sector_ID");
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.Employees).WithMany(p => p.PrUsers)
                .HasForeignKey(d => d.EmployeesId)
                .HasConstraintName("FK_PR_User_Employees");
        });

        modelBuilder.Entity<PrUserGroup>(entity =>
        {
            entity.ToTable("PR_UserGroup");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PrGroupModuleMenuId).HasColumnName("PR_GroupModuleMenu_ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.PrGroupModuleMenu).WithMany(p => p.PrUserGroups)
                .HasForeignKey(d => d.PrGroupModuleMenuId)
                .HasConstraintName("FK_PR_UserGroup_PR_GroupModuleMenu");

            entity.HasOne(d => d.User).WithMany(p => p.PrUserGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PR_UserGroup_PR_User");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.ToTable("UserType");

            entity.Property(e => e.Id).HasColumnName("ID");
        });
        modelBuilder.HasSequence("A__User_Login_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("DataUpdatesLogTb_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("ExceptionLogTb_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HR_Branch_Department_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HR_Branches_SEQ")
            .StartsAt(2L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HR_Departments_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("HR_Employee_Attendance_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("HR_Employee_Lateness_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("HR_Employee_LeaveBalance_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("HR_Employee_Leaves_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("HR_Employees_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HR_JobGrades_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HR_Jobs_SEQ")
            .StartsAt(189L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("HR_LeaveTypes_SEQ")
            .StartsAt(4L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("HR_Machine_IP_SEQ")
            .StartsAt(2L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("HR_MachineMoves_SEQ")
            .StartsAt(3L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("LogInHistoryTb_SEQ")
            .StartsAt(110L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_Group_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_GroupModuleMenu_SEQ")
            .StartsAt(5046L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_Menu_SEQ")
            .StartsAt(63L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_Module_SEQ")
            .StartsAt(2L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_System_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_User_SEQ")
            .StartsAt(3L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PR_UserGroup_SEQ")
            .StartsAt(239L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("UserType_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
