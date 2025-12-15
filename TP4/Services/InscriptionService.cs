using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Services
{
    public class InscriptionService
    {
        public List<Inscription> GetInscriptionsMembre(Membre membre)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                return context.Inscriptions.Include(a => a.Cours).ThenInclude(a => a.TypeCours).Where(a => a.MembreId == membre.MembreId).ToList();
            }
        }

        public Inscription? RemoveInscription(Inscription inscription)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (context.Inscriptions.Any(a => a.InscriptionId == inscription.InscriptionId))
                {
                    context.Inscriptions.Remove(inscription);
                    context.SaveChanges();
                    return inscription;
                }
                return null;
            }
        }
        public Inscription? AddInscription(Cours cours, Membre membre)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (context.Inscriptions.Any(a => a.CoursId == cours.CoursId && a.MembreId == membre.MembreId))
                {
                    return null;
                }
                // Récapérer toutes les inscriptions du membre
                List<Inscription> inscriptionsMembre = context.Inscriptions
                                                .Include(i => i.Cours)
                                                .Where(i => i.MembreId == membre.MembreId)
                                                .ToList();

                // Trouve les cours qui se qui se donnent dans la même période. avec celui reçu
                int memeTemps = inscriptionsMembre
                    .Where(i =>
                        i.Cours.DateDebut <= cours.DateFin &&
                        cours.DateDebut <= i.Cours.DateFin
                    )
                    .Count();

                if (memeTemps >= 3)
                {
                    return null;
                }

                Inscription inscription = new Inscription();

                inscription.DateInscrit = DateTime.Today;
                inscription.CoursId = cours.CoursId;
                inscription.MembreId = membre.MembreId;

                context.Inscriptions.Add(inscription);
                context.SaveChanges();
                return context.Inscriptions
                      .Include(i => i.Cours)
                      .ThenInclude(c => c.TypeCours)
                      .FirstOrDefault(i => i.InscriptionId == inscription.InscriptionId);
            }

        }

    }
}
