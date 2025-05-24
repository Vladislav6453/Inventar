using Inventar.DB;
using Inventar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventar.VM
{
    internal class NaznachenieVM : BaseVM
    {
        private ObservableCollection<Employee> employees = new();
        private ObservableCollection<Equipment> equipments = new();
        private ObservableCollection<Appointment> spisokNaznach = new();
        private Employee newowner;
        private Equipment shtuka;
        private DateTime ot;
        private DateTime doo;
        private string search;

        public string Search
        {
            get => search;
            set
            {
                search = value;
                SelectAll();
            }
        }
        public ObservableCollection<Appointment> SpisokNaznach
        {
            get => spisokNaznach;
            set
            {
                spisokNaznach = value;
                Signal();
            }
        }
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
                bool Peresech = false;
                Appointment appointment = new Appointment();
                foreach (var naznach in appointments)
                {
                    if(appointment.OverLaps(naznach))
                    {
                        Peresech = true;
                        MessageBoxResult Oshibka = MessageBox.Show(
                        "Вы не можете создать это назначение, потому  что этот временной интервал уже занят.",
                        MessageBoxButton.Yes,
                        MessageBoxImage.Question);
                        if (Oshibka == MessageBoxResult.Yes)
                        {
                            SelectAll();
                            Ot = DateTime.Now;
                            Do = DateTime.Now;
                        }
                        break;
                    }

                }
                
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
            SpisokNaznach = new ObservableCollection<Appointment>(AppointmentDB.GetDb().SelectAll(Search));
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

        public bool OverLaps(Appointment other)
        {
            if(NewOwner.ID == other.EmployeeID || Shtuka.ID == other.ID)
            {
                    return Ot < other.ReturnDate && Do > other.EquipmentDate;
            }
            return false;
            
        }
    }
}
