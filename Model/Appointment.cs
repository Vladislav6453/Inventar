using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar.Model
{
    public class Appointment
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public int EquipmentID { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime EquipmentDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
