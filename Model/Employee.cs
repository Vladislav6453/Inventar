using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar.Model
{
    public class Employee
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string PhoneNumber { get; set; }
        public double WorkExperience { get; set; }
        public string Email { get; set; }
        public JobTitle JobTitle { get; set; }
        public int IDJobTitle { get; set; }
        public string Ktokto => FirstName + " " + SurName;
    }
}
