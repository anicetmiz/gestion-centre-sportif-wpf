using TP4.Models;

namespace TP4.Events
{
    public class RemoveEvent : EventArgs
    {
        public Cours? Cours { get; set; }
    }
}