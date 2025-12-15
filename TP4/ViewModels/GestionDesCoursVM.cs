using Semaine10Exercices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TP4.Models;
using TP4.Services;
using TP4.Events;
using TP4.Views;
using Microsoft.Extensions.Logging.Abstractions;

namespace TP4.ViewModels
{

    public class GestionDesCoursVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly CoursService cS;
        private ObservableCollection<Cours> listCours;
        public ObservableCollection<Cours> ListCours
        {
            get => listCours;
            set
            {
                listCours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListCours)));

            }
        }

        public event EventHandler<RemoveEvent>? removeEvent;
        public event EventHandler? FenetreFermeeEvent;

        public Cours Cours { get; set; }
        public GestionDesCoursVM()
        {
            cS = new CoursService();
            ListCours = new ObservableCollection<Cours>(cS.GetCoursAll());


        }
        // pour detecter la fermeture a fin d'actualiser le main
        public void NotifierFermeture()
        {
            FenetreFermeeEvent?.Invoke(this, EventArgs.Empty);
        }


        private ICommand cree;
        public ICommand Cree
        {
            get
            {
                if (cree == null)
                    cree = new RelayCommand(AddCours, CanAddCours);
                return cree;
            }

        }

        private bool CanAddCours(object? obj)
        {
            return true;
        }

        private void AddCours(object? obj)
        {
            DetailsCoursVM vm = new DetailsCoursVM();
            DetailsCoursView fenetre = new DetailsCoursView(vm);



            vm.sauCoEvent += (s, e) =>
            {
                if (e.Cours != null)
                    ListCours.Add(e.Cours);

            };
            fenetre.ShowDialog();
        }

        private ICommand modifier;
        public ICommand Modifier
        {
            get
            {
                if (modifier == null)
                {
                    modifier = new RelayCommand(UpdateCours, CanUpdateCours);
                }
                return modifier;
            }
        }

        private bool CanUpdateCours(object? obj)
        {
            if (Cours == null)
            {
                return false;
            }
            return true;

        }

        private void UpdateCours(object? obj)
        {
            DetailsCoursVM vm = new DetailsCoursVM(Cours);
            DetailsCoursView fenetre = new DetailsCoursView(vm);



            vm.sauCoEvent += (s, e) =>
            {
                if (e.Cours != null)
                    ListCours = new ObservableCollection<Cours>(cS.GetCoursAll());

            };
            fenetre.ShowDialog();
        }

        private ICommand supprimer;
        public ICommand Supprimer
        {
            get
            {
                supprimer = new RelayCommand(RemoveCours, CanRemoveCours);
                return supprimer;
            }
        }
        private bool CanRemoveCours(object? obj)
        {
            if (Cours == null)
            {
                return false;
            }
            return true;
        }
        private void RemoveCours(object? obj)
        {
            cS.RemoveCours(Cours);

            removeEvent?.Invoke(this, new RemoveEvent { Cours = Cours });

            ListCours.Remove(Cours);
        }


    }
}
