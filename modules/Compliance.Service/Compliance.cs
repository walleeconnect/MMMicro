using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Service
{
    public class ComplianceDTO
    {
        public ComplianceDTO() { }
        public int Id { get; set; }
        public string ComplianceName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
