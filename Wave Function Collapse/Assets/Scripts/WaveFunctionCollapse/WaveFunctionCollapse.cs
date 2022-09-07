using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
    public List<Tile> possibleTiles;
    public Tile selectedTile = null;
    public float shannonEntropy = 0;
    public bool hasCollapsed {get; private set;} = false;

    public Square(List<Tile> _possibleTiles) {
        possibleTiles = _possibleTiles;
        shannonEntropy = GetShannonEntropy();
    }

    public void Collapse() {
        selectedTile = Tile.SelectTile(possibleTiles);
        hasCollapsed = true;
    }

    float GetShannonEntropy() {
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
        
        return PixelMap.CreateSprite(GeneratePixelMap(tiles, _size), _size);
    }

    static Dictionary<Vector2Int, Color> GeneratePixelMap(List<Tile> _tiles, Vector2Int _size) {
        int i = 0;
        float currentTime = Time.realtimeSinceStartup;
        
        Dictionary<Vector2Int, Square> squares = new Dictionary<Vector2Int, Square>();
        for (int x = 0; x < _size.x; x++) {
            for (int y = 0; y < _size.y; y++) {
                squares.Add(new Vector2Int(x, y), new Square(_tiles));
            }
        }

        while(!IsFullyCollapsed(squares)) {
            squares[GetSquareWithLowestEntropy(squares, _size)].Collapse();
            
            //update neighbour's possible tiles;

            i++;
        }

        Dictionary<Vector2Int, Color> dictToReturn = new Dictionary<Vector2Int, Color>();
        foreach (var square in squares) {
            dictToReturn.Add(square.Key, square.Value.selectedTile.color);
        }

        Debug.Log("Iterations: " + i);
        Debug.Log("Execution Time: " + RoundToDigit.Round(Time.realtimeSinceStartup - currentTime, 4) + " seconds");

        return dictToReturn;
    }   

    //TODO: somehow it still has a chance of outputting a square that has already been collapsed??
    static Vector2Int GetSquareWithLowestEntropy(Dictionary<Vector2Int, Square> _squareDict, Vector2Int _size) {
        KeyValuePair<Vector2Int, Square> lowestSquare = new KeyValuePair<Vector2Int, Square>();
        
        Vector2Int squarePos = new Vector2Int(Random.Range(0, _size.x), Random.Range(0, _size.y));
        
        if (lowestSquare.Value != null) {
            foreach (var square in _squareDict) {
            if (!square.Value.hasCollapsed || square.Value.selectedTile == null) continue; //why doesnt this work? (the OR should be overkill)
            
            if (square.Value.shannonEntropy < lowestSquare.Value.shannonEntropy) {
                    lowestSquare = square;
                    squarePos = square.Key;
                }
            }
        }
        

        return squarePos;
    }

    static bool IsFullyCollapsed(Dictionary<Vector2Int, Square> _squareDict) {
        foreach (var square in _squareDict) {
            if (!square.Value.hasCollapsed) return false;
        }
        return true;
    } 
}