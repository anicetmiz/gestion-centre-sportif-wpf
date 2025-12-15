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
using TP4.ViewModels;

namespace TP4.Views
{
    /// <summary>
    /// Logique d'interaction pour GestionMembres.xaml
    /// </summary>
    public partial class GestionMembres : Window
    {
        public GestionMembres()
        {
            InitializeComponent();
            GestionMembresVM vm = new GestionMembresVM();
            this.DataContext = vm;
            vm.removeEvent += (s, e) =>
            {
                if (e.Membre != null)
                {
                    MessageBox.Show("Membre supprimer avec succée");

                }
                else
                {
                    MessageBox.Show("Membre non supprimer");
                }

                this.Closed += (s, e) =>
                {
                    vm.NotifierFermeture();
                };

            };
        }
    }
}
