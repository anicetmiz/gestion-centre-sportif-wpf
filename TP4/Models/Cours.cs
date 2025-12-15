using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Models
{
    public class Cours
    {
        public int CoursId { get; set; }
        public string Nom { get; set; }
        public double Cout { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int NbPlace { get; set; }


        [NotMapped]

        public int PlaceDisponible
        {
            get => NbPlace - Inscriptions.Count();

        }
        public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();

        //fKey vers TypeCours
        public int TypeCoursId { get; set; }
        public TypeCours TypeCours { get; set; }
    }
}
