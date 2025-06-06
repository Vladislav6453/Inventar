using Inventar.DB;
using Inventar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using static System.Reflection.Metadata.BlobBuilder;

namespace Inventar.VM 
{
    internal class PoiskVM : BaseVM
    {
        private ObservableCollection<JobTitle> jobtitles = new();
        private ObservableCollection<EquipmentTipe> equipmentTipes = new();
        private ObservableCollection<Employee> veborEmployee = new();
        private ObservableCollection<Equipment> veborEquipment = new();
        private Employee employee = new Employee();
        private Equipment equipment = new Equipment();
        private Appointment appointment = new Appointment();
        private Brush backGroundSotrudnik = Brushes.Gray;
        private Brush backGroundOborud;
        private Brush backGroundNaznach;
        private int minusBorderSotrudnik = 3;
        private int minusBorderOborud;
        private int minusBorderNaznach;
        private ObservableCollection<Appointment> spisokNaznach = new();
        private ObservableCollection<Equipment> spisokOborud = new();
        private ObservableCollection<Employee> spisokSotrudnik = new();
        private Employee selectedSotrudnik;
        private Equipment selectedOborud;
        private Appointment selectedNaznach;
        private Visibility visibilitySpisokSotrudnik = Visibility.Visible;
        private Visibility visibilitySpisokOborud = Visibility.Collapsed;
        private Visibility visibilitySpisokNaznach = Visibility.Collapsed;
        private Visibility visibilitySotrudnik = Visibility.Visible;
        private Visibility visibilityOborud = Visibility.Collapsed;
        private Visibility visibilityNaznach = Visibility.Collapsed;
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

        public Visibility VisibilitySotrudnik
        {
            get => visibilitySotrudnik;
            set
            {
                visibilitySotrudnik = value;
                Signal();
            }
        }

        public Visibility VisibilityOborud
        {
            get => visibilityOborud;
            set
            {
                visibilityOborud = value;
                Signal();
            }
        }

        public Visibility VisibilityNaznach
        {
            get => visibilityNaznach;
            set
            {
                visibilityNaznach = value;
                Signal();
            }
        }

        public ObservableCollection<Employee> SpisokSotrudnik
        {
            get => spisokSotrudnik;
            set
            {
                spisokSotrudnik = value;
                Signal();
            }
        }

