using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebArchitecture.Entities.Models
{
    public enum ApplicationStatus
    {
        New,
        InProcess,
        Approved,
        Rejected,
        OnHold
    }

    public class Application
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus Status { get; set; }
    }

}
