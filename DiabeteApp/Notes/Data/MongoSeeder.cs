using MongoDB.Driver;
using Notes.Entities;

namespace Notes.Data
{
    public static class MongoSeeder
    {
        public static void Seed(IMongoCollection<Note> notesCollection)
        {
            // Si la collection contient déjà des notes, on ne fait rien
            if (notesCollection.Find(_ => true).Any())
                return;

            var notes = new List<Note>
            {
                new Note
                {
                    Contenu = @"Le patient déclare qu'il 'se sent très bien'.
Poids égal ou inférieur au poids recommandé.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 1
                },

                new Note
                {
                    Contenu = @"Le patient déclare qu'il ressent beaucoup de stress au travail.
Il se plaint également que son audition est anormale dernièrement.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 2
                },
                new Note
                {
                    Contenu = @"Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois.
Il remarque également que son audition continue d'être anormale.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 2
                },

                new Note
                {
                    Contenu = "Le patient déclare qu'il fume depuis peu.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 3
                },
                new Note
                {
                    Contenu = @"Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière.
Il se plaint également de crises d’apnée respiratoire anormales.
Tests de laboratoire indiquant un taux de cholestérol LDL élevé.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 3
                },

                new Note
                {
                    Contenu = @"Le patient déclare qu'il lui est devenu difficile de monter les escaliers.
Il se plaint également d’être essoufflé.
Tests de laboratoire indiquant que les anticorps sont élevés.
Réaction aux médicaments.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 4
                },
                new Note
                {
                    Contenu = @"Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 4
                },
                new Note
                {
                    Contenu = @"Le patient déclare avoir commencé à fumer depuis peu.
Hémoglobine A1C supérieure au niveau recommandé.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 4
                },
                new Note
                {
                    Contenu = @"Taille, Poids, Cholestérol, Vertige et Réaction.",
                    DateCreation = DateTime.UtcNow,
                    IdPatient = 4
                },
            };

            notesCollection.InsertMany(notes);
        }
    }
}
