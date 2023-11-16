public class SeedData
{
    public static void Initiate()
    {
        using (var context = new LaReserveContext())
        {
            if (context.Animaux.Any())
            {
                return;   // DB already filled
            }

            else
            {
                // Add Lieu
                Lieu foret = new Lieu
                {
                    Nom = "La Forêt",
                    Type = Type.Ursidé

                };

                Lieu savane = new Lieu
                {
                    Nom = "La Savane",
                    Type = Type.Félin

                };

                Lieu planeteDesSinges = new Lieu
                {
                    Nom = "La Planete des Singes",
                    Type = Type.Primate

                };

                Lieu leBassin = new Lieu
                {
                    Nom = "Le bassin",
                    Type = Type.Marin

                };

                context.Lieux.AddRange(
                foret,
                savane,
                planeteDesSinges,
                leBassin
                );

                //Add Animals
                Animal lili = new Animal
                {
                    Nom = "Lili",
                    Age = 2,
                    Espece = "Panda",
                    Type = Type.Ursidé,
                    Lieu = foret
                };


                Animal lilou = new Animal
                {
                    Nom = "Lilou",
                    Age = 10,
                    Espece = "Lion",
                    Type = Type.Félin,
                    Lieu = savane
                };

                Animal pico = new Animal
                {
                    Nom = "Loulou",
                    Age = 5,
                    Espece = "Ouistiti",
                    Type = Type.Primate,
                    Lieu = planeteDesSinges
                };

                context.Animaux.AddRange(
                lili,
                lilou,
                pico
                );


                //Add Animals
                Personnel chloe = new Personnel
                {
                    Nom = "Bernard",
                    Prenom = "Chloe",
                    Metier = Metier.Vétérinaire,
                    Specialisation = Type.Félin
                };

                Personnel anais = new Personnel
                {
                    Nom = "Degorre",
                    Prenom = "Anaïs",
                    Metier = Metier.Soigneur,
                    Specialisation = Type.Ursidé
                };

                Personnel bob = new Personnel
                {
                    Nom = "Camerez",
                    Prenom = "Bob",
                    Metier = Metier.Directeur,
                    Specialisation = Type.Marin
                };

                Personnel leo = new Personnel
                {
                    Nom = "Degorre",
                    Prenom = "Anaïs",
                    Metier = Metier.Soigneur,
                    Specialisation = Type.Oiseau
                };

                context.Personnels.AddRange(
                chloe,
                anais,
                bob,
                leo
                );

                //Add Evenements
                Evenement nuitEtoilee = new Evenement
                {
                    Nom = "Nuit étoilée",
                    Description = "Venez découvrir le spectacle au coeur de la planète des signes lors d'une nuit étoilée...",
                    Date = new DateTime(2022, 12, 20, 19, 0, 0),
                    Lieu = planeteDesSinges,
                    Animal = lili,
                    Personnel=chloe

                };



                Evenement feuFollet = new Evenement
                {
                    Nom = "La journée Feu follet",
                    Description = "Découvrez un spectacle hors norme, mélant eau, feu et bien encore...",
                    Date = new DateTime(2023, 01, 14, 13, 0, 0),
                    Lieu = leBassin,
                    Animal = lilou,
                    Personnel=anais

                };

                context.Evenements.AddRange(
                nuitEtoilee,
                feuFollet

                );
            }

            context.SaveChanges();
        }

    }

}
