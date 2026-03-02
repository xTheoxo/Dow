using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dow
{
    class Program
    {

        const string VERSION_LOCALE = "1.1x";

        static async Task VerifierMiseAJour()
        {
            string url = "https://api.github.com/repos/xTheoxo/Dow/releases/latest";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Dow-App");

                    string json = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        string versionGithub = doc.RootElement
                                                  .GetProperty("tag_name")
                                                  .GetString();

                        Console.WriteLine($"Version actuelle : {VERSION_LOCALE}");
                        

                        if (versionGithub != VERSION_LOCALE)
                        {
                            Console.WriteLine($"Version GitHub : {versionGithub}");
                            Console.WriteLine("Nouvelle version disponible !");
                            Console.WriteLine("https://github.com/xTheoxo/Dow/releases");
                        }
                        else
                        {
                            Console.WriteLine("Vous avez la dernière version.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la vérification : " + ex.Message);
            }
        }


        static async Task Main(string[] args)
        {
            await VerifierMiseAJour();

            string? reponse_dl;
            string url = "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar";
            string? choixVersion = null;
            string cheminJar;

            string? reponse_bat;
            string cheminEula;
            string contenuEula;
            string cheminBat;
            string contenuBat;

            string? path = @"";

            Dictionary<string, string> versions_vanilla = new Dictionary<string, string>()
            {
                { "1.21.11", "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar" },
                { "1.20.4", "https://piston-data.mojang.com/v1/objects/8dd1a28015f51b1803213892b50b7b4fc76e594d/server.jar" },
                { "1.20.2", "https://piston-data.mojang.com/v1/objects/5b868151bd02b41319f54c8d4061b8cae84e665c/server.jar" },
                { "1.19.4", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar" },
                { "1.8.9", "https://launcher.mojang.com/v1/objects/b58b2ceb36e01bcd8dbf49c8fb66c55a9f0676cd/server.jar" },
                { "1.7.10", "https://launcher.mojang.com/v1/objects/952438ac4e01b4d115c5fc38f891710c4941df29/server.jar" }
            };

            Console.WriteLine(" ");
            Console.WriteLine("Entrez le chemin du fichier : ex C:\\Users\\Prenom\\Desktop\\NOMDUDOSSIER ");
            Console.WriteLine("Ou mettez directement le nom dossier et il se créera au meme endroit que ce programme");
            Console.WriteLine("Marquez exit pour fermer le programme");
            Console.Write("> ");
            path = Console.ReadLine();

            while (Directory.Exists(path) || path == null)
            {
                Console.WriteLine("Le dossier existe déjà ou le chemin est null");
                Console.Write("> ");
                path = Console.ReadLine();

                if (path == "null")
                {
                    Console.WriteLine("Fermeture du programme");
                    return;
                }
            }

            Console.WriteLine(" ");
            
            if (path != "exit")
            { 
                // Création du dossier
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("Le dossier a été créé avec succès : " + Directory.GetCreationTime(path));

                // Téléchargement du fichier .jar du serveur minecraft
                Console.WriteLine("Voulez-vous télécharger la fichier .jar du serveur minecraft ? (y/n)");
                Console.Write("> ");
                reponse_dl = Console.ReadLine();

                // Si l'utilisateur veut télécharger le fichier .jar du serveur minecraft
                if (reponse_dl == "y")
                {
                    do
                    {
                        Console.WriteLine("Choisissez une version :");

                        // Affichage des versions disponibles
                        foreach (var version in versions_vanilla.Keys)
                        {
                            Console.WriteLine("- " + version);
                        }

                        Console.WriteLine(" ");

                        Console.Write("> ");
                        choixVersion = Console.ReadLine();

                        // Si la version choisie est dans la liste des versions disponibles, on télécharge le fichier .jar du serveur minecraft
                        if (versions_vanilla.ContainsKey(choixVersion))
                        {
                            url = versions_vanilla[choixVersion];
                            Console.WriteLine("Téléchargement de la version " + choixVersion);

                            // Le met dans le dossier créé précédement et le renomme en server.jar
                            cheminJar = Path.Combine(path, "server.jar");

                            // Voir doc Microsoft > HttpClient
                            using (HttpClient client = new HttpClient())
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                response.EnsureSuccessStatusCode();

                                using (FileStream fs = new FileStream(cheminJar, FileMode.Create))
                                {
                                    await response.Content.CopyToAsync(fs);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Version invalide !");
                        }
                    }
                    while (!versions_vanilla.ContainsKey(choixVersion) && choixVersion != "exit");

                    Console.WriteLine("Voulez-vous créer le fichier .bat qui sert a démarrer le serveur ? (y/n)");
                    Console.Write("> ");
                    reponse_bat = Console.ReadLine();


                    if (reponse_bat == "y")
                    {
                        //Console.WriteLine("Accepter-vous le Eula de minecraft ? (y/n)");)
                        // EULA
                        Console.WriteLine("Acceptation du EULA");
                        cheminEula = Path.Combine(path, "eula.txt");
                        contenuEula = @"eula=true";
                        File.WriteAllText(cheminEula, contenuEula);

                        Console.WriteLine("Création du fichier start.bat");
                        cheminBat = Path.Combine(path, "start.bat");

                        contenuBat =
                        @"@echo off
cd /d %~dp0
java -Xmx4G -jar server.jar nogui
pause";

                        File.WriteAllText(cheminBat, contenuBat);

                        Console.WriteLine("Fichier .bat créé avec succès !");

                        Console.WriteLine(path);
                        Process.Start(path + "/start.bat");
                    }
                }
            }
        }
    }
}