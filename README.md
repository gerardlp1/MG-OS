# 🖥️ MG-OS
# 
# Sistema operatiu educatiu desenvolupat amb Cosmos per aprendre com funciona un SO des de dins.
# 
# 🌟 Característiques destacades
# 🧠 Sistema operatiu creat des de zero amb Cosmos (.NET/C#)
# 👨‍💻 Projecte desenvolupat per 2 estudiants
# 🔧 Centrat en l’aprenentatge de:
# Gestió de memòria
# Entrada/sortida (I/O)
# Interacció amb el maquinari bàsic
# 📚 Ideal per iniciar-se en el desenvolupament de sistemes operatius
# 🚀 Projecte open-source en evolució
# ℹ️ Descripció
# 
# MG-OS és un sistema operatiu educatiu desenvolupat amb el framework Cosmos, amb l’objectiu d’entendre el funcionament intern d’un sistema operatiu.
# 
# Aquest projecte no pretén competir amb sistemes com Windows o Linux, sinó servir com a eina pràctica per comprendre conceptes fonamentals com:
# 
# El procés d’arrencada d’un sistema operatiu
# La interacció amb el maquinari
# La gestió de processos bàsics
# El funcionament d’una consola o interfície simple
# 👥 Autors
# MANEL SANCHEZ
# GERARD LEIVA
# 
# Projecte desenvolupat dins l’àmbit formatiu d’ASIX.
# 
# 🎯 Objectiu del projecte
# 
# Els objectius principals de MG-OS són:
# 
# Aprendre el desenvolupament de sistemes operatius
# Practicar programació en C# a baix nivell
# Entendre el funcionament intern d’un SO
# Crear una base per a futurs experiments i millores
# 🚀 Execució
# 
# Exemple bàsic de funcionament:
# 
# public override void Run()
# {
#     Console.WriteLine("Benvingut a MG-OS");
# }
# 
# En executar el sistema, s’inicia una consola bàsica on es poden implementar comandes personalitzades.
# 
# ⬇️ Instal·lació
# 🔧 Requisits
# Visual Studio
# 
# .NET compatible amb Cosmos
# Cosmos User Kit
# 📦 Passos
# Instal·lar Cosmos User Kit
# Clonar el repositori:
# git clone https://github.com/tu-usuari/MG-OS.git
# Obrir el projecte amb Visual Studio
# Executar-lo amb Cosmos (ISO o màquina virtual)
# 🧪 Estat del projecte
# 
# 🚧 En desenvolupament
# 
# Funcionalitats actuals:
# 
# Arrencada bàsica del sistema
# Sortida per consola
# Estructura inicial del kernel
# 
# Millores previstes:
# 
# Sistema de comandes
# Gestió de memòria bàsica
# Sistema de fitxers simple
# 💡 Contribucions
# 
# Aquest és un projecte d’aprenentatge, però qualsevol aportació és benvinguda.
# 
# Pots:
# 
# Obrir incidències (issues) 🐛
# Proposar millores 🚀
# Donar feedback 💬
# 📖 Finalitat educativa

# MG-OS està pensat com una eina d’aprenentatge.
# Si estàs començant en sistemes operatius, aquest projecte et pot ajudar a entendre conceptes clau de manera pràctica.

# 📜 Llicència

# MIT License
# 
# 🖥️ Comandes inicials del shell de MG-OS
# 
# Per al disseny del shell mínim de MG-OS, s’ha definit un conjunt de comandes bàsiques orientades a un ús senzill del sistema operatiu. Aquestes comandes permeten gestionar fitxers i directoris, consultar informació del sistema i executar accions útils.
# 
# S’han escollit noms curts, clars i fàcils de recordar, evitant copiar directament les comandes tradicionals de Linux per donar identitat pròpia al sistema.
# 
# 📁 Gestió de fitxers i directoris
# llista:
# Mostra el contingut del directori actual.
# 
# Funció: Llistar fitxers i carpetes.
# 
# Exemple:
# llista
# 
# entra:
# Canvia el directori actual al directori indicat.
# 
# Funció: Navegar entre carpetes.
# 
# Paràmetres: Nom del directori.
# 
# Exemple:
# entra documents
# 
# crea:
# Crea un directori nou dins de la ubicació actual.
# 
# Funció: Crear carpetes.
# 
# Paràmetres: Nom del directori.
# 
# Exemple: 
# crea projecte
# borra
# 
# Elimina un directori (preferiblement buit en la primera versió).
# 
# Funció: Eliminar carpetes.
# 
# Paràmetres: Nom del directori.
# 
# Exemple:
# borra proves
# 
# mostra:
# Mostra el contingut d’un fitxer de text.
# 
# Funció: Llegir fitxers.
# 
# Paràmetres: Nom del fitxer.
# 
# Exemple:
# mostra notes.txt
# 
# ⚙️ Informació del sistema
# ajuda:
# Mostra la llista de comandes disponibles amb una breu descripció.
# 
# Exemple:
# ajuda
# 
# ![ajuda](./assets/imatge-ajuda-mg-os.png)
# versio:
# Mostra la versió actual de MG-OS.
# 
# Exemple:
# versio
# 
# mem: 
# Mostra la memòria disponible del sistema.
# 
# Exemple:
# mem
# 
# temps:
# Mostra el temps de funcionament des de l’arrencada del sistema.
# 
# Exemple:
# temps
# 
# 🧰 Útils
# net:
# Neteja la pantalla de la consola.
# 
# Exemple:
# net
# 
# diu:
# Mostra per pantalla el text introduït després de la comanda.
# 
# Exemple:
# diu Hola MG-OS
# 
# apaga:
# Apaga el sistema operatiu.
# 
# Exemple:
# apaga
#
# reinicia:
# Reinicia el sistema operatiu.
# 
# Exemple:
# reinicia
