using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dow
{
    class Program
    {
        static async Task Main(string[] args)
        {
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

            const string maps = "1.0";

            Console.WriteLine("v" + maps);
            Dictionary<string, string> versions = new Dictionary<string, string>()
            {
                { "1.21.11", "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar" },
                { "1.20.4", "https://fill-data.papermc.io/v1/objects/cabed3ae77cf55deba7c7d8722bc9cfd5e991201c211665f9265616d9fe5c77b/paper-1.20.4-499.jar" },
                { "1.20.2", "https://fill-data.papermc.io/v1/objects/ba340a835ac40b8563aa7eda1cd6479a11a7623409c89a2c35cd9d7490ed17a7/paper-1.20.2-318.jar" },
                { "1.19.4", "https://fill-data.papermc.io/v1/objects/e587d78cba3e99ef8c4bc24cf20cc3bdbbe89e33b0b572070446af4eb6be5ccf/paper-1.19.4-550.jar" }
            };

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

                        foreach (var version in versions.Keys)
                        {
                            Console.WriteLine("- " + version);
                        }

                        Console.WriteLine(" ");

                        Console.Write("> ");
                        choixVersion = Console.ReadLine();

                        // Si la version choisie est dans la liste des versions disponibles, on télécharge le fichier .jar du serveur minecraft
                        if (versions.ContainsKey(choixVersion))
                        {
                            url = versions[choixVersion];
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
                    while (!versions.ContainsKey(choixVersion) && choixVersion != "exit");

                    Console.WriteLine("Voulez-vous créer le fichier .bat qui sert a démarrer le serveur");
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
java -Xmx4G -jar server.jar nogui
pause";

                        File.WriteAllText(cheminBat, contenuBat);

                        Console.WriteLine("Fichier .bat créé avec succès !");
                    }
                }
            }
        }
    }
}