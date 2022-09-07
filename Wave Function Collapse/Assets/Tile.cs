using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public Color color {get; private set;}
    public float frequency {get; private set;}
    Dictionary<Color, List<Vector2Int>> rules = new Dictionary<Color, List<Vector2Int>>();

    public List<Vector2Int> directions {get; private set;} = new List<Vector2Int> {
        {new Vector2Int(1, 0)}, //right
        {new Vector2Int(-1, 0)}, //left
        {new Vector2Int(0, 1)}, //bottom
        {new Vector2Int(0, -1)} //top
    };

    public Tile(){}

    public Tile(Color color, Dictionary<Vector2Int, Color> example) {
        this.color = color;
        SetRules(color, example);
    }

    public Dictionary<Color, List<Vector2Int>> GetRules() {
        return rules;
    }

    void SetRules(Color color, Dictionary<Vector2Int, Color> example) {
        int rarity = 0;
        foreach (var pixel in example) {
            if (pixel.Value == color) {
                rarity++;
                Dictionary<Vector2Int, Color> adjacents = GetAdjacents(pixel.Key, example);

                foreach (var adj in adjacents) {
                    if (!rules.ContainsKey(adj.Value)) {
                        rules.Add(adj.Value, new List<Vector2Int>(){adj.Key});
                    } else if (!rules[adj.Value].Contains(adj.Key)) {
                        rules[adj.Value].Add(adj.Key);
                    }
                }
            }
        }
        frequency = (float)rarity/(float)example.Count;
    }

    Dictionary<Vector2Int, Color> GetAdjacents(Vector2Int pos, Dictionary<Vector2Int, Color> example) {
                
        Dictionary<Vector2Int, Color> dict = new Dictionary<Vector2Int, Color>();
        foreach (Vector2Int direction in directions) {
            if (example.ContainsKey(pos+direction)) dict.Add(direction, example[pos+direction]);
        }
        return dict;
    }
}