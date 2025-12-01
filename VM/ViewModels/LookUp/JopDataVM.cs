using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels
{
    public class JopDataVM
    {
        public int Id { get; set; }

        public string? TitleAr { get; set; }

        public string? TitleEn { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedUserId { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public DateOnly? UpdatedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateOnly? DeletedDate { get; set; }


        public virtual ICollection<HrBranchDepartment> HrBranchDepartments { get; set; } = new List<HrBranchDepartment>();

        public virtual ICollection<HrMachineIp> HrMachineIps { get; set; } = new List<HrMachineIp>();
    }
}
