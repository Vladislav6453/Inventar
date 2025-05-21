using Inventar.DB;
using Inventar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Inventar.VM
{
    public class DobavlenieVM : BaseVM
    {

        private ObservableCollection<JobTitle> jobtitles = new();
        private ObservableCollection<EquipmentTipe> equipmentTipes = new();
        private Visibility visibilityOborudovanie = Visibility.Collapsed;
        private Visibility visibilitySotrudnik = Visibility.Visible;
        private Employee employee = new Employee();
        private Equipment equipment = new Equipment();
        private Brush backGroundSotrudnik;
        private int minusBorderSotrudnik;
        private Brush backGroundOborud;
        private int minusBorderOborud;

        public ObservableCollection<JobTitle> JobTitles
        {
            get => jobtitles;
            set
            {
                jobtitles = value;
                Signal();
            }
        }

        public ObservableCollection<EquipmentTipe> EquipmentTipes
        {
            get => equipmentTipes;
            set
            {
                equipmentTipes = value;
                Signal();
            }
        }

        public Employee Employee
        {

            get => employee;
            set
            {
                employee = value;
                Signal();
            }
        }

        public Equipment Equipment
        {
            get => equipment;
            set
            {
                equipment = value;
                Signal();
            }
        }

        public Brush BackGroundSotrudnik 
        {
            get => backGroundSotrudnik;
            set 
            { 
                backGroundSotrudnik = value;
                Signal(); 
            }
        }

        public int MinusBorderSotrudnik
        {
            get => minusBorderSotrudnik;
            set 
            {
                minusBorderSotrudnik = value; 
                Signal();
            }
        }

        public Brush BackGroundOborud 
        {
            get => backGroundOborud;
            set 
            { 
                backGroundOborud = value; 
                Signal();
            }
        }

        public Visibility VisibilityOborudovanie
        {
            get => visibilityOborudovanie;
            set
            {
                visibilityOborudovanie = value;
                Signal();
            }
        }

        public Visibility VisibilitySotrudnik
        {
            get => visibilitySotrudnik;
            set
            {
                visibilitySotrudnik = value;
                Signal();
            }
        }

        public int MinusBorderOborud 
        { 
            get => minusBorderOborud;
            set
            { 
                minusBorderOborud = value; 
                Signal();
            }
        }

        public CommandMvvm Home { get; set; }
        public CommandMvvm Poisk { get; set; }
        public CommandMvvm Naznachit { get; set; }
        public CommandMvvm SmenaSotrudnik { get; set; }
        public CommandMvvm SmenaOborud { get; set; }
        public CommandMvvm Create {  get; set; }

        public DobavlenieVM()
        {
            VisibleSotrudnik();
            SelectAll();

            Naznachit = new CommandMvvm(() =>
            {
                NaznachenieWindow naznachenie = new NaznachenieWindow();
                this.close();
                naznachenie.ShowDialog();
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

            SmenaSotrudnik = new CommandMvvm(() =>
            {
                VisibleSotrudnik();
                SelectAll();
            }, () => true);

            SmenaOborud = new CommandMvvm(() =>
            {
                BackGroundOborud = Brushes.Gray;
                MinusBorderOborud = 3;
                BackGroundSotrudnik = Brushes.Transparent;
                MinusBorderSotrudnik = 0;
                VisibilitySotrudnik = Visibility.Collapsed;
                VisibilityOborudovanie = Visibility.Visible;
                SelectAll();
            }, () => true);

            Create = new CommandMvvm(() =>
            {
                if(VisibilityOborudovanie == Visibility.Visible)
                {
                    Equipment.IDEquipmentTipe = Equipment.EquipmentTipe.ID;
                    EquipmentDB.GetDb().Insert(Equipment);
                    Equipment.Name = null;
                    Equipment.InventoryNumber = 0;
                    Equipment.DateOfPurchase = DateTime.Now;
                    Equipment.ServiceLife = 0;
                    Equipment.Price = 0;
                    Equipment.EquipmentTipe = null;
                }

                if (VisibilitySotrudnik == Visibility.Visible)
                {
                    Employee.IDJobTitle = Employee.JobTitle.ID;
                    EmployeeDB.GetDb().Insert(Employee);
                    Employee.FirstName = string.Empty;
                    Employee.LastName = string.Empty;
                    Employee.SurName = string.Empty;
                    Employee.PhoneNumber = string.Empty;
                    Employee.WorkExperience = 0;
                    Employee.Email = string.Empty;
                    Employee.JobTitle = null;
                }
            }, 
            () =>
            {
                if (VisibilityOborudovanie == Visibility.Visible)
                    return Equipment !=  null &&
                Equipment.Name != null &&
                Equipment.InventoryNumber != 0 &&
                Equipment.DateOfPurchase <= DateTime.Now &&
                Equipment.ServiceLife != 0 &&
                Equipment.Price != 0 &&
                Equipment.EquipmentTipe != null;
                else
                    return Employee != null &&
                Employee.FirstName != string.Empty &&
                Employee.LastName != null &&
                Employee.SurName != null &&
                Employee.PhoneNumber != null &&
                Employee.WorkExperience != 0 &&
                Employee.Email != null &&
                Employee.JobTitle != null;
            }
            );
        }

        private void VisibleSotrudnik()
        {
            BackGroundSotrudnik = Brushes.Gray;
            MinusBorderSotrudnik = 3;
            BackGroundOborud = Brushes.Transparent;
            MinusBorderOborud = 0;
            VisibilityOborudovanie = Visibility.Collapsed;
            VisibilitySotrudnik = Visibility.Visible;
        }

        private void SelectAll()
        {
            JobTitles = new ObservableCollection<JobTitle>(JobTitleDB.GetDb().SelectAll());
            EquipmentTipes = new ObservableCollection<EquipmentTipe>(EquipmentTipeDB.GetDb().SelectAll());
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
