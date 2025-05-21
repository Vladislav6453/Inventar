using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar.Model
{
    public class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InventoryNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public double ServiceLife { get; set; }
        public decimal Price { get; set; }
        public int IDEquipmentTipe { get; set; }
        public EquipmentTipe EquipmentTipe { get; set; }
    }
}
