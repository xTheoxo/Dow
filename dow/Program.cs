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

        const string VERSION_LOCALE = "1.2.3x";

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

            string? havedir = "n";
            string? createdir = "n";
            string? reponse_jar = "n";
            string url = "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar";
            string? choixVersion = null;
            string cheminJar;
            string cheminPlayit;
            bool pathregister = false;
            string urlPlayit;

            string? reponse_bat = "n";
            string cheminEula;
            string contenuEula;
            string cheminBat;
            string contenuBat;
            string reponse_jdk = "n";
            string urlJdk;
            string? reponse_start = "n";
            string? tout = "n";
            string? reponse_playit = "n";
            string? start_playit = "n";
            Dictionary<string, string>? version_choix = null;

            int choix = 0;
            string fin;

            string? path = @"";

            Dictionary<string, string> versions_vanilla = new Dictionary<string, string>()
            {
                { "1.21.11", "https://piston-data.mojang.com/v1/objects/64bb6d763bed0a9f1d632ec347938594144943ed/server.jar" },
                { "1.21.8", "https://piston-data.mojang.com/v1/objects/6bce4ef400e4efaa63a13d5e6f6b500be969ef81/server.jar" },
                { "1.20.4", "https://piston-data.mojang.com/v1/objects/8dd1a28015f51b1803213892b50b7b4fc76e594d/server.jar" },
                { "1.20.2", "https://piston-data.mojang.com/v1/objects/5b868151bd02b41319f54c8d4061b8cae84e665c/server.jar" },
                { "1.20.1", "https://piston-data.mojang.com/v1/objects/84194a2f286ef7c14ed7ce0090dba59902951553/server.jar" },
                { "1.19.4", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar" },
                { "1.12.2", "https://piston-data.mojang.com/v1/objects/8f3112a1049751cc472ec13e397eade5336ca7ae/server.jar" },
                { "1.8.9", "https://launcher.mojang.com/v1/objects/b58b2ceb36e01bcd8dbf49c8fb66c55a9f0676cd/server.jar" },
                { "1.7.10", "https://launcher.mojang.com/v1/objects/952438ac4e01b4d115c5fc38f891710c4941df29/server.jar" }
            };

            Dictionary<string, string> versions_paper = new Dictionary<string, string>()
            {
                { "1.21.11", "https://fill-data.papermc.io/v1/objects/367f5088c7cc5c8f83cbededf4760622d4a27425be45611d3db6f11c75fac901/paper-1.21.11-126.jar" },
                { "1.21.10", "https://fill-data.papermc.io/v1/objects/158703f75a26f842ea656b3dc6d75bf3d1ec176b97a2c36384d0b80b3871af53/paper-1.21.10-130.jar" },
                { "1.21.8", "https://fill-data.papermc.io/v1/objects/8de7c52c3b02403503d16fac58003f1efef7dd7a0256786843927fa92ee57f1e/paper-1.21.8-60.jar" },
                { "1.21.7", "https://fill-data.papermc.io/v1/objects/83838188699cb2837e55b890fb1a1d39ad0710285ed633fbf9fc14e9f47ce078/paper-1.21.7-32.jar" },
                { "1.21.6", "https://fill-data.papermc.io/v1/objects/4b011f5adb5f6c72007686a223174fce82f31aeb4b34faf4652abc840b47e640/paper-1.20.6-151.jar" },
                { "1.21.1", "https://fill-data.papermc.io/v1/objects/39bd8c00b9e18de91dcabd3cc3dcfa5328685a53b7187a2f63280c22e2d287b9/paper-1.21.1-133.jar" },
                { "1.20.4", "https://fill-data.papermc.io/v1/objects/cabed3ae77cf55deba7c7d8722bc9cfd5e991201c211665f9265616d9fe5c77b/paper-1.20.4-499.jar" },
                { "1.20.2", "https://fill-data.papermc.io/v1/objects/ba340a835ac40b8563aa7eda1cd6479a11a7623409c89a2c35cd9d7490ed17a7/paper-1.20.2-318.jar" },
                { "1.20.1", "https://fill-data.papermc.io/v1/objects/234a9b32098100c6fc116664d64e36ccdb58b5b649af0f80bcccb08b0255eaea/paper-1.20.1-196.jar" },
                { "1.19.4", "https://fill-data.papermc.io/v1/objects/e587d78cba3e99ef8c4bc24cf20cc3bdbbe89e33b0b572070446af4eb6be5ccf/paper-1.19.4-550.jar" },
                { "1.19.3", "https://fill-data.papermc.io/v1/objects/3007f2c638d5f04ed32b6adaa33053fe3634ccfa74345c83d3ea4982d38db5dc/paper-1.19.3-448.jar" },
                { "1.12.2", "https://fill-data.papermc.io/v1/objects/3a2041807f492dcdc34ebb324a287414946e3e05ec3df6fd03f5b5f7d9afc210/paper-1.12.2-1620.jar" },
                { "1.8.8", "https://fill-data.papermc.io/v1/objects/7ff6d2cec671ef0d95b3723b5c92890118fb882d73b7f8fa0a2cd31d97c55f86/paper-1.8.8-445.jar" },
                { "1.7.10", "https://fill-data.papermc.io/v1/objects/33772078d92e9dbb027602da016524ef29af5b4c12eaddac1fe2465b01108185/paper-1.7.10-2025.jar" }
            };
        
            Dictionary<string, string> versions_spigot = new Dictionary<string, string>()
            {
                { "1.21.11", "https://getbukkit.org/get/AfuyQcEkLcPU9p6jitBdBkGotQvy8ghM" },
                { "1.21.8", "https://getbukkit.org/get/8smOqbVnSl8RvSvlvkSNsUxEv0Y3g7Vo" },
                { "1.21.5", "https://getbukkit.org/get/cNW08KHVlCEwof2IkXbxXIKeDPbfgMBU" },
                { "1.21.4", "https://getbukkit.org/get/vUvveVKWgnYEO4zC7ey2pqkGtaAxvS6v" },
                { "1.21.3", "https://getbukkit.org/get/RnpgqIvNyXe7nFTZZJyIkXl4shuhFUXm" },
                { "1.21.1", "https://getbukkit.org/get/yqv2djplb2mijkqTIUUmC7F6pbJMpFdX" },
                { "1.20.6", "https://getbukkit.org/get/QRmEAkJCu1HcZkgxVSmVgHZYwZ03Ua1R" },
                { "1.20.4", "https://getbukkit.org/get/vdYtG2jRHgslyLJLaIUK2xVAL95LshGI" },
                { "1.20.2", "https://getbukkit.org/get/ZcRnnYaSJs89j5TUCLdyXVsmem6ovqnn" },
                { "1.20.1", "https://getbukkit.org/get/U9uE8nD9E8ubIns3NuTK6rnKrUVOVc45" },
                { "1.19.4", "https://getbukkit.org/get/XYtMHSKmmb8UNCj2RNwXmGjRvHISRwoj" },
                { "1.19.3", "https://getbukkit.org/get/h5zwRyDERLQVgY9ROBNlcyLLRzEsWSjH" },
                { "1.19.2", "https://getbukkit.org/get/OaTGj2mTbLKl9o6qGXn2wnCZBdwW3Yk6" },
                { "1.19.1", "https://getbukkit.org/get/SCPNVGrhmI4uSfXNOPi0N6AAvCtYTHgu" },
                { "1.19", "https://getbukkit.org/get/YH4OAZAZumfJ6EisLG4xfU9op3WvEEkc" },
                { "1.18.2", "https://getbukkit.org/get/M3CuBYuR72VaQB3W6T2TTbPiklfqSn1u" },
                { "1.18.1", "https://getbukkit.org/get/smdDWNBN1RV5KfOacaJvnDOlIf5BdAJN" },
                { "1.18", "https://getbukkit.org/get/DExv10cl86CUN62iB1Df4NVdG7GD1pNV" },
                { "1.16.5", "https://getbukkit.org/get/GSXo3m2tDdbXkJA5QAw0vLihBnEEP55G" },
                { "1.16.4", "https://getbukkit.org/get/iuObpmqGVCpBoU3BC823H5b2jKx3muCI" },
                { "1.16.3", "https://getbukkit.org/get/ZJZrBToD0A3qOwOG6FTaZ8j3ybrKe3oJ" },
                { "1.16.2", "https://getbukkit.org/get/WRBd7m29OQ4AmKDEOAU1C8ijgJrrAUio" },
                { "1.16.1", "https://getbukkit.org/get/1mFuDJYv8k9kpLvY4eyZ6bstqyXjHBdx" },
                { "1.15.2", "https://getbukkit.org/get/mpe1uTseEONyg3iFtJQaitrEoIFic75N" },
                { "1.15.1", "https://getbukkit.org/get/OT1jS2PYF5YnHuHbdjKBiPKBXGHzPThb" },
                { "1.15", "https://getbukkit.org/get/YeJiojZ6jVPQefdfxCT2ZzXXkHh3mmIn" },
                { "1.14.4", "https://getbukkit.org/get/rZlZWBTsIJauWb20uqHQTjIuv0ayKTMP" },
                { "1.14.3", "https://getbukkit.org/get/wKZI0DrixwfUBWAsiiyjqGLFpuKQ0uWo" },
                { "1.14.2", "https://getbukkit.org/get/npSdlqa7cDVGaQo62CjoqY4ZzqIJkKqz" },
                { "1.14.1", "https://getbukkit.org/get/h3hm8JHO3SlF0DP7RQMA4DwCm9mlK9l2" },
                { "1.14", "https://getbukkit.org/get/BT1o9lrMqDMj3hnXlRuHotimk7zX2F7g" },
                { "1.13.2", "https://getbukkit.org/get/jh3h8QSvol0gXPladGNin80nGid4DmWv" },
                { "1.13.1", "https://getbukkit.org/get/BxDmQtGsHUacEIUrXaJ5fy0m3Xp19A6i" },
                { "1.13", "https://getbukkit.org/get/pceKF1UGA4wPlu67X2lPMg4dfnhCjjb7" },
                { "1.12.2", "https://getbukkit.org/get/Uov36RPe0zZBdi42t7OwtMFq3qaCXNyT" },
                { "1.12.1", "https://getbukkit.org/get/OuENqxqgWDRSeS1Ec00KvnAjiLqRj57C" },
                { "1.12", "https://getbukkit.org/get/DWfRX0AnTBtgWJY7CbsLXcFJKj8HUCu7" },
                { "1.11.2", "https://getbukkit.org/get/a71XhriEGe5uvifgsghIIIZQKz549qtZ" },
                { "1.11", "https://getbukkit.org/get/g7znNa4ffhmOuMwZajenBNc2y5fFNYdi" },
                { "1.10.2", "https://getbukkit.org/get/emoWettOnCWTmPquJ86DpVh06tFEVCio" },
                { "1.9.4", "https://getbukkit.org/get/UZT6lu9xerOcWKTjKYdv3sIDVU2QGhf5" },
                { "1.9.2", "https://getbukkit.org/get/1VJgmwMfix4Qo2VJ7McbDo1ZekCPWMk6" },
                { "1.9", "https://getbukkit.org/get/aI5o1m9YfyNOu8yGzX9LvtYmXeWSfNZU" },
                { "1.8.8", "https://getbukkit.org/get/xo7CwyaiWNY7Ghtj3fR8bRCehoqLb5Pi" },
                { "1.8.3", "https://getbukkit.org/get/9rrkoYWkmC1MT0frDxNuAqYUogr9hfxR" },
                { "1.8", "https://getbukkit.org/get/J2wgWzOlirjx2drpuF4ncCxvVwqDR5p0" }
            };

            Console.WriteLine(" ");
            Console.WriteLine("Avez-vous déjà un dossier serveur ? (y/n)");
            //Console.WriteLine("Si oui mais qu'il est vide l'appli vous en créra un pour vous donc répondez non à cette question");
            Console.Write("> ");
            havedir = Console.ReadLine();
            pathregister = true;

            if (havedir == "y")
            {
                do
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Entrez le chemin du dossier du serveur : ex C:\\Users\\Prenom\\Desktop\\NOMDUDOSSIER");
                    //Console.WriteLine("Ou mettez directement le nom dossier et il se créera au meme endroit que ce programme");
                    Console.Write("> ");
                    path = Console.ReadLine();
                    if (path == "" )
                    {
                        Console.WriteLine("Le chemin est null");
                    }
                    else if (!Directory.Exists(path))
                    {
                        Console.Write("Le dossier n'exite pas");
                    }
                    if (path == "exit")
                    {
                        Console.WriteLine("Fermeture du programme");
                        return;
                    }
                }
                while (!Directory.Exists(path) || path == "");

            }

            while (pathregister == true)
            {
                if (havedir == "y")
                { 
                Console.WriteLine(" ");
                Console.WriteLine("1.Tout faire !");
                Console.WriteLine("2.Télécharger le .jar");
                Console.WriteLine("3.Créer seulement .bat");
                Console.WriteLine("4.Télécharger et installer le JDK");
                Console.WriteLine("5.Démarrer le serveur");
                Console.WriteLine("6.Installer Playit");
                Console.WriteLine("7.Démarrer Playit");
                Console.WriteLine("8.Fermer le programme");

                Console.Write("> ");
                choix = Convert.ToInt32(Console.ReadLine()); //!= null ? Convert.ToInt32(Console.ReadLine()) : 0;
                }
                if (havedir == "n")
                {
                    Console.WriteLine(" ");
                    choix = 1;
                    pathregister = true;
                }

                switch (choix)
                {
                    case 1:
                        tout = "y";
                        reponse_bat = "y";
                        reponse_jar = "y";
                        reponse_jdk = "y";
                        reponse_start = "y";
                        reponse_playit = "y";
                        start_playit = "y";
                        havedir = "y";
                        break;
                    case 2:
                        reponse_jar = "y";
                        break;
                    case 3:
                        reponse_bat = "y";
                        break;
                    case 4:
                        reponse_jdk = "y";
                        break;
                    case 5:
                        Process.Start("powershell", "cd '" + path + "' ; start start.bat");
                        break;
                    case 6:
                        reponse_playit = "y";
                        break;
                    case 7:
                        Process.Start("powershell", "cd '" + path + "' ; start Playit.exe");
                        break;
                    case 8:
                        Console.WriteLine("Fermeture du programme...");
                        fin = Console.ReadLine();
                        return;
                }


                if (tout == "y")
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Voulez-vous créer un dossier pour votre serveur ? (y/n)");

                    Console.Write("> ");
                    createdir = Console.ReadLine();
                    if (createdir == "y")
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("Entrez le chemin du dossier du serveur : ex C:\\Users\\Prenom\\Desktop\\NOMDUDOSSIER");
                        Console.WriteLine("Ou mettez directement le nom dossier et il se créera au meme endroit que ce programme");

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
                                fin = Console.ReadLine();
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
                    }
                    else if (havedir == "n")
                    {
                        Console.WriteLine("Le programme n'a donc aucun dossier ou télécharger les modules");
                        Console.WriteLine("Fermeture du programme...");
                        fin = Console.ReadLine();
                        return;
                        
                    }
                }
                    // Téléchargement du fichier .jar du serveur minecraft
                if (reponse_jar == "y")
                {
                        Console.WriteLine("Voulez-vous télécharger la fichier .jar du serveur minecraft ? (y/n)");
                        Console.Write("> ");
                        reponse_jar = Console.ReadLine();

                        Console.WriteLine(" ");
                }

                    // Si l'utilisateur veut télécharger le fichier .jar du serveur minecraft
                if (reponse_jar == "y")
                {
                        do
                        {
                            Console.WriteLine("Choisissez une version");
                            Console.WriteLine("Versions disponibles :");
                            Console.WriteLine("1.Vanilla");
                            Console.WriteLine("2.Plugins (Paper)");
                            Console.WriteLine("3.Plugins (Spigot)");
                            //Console.WriteLine("4.Moddés (Forge)");
                            //Console.WriteLine("5.Moddés (Fabric)");

                            Console.Write("> ");
                            choix = Convert.ToInt32(Console.ReadLine());

                            while (choix < 1 || choix > 3)
                            {
                                Console.WriteLine("Veuillez entrer un nombre entre 1 et 3");
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
                                case 3:
                                    Console.WriteLine("Vous avez choisi Spigot");
                                    version_choix = versions_spigot;
                                    break;
                                    /*
                                case 4:
                                    Console.WriteLine("Vous avez choisi Forge");
                                    //version_choix = versions_forge;
                                    break;

                                case 5:
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
                        reponse_jar = "n";
                }


                if (reponse_bat == "y")
                {
                        Console.WriteLine("Voulez-vous créer le fichier .bat qui sert a démarrer le serveur ? (y/n)");
                        Console.Write("> ");
                        reponse_bat = Console.ReadLine();
                }

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
                        reponse_bat = "n";
                }

                if (reponse_jdk == "y")
                { 
                    Console.WriteLine("Voulez-vous télécharger et l'installer le JDK ? (y/n)");
                    Console.Write("> ");
                    reponse_jdk = Console.ReadLine();
                }
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


                        // Vérifier que le JDK est installé
                    reponse_jdk = "n";
                }
                    // Lance un cmd pour démarrer le start.bat
                    //Process.Start(Path.Combine(path, "start.bat"));

                if (reponse_start == "y")
                {
                        Console.WriteLine("Voulez-vous démarrer le serveur maintenant ? (y/n)");
                        Console.Write("> ");
                        reponse_start = Console.ReadLine();
                }
                if (reponse_start == "y")
                {
                            Process.Start("powershell", "cd '" + path + "' ; start start.bat");
                            reponse_start = "n";
                }


                if (reponse_playit == "y")
                {
                    Console.WriteLine("Voulez-vous télécharger Playit ? (y/n)");
                    Console.Write("> ");
                    reponse_playit = Console.ReadLine();

                    urlPlayit = "https://github.com/playit-cloud/playit-agent/releases/download/v0.17.1/playit-windows-x86_64-signed.exe";

                    Console.WriteLine(" ");

                    Console.WriteLine($"Téléchargement de Playit dans {path}");

                    // Le met dans le dossier créé précédement et le renomme en jdk-21.exe
                    cheminPlayit = Path.Combine(path, "Playit.exe");

                    // Voir doc Microsoft > HttpClient
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(urlPlayit))
                    {
                        response.EnsureSuccessStatusCode();

                        using (FileStream fs = new FileStream(cheminPlayit, FileMode.Create))
                        {
                            await response.Content.CopyToAsync(fs);
                        }
                    }

                    Console.WriteLine("Installation terminée !");

                     Console.WriteLine("Penser à le lancer en même temps que votre serveur");
                     Console.WriteLine("sinon vos amis ne pourront pas vous rejoindre");
                     Console.WriteLine("Playit retrouvable ici > https://playit.gg/");

                    reponse_playit = "n";
                }

                if (start_playit == "y")
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Voulez-vous démarrer Playit Maintenant ? (y/n)");
                    Console.Write("> ");
                    reponse_start = Console.ReadLine();
                }
                if (start_playit == "y")
                {
                    Process.Start("powershell", "cd '" + path + "' ; start Playit.exe");
                    reponse_start = "n";
                }
            }
        }
    }
}