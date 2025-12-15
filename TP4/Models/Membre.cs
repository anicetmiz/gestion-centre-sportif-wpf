using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Models
{
    public class Membre
    {
        public int MembreId { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public DateTime? DateNaissance { get; set; }
        public ICollection<Inscription> Inscriptions { get; set; }
    }
}
