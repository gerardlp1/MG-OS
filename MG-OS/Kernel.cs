using System;
using System.IO;
using System.Drawing;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;

namespace MG_OS
{
    public class Kernel : Sys.Kernel
    {
        private Sys.FileSystem.CosmosVFS fs;
        private string directoriActual = @"0:\";
        private string[] historial = new string[5];

        private Canvas canvas;
        private string[] sortida = new string[14];
        private string ultimaComanda = "";

        protected override void BeforeRun()
        {
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());

            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(800, 600, ColorDepth.ColorDepth32));

            MostrarBenvinguda();
            BeepInici();

            AfegirSortida("MG-OS iniciat correctament.");
            AfegirSortida("Teclat configurat amb distribucio espanyola/europea.");
            AfegirSortida("Sistema de fitxers inicialitzat.");
            AfegirSortida("Escriu 'ajuda' per veure les comandes.");

            DibuixarInterficieAmbInput("");
        }

        protected override void Run()
        {
            string input = "";

            while (true)
            {
                DibuixarInterficieAmbInput(input);

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        ultimaComanda = input;
                        ExecutarComanda(input, true);
                    }

                    input = "";
                    DibuixarInterficieAmbInput(input);
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                        input = input.Substring(0, input.Length - 1);
                }
                else
                {
                    if (input.Length < 70)
                        input += key.KeyChar;
                }
            }
        }

        private void MostrarBenvinguda()
        {
            canvas.Clear(Color.FromArgb(10, 18, 35));

            canvas.DrawRectangle(Color.Cyan, 80, 70, 640, 420);
            canvas.DrawRectangle(Color.DarkBlue, 100, 90, 600, 380);

            canvas.DrawRectangle(Color.Cyan, 330, 135, 140, 140);
            canvas.DrawRectangle(Color.White, 350, 155, 100, 100);

            canvas.DrawString("MG-OS", PCScreenFont.Default, Color.White, 365, 310);
            canvas.DrawString("Sistema operatiu educatiu", PCScreenFont.Default, Color.LightCyan, 280, 350);
            canvas.DrawString("Desenvolupat amb Cosmos", PCScreenFont.Default, Color.LightGray, 295, 380);
            canvas.DrawString("Prem una tecla per continuar", PCScreenFont.Default, Color.Yellow, 270, 430);

            canvas.Display();
            Console.ReadKey();
        }

        private void DibuixarInterficie()
        {
            canvas.Clear(Color.FromArgb(8, 12, 24));

            canvas.DrawRectangle(Color.Cyan, 15, 15, 770, 570);
            canvas.DrawRectangle(Color.FromArgb(18, 30, 55), 25, 25, 750, 550);

            canvas.DrawRectangle(Color.FromArgb(30, 80, 120), 35, 35, 730, 55);
            canvas.DrawString("MG-OS", PCScreenFont.Default, Color.White, 55, 55);
            canvas.DrawString("Ruta: " + directoriActual, PCScreenFont.Default, Color.LightCyan, 260, 55);
            canvas.DrawString("Versio 0.1", PCScreenFont.Default, Color.LightGray, 650, 55);

            canvas.DrawRectangle(Color.FromArgb(12, 20, 35), 35, 110, 730, 380);
            canvas.DrawString("Sortida del sistema:", PCScreenFont.Default, Color.Yellow, 50, 125);

            int y = 150;
            for (int i = 0; i < sortida.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(sortida[i]))
                {
                    canvas.DrawString(sortida[i], PCScreenFont.Default, Color.White, 50, y);
                    y += 23;
                }
            }

            canvas.DrawRectangle(Color.FromArgb(20, 40, 65), 35, 505, 730, 35);
            canvas.DrawString("Ultima comanda: " + ultimaComanda, PCScreenFont.Default, Color.LightGreen, 50, 518);
        }

        private void DibuixarInterficieAmbInput(string input)
        {
            DibuixarInterficie();

            canvas.DrawRectangle(Color.FromArgb(40, 80, 120), 35, 550, 730, 30);
            canvas.DrawString("> " + input, PCScreenFont.Default, Color.White, 50, 558);

            canvas.Display();
        }

        private void AfegirSortida(string text)
        {
            for (int i = 0; i < sortida.Length - 1; i++)
                sortida[i] = sortida[i + 1];

            sortida[sortida.Length - 1] = text;
        }

        private void ExecutarComanda(string input, bool guardar)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comanda = parts[0].ToLower();

            bool correcta = true;

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
                    AfegirSortida("MG-OS versio 0.1");
                    AfegirSortida("Desenvolupat per Manel Sanchez i Gerard Leiva");
                    break;

                case "net":
                    NetejarSortida();
                    break;

                case "diu":
                    correcta = EscriureText(parts, input);
                    break;

                case "historial":
                    MostrarHistorial();
                    break;

                case "llista":
                    LlistarDirectori();
                    break;

                case "crea":
                    correcta = CrearDirectori(parts);
                    break;

                case "entra":
                    correcta = CanviarDirectori(parts);
                    break;

                case "borra":
                    correcta = EsborrarDirectori(parts);
                    break;

                case "mostra":
                    correcta = MostrarFitxer(parts);
                    break;

                case "suma":
                    correcta = OperacioDosNombres(parts, "+");
                    break;

                case "resta":
                    correcta = OperacioDosNombres(parts, "-");
                    break;

                case "mult":
                    correcta = OperacioDosNombres(parts, "*");
                    break;

                case "div":
                    correcta = OperacioDosNombres(parts, "/");
                    break;

                case "mod":
                    correcta = OperacioDosNombres(parts, "%");
                    break;

                case "arrel":
                    correcta = ArrelQuadrada(parts);
                    break;

                case "apaga":
                    AfegirSortida("Apagant MG-OS...");
                    DibuixarInterficieAmbInput("");
                    BeepCorrecte();
                    Sys.Power.Shutdown();
                    return;

                case "reinicia":
                    AfegirSortida("Reiniciant MG-OS...");
                    DibuixarInterficieAmbInput("");
                    BeepCorrecte();
                    Sys.Power.Reboot();
                    return;

                default:
                    AfegirSortida("Comanda no reconeguda.");
                    BeepError();
                    return;
            }

            if (correcta)
                BeepCorrecte();
            else
                BeepError();
        }

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

        private void MostrarAjuda()
        {
            AfegirSortida("Comandes disponibles:");
            AfegirSortida("ajuda, versio, net, diu [text]");
            AfegirSortida("historial, repeteix [n]");
            AfegirSortida("llista, crea [dir], entra [dir]");
            AfegirSortida("borra [dir], mostra [fit]");
            AfegirSortida("suma, resta, mult, div, mod, arrel");
            AfegirSortida("apaga, reinicia");
        }

        private void GuardarComanda(string comanda)
        {
            for (int i = historial.Length - 1; i > 0; i--)
                historial[i] = historial[i - 1];

            historial[0] = comanda;
        }

        private void MostrarHistorial()
        {
            AfegirSortida("Ultimes comandes:");

            bool buit = true;

            for (int i = 0; i < historial.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(historial[i]))
                {
                    AfegirSortida((i + 1) + " - " + historial[i]);
                    buit = false;
                }
            }

            if (buit)
                AfegirSortida("Encara no hi ha comandes guardades.");
        }

        private void RepetirComanda(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: repeteix numero");
                BeepError();
                return;
            }

            if (!int.TryParse(parts[1], out int numero))
            {
                AfegirSortida("Has d'introduir un numero valid.");
                BeepError();
                return;
            }

            if (numero < 1 || numero > 5)
            {
                AfegirSortida("El numero ha d'estar entre 1 i 5.");
                BeepError();
                return;
            }

            string comandaRecuperada = historial[numero - 1];

            if (string.IsNullOrWhiteSpace(comandaRecuperada))
            {
                AfegirSortida("No hi ha cap comanda en aquesta posicio.");
                BeepError();
                return;
            }

            AfegirSortida("Executant: " + comandaRecuperada);
            ExecutarComanda(comandaRecuperada, true);
        }

        private void NetejarSortida()
        {
            for (int i = 0; i < sortida.Length; i++)
                sortida[i] = "";

            AfegirSortida("Pantalla netejada.");
        }

        private bool EscriureText(string[] parts, string input)
        {
            if (parts.Length <= 1)
            {
                AfegirSortida("Escriu un text despres de 'diu'.");
                return false;
            }

            AfegirSortida(input.Substring(4));
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

        private void LlistarDirectori()
        {
            try
            {
                string[] directoris = Directory.GetDirectories(directoriActual);
                string[] fitxers = Directory.GetFiles(directoriActual);

                AfegirSortida("Contingut de " + directoriActual);

                foreach (string dir in directoris)
                    AfegirSortida("[DIR] " + dir);

                foreach (string file in fitxers)
                    AfegirSortida("[FILE] " + file);

                if (directoris.Length == 0 && fitxers.Length == 0)
                    AfegirSortida("Directori buit.");
            }
            catch
            {
                AfegirSortida("Error llistant directori.");
            }
        }

        private bool CrearDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: crea nom_directori");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);
                Directory.CreateDirectory(ruta);
                AfegirSortida("Directori creat: " + ruta);
                return true;
            }
            catch
            {
                AfegirSortida("Error creant directori.");
                return false;
            }
        }

        private bool CanviarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: entra nom_directori");
                return false;
            }

            try
            {
                if (parts[1] == "..")
                {
                    directoriActual = @"0:\";
                    AfegirSortida("Directori actual: " + directoriActual);
                    return true;
                }

                string ruta = RutaCompleta(parts[1]);

                if (!ruta.EndsWith(@"\"))
                    ruta += @"\";

                if (Directory.Exists(ruta))
                {
                    directoriActual = ruta;
                    AfegirSortida("Directori actual: " + directoriActual);
                    return true;
                }

                AfegirSortida("Error: el directori no existeix.");
                return false;
            }
            catch
            {
                AfegirSortida("Error canviant directori.");
                return false;
            }
        }

        private bool EsborrarDirectori(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: borra nom_directori");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);
                Directory.Delete(ruta);
                AfegirSortida("Directori esborrat: " + ruta);
                return true;
            }
            catch
            {
                AfegirSortida("Error esborrant directori.");
                return false;
            }
        }

        private bool MostrarFitxer(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: mostra nom_fitxer");
                return false;
            }

            try
            {
                string ruta = RutaCompleta(parts[1]);
                AfegirSortida(File.ReadAllText(ruta));
                return true;
            }
            catch
            {
                AfegirSortida("Error llegint fitxer.");
                return false;
            }
        }

        private bool OperacioDosNombres(string[] parts, string tipus)
        {
            if (parts.Length != 3)
            {
                AfegirSortida("Format correcte: comanda nombre nombre");
                return false;
            }

            if (!double.TryParse(parts[1], out double a) ||
                !double.TryParse(parts[2], out double b))
            {
                AfegirSortida("Has d'introduir nombres valids.");
                return false;
            }

            switch (tipus)
            {
                case "+":
                    AfegirSortida("Resultat: " + (a + b));
                    break;

                case "-":
                    AfegirSortida("Resultat: " + (a - b));
                    break;

                case "*":
                    AfegirSortida("Resultat: " + (a * b));
                    break;

                case "/":
                    if (b == 0)
                    {
                        AfegirSortida("Error: no es pot dividir per zero.");
                        return false;
                    }
                    AfegirSortida("Resultat: " + (a / b));
                    break;

                case "%":
                    if (b == 0)
                    {
                        AfegirSortida("Error: no es pot fer modul amb zero.");
                        return false;
                    }
                    AfegirSortida("Resultat: " + (a % b));
                    break;
            }

            return true;
        }

        private bool ArrelQuadrada(string[] parts)
        {
            if (parts.Length != 2)
            {
                AfegirSortida("Format correcte: arrel nombre");
                return false;
            }

            if (!double.TryParse(parts[1], out double x))
            {
                AfegirSortida("Has d'introduir un nombre valid.");
                return false;
            }

            if (x < 0)
            {
                AfegirSortida("Error: no es pot calcular arrel negativa.");
                return false;
            }

            AfegirSortida("Resultat: " + Math.Sqrt(x));
            return true;
        }
    }
}