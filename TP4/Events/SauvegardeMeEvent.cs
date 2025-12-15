using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Events
{
    public class SauvegardeMeEvent : EventArgs
    {
        public Membre? Membre { get; set; }
    }
}
