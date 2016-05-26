using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebArchitecture.Entities.Models
{
    public class Student
    {
        [StringLength(7)]
        public string StudentID { get; set; }
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string PreferredName { get; set; }

        public virtual IEnumerable<Application> Applications { get; set; }

    }
}
