using System.Collections.Generic;
using System.IO;
using Godot;

namespace BattleSpells.Scripts.Helper
{
    public static class ResourceLoaderHelper
    {
        /// <summary>
        /// Carica tutte le risorse di tipo T da una cartella specificata.
        /// </summary>
        /// <typeparam name="T">Il tipo della risorsa, es. HeroDefinition o CardDefinition.</typeparam>
        /// <param name="folderPath">Il percorso della cartella, ad esempio "res://Resources/Heroes".</param>
        /// <returns>Una lista delle risorse caricate.</returns>
        public static List<T> LoadResourcesFromFolder<T>(string folderPath) where T : Resource
        {
            List<T> resources = [];

            var dir = DirAccess.Open(folderPath);
            if (dir == null)
            {
                GD.PrintErr("Impossibile aprire la cartella: " + folderPath);
                return resources;
            }

            dir.ListDirBegin();
            while (true)
            {
                string fileName = dir.GetNext();
                if (string.IsNullOrEmpty(fileName))
                    break;

                // Salta le cartelle speciali e le directory
                if (fileName == "." || fileName == "..")
                    continue;

                // Se si tratta di un file e termina con .tres (o .res), caricalo
                if (!dir.CurrentIsDir() && fileName.ToLower().EndsWith(".tres"))
                {
                    string fullPath = folderPath + "/" + fileName;
                    T res = ResourceLoader.Load<T>(fullPath);
                    if (res != null)
                    {
                        resources.Add(res);
                        GD.Print("Risorsa caricata: " + fullPath);
                    }
                    else
                    {
                        GD.PrintErr("Impossibile caricare la risorsa: " + fullPath);
                    }
                }
            }

            dir.ListDirEnd();
            return resources;
        }
    }
}
