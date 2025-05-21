using Inventar.DB;
using Inventar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Reflection.Metadata.BlobBuilder;

namespace Inventar.VM
{
    internal class HomeVM : BaseVM
    {
        private string search;
        private Equipment nashel;
        private ObservableCollection<Equipment> poiskSpisok = new();

        public ObservableCollection<Equipment> PoiskSpisok
        {
            get => poiskSpisok;
            set
            {
                poiskSpisok = value;
                Signal();
            }
        }

        public Equipment Nashel
        {
            get => nashel;
            set
            {
                nashel = value;
                Signal();
            }
        }

        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchOborud(search);
            }
        }

        public CommandMvvm Naznach { get; set; }
        public CommandMvvm Poisk { get; set; }
        public CommandMvvm Dobav { get; set; }

        public HomeVM()
        {
            SelectAll();

            Dobav = new CommandMvvm(() =>
            {
                DobavlenieWindow dobavlenie = new DobavlenieWindow();
                dobavlenie.ShowDialog();
                SelectAll();
            }, () => true);

            Poisk = new CommandMvvm(() =>
            {
                PoiskWindow poisk = new PoiskWindow();
                poisk.ShowDialog();
                SelectAll();
            }, () => true);

            Naznach = new CommandMvvm(() =>
            {
                NaznachenieWindow Naznachenie = new NaznachenieWindow();
                Naznachenie.ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            PoiskSpisok = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SelectAll());
        }

        private void SearchOborud(string search)
        {
            PoiskSpisok = new ObservableCollection<Equipment>(OborudPoisk.GetTable().SearchOborud(search));
        }

    }
}
