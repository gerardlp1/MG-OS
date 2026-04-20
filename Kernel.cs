using System;
using Sys = Cosmos.System;

namespace MG_OS
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("MG-OS iniciat correctament.");
            Console.WriteLine("Escriu 'ajuda' per veure les comandes disponibles.");
            Console.WriteLine();
        }

        protected override void Run()
        {
            Console.Write("MG-OS> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            string[] parts = input.Split(' ');
            string comanda = parts[0].ToLower();

            switch (comanda)
            {
                case "ajuda":
                    Console.WriteLine("Comandes disponibles:");
                    Console.WriteLine("ajuda     - Mostra aquesta ajuda");
                    Console.WriteLine("versio    - Mostra la versió del sistema");
                    Console.WriteLine("net       - Neteja la pantalla");
                    Console.WriteLine("diu       - Escriu un text per pantalla");
                    Console.WriteLine("apaga     - Apaga el sistema");
                    break;

                case "versio":
                    Console.WriteLine("MG-OS versió 0.1");
                    Console.WriteLine("Desenvolupat per Manel Sanchez i Gerard Leiva");
                    break;

                case "net":
                    Console.Clear();
                    break;

                case "diu":
                    if (parts.Length > 1)
                    {
                        string text = input.Substring(4);
                        Console.WriteLine(text);
                    }
                    else
                    {
                        Console.WriteLine("Has d'escriure un text després de la comanda.");
                    }
                    break;

                case "apaga":
                    Console.WriteLine("Apagant MG-OS...");
                    Sys.Power.Shutdown();
                    break;

                default:
                    Console.WriteLine("Comanda no reconeguda. Escriu 'ajuda' per veure les opcions.");
                    break;
            }
        }
    }
}