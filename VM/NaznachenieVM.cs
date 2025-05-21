using Inventar.DB;
using Inventar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar.VM
{
    internal class NaznachenieVM : BaseVM
    {
        private ObservableCollection<Employee> employees = new();
        private ObservableCollection<Equipment> equipments = new();
        private Employee newowner;
        private Equipment shtuka;
        private DateTime ot;
        private DateTime doo;

        public DateTime Ot
        {
            get => ot;
            set
            {
                ot = value;
                Signal();
            }
        }

        public DateTime Do
        {
            get => doo;
            set
            {
                doo = value;
                Signal();
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                Signal();
            }
        }

        public ObservableCollection<Equipment> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                Signal();
            }
        }

        public Employee NewOwner
        {
            get => newowner;
            set
            {
                newowner = value;
                Signal();
            }
        }

        public Equipment Shtuka
        {
            get => shtuka;
            set
            {
                shtuka = value;
                Signal();
            }
        }

        public CommandMvvm Home { get; set; }
        public CommandMvvm Poisk { get; set; }
        public CommandMvvm Dobav { get; set; }
        public CommandMvvm Naznachit { get; set; }

        public NaznachenieVM()
        {
            SelectAll();

            Dobav = new CommandMvvm(() =>
            {
                DobavlenieWindow dobavlenie = new DobavlenieWindow();
                this.close();
                dobavlenie.ShowDialog();
                SelectAll();
            }, () => true);

            Poisk = new CommandMvvm(() =>
            {
                PoiskWindow poisk = new PoiskWindow();
                this.close();
                poisk.ShowDialog();
                SelectAll();
            }, () => true);

            Home = new CommandMvvm(() =>
            {
                HomeWindow Home = new HomeWindow();
                this.close();
                Home.ShowDialog();
                SelectAll();
            }, () => true);

            Naznachit = new CommandMvvm(() =>
            {
                Appointment appointment = new Appointment();
                appointment.EmployeeID = NewOwner.ID;
                appointment.EquipmentID = Shtuka.ID;
                appointment.EquipmentDate = Ot;
                appointment.ReturnDate = Do;
                AppointmentDB.GetDb().Insert(appointment);
                NewOwner = null;
                Shtuka = null;
                Ot = DateTime.Now;
                Do = DateTime.Now;
            }, 
            () => 
            NewOwner != null &&
            Shtuka != null &&
            Ot != null &&
            Do != null &&
            Do > Ot
            );
        }

        private void SelectAll()
        {
            Employees = new ObservableCollection<Employee>(EmployeeDB.GetDb().SelectAll());
            Equipments = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
