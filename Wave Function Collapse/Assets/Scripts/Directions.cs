using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions {
    public static List<Vector2Int> directions {get; private set;} = new List<Vector2Int>() {
        {new Vector2Int(-1, 1)}, //UR
        {new Vector2Int(0, 1)}, //R
        {new Vector2Int(1, 1)}, //DR
        {new Vector2Int(1, -1)}, //UL
        {new Vector2Int(0, -1)}, //L
        {new Vector2Int(-1, -1)}, //DL
        {new Vector2Int(1, 0)}, //U
        {new Vector2Int(-1, 0)}, //D
    };
}
