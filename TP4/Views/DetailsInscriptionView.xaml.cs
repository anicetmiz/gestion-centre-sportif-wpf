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
    /// Logique d'interaction pour DetailsInscriptionView.xaml
    /// </summary>
    public partial class DetailsInscriptionView : Window
    {
        public DetailsInscriptionView(DetailsInscriptionVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            vm.sauInEvent += (s, e) =>
            {
                if (e.Inscription == null)
                {
                    MessageBox.Show("impossible de s'inscrire a ce cours");
                }
                else
                {
                    MessageBox.Show($"inscription au cours {e.Inscription.Cours.Nom} reussi");
                }
            };
        }


    }
}
