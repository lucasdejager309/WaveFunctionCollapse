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

    public Square() {
    }

    public Color Collapse() {
        Tile tile = Tile.SelectTile(possibleTiles);
        selectedTile = tile;
        hasCollapsed = true;

        return tile.color;
    }

    public void UpdatePossibleTiles(Color _color, Vector2Int _relativePos) {
        //do stuff
    }

    public static Vector2Int GetSquareWithLowestEntropy(Dictionary<Vector2Int, Square> _squareDict, Vector2Int _size, int i) {
        KeyValuePair<Vector2Int, Square> lowestSquare = new KeyValuePair<Vector2Int, Square>(new Vector2Int(Random.Range(0, _size.x), Random.Range(0, _size.y)), new Square());

        if (i == 0) return lowestSquare.Key;

        foreach (var square in _squareDict) {
            if (!square.Value.hasCollapsed) {
                if (square.Value.shannonEntropy <= lowestSquare.Value.shannonEntropy) {
                    lowestSquare = square;
                }
            }
        }

        return lowestSquare.Key;
    }

    float GetShannonEntropy() {
        float weightSum = 0;
        foreach (Tile tile in possibleTiles) {
            weightSum += tile.weight;
        }

        return (Mathf.Log(weightSum) - (Mathf.Log(weightSum) / weightSum));
    }
}