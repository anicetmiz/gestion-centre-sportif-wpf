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
    /// Logique d'interaction pour DetailsMembreView.xaml
    /// </summary>
    public partial class DetailsMembreView : Window
    {
        public DetailsMembreView(DetailsMembreVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            vm.sauMeEvent += MembreRecu;
            vm.AnnuleEvent += (s, e) =>
            {
                this.Close();
            };
        }
        private void MembreRecu(object sender, SauvegardeMeEvent e)
        {
            if (e.Membre != null)
            {
                this.Close();
                MessageBox.Show("Membre sauvegarder avec succe");
            }
            else
            {
                MessageBox.Show("Membre non sauvegarder");
            }
        }
    }
}

