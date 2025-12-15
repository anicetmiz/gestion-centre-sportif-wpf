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
    public class DetailsMembreVM : IDataErrorInfo
    {
        public Membre _membre;


        public string Prenom
        {
            get
            {
                return _membre.Prenom;
            }
            set
            {
                _membre.Prenom = value;
                // est ce que il faut onepropretiechanged ?
            }
        }

        public string Nom
        {
            get
            {
                return _membre.Nom;
            }
            set
            {
                _membre.Nom = value;
                // est ce que il faut onepropretiechanged ?
            }
        }
        public DateTime? DateNaissance
        {
            get
            {
                return _membre.DateNaissance;
            }
            set
            {
                _membre.DateNaissance = value;
                // est ce que il faut onepropretiechanged ?
            }
        }
        private Dictionary<string, string> errorList;
        public string Error
        {
            get
            {
                return "";
            }
        }
        private MembreService mS = new MembreService();
        public event EventHandler<SauvegardeMeEvent>? sauMeEvent;
        public event EventHandler<EventArgs>? AnnuleEvent;
        public DetailsMembreVM()
        {
            _membre = new Membre();
            DateNaissance = null;


            errorList = new Dictionary<string, string>();

            errorList[nameof(Prenom)] = "";
            errorList[nameof(Nom)] = "";
            errorList[nameof(DateNaissance)] = "";

        }
        public DetailsMembreVM(Membre membre)
        {
            _membre = membre;

            


            errorList = new Dictionary<string, string>();

            errorList[nameof(Prenom)] = "";
            errorList[nameof(Nom)] = "";
            errorList[nameof(DateNaissance)] = "";
        }
        public string this[string propertyName]
        {
            get
            {
                string result = "";

                switch (propertyName)
                {
                    case nameof(Prenom):
                        if (string.IsNullOrEmpty(this.Nom))
                        {
                            result = "LE PRENOM NE PEUT PAS ETRE VIDE";
                            errorList[propertyName] = result;
                        }
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(Nom):
                        if (string.IsNullOrEmpty(this.Nom))
                        {
                            result = "LE NOM NE PEUT PAS ETRE VIDE";
                            errorList[propertyName] = result;
                        }
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(DateNaissance):
                        if (this.DateNaissance == null)
                        {
                            result = " LA DATE NAISSANCE DOIT PAS ETRE VIDE ";
                            errorList[propertyName] = result;
                        }
                        else if (this.DateNaissance >= DateTime.Today)
                        {
                            result = " LA DATE NAISSANCE DOIT ETRE DANS LE PASSÉ ";
                            errorList[propertyName] = result;
                        }
                       
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;


                }
                return result;
            }
        }



        private ICommand inscription;
        public ICommand Inscription
        {
            get
            {
                if (inscription == null)
                    inscription = new RelayCommand(InscriptionMembre, CanInscriptionMembre);
                return inscription;
            }

        }

        private bool CanInscriptionMembre(object? obj)
        {
            if (_membre.MembreId == 0)
            {
                return false;
            }
            return true;
        }

        private void InscriptionMembre(object? obj)
        {
            DetailsInscriptionVM vm = new DetailsInscriptionVM(_membre);
            DetailsInscriptionView fenetre = new DetailsInscriptionView(vm);
            //vm.sauMeEvent += (s, e) =>
            //{
            //    if (e.Membre != null)
            //        ListMembres.Add(e.Membre);

            //};
            fenetre.ShowDialog();
        }

        private ICommand sauvegarder;
        public ICommand Sauvegarder
        {
            get
            {
                if (sauvegarder == null)
                {
                    sauvegarder = new RelayCommand(SaveMembre, CanSaveMembre);
                }
                return sauvegarder;
            }
        }

        private bool CanSaveMembre(object? obj)
        {
            // valider le chamd calculer 
            //string.IsNullOrEmpty(Nom) || Cout == 0 || Cout % 2 != 0 || DateDebut > DateFin || Type == null
            if (errorList.Values.Any(e => !string.IsNullOrEmpty(e)))
            {
                return false;
            }
            //if (errorList.Any())
            //{
            //    return false;
            //}
            return true;
        }

        private void SaveMembre(object? obj)
        {
            Membre? NouveauMembre = new Membre();
            if (_membre.MembreId == 0)
            {
                NouveauMembre = mS.AddMembre(_membre);
            }
            else
            {
                NouveauMembre = mS.updateMembre(_membre);
            }

            sauMeEvent?.Invoke(this, new SauvegardeMeEvent { Membre = NouveauMembre });
        }

        private ICommand annuler;
        public ICommand Annuler
        {
            get
            {
                if (annuler == null)
                {
                    annuler = new RelayCommand(AnnulCours, CanAnnulCours);
                }
                return annuler;
            }
        }
        private bool CanAnnulCours(object? o)
        {

            return true;
        }

        private void AnnulCours(object? o)
        {
            AnnuleEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}


