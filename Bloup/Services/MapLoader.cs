using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace Bloup.Services;

public class MapLoader
{
    // Dictionnaire pour stocker la carte : position (Vector2) = cle, valeur = ID de la tuile.
    public Dictionary<Vector2, int> TileMap { get; private set; } = [];

    // Méthode pour charger une carte à partir d'un fichier CSV.
    public void LoadMap(string filePath)
    {
        // Vérifie si le fichier existe
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Le fichier spécifié est introuvable : {filePath}");
        }

        try
        {
            // Ouvre le fichier pour lecture.
            using StreamReader reader = new(filePath);

            int y = 0; // Représente la ligne (coordonnée verticale).
            string line;

            // Lecture ligne par ligne.
            while ((line = reader.ReadLine()) != null)
            {
                // Découpe la ligne en valeurs séparées par des virgules.
                string[] items = line.Split(',');

                for (int x = 0; x < items.Length; x++) // Parcours des colonnes.
                {
                    // Tente de convertir chaque valeur en entier.
                    if (int.TryParse(items[x], out int tileId))
                    {
                        // Ajoute la tuile au dictionnaire.
                        TileMap[new Vector2(x, y)] = tileId;
                    }
                }

                y++; // Passe à la rangée suivante.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du chargement de la carte : {ex.Message}");
        }
    }
}