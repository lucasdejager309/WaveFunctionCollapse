using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse {

    static int i = 0;
    
    public static Sprite GenerateFromImage(Texture2D _example, Vector2Int _size) {
        List<Tile> tiles = Tile.GetTiles(_example);
        
        return PixelMap.CreateSprite(GeneratePixelMap(tiles, _size), _size);
    }

    static Dictionary<Vector2Int, Color> GeneratePixelMap(List<Tile> _tiles, Vector2Int _size) {
        float currentTime = Time.realtimeSinceStartup;
        
        Dictionary<Vector2Int, Square> squares = new Dictionary<Vector2Int, Square>();
        for (int x = 0; x < _size.x; x++) {
            for (int y = 0; y < _size.y; y++) {
                squares.Add(new Vector2Int(x, y), new Square(_tiles));
            }
        }

        while(!IsFullyCollapsed(squares)) {
            Vector2Int squarePos = Square.GetSquareWithLowestEntropy(squares, _size, i);
            Color color = squares[squarePos].Collapse();
            
            Debug.Log(GetNeighbours(squarePos, _size).Count);
            foreach (Vector2Int neighbour in GetNeighbours(squarePos, _size)) {
                squares[squarePos].UpdatePossibleTiles(color, neighbour-squarePos);
            }

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

    static bool IsFullyCollapsed(Dictionary<Vector2Int, Square> _squareDict) {
        foreach (var square in _squareDict) {
            if (!square.Value.hasCollapsed) return false;
        }
        return true;
    } 

    //idk of deze goed werkt
    static List<Vector2Int> GetNeighbours(Vector2Int _pos, Vector2Int _size) {
        List<Vector2Int> directions = new List<Vector2Int>() {
            {new Vector2Int(-1, 1)}, //UR
            {new Vector2Int(0, 1)}, //R
            {new Vector2Int(1, 1)}, //DR
            {new Vector2Int(1, -1)}, //UL
            {new Vector2Int(0, -1)}, //L
            {new Vector2Int(-1, -1)}, //DL
            {new Vector2Int(1, 0)}, //U
            {new Vector2Int(-1, 0)}, //D
        };

        List<Vector2Int> neighbours = new List<Vector2Int>();
        
        for (int i = 0; i < directions.Count; i++) {
            Vector2Int neighbourPos = _pos+directions[i];
            if ((neighbourPos.x > _size.x || neighbourPos.x < 0) || (neighbourPos.y > _size.y || neighbourPos.y < 0)) {
                neighbours.Add(neighbourPos);
            }
        }

        return neighbours;
    }
}