using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Models
{
    public class Inscription
    {
        public int InscriptionId { get; set; }
        public DateTime DateInscrit { get; set; }

        // fKey vers membre
        public int MembreId { get; set; }
        public Membre Membre { get; set; }

        //fk vers cours
        public int CoursId { get; set; }
        public Cours Cours { get; set; }

    }
}
