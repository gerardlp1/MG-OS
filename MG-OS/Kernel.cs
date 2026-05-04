using System;
using System.IO;
using Sys = Cosmos.System;

namespace MG_OS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;
        private string directoriActual = @"0:\";

        // Guarda les ultimes 5 comandes executades
        private string[] historial = new string[5];

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

            BeepInici();
        }

        protected override void Run()
        {
            Console.Write("MG-OS " + directoriActual + "> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            ExecutarComanda(input, true);
        }

        private void ExecutarComanda(string input, bool guardar)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comanda = parts[0].ToLower();

            bool comandaCorrecta = true;

            // La comanda repeteix no es guarda directament per evitar bucles estranys
            if (comanda == "repeteix")
            {
                RepetirComanda(parts);
                return;
            }

            if (guardar)
                GuardarComanda(input);

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
                    {
                        Console.WriteLine("Escriu un text despres de 'diu'.");
                        comandaCorrecta = false;
                    }
                    break;

                case "historial":
                    MostrarHistorial();
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
                    BeepCorrecte();
                    Sys.Power.Shutdown();
                    return;

                case "reinicia":
                    Console.WriteLine("Reiniciant MG-OS...");
                    BeepCorrecte();
                    Sys.Power.Reboot();
                    return;

                default:
                    Console.WriteLine("Comanda no reconeguda.");
                    BeepError();
                    return;
            }

            if (comandaCorrecta)
                BeepCorrecte();
            else
                BeepError();
        }

        // ================= HISTORIAL =================

        private void GuardarComanda(string comanda)
        {
            for (int i = historial.Length - 1; i > 0; i--)
            {
                historial[i] = historial[i - 1];
            }

            historial[0] = comanda;
        }

        private void MostrarHistorial()
        {
            Console.WriteLine("Ultimes comandes:");

            bool buit = true;

            for (int i = 0; i < historial.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(historial[i]))
                {
                    Console.WriteLine((i + 1) + " - " + historial[i]);
                    buit = false;
                }
            }

            if (buit)
                Console.WriteLine("Encara no hi ha comandes guardades.");
        }

        private void RepetirComanda(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Format correcte: repeteix numero");
                BeepError();
                return;
            }

            if (!int.TryParse(parts[1], out int numero))
            {
                Console.WriteLine("Has d'introduir un numero valid.");
                BeepError();
                return;
            }

            if (numero < 1 || numero > 5)
            {
                Console.WriteLine("El numero ha d'estar entre 1 i 5.");
                BeepError();
                return;
            }

            string comandaRecuperada = historial[numero - 1];

            if (string.IsNullOrWhiteSpace(comandaRecuperada))
            {
                Console.WriteLine("No hi ha cap comanda en aquesta posicio.");
                BeepError();
                return;
            }

            Console.WriteLine("Executant: " + comandaRecuperada);
            ExecutarComanda(comandaRecuperada, true);
        }

        // ================= AUDIO =================

        private void BeepInici()
        {
            try
            {
                Cosmos.System.PCSpeaker.Beep(800, 150);
                Cosmos.System.PCSpeaker.Beep(1000, 150);
            }
            catch { }
        }

        private void BeepCorrecte()
        {
            try
            {
                Cosmos.System.PCSpeaker.Beep(1000, 120);
            }
            catch { }
        }

        private void BeepError()
        {
            try
            {
                Cosmos.System.PCSpeaker.Beep(300, 250);
            }
            catch { }
        }

        // ================= SISTEMA =================

        private void MostrarAjuda()
        {
            Console.WriteLine("Comandes disponibles:");
            Console.WriteLine("ajuda          - Mostra aquesta ajuda");
            Console.WriteLine("versio         - Mostra la versio del sistema");
            Console.WriteLine("net            - Neteja la pantalla");
            Console.WriteLine("diu [text]     - Escriu text per pantalla");
            Console.WriteLine("historial      - Mostra les ultimes 5 comandes");
            Console.WriteLine("repeteix [n]   - Torna a executar una comanda anterior");
            Console.WriteLine("llista         - Mostra fitxers i directoris");
            Console.WriteLine("crea [dir]     - Crea un directori");
            Console.WriteLine("entra [dir]    - Entra en un directori");
            Console.WriteLine("borra [dir]    - Esborra un directori buit");
            Console.WriteLine("mostra [fit]   - Mostra el contingut d'un fitxer");
            Console.WriteLine("suma a b       - Suma dos nombres");
            Console.WriteLine("resta a b      - Resta dos nombres");
            Console.WriteLine("mult a b       - Multiplica dos nombres");
            Console.WriteLine("div a b        - Divideix dos nombres");
            Console.WriteLine("mod a b        - Calcula el modul");
            Console.WriteLine("arrel x        - Calcula l'arrel quadrada");
            Console.WriteLine("apaga          - Apaga el sistema");
            Console.WriteLine("reinicia       - Reinicia el sistema");
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
                Console.WriteLine("Error llistant directori.");
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
                Directory.CreateDirectory(ruta);
                Console.WriteLine("Directori creat: " + ruta);
            }
            catch
            {
                Console.WriteLine("Error creant directori.");
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
                    directoriActual = @"0:\";
                    Console.WriteLine("Directori actual: " + directoriActual);
                    return;
                }

                string ruta = RutaCompleta(parts[1]);

                if (!ruta.EndsWith(@"\"))
                    ruta += @"\";

                if (Directory.Exists(ruta))
                {
                    directoriActual = ruta;
                    Console.WriteLine("Directori actual: " + directoriActual);
                }
                else
                {
                    Console.WriteLine("Error: el directori no existeix.");
                }
            }
            catch
            {
                Console.WriteLine("Error canviant directori.");
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
                Directory.Delete(ruta);
                Console.WriteLine("Directori esborrat: " + ruta);
            }
            catch
            {
                Console.WriteLine("Error esborrant directori.");
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
                Console.WriteLine(File.ReadAllText(ruta));
            }
            catch
            {
                Console.WriteLine("Error llegint fitxer.");
            }
        }

        // ================= MATEMATIQUES =================

        private void OperacioDosNombres(string[] parts, string tipus)
        {
            if (parts.Length != 3)
            {
                Console.WriteLine("Format correcte: comanda nombre nombre");
                return;
            }

            if (!double.TryParse(parts[1], out double a) ||
                !double.TryParse(parts[2], out double b))
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