using Microsoft.Identity.Client;
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

namespace TP4.ViewModels
{
    public class DetailsInscriptionVM : INotifyPropertyChanged
    {
        private Membre _membre;
        CoursService cS = new CoursService();
        InscriptionService iS = new InscriptionService();
        public ObservableCollection<Cours> ListCours { get; set; }

        private ObservableCollection<Inscription> _listInscriptions;
        public ObservableCollection<Inscription> ListInscriptions
        {
            get
            {
                return _listInscriptions;
            }

            set
            {
                _listInscriptions = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListInscriptions)));
            }
        }
        public Inscription Inscription { get; set; }
        public Cours Cours { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<SauvegardeInEvent>? sauInEvent;
        public DetailsInscriptionVM(Membre membre)
        {
            _membre = membre;
            ListCours = new ObservableCollection<Cours>(cS.GetCoursAll().Where(a => !a.Inscriptions.Any(a => a.MembreId == _membre.MembreId)));
            ListInscriptions = new ObservableCollection<Inscription>(iS.GetInscriptionsMembre(membre));

        }

        private ICommand ajouter;
        public ICommand Ajouter
        {
            get
            {
                if (ajouter == null)
                {
                    ajouter = new RelayCommand(AddInscription, CanAddInscription);
                }
                return ajouter;
            }
        }

        private bool CanAddInscription(object? obj)
        {

            if (Cours == null)
            {
                return false;
            }
            return true;
        }

        private void AddInscription(object? obj)
        {
            Inscription? NewInscription = new Inscription();
            bool test = false;

            NewInscription = iS.AddInscription(Cours, _membre);
            if (NewInscription != null)
            {
                ListInscriptions.Add(NewInscription);

                test = ListInscriptions.Any(a => a.InscriptionId == NewInscription.InscriptionId);
            }
            if (test)
            {
                ListCours.Remove(Cours);
            }


            sauInEvent?.Invoke(this, new SauvegardeInEvent { Inscription = NewInscription });
        }
        private ICommand supprimer;
        public ICommand Supprimer
        {
            get
            {
                supprimer = new RelayCommand(RemoveInscription, CanRemoveInscription);
                return supprimer;
            }
        }
        private bool CanRemoveInscription(object? obj)
        {
            if (Inscription == null || Inscription.Cours == null)
            {
                return false;
            }


            if (Inscription.Cours.DateDebut < DateTime.Today)
            {
                return false;
            }
            return true;
        }
        private void RemoveInscription(object? obj)
        {
            iS.RemoveInscription(Inscription);
            ListCours.Add(Inscription.Cours);
            ListInscriptions.Remove(Inscription);

        }

    }
}