        public ObservableCollection<Equipment> SpisokOborud
        {
            get => spisokOborud;
            set
            {
                spisokOborud = value;
                Signal();
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

        public int MinusBorderSotrudnik
        {
            get => minusBorderSotrudnik;
            set
            {
                minusBorderSotrudnik = value;
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

        public int MinusBorderNaznach
        {
            get => minusBorderNaznach;
            set
            {
                minusBorderNaznach = value;
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

        public Brush BackGroundOborud
        {
            get => backGroundOborud;
            set
            {
                backGroundOborud = value;
                Signal();
            }
        }

        public Brush BackGroundNaznach
        {
            get => backGroundNaznach;
            set
            {
                backGroundNaznach = value;
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

        public Appointment Appointment
        {
            get => appointment;
            set
            {
                appointment = value;
                Signal();
            }
        }

        public Employee SelectedSotrudnik
        {
            get => selectedSotrudnik;
            set
            {
                selectedSotrudnik = value;
                if(selectedSotrudnik != null)
                    SelectedSotrudnik.JobTitle = JobTitles.FirstOrDefault(title => title.ID == SelectedSotrudnik.IDJobTitle);
                Signal();
                

            }
        }

        public Equipment SelectedOborud
        {
            get => selectedOborud;
            set
            {
                selectedOborud = value;
                if (selectedOborud != null)
                    SelectedOborud.EquipmentTipe = EquipmentTipes.FirstOrDefault(name => name.ID == SelectedOborud.IDEquipmentTipe);
                Signal();
            }
        }

        public Appointment SelectedNaznach
        {
            get => selectedNaznach;
            set
            {

                selectedNaznach = value;
                if (selectedNaznach != null)
                {
                    SelectedNaznach.Employee = VeborEmployee.FirstOrDefault(emp => emp.ID == selectedNaznach.EmployeeID);
                    SelectedNaznach.Equipment = VeborEquipment.FirstOrDefault(eq => eq.ID == selectedNaznach.EquipmentID);
                }
                    
                Signal();
            }
        }

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

        public ObservableCollection<Employee> VeborEmployee
        {
            get => veborEmployee;
            set
            {
                veborEmployee = value;
                Signal();
            }
        }

        public ObservableCollection<Equipment> VeborEquipment
        {
            get => veborEquipment;
            set
            {
                veborEquipment = value;
                Signal();
            }
        }

        public Visibility VisibilitySpisokSotrudnik
        {
            get => visibilitySpisokSotrudnik;
            set
            {
                visibilitySpisokSotrudnik = value;
                Signal();
            }
        }

        public Visibility VisibilitySpisokOborud
        {
            get => visibilitySpisokOborud;
            set
            {
                visibilitySpisokOborud = value;
                Signal();
            }
        }

        public Visibility VisibilitySpisokNaznach
        {
            get => visibilitySpisokNaznach;
            set
            {
                visibilitySpisokNaznach = value;
                Signal();
            }
        }

        public CommandMvvm Naznach { get; set; }
        public CommandMvvm Home { get; set; }
        public CommandMvvm Dobav { get; set; }
        public CommandMvvm RedactSotrudnik { get; set; }
        public CommandMvvm RedactOborud { get; set; }
        public CommandMvvm RedactNaznach { get; set; }
        public CommandMvvm Redact { get; set; }
        public CommandMvvm Delete { get; set; }

        public PoiskVM()
        {
            SelectAll();

            Redact = new CommandMvvm(() =>
            {
                if (VisibilitySotrudnik == Visibility.Visible)
                {
                    SelectedSotrudnik.IDJobTitle = SelectedSotrudnik.JobTitle.ID;
                    if (SelectedSotrudnik.ID == 0)
                    {
                        EmployeeDB.GetDb().Insert(SelectedSotrudnik);
                    }
                    else
                        EmployeeDB.GetDb().Update(SelectedSotrudnik);
                    SelectAll();
                }

               else if (VisibilityOborud == Visibility.Visible)
                {
                    SelectedOborud.IDEquipmentTipe = SelectedOborud.EquipmentTipe.ID;
                    if (SelectedOborud.ID == 0)
                    {
                        EquipmentDB.GetDb().Insert(SelectedOborud);
                    }
                    else
                        EquipmentDB.GetDb().Update(SelectedOborud);
                    SelectAll();
                }

              else  if (VisibilityNaznach == Visibility.Visible)
                {
                    SelectedNaznach.EmployeeID = SelectedNaznach.Employee.ID;
                    SelectedNaznach.EquipmentID = SelectedNaznach.Equipment.ID;
                    if (SelectedNaznach.ID == 0)
                    {
                        AppointmentDB.GetDb().Insert(SelectedNaznach);
                    }
                    else
                        AppointmentDB.GetDb().Update(SelectedNaznach);
                    SelectAll();
                }
            } ,
            () =>              true
            );

            Delete = new CommandMvvm(() =>
            {
               MessageBoxResult vebor = MessageBox.Show(
               "Вы точно это хотите удалить?",
               "Подтверждение",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question);
                if (vebor == MessageBoxResult.Yes)
                {
                    if(VisibilitySotrudnik == Visibility.Visible)
                    {
                        foreach(var naznach in SpisokNaznach)
                        {
                            if(naznach.EmployeeID == SelectedSotrudnik.ID)
                            {
                                MessageBox.Show(
                                "Вы не можете удалить этого сотрудника, потому что он уже находится в назначении.", "Ок",
                                MessageBoxButton.OK,
                                MessageBoxImage.Question);
                                SelectAll();
                                return;
                            }
                        }
                        EmployeeDB.GetDb().Remove(SelectedSotrudnik);
                        SelectAll();

                    }
                    if (VisibilityOborud == Visibility.Visible)
                    {
                        foreach (var naznach in SpisokNaznach)
                        {
                            if (naznach.EquipmentID == SelectedOborud.ID)
                            {
                                MessageBox.Show(
                                "Вы не можете удалить это оборудование, потому что оно уже находится в назначении.", "Ок",
                                MessageBoxButton.OK,
                                MessageBoxImage.Question);
                                SelectAll();
                                return;
                            }
                        }
                        EquipmentDB.GetDb().Remove(SelectedOborud);
                        SelectAll();
                    }
                    if (VisibilityNaznach == Visibility.Visible)
                    {
                        AppointmentDB.GetDb().Remove(SelectedNaznach);
                        SelectAll();
                    }
                }
                SelectAll();
            }, () => true);

            RedactSotrudnik = new CommandMvvm(() =>
            {
                BackGroundSotrudnik = Brushes.Gray;
                MinusBorderSotrudnik = 3;
                BackGroundOborud = Brushes.Transparent;
                MinusBorderOborud = 0;
                BackGroundNaznach = Brushes.Transparent;
                MinusBorderNaznach = 0;
                VisibilitySotrudnik = Visibility.Visible;
                VisibilityOborud = Visibility.Collapsed;
                VisibilityNaznach = Visibility.Collapsed;
                VisibilitySpisokSotrudnik = Visibility.Visible;
                VisibilitySpisokOborud = Visibility.Collapsed;
                VisibilitySpisokNaznach = Visibility.Collapsed;
                SelectAll();
            }, () => true);

            RedactOborud = new CommandMvvm(() =>
            {
                BackGroundSotrudnik = Brushes.Transparent;
                MinusBorderSotrudnik = 0;
                BackGroundOborud = Brushes.Gray;
                MinusBorderOborud = 3;
                BackGroundNaznach = Brushes.Transparent;
                MinusBorderNaznach = 0;
                VisibilitySotrudnik = Visibility.Collapsed;
                VisibilityOborud = Visibility.Visible;
                VisibilityNaznach = Visibility.Collapsed;
                VisibilitySpisokSotrudnik = Visibility.Collapsed;
                VisibilitySpisokOborud = Visibility.Visible;
                VisibilitySpisokNaznach = Visibility.Collapsed;
                SelectAll();
            }, () => true);

            RedactNaznach = new CommandMvvm(() =>
            {
                BackGroundSotrudnik = Brushes.Transparent;
                MinusBorderSotrudnik = 0;
                BackGroundOborud = Brushes.Transparent;
                MinusBorderOborud = 0;
                BackGroundNaznach = Brushes.Gray;
                MinusBorderNaznach = 3;
                VisibilitySotrudnik = Visibility.Collapsed;
                VisibilityOborud = Visibility.Collapsed;
                VisibilityNaznach = Visibility.Visible;
                VisibilitySpisokSotrudnik = Visibility.Collapsed;
                VisibilitySpisokOborud = Visibility.Collapsed;
                VisibilitySpisokNaznach = Visibility.Visible;
                SelectAll();
            }, () => true);

            Home = new CommandMvvm(() =>
            {
                HomeWindow Home = new HomeWindow();
                this.close();
                Home.ShowDialog();
                SelectAll();
            }, () => true);

            Dobav = new CommandMvvm(() =>
            {
                DobavlenieWindow dobavlenie = new DobavlenieWindow();
                this.close();
                dobavlenie.ShowDialog();
                SelectAll();
            }, () => true);

            Naznach = new CommandMvvm(() =>
            {
                NaznachenieWindow naznachenie = new NaznachenieWindow();
                this.close();
                naznachenie.ShowDialog();
                SelectAll();
            }, () => true);
        }
        private void SelectAll()
        {
            JobTitles = new ObservableCollection<JobTitle>(JobTitleDB.GetDb().SelectAll());
            JobTitles = new ObservableCollection<JobTitle>(JobTitleDB.GetDb().SelectAll());
            EquipmentTipes = new ObservableCollection<EquipmentTipe>(EquipmentTipeDB.GetDb().SelectAll());
            VeborEmployee = new ObservableCollection<Employee>(EmployeeDB.GetDb().SelectAll());
            VeborEquipment = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SelectAll());
            SpisokSotrudnik = new ObservableCollection<Employee>(EmployeeDB.GetDb().SelectAll(Search));
            SpisokOborud = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SelectAll(Search));
            SpisokNaznach = new ObservableCollection<Appointment>(AppointmentDB.GetDb().SelectAll(Search));
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
