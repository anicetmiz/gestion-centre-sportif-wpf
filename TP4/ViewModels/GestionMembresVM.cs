using Semaine10Exercices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TP4.Events;
using TP4.Models;
using TP4.Services;
using TP4.Views;

namespace TP4.ViewModels
{
    public class GestionMembresVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly MembreService mS;
        private ObservableCollection<Membre> listMembres = new ObservableCollection<Membre>();

        public ObservableCollection<Membre> ListMembres
        {
            get => listMembres;
            set
            {
                listMembres = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListMembres)));

            }
        }
        public event EventHandler<RemoveMembreEvent>? removeEvent;
        public event EventHandler? FenetreFermeeEvent;

        public Membre Membre { get; set; }
        public GestionMembresVM()
        {
            mS = new MembreService();
            ListMembres = new ObservableCollection<Membre>(mS.GetMembreAll());


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
                    cree = new RelayCommand(AddMembre, CanAddMembre);
                return cree;
            }

        }

        private bool CanAddMembre(object? obj)
        {
            return true;
        }

        private void AddMembre(object? obj)
        {
            DetailsMembreVM vm = new DetailsMembreVM();
            DetailsMembreView fenetre = new DetailsMembreView(vm);
            vm.sauMeEvent += (s, e) =>
            {
                if (e.Membre != null)
                    ListMembres.Add(e.Membre);

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
                    modifier = new RelayCommand(UpdateMembre, CanUpdateMembre);
                }
                return modifier;
            }
        }

        private bool CanUpdateMembre(object? obj)
        {
            if (Membre == null)
            {
                return false;
            }
            return true;

        }

        private void UpdateMembre(object? obj)
        {
            DetailsMembreVM vm = new DetailsMembreVM(Membre);
            DetailsMembreView fenetre = new DetailsMembreView(vm);
            vm.sauMeEvent += (s, e) =>
            {
                if (e.Membre != null)
                    ListMembres = new ObservableCollection<Membre>(mS.GetMembreAll());

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
            if (Membre == null)
            {
                return false;
            }
            return true;
        }
        private void RemoveCours(object? obj)
        {
            mS.RemoveMembre(Membre);

            removeEvent?.Invoke(this, new RemoveMembreEvent { Membre = Membre });

            ListMembres.Remove(Membre);
        }
    }
}
