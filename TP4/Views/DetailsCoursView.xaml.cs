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
using TP4.Events;
using TP4.ViewModels;

namespace TP4.Views
{
    /// <summary>
    /// Logique d'interaction pour DetailsCoursView.xaml
    /// </summary>
    public partial class DetailsCoursView : Window
    {
        public DetailsCoursView(DetailsCoursVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            vm.sauCoEvent += CoursRecu;
            vm.AnnuleEvent += (s, e) =>
            {
                this.Close();
            };
        }
        private void CoursRecu(object sender, SauvegardeCoEvent e)
        {
            if (e.Cours != null)
            {
                this.Close();
                MessageBox.Show("Cours sauvegarder avec succe");
            }
            else
            {
                MessageBox.Show("Cours non sauvegarder");
            }
        }
    }
}
