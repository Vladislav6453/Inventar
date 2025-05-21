using Inventar.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventar
{
    /// <summary>
    /// Логика взаимодействия для Poisk.xaml
    /// </summary>
    public partial class PoiskWindow : Window
    {
        public PoiskWindow()
        {
            InitializeComponent();
            (DataContext as PoiskVM).SetClose(Close);
        }
    }
}
