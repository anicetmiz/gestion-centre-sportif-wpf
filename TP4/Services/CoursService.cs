using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Services
{
    public class CoursService
    {
        public List<TypeCours> GetTypeCours()
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                return context.TypeCours.ToList();
            }
        }
        public List<Cours> GetCoursAll()
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                return context.Cours.Include(a => a.TypeCours).Include(a => a.Inscriptions).Where(a => a.DateDebut >= DateTime.Today).ToList();
            }
        }
        public List<Cours> GetCoursByType(TypeCours type)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                return context.Cours.Include(a => a.TypeCours)
                    .Include(a => a.Inscriptions)
                    .Where(a => a.TypeCoursId == type.TypeCoursId)
                    .ToList();
            }
        }
        public void RemoveCours(Cours cours)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (!context.Cours.Any())
                {
                    return;
                }
                Cours unCours = context.Cours.Include(a => a.Inscriptions).First(a => a.CoursId == cours.CoursId);
                context.Inscriptions.RemoveRange(unCours.Inscriptions);
                context.Cours.Remove(unCours);
                context.SaveChanges();
            }
        }
        public Cours? AddCours(Cours coursRecu)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {
                if (context.Cours.Any(a => a.CoursId == coursRecu.CoursId))
                {
                    return null;
                }
                Cours cours = new Cours()
                {
                    Nom = coursRecu.Nom,
                    Cout = coursRecu.Cout,
                    DateDebut = coursRecu.DateDebut,
                    DateFin = coursRecu.DateFin,
                    NbPlace = coursRecu.NbPlace,
                    TypeCours = coursRecu.TypeCours,
                    TypeCoursId = coursRecu.TypeCours.TypeCoursId
                };

                context.TypeCours.Attach(coursRecu.TypeCours);

                context.Cours.Add(cours);
                context.SaveChanges();
                return cours;


            }
        }
        public Cours? updateCours(Cours coursRecu)
        {
            using (CentreSportifDbContext context = new CentreSportifDbContext())
            {

                if (!context.Cours.Any(a => a.CoursId == coursRecu.CoursId))
                {
                    return null;
                }
                Cours cours = context.Cours.First(a => a.CoursId == coursRecu.CoursId);
                cours.Nom = coursRecu.Nom;
                cours.Cout = coursRecu.Cout;
                cours.DateDebut = coursRecu.DateDebut;
                cours.DateFin = coursRecu.DateFin;
                cours.NbPlace = coursRecu.NbPlace;
                cours.TypeCoursId = coursRecu.TypeCours.TypeCoursId;
                context.SaveChanges();
                return cours;


            }
        }
    }
}
