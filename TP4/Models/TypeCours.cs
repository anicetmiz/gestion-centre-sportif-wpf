using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Models
{
    public class TypeCours
    {
        public int TypeCoursId { get; set; }
        public string Nom { get; set; }
        public ICollection<Cours> Cours { get; set; }
    }
}
