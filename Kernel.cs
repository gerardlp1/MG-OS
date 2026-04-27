using System;
using System.IO;
using Sys = Cosmos.System;

namespace MG_OS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;
        private string directoriActual = @"0:\";

        protected override void BeforeRun()
        {
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());

            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            Console.Clear();
            Console.WriteLine("MG-OS iniciat correctament.");
            Console.WriteLine("Teclat configurat amb distribucio espanyola/europea.");
            Console.WriteLine("Sistema de fitxers inicialitzat.");
            Console.WriteLine("Escriu 'ajuda' per veure les comandes disponibles.");
            Console.WriteLine();
        }

        protected override void Run()
        {
            Console.Write("MG-OS " + directoriActual + "> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comanda = parts[0].ToLower();

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
                    if (parts.Length > 1)
                        Console.WriteLine(input.Substring(4));
                    else
                        Console.WriteLine("Escriu un text despres de 'diu'.");
                    break;

                case "llista":
                    LlistarDirectori();
                    break;

                case "crea":
                    CrearDirectori(parts);
                    break;

                case "entra":
                    CanviarDirectori(parts);
                    break;

                case "borra":
                    EsborrarDirectori(parts);
                    break;

                case "mostra":
                    MostrarFitxer(parts);
                    break;

                case "suma":
                    OperacioDosNombres(parts, "+");
                    break;

                case "resta":
                    OperacioDosNombres(parts, "-");
                    break;

                case "mult":
                    OperacioDosNombres(parts, "*");
                    break;

                case "div":
                    OperacioDosNombres(parts, "/");
                    break;

                case "mod":
                    OperacioDosNombres(parts, "%");
                    break;

                case "arrel":
                    ArrelQuadrada(parts);
                    break;

                case "apaga":
                    Console.WriteLine("Apagant MG-OS...");
                    Sys.Power.Shutdown();
                    break;

                case "reinicia":
                    Console.WriteLine("Reiniciant MG-OS...");
                    Sys.Power.Reboot();
                    break;

                default:
                    Console.WriteLine("Comanda no reconeguda.");
                    break;
            }
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

        private string RutaCompleta(string nom)
        {
            if (nom == "..")
                return nom;

            if (nom.Contains(@":\"))
                return nom;

            return directoriActual + nom;
        }

        private void LlistarDirectori()
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
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut llistar el directori.");
            }
        }

        private void CrearDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: crea nom_directori");
                return;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: aquest directori ja existeix.");
                    return;
                }

                Directory.CreateDirectory(ruta);
                Console.WriteLine("Directori creat: " + ruta);
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut crear el directori.");
            }
        }

        private void CanviarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: entra nom_directori");
                return;
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
                    return;
                }

                string ruta = RutaCompleta(parts[1]);

                if (!ruta.EndsWith(@"\"))
                    ruta += @"\";

                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: el directori no existeix.");
                    return;
                }

                directoriActual = ruta;
                Console.WriteLine("Directori actual: " + directoriActual);
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut canviar de directori.");
            }
        }

        private void EsborrarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: borra nom_directori");
                return;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("Error: el directori no existeix.");
                    return;
                }

                Directory.Delete(ruta);
                Console.WriteLine("Directori esborrat: " + ruta);
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut esborrar el directori. Potser no esta buit.");
            }
        }

        private void MostrarFitxer(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: mostra nom_fitxer");
                return;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);

                if (!File.Exists(ruta))
                {
                    Console.WriteLine("Error: el fitxer no existeix.");
                    return;
                }

                Console.WriteLine(File.ReadAllText(ruta));
            }
            catch
            {
                Console.WriteLine("Error: no s'ha pogut llegir el fitxer.");
            }
        }

        private void OperacioDosNombres(string[] parts, string tipus)
        {
            if (parts.Length != 3)
            {
                Console.WriteLine("Format correcte: comanda nombre nombre");
                return;
            }

            if (!double.TryParse(parts[1], out double a) || !double.TryParse(parts[2], out double b))
            {
                Console.WriteLine("Has d'introduir nombres valids.");
                return;
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
                        Console.WriteLine("Error: no es pot dividir per zero.");
                    else
                        Console.WriteLine("Resultat: " + (a / b));
                    break;
                case "%":
                    if (b == 0)
                        Console.WriteLine("Error: no es pot fer modul amb zero.");
                    else
                        Console.WriteLine("Resultat: " + (a % b));
                    break;
            }
        }

        private void ArrelQuadrada(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: arrel nombre");
                return;
            }

            if (!double.TryParse(parts[1], out double x))
            {
                Console.WriteLine("Has d'introduir un nombre valid.");
                return;
            }

            if (x < 0)
            {
                Console.WriteLine("Error: no es pot calcular l'arrel d'un nombre negatiu.");
                return;
            }

            Console.WriteLine("Resultat: " + Math.Sqrt(x));
        }
    }
}