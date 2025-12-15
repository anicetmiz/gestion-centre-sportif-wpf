using Semaine10Exercices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TP4.Models;
using TP4.Services;
using TP4.Views;

namespace TP4.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private CoursService cS;
        public ObservableCollection<TypeCours> TypeCours { get; set; }
        private TypeCours type;
        public TypeCours Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
                filtrer();


            }
        }

        private ObservableCollection<Cours> listCours;

        public ObservableCollection<Cours> ListCours
        {
            get
            {
                return listCours;
            }

            set
            {
                listCours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListCours)));


            }
        }
        private int total;
        public int Total
        {
            get => total;
            set
            {
                total = value;
            }
        }
        public MainWindowVM()
        {
            cS = new CoursService();
            TypeCours = new ObservableCollection<TypeCours>();
            //J,ajoute un objet typecOURS AVEC LE NOM Tous 
            TypeCours.Add(new TypeCours { TypeCoursId = 0, Nom = "Tous" });

            //remplisssage du comboBox
            foreach (TypeCours t in cS.GetTypeCours())
            {
                TypeCours.Add(t);
            }
            Type = TypeCours.First();




        }

        public void filtrer()
        {
            if (Type.TypeCoursId != 0)
            {
                ListCours = new ObservableCollection<Cours>(cS.GetCoursByType(Type));
            }
            else
            {
                ListCours = new ObservableCollection<Cours>(cS.GetCoursAll());
            }

        }
        private ICommand courSelection;
        public ICommand CourSelection
        {
            get
            {
                if (courSelection == null)
                {
                    courSelection = new RelayCommand(OuvrireGestionCours, CanOuvrireGestionCours);
                }
                return courSelection;
            }
        }

        private bool CanOuvrireGestionCours(object? obj)
        {
            return true;
        }

        private void OuvrireGestionCours(object? obj)
        {
            GestionCoursView fenetre = new GestionCoursView();
            GestionDesCoursVM vm = (GestionDesCoursVM)fenetre.DataContext;
            //sabonne a l'evenement et refais la demande a la bd quand levenement close et lancer
            vm.FenetreFermeeEvent += (s, e) =>
            {
                ListCours = new ObservableCollection<Cours>(cS.GetCoursAll());

            };

            fenetre.ShowDialog();
        }

        private ICommand membreSelection;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// public event PropertyChangedEventHandler? PropertyChanged;
        /// </summary>

        public ICommand MembreSelection
        {
            get
            {
                if (membreSelection == null)
                {
                    membreSelection = new RelayCommand(OuvrireGestionMembre, CanOuvrireGestionMembre);
                }
                return membreSelection;

            }
        }

        private bool CanOuvrireGestionMembre(object? obj)
        {
            return true;
        }

        private void OuvrireGestionMembre(object? obj)
        {
            GestionMembres fenetre = new GestionMembres();
            //GestionMembresVM vm = (GestionMembresVM)fenetre.DataContext;
            ////sabonne a l'evenement et refais la demande a la bd quand levenement close et lancer
            //vm.FenetreFermeeEvent += (s, e) =>
            //{
            //    ListCours = new ObservableCollection<Cours>(cS.GetCoursAll());

            //};
            fenetre.ShowDialog();
        }
    }
}
