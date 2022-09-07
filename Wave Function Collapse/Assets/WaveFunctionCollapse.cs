using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
    public List<Tile> possibleTiles;
    public Tile selectedTile = null;
    public bool hasCollapsed {get; private set;} = false;

    public Square(List<Tile> _possibleTiles) {
        possibleTiles = _possibleTiles;
    }

    public void Collapse() {
        selectedTile = Tile.SelectTile(possibleTiles);
        hasCollapsed = true;
    }

    public float GetShannonEntropy() {
        float weightSum = 0;
        foreach (Tile tile in possibleTiles) {
            weightSum += tile.weight;
        }

        return (Mathf.Log(weightSum) - (Mathf.Log(weightSum) / weightSum));
    }
}

public class WaveFunctionCollapse {
    public static Sprite GenerateFromImage(Texture2D _example, Vector2Int _size) {
        List<Tile> tiles = Tile.GetTiles(_example);
        
        GeneratePixelMap(tiles, _size);
        
        //convert pixel map to sprite

        //return sprite
        return null;
    }

    static Dictionary<Vector2Int, Color> GeneratePixelMap(List<Tile> _tiles, Vector2Int _size) {
        Dictionary<Vector2Int, Square> squares = new Dictionary<Vector2Int, Square>();
        for (int x = 0; x < _size.x; x++) {
            for (int y = 0; y < _size.y; y++) {
                squares.Add(new Vector2Int(x, y), new Square(_tiles));
            }
        }

        while(!IsFullyCollapsed(squares)) {
            squares[GetSquareWithLowestEntropy(squares)].Collapse();
            
            //update neighbour's possible tiles;
        }

        return null;
    }   

    static Vector2Int GetSquareWithLowestEntropy(Dictionary<Vector2Int, Square> _squareDict) {
        KeyValuePair<Vector2Int, Square> lowestSquare = new KeyValuePair<Vector2Int, Square>();
        foreach (var square in _squareDict) {
            if (square.Value.GetShannonEntropy() < lowestSquare.Value.GetShannonEntropy()) {
                lowestSquare = square;
            }
        }

        return lowestSquare.Key;
    }

    static bool IsFullyCollapsed(Dictionary<Vector2Int, Square> _squareDict) {
        foreach (var square in _squareDict) {
            if (!square.Value.hasCollapsed) return false;
        }
        return true;
    } 
}