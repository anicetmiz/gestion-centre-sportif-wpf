using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Services
{
    public class MembreService
    {
        public List<Membre> GetMembreAll()
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                return context.Membres.Include(a => a.Inscriptions).ToList();
            }
        }
        public void RemoveMembre(Membre membre)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (!context.Membres.Any())
                {
                    return;
                }
                Membre unMembre = context.Membres.Include(a => a.Inscriptions).First(a => a­.MembreId == membre.MembreId);
                context.Inscriptions.RemoveRange(unMembre.Inscriptions);
                context.Membres.Remove(unMembre);
                context.SaveChanges();
            }
        }

        public Membre? AddMembre(Membre membreRecu)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (context.Membres.Any(a => a.MembreId == membreRecu.MembreId))
                {
                    return null;
                }
                Membre membre = new Membre()
                {
                    Prenom = membreRecu.Prenom,
                    Nom = membreRecu.Nom,

                    DateNaissance = membreRecu.DateNaissance,

                };
                context.Membres.Add(membre);
                context.SaveChanges();
                return membre;


            }
        }
        public Membre? updateMembre(Membre membreRecu)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {

                if (!context.Membres.Any(a => a.MembreId == membreRecu.MembreId))
                {
                    return null;
                }
                Membre membre = context.Membres.First(a => a.MembreId == membreRecu.MembreId);
                membre.Prenom = membreRecu.Prenom;
                membre.Nom = membreRecu.Nom;
                membre.DateNaissance = membreRecu.DateNaissance;
                context.SaveChanges();
                return membre;


            }
        }
    }
}
