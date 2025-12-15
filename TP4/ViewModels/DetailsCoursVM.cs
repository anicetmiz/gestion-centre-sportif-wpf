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
    public class DetailsCoursVM : IDataErrorInfo, INotifyPropertyChanged
    {
        public Cours _cours;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Nom
        {
            get
            {
                return _cours.Nom;
            }
            set
            {
                _cours.Nom = value;
                // est ce que il faut onepropretiechanged ?
            }
        }

        public double Cout
        {
            get
            {
                return _cours.Cout;
            }
            set
            {
                _cours.Cout = value;
                // est ce que il faut onepropretiechanged ?
            }
        }
        public DateTime? DateDebut
        {
            get
            {
                return _cours.DateDebut;
            }
            set
            {
                _cours.DateDebut = value;
                // est ce que il faut onepropretiechanged ?
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateDebut)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateFin)));
            }
        }
        public DateTime? DateFin
        {
            get
            {
                return _cours.DateFin;
            }
            set
            {
                _cours.DateFin = value;

                // est ce que il faut onepropretiechanged ?
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateDebut)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateFin)));
            }
        }
        public int NbPlace
        {
            get
            {
                return _cours.NbPlace;
            }
            set
            {
                _cours.NbPlace = value;
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
        private CoursService cS = new CoursService();
        public ObservableCollection<TypeCours> TypeCours { get; set; }
        public TypeCours Type
        {
            get => _cours.TypeCours;
            set
            {
                _cours.TypeCours = value;

            }
        }
        public event EventHandler<SauvegardeCoEvent>? sauCoEvent;
        public event EventHandler<EventArgs>? AnnuleEvent;



        public DetailsCoursVM()
        {
            _cours = new Cours();

            DateDebut = null;
            DateFin = null;

            TypeCours = new ObservableCollection<TypeCours>(cS.GetTypeCours());
            errorList = new Dictionary<string, string>();

            errorList[nameof(Nom)] = "";
            errorList[nameof(Cout)] = "";
            errorList[nameof(DateDebut)] = "";
            errorList[nameof(DateFin)] = "";
            errorList[nameof(NbPlace)] = "";
            errorList[nameof(Type)] = "";

        }
        public DetailsCoursVM(Cours cours)
        {
            _cours = cours;

            TypeCours = new ObservableCollection<TypeCours>(cS.GetTypeCours());
            //remplisssage du comboBox
            //foreach (TypeCours t in cS.GetTypeCours())
            //{
            //    TypeCours.Add(t);
            //}
            errorList = new Dictionary<string, string>();
            errorList[nameof(Nom)] = "";
            errorList[nameof(Cout)] = "";
            errorList[nameof(DateDebut)] = "";
            errorList[nameof(DateFin)] = "";
            errorList[nameof(NbPlace)] = "";
            errorList[nameof(Type)] = "";

            Type = TypeCours.First(t => t.TypeCoursId == cours.TypeCoursId);
        }

        public string this[string propertyName]
        {
            get
            {
                string result = "";

                switch (propertyName)
                {
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

                    case nameof(Cout):
                        if (this.Cout == 0 || this.Cout % 2 != 0)
                        {
                            result = "LE COUT DOIT ETRE PAIRE ET NE DOIT PAS ETRES VIDE";
                            errorList[propertyName] = result;
                        }
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(DateDebut):
                        if (this.DateDebut == null)
                        {
                            result = " LA DATE DE DEBUT DOIT PAS ETRE VIDE ";
                            errorList[propertyName] = result;
                        }
                        else if (this.DateFin.HasValue && this.DateDebut.Value.Date >= DateFin.Value.Date)
                        {
                            result = "LA DATE DU DEBUT DOIT ETRE AVANT LA DATE DE FIN , LA DATE DEBUT PEUT PAS ETRE VIDE ET LA DATE DEBUT PEUT PAS ETRE DANS LE PASSÉ ";
                            errorList[propertyName] = result;
                        }

                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(DateFin):

                        if (this.DateFin == null)
                        {
                            result = " LA DATE DE FIN DOIT PAS ETRE VIDE ";
                            errorList[propertyName] = result;
                        }
                        else if (this.DateDebut.HasValue && this.DateDebut.Value.Date >= DateFin.Value.Date)
                        {
                            result = "LA DATE DU DEBUT DOIT ETRE AVANT LA DATE DE FIN ET LA DATE DEBUT PEUT PAS ETRE VIDE ";
                            errorList[propertyName] = result;
                        }
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(NbPlace):
                        if (this.NbPlace == 0)
                        {
                            result = "LE PLACE DISPONIBLE NE PEUT PAS ETRE VIDE";
                            errorList[propertyName] = result;
                        }
                        else
                        {
                            errorList[propertyName] = "";
                            //errorList.Remove(propertyName);
                        }
                        break;

                    case nameof(Type):
                        if (this.Type == null)
                        {
                            result = "LE TYPE NE PEUT PAS ETRE VIDE";
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

        private ICommand sauvegarder;
        public ICommand Sauvegarder
        {
            get
            {
                if (sauvegarder == null)
                {
                    sauvegarder = new RelayCommand(SaveCours, CanSaveCours);
                }
                return sauvegarder;
            }
        }

        private bool CanSaveCours(object? obj)
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

        private void SaveCours(object? obj)
        {
            Cours? NouveauCours = new Cours();
            if (_cours.CoursId == 0)
            {
                NouveauCours = cS.AddCours(_cours);
            }
            else
            {
                NouveauCours = cS.updateCours(_cours);
            }

            sauCoEvent?.Invoke(this, new SauvegardeCoEvent { Cours = NouveauCours });
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
