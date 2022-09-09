using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public float weight {get; private set;}
    public Color color {get; private set;}
    public Dictionary<Color, List<Vector2Int>> allowedNeighbours = new Dictionary<Color, List<Vector2Int>>();

    public static List<Tile> GetTiles(Texture2D _exampleImage) {
        //get pixel map
        Dictionary<Vector2Int, Color> examplePixelMap = PixelMap.GetPixelMap(_exampleImage);
        
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

    public static Tile SelectTile(List<Tile> _tiles) {
        Tile pickedTile = null;
        float probabilitySum = 0;
        
        //get sum of probabilities
        foreach(Tile currentTile in _tiles) {
            probabilitySum += currentTile.weight;
        }

        //generate random number
        float randomFloat = Random.Range(0, probabilitySum+1);


        foreach(Tile currentTile in _tiles) {
            if (randomFloat > 0) {
                randomFloat -= currentTile.weight;
                pickedTile = currentTile;
            } else break;
        }

        if (pickedTile == null && _tiles.Count > 0) {
            pickedTile = _tiles[0];
        }
     
        return pickedTile;
    }

    public static Tile GetTileFromList(Color _color, List<Tile> _list) {
        foreach (Tile tile in _list) {
            if (tile.color == _color) return tile;
        }

        return null;
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

    public Tile(Color _color) {
        color = _color;
    }

    void SetAllowedNeighbours(Color color, Dictionary<Vector2Int, Color> _examplePixelMap) {      
        foreach (var pixel in _examplePixelMap) {
            if (pixel.Value == color) {
                //get neighbours
                Dictionary<Vector2Int, Color> neighbours = new Dictionary<Vector2Int, Color>();
                foreach (Vector2Int direction in Directions.directions) {
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