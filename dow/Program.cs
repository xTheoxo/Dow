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

        const string VERSION_LOCALE = "1.2.1x";

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
            string reponse_jdk;
            string urlJdk;

            Dictionary<string, string>? version_choix = null;

            int choix;

            string? path = @"";

            Dictionary<string, string> versions_vanilla = new Dictionary<string, string>()
            {
                { "1.21.11", "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar" },
                { "1.21.8", "https://piston-data.mojang.com/v1/objects/6bce4ef400e4efaa63a13d5e6f6b500be969ef81/server.jar" },
                { "1.20.4", "https://piston-data.mojang.com/v1/objects/8dd1a28015f51b1803213892b50b7b4fc76e594d/server.jar" },
                { "1.20.2", "https://piston-data.mojang.com/v1/objects/5b868151bd02b41319f54c8d4061b8cae84e665c/server.jar" },
                { "1.19.4", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar" },
                { "1.12.2", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar" },
                { "1.8.9", "https://launcher.mojang.com/v1/objects/b58b2ceb36e01bcd8dbf49c8fb66c55a9f0676cd/server.jar" },
                { "1.7.10", "https://launcher.mojang.com/v1/objects/952438ac4e01b4d115c5fc38f891710c4941df29/server.jar" }
            };

            Dictionary<string, string> versions_paper = new Dictionary<string, string>()
            {
                { "1.21.11", "https://fill-data.papermc.io/v1/objects/367f5088c7cc5c8f83cbededf4760622d4a27425be45611d3db6f11c75fac901/paper-1.21.11-126.jar" },
                { "1.21.8", "https://fill-data.papermc.io/v1/objects/8de7c52c3b02403503d16fac58003f1efef7dd7a0256786843927fa92ee57f1e/paper-1.21.8-60.jar" },
                { "1.21.7", "https://fill-data.papermc.io/v1/objects/83838188699cb2837e55b890fb1a1d39ad0710285ed633fbf9fc14e9f47ce078/paper-1.21.7-32.jar" },
                { "1.21.6", "https://fill-data.papermc.io/v1/objects/4b011f5adb5f6c72007686a223174fce82f31aeb4b34faf4652abc840b47e640/paper-1.20.6-151.jar" },
                { "1.20.4", "https://fill-data.papermc.io/v1/objects/cabed3ae77cf55deba7c7d8722bc9cfd5e991201c211665f9265616d9fe5c77b/paper-1.20.4-499.jar" },
                { "1.20.2", "https://fill-data.papermc.io/v1/objects/ba340a835ac40b8563aa7eda1cd6479a11a7623409c89a2c35cd9d7490ed17a7/paper-1.20.2-318.jar" },
                { "1.20.1", "https://fill-data.papermc.io/v1/objects/234a9b32098100c6fc116664d64e36ccdb58b5b649af0f80bcccb08b0255eaea/paper-1.20.1-196.jar" },
                { "1.19.4", "https://fill-data.papermc.io/v1/objects/e587d78cba3e99ef8c4bc24cf20cc3bdbbe89e33b0b572070446af4eb6be5ccf/paper-1.19.4-550.jar" },
                { "1.19.3", "https://fill-data.papermc.io/v1/objects/3007f2c638d5f04ed32b6adaa33053fe3634ccfa74345c83d3ea4982d38db5dc/paper-1.19.3-448.jar" },
                { "1.12.2", "https://fill-data.papermc.io/v1/objects/3a2041807f492dcdc34ebb324a287414946e3e05ec3df6fd03f5b5f7d9afc210/paper-1.12.2-1620.jar" },
                { "1.8.8", "https://fill-data.papermc.io/v1/objects/7ff6d2cec671ef0d95b3723b5c92890118fb882d73b7f8fa0a2cd31d97c55f86/paper-1.8.8-445.jar" },
                { "1.7.10", "https://fill-data.papermc.io/v1/objects/33772078d92e9dbb027602da016524ef29af5b4c12eaddac1fe2465b01108185/paper-1.7.10-2025.jar" }
            };

            Console.WriteLine(" ");
            Console.WriteLine("Entrez le chemin du fichier : ex C:\\Users\\Prenom\\Desktop\\NOMDUDOSSIER ");
            Console.WriteLine("Ou mettez directement le nom dossier et il se créera au meme endroit que ce programme");
            Console.WriteLine("Marquez exit pour fermer le programme");
            

            while (Directory.Exists(path) || path == "")
            {
                Console.Write("> ");
                path = Console.ReadLine();

                if (path == "")
                {
                    Console.WriteLine("Le chemin est null");

                }
                if (path == "exit")
                {
                    Console.WriteLine("Fermeture du programme");
                    return;
                }
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Le dossier existe déjà");
                }
                
            }

            Console.WriteLine(" ");

            // Création du dossier
            DirectoryInfo di = Directory.CreateDirectory(path);
            Console.WriteLine("Le dossier a été créé avec succès : " + Directory.GetCreationTime(path));

            // Téléchargement du fichier .jar du serveur minecraft
            Console.WriteLine("Voulez-vous télécharger la fichier .jar du serveur minecraft ? (y/n)");
            Console.Write("> ");
            reponse_dl = Console.ReadLine();

            Console.WriteLine(" ");

            // Si l'utilisateur veut télécharger le fichier .jar du serveur minecraft
            if (reponse_dl == "y")
            {
                do
                {
                    Console.WriteLine("Choisissez une version");
                    Console.WriteLine("Versions disponibles :");
                    Console.WriteLine("1.Vanilla");
                    Console.WriteLine("2.Plugins (Paper)");
                    //Console.WriteLine("3.Moddés (Forge)");
                    //Console.WriteLine("4.Moddés (Fabric)");

                    Console.Write("> ");
                    choix = Convert.ToInt32(Console.ReadLine());

                    while (choix < 1 || choix > 2)
                    {
                        Console.WriteLine("Veuillez entrer un nombre entre 1 et 4");
                        Console.Write("> ");
                        choix = Convert.ToInt32(Console.ReadLine());
                    }

                    switch (choix)
                    {
                        case 1:
                            Console.WriteLine("Vous avez choisi Vanilla");
                            version_choix = versions_vanilla;
                            break;

                        case 2:
                            Console.WriteLine("Vous avez choisi Paper");
                            version_choix = versions_paper;
                            break;
                            /*
                        case 3:
                            Console.WriteLine("Vous avez choisi Forge");
                            //version_choix = versions_forge;
                            break;

                        case 4:
                            Console.WriteLine("Vous avez choisi Fabric");
                            //version_choix = versions_fabric;
                            break;*/
                    }

                    Console.WriteLine(" ");

                    // Affichage des versions disponibles
                    foreach (var version in version_choix.Keys)
                    {
                        Console.WriteLine("- " + version);
                    }

                    Console.WriteLine(" ");

                    Console.Write("> ");
                    choixVersion = Console.ReadLine();

                    // Si la version choisie est dans la liste des versions disponibles, on télécharge le fichier .jar du serveur minecraft
                    if (version_choix.ContainsKey(choixVersion))
                    {
                        url = version_choix[choixVersion];
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
                while (!version_choix.ContainsKey(choixVersion) && choixVersion != "exit");

            }
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

                Console.WriteLine(" ");
            }


            Console.WriteLine("Voulez-vous télécharger et l'installer le JDK ? (y/n)");
            Console.Write("> ");
            reponse_jdk = Console.ReadLine();

            if (reponse_jdk == "y")
            {
                urlJdk = "https://download.oracle.com/java/21/latest/jdk-21_windows-x64_bin.exe";

                Console.WriteLine(" ");

                Console.WriteLine("Téléchargement du JDK en cours...");

                // Le met dans le dossier créé précédement et le renomme en jdk-21.exe
                cheminJar = Path.Combine(path, "jdk-21.exe");

                // Voir doc Microsoft > HttpClient
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(urlJdk))
                {
                    response.EnsureSuccessStatusCode();

                    using (FileStream fs = new FileStream(cheminJar, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }


                // Installation du JDK
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = path + "/" + "jdk-21.exe",
                    Arguments = "/s",
                    Verb = "runas", // demande admin
                    UseShellExecute = true
                };

                Process process = Process.Start(psi);

                if (process != null)
                {
                    process.WaitForExit(); // Attend la fin de l'installation

                    Console.WriteLine("Installation terminée !");
                    File.Delete(path + "/" + "jdk-21.exe");
                }


                //Process.Start(path + "/start.bat");

                // Vérifier que le JDK est installé
            }
        }
    }
}