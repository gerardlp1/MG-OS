using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using System;
using System.IO;
using Sys = Cosmos.System;
using IL2CPU.API.Attribs;

namespace MG_OS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;
        private string directoriActual = @"0:\";

        private AudioMixer mixer;
        private AudioManager audioManager;
        private bool audioDisponible = false;

        //private const string SoInici = @"0:.\assets\audio\hola.wav";
        //private const string SoCorrecte = @"0:.\assets\audio\fahh.wav";
        //private const string SoError = @"0:.\assets\audio\bob.wav";

        [ManifestResourceStream(ResourceName = "MG-OS.assets.audio.fahh.wav")]
        static byte[] SoFahh;

        static byte[] SoInici = SoFahh;
        static byte[] SoCorrecte = SoFahh;
        static byte[] SoError = SoFahh;

        protected override void BeforeRun()
        {
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());

            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            InicialitzarAudio();

            Console.Clear();
            Console.WriteLine("MG-OS iniciat correctament.");
            Console.WriteLine("Teclat configurat amb distribucio espanyola/europea.");
            Console.WriteLine("Sistema de fitxers inicialitzat.");
            Console.WriteLine("Sistema d'audio inicialitzat.");
            Console.WriteLine("Escriu 'ajuda' per veure les comandes disponibles.");
            Console.WriteLine();

            //ReproduirSo(SoFahh);
        }

        protected override void Run()
        {
            //ReproduirSo(SoFahh);

            Console.Write("MG-OS " + directoriActual + "> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comanda = parts[0].ToLower();
            bool correcte = true;

            switch (comanda)
            {
                case "ajuda":
                    MostrarAjuda();
                    break;

                case "versio":
                    Console.WriteLine("MG-OS versio 0.1");
                    Console.WriteLine("Desenvolupat per Manel Sanchez i Gerard Leiva");
                    break;

                case "net":
                    Console.Clear();
                    break;

                case "diu":
                    correcte = EscriureText(parts, input);
                    break;

                case "llista":
                    correcte = LlistarDirectori();
                    break;

                case "crea":
                    correcte = CrearDirectori(parts);
                    break;

                case "entra":
                    correcte = CanviarDirectori(parts);
                    break;

                case "borra":
                    correcte = EsborrarDirectori(parts);
                    break;

                case "mostra":
                    correcte = MostrarFitxer(parts);
                    break;

                case "suma":
                    correcte = OperacioDosNombres(parts, "+");
                    break;

                case "resta":
                    correcte = OperacioDosNombres(parts, "-");
                    break;

                case "mult":
                    correcte = OperacioDosNombres(parts, "*");
                    break;

                case "div":
                    correcte = OperacioDosNombres(parts, "/");
                    break;

                case "mod":
                    correcte = OperacioDosNombres(parts, "%");
                    break;

                case "arrel":
                    correcte = ArrelQuadrada(parts);
                    break;

                case "apaga":
                    Console.WriteLine("Apagant MG-OS...");
                    ReproduirSo(SoCorrecte);
                    Sys.Power.Shutdown();
                    break;

                case "reinicia":
                    Console.WriteLine("Reiniciant MG-OS...");
                    ReproduirSo(SoCorrecte);
                    Sys.Power.Reboot();
                    break;

                default:
                    Console.WriteLine("Comanda no reconeguda.");
                    ReproduirSo(SoError);
                    return;
            }

            if (correcte)
                ReproduirSo(SoCorrecte);
            else
                ReproduirSo(SoError);
        }

        private void InicialitzarAudio()
        {
            try
            {
                mixer = new AudioMixer();

                var driver = AC97.Initialize(bufferSize: 4096);

                audioManager = new AudioManager()
                {
                    Stream = mixer,
                    Output = driver
                };

                audioManager.Enable();
                //audioDisponible = true;
            }
            catch
            {
                //audioDisponible = false;
                Console.WriteLine("Avis: no s'ha pogut inicialitzar l'audio.");
            }
        }

        private void ReproduirSo(byte[] file)
        {
            var audioStream = MemoryAudioStream.FromWave(file);
            mixer.Streams.Add(audioStream);
        }

        private void MostrarAjuda()
        {
            Console.WriteLine("Comandes disponibles:");
            Console.WriteLine("ajuda        - Mostra aquesta ajuda");
            Console.WriteLine("versio       - Mostra la versio del sistema");
            Console.WriteLine("net          - Neteja la pantalla");
            Console.WriteLine("diu [text]   - Escriu text per pantalla");
            Console.WriteLine("llista       - Mostra fitxers i directoris");
            Console.WriteLine("crea [dir]   - Crea un directori");
            Console.WriteLine("entra [dir]  - Entra en un directori");
            Console.WriteLine("borra [dir]  - Esborra un directori buit");
            Console.WriteLine("mostra [fit] - Mostra el contingut d'un fitxer");
            Console.WriteLine("suma a b     - Suma dos nombres");
            Console.WriteLine("resta a b    - Resta dos nombres");
            Console.WriteLine("mult a b     - Multiplica dos nombres");
            Console.WriteLine("div a b      - Divideix dos nombres");
            Console.WriteLine("mod a b      - Calcula el modul");
            Console.WriteLine("arrel x      - Calcula l'arrel quadrada");
            Console.WriteLine("apaga        - Apaga el sistema");
            Console.WriteLine("reinicia     - Reinicia el sistema");
        }

        private bool EscriureText(string[] parts, string input)
        {
            if (parts.Length <= 1)
            {
                Console.WriteLine("Escriu un text despres de 'diu'.");
                return false;
            }

            Console.WriteLine(input.Substring(4));
            return true;
        }

        private string RutaCompleta(string nom)
        {
            if (nom == "..")
                return nom;

            if (nom.Contains(@":\"))
                return nom;

            return directoriActual + nom;
        }

        private bool LlistarDirectori()
        {
            try
            {
                string[] directoris = Directory.GetDirectories(directoriActual);
                string[] fitxers = Directory.GetFiles(directoriActual);

                Console.WriteLine("Contingut de " + directoriActual);

                foreach (string dir in directoris)
                    Console.WriteLine("[DIR]  " + dir);

                foreach (string file in fitxers)
                    Console.WriteLine("[FILE] " + file);

                if (directoris.Length == 0 && fitxers.Length == 0)
                    Console.WriteLine("Directori buit.");

                return true;
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut llistar el directori.");
                return false;
            }
        }

        private bool CrearDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: crea nom_directori");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: aquest directori ja existeix.");
                    return false;
                }

                Directory.CreateDirectory(ruta);
                Console.WriteLine("Directori creat: " + ruta);
                return true;
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut crear el directori.");
                return false;
            }
        }

        private bool CanviarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: entra nom_directori");
                return false;
            }

            try
            {
                if (parts[1] == "..")
                {
                    if (directoriActual != @"0:\")
                    {
                        string temp = directoriActual.TrimEnd('\\');
                        int index = temp.LastIndexOf('\\');

                        if (index >= 0)
                            directoriActual = temp.Substring(0, index + 1);
                        else
                            directoriActual = @"0:\";
                    }

                    Console.WriteLine("Directori actual: " + directoriActual);
                    return true;
                }

                string ruta = RutaCompleta(parts[1]);

                if (!ruta.EndsWith(@"\"))
                    ruta += @"\";

                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: el directori no existeix.");
                    return false;
                }

                directoriActual = ruta;
                Console.WriteLine("Directori actual: " + directoriActual);
                return true;
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut canviar de directori.");
                return false;
            }
        }

        private bool EsborrarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: borra nom_directori");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: el directori no existeix.");
                    return false;
                }

                Directory.Delete(ruta);
                Console.WriteLine("Directori esborrat: " + ruta);
                return true;
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut esborrar el directori. Potser no esta buit.");
                return false;
            }
        }

        private bool MostrarFitxer(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: mostra nom_fitxer");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (!File.Exists(ruta))
                {
                    Console.WriteLine("Error: el fitxer no existeix.");
                    return false;
                }

                Console.WriteLine(File.ReadAllText(ruta));
                return true;
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut llegir el fitxer.");
                return false;
            }
        }

        private bool OperacioDosNombres(string[] parts, string tipus)
        {
            if (parts.Length != 3)
            {
                Console.WriteLine("Format correcte: comanda nombre nombre");
                return false;
            }

            if (!double.TryParse(parts[1], out double a) || !double.TryParse(parts[2], out double b))
            {
                Console.WriteLine("Has d'introduir nombres valids.");
                return false;
            }

            switch (tipus)
            {
                case "+":
                    Console.WriteLine("Resultat: " + (a + b));
                    break;

                case "-":
                    Console.WriteLine("Resultat: " + (a - b));
                    break;

                case "*":
                    Console.WriteLine("Resultat: " + (a * b));
                    break;

                case "/":
                    if (b == 0)
                    {
                        Console.WriteLine("Error: no es pot dividir per zero.");
                        return false;
                    }
                    Console.WriteLine("Resultat: " + (a / b));
                    break;

                case "%":
                    if (b == 0)
                    {
                        Console.WriteLine("Error: no es pot fer modul amb zero.");
                        return false;
                    }
                    Console.WriteLine("Resultat: " + (a % b));
                    break;
            }

            return true;
        }

        private bool ArrelQuadrada(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: arrel nombre");
                return false;
            }

            if (!double.TryParse(parts[1], out double x))
            {
                Console.WriteLine("Has d'introduir un nombre valid.");
                return false;
            }

            if (x < 0)
            {
                Console.WriteLine("Error: no es pot calcular l'arrel d'un nombre negatiu.");
                return false;
            }

            Console.WriteLine("Resultat: " + Math.Sqrt(x));
            return true;
        }
    }
}