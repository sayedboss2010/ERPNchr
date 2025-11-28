using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedUserId { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public DateOnly? UpdatedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateOnly? DeletedDate { get; set; }
    }
}
