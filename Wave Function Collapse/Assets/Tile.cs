using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public float weight {get; private set;}
    public Color color {get; private set;}
    public Dictionary<Color, List<Vector2Int>> allowedNeighbours = new Dictionary<Color, List<Vector2Int>>();

    public static List<Tile> GetTiles(Texture2D _exampleImage) {
        //get pixel map
        Dictionary<Vector2Int, Color> examplePixelMap = GetPixelMap(_exampleImage);
        
        List<Color> colors = new List<Color>();
        List<Tile> tilesToReturn = new List<Tile>();
        foreach (var pixel in examplePixelMap) {
            //check if color isnt already in tilesToReturn
            if (!colors.Contains(pixel.Value)) {
                colors.Add(pixel.Value);
                            
                tilesToReturn.Add(new Tile(pixel.Value, examplePixelMap));
            }
        }

        return tilesToReturn;
    }

    public static Dictionary<Vector2Int, Color> GetPixelMap(Texture2D _image) {
        Dictionary<Vector2Int, Color> dict = new Dictionary<Vector2Int, Color>();
        for (int x = 0; x < _image.width; x++) {
            for (int y = 0; y < _image.height; y++) {
                dict.Add(new Vector2Int(x, y), _image.GetPixel(x, y));
            }
        }

        return dict;
    }

    public static Tile SelectTile(List<Tile> _tiles) {
        Tile pickedTile = null;
        float probabilitySum = 0;
        
        //get sum of probabilities
        foreach(Tile currentObject in _tiles) {
            probabilitySum += currentObject.weight;
        }

        //generate random number
        float randomFloat = Random.Range(0, probabilitySum+1);


        foreach(Tile currentObject in _tiles) {
            if (randomFloat > 0) {
                randomFloat -= currentObject.weight;
                pickedTile = currentObject;
            } else break;
        }

        if (pickedTile == null) {
            pickedTile = _tiles[_tiles.Count-1];
        }
     
        return pickedTile;
    }

    public Tile(Color _color, Dictionary<Vector2Int, Color> _examplePixelMap) {
        color = _color;

        float rarity = 0;
        foreach (var pixel in _examplePixelMap) {
            if (pixel.Value == color) rarity++;
        }
        weight = rarity/_examplePixelMap.Count;

        SetAllowedNeighbours(color, _examplePixelMap);
    }

    void SetAllowedNeighbours(Color color, Dictionary<Vector2Int, Color> _examplePixelMap) {
        List<Vector2Int> directions = new List<Vector2Int> {
            {new Vector2Int(1, 0)}, //right
            {new Vector2Int(-1, 0)}, //left
            {new Vector2Int(0, 1)}, //bottom
            {new Vector2Int(0, -1)} //top
        };
        
        foreach (var pixel in _examplePixelMap) {
            if (pixel.Value == color) {
                //get neighbours
                Dictionary<Vector2Int, Color> neighbours = new Dictionary<Vector2Int, Color>();
                foreach (Vector2Int direction in directions) {
                    if (_examplePixelMap.ContainsKey(pixel.Key+direction)) neighbours.Add(direction, _examplePixelMap[pixel.Key+direction]);
                }

                foreach (var neighbour in neighbours) {
                    if (!allowedNeighbours.ContainsKey(neighbour.Value)) {
                        //if allowedneighbours doesnt contain color, create new entry
                        allowedNeighbours.Add(neighbour.Value, new List<Vector2Int>(){neighbour.Key});
                    } else if (!allowedNeighbours[neighbour.Value].Contains(neighbour.Key)) {
                        //else if entry of color doesnt contain direction, add direction
                        allowedNeighbours[neighbour.Value].Add(neighbour.Key);
                    }
                }
            }
            
        }
    }
}