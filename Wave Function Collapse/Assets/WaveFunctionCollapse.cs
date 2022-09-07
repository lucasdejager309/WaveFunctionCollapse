using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField] Texture2D exampleImage;
    List<Tile> tilePallete = new List<Tile>();
    Dictionary<Vector2Int, Tile> tileDict = new Dictionary<Vector2Int, Tile>();
    Dictionary<Vector2Int, bool> isTakenDict = new Dictionary<Vector2Int, bool>();

    [SerializeField] SpriteRenderer spriteRenderer; //TEMP

    //TEMP
    void Start() {
        tilePallete = GetTiles(exampleImage);

        spriteRenderer.sprite = CreateSprite(GetPixelMap(exampleImage), new Vector2Int(exampleImage.width, exampleImage.height));
    }

    //CREATE PIXELMAP FROM TILES
    Dictionary<Vector2Int, Color> CreateNewPixelMap(List<Tile> tiles, Vector2Int size) {

        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                isTakenDict.Add(new Vector2Int(x, y), false);
            }
        }

        while (!IsFullyCollapsed(isTakenDict)) {
            Vector2Int posToCollapse;
            
            if (!isTakenDict.ContainsValue(true)) {
                //first position
                posToCollapse = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
                CollapseTile(posToCollapse);
            } else {
                //brrrrr
            }
        }


        return null;
    }

    void CollapseTile(Vector2Int posToCollapse) {
        tileDict.Add(posToCollapse, GetTileForPos(posToCollapse, tilePallete));
        isTakenDict[posToCollapse] = true;
    }

    Tile GetTileForPos(Vector2Int pos, List<Tile> tiles) {
        //ALL TILES ARE POSSIBLE
        List<Tile> possibleTiles = tiles;

        Vector2Int[] adjacentPositions = GetAdjacentPositions(pos);
        foreach(Vector2Int adjPos in adjacentPositions) {
            if (isTakenDict[adjPos]) {
                Tile adjTile = tileDict[adjPos];
                Dictionary<Color, List<Vector2Int>> rules = adjTile.GetRules();

                foreach (var rule in rules) {
                    if (!rule.Value.Contains(-(pos-adjPos))) {
                        Tile tileToRemove = new Tile();
                        foreach (Tile tile in possibleTiles) {
                            if (tile.color == rule.Key) {
                                tileToRemove = tile;
                            }
                        }
                        possibleTiles.Remove(tileToRemove);
                    }
                }
            }
        }

        return possibleTiles[0];
    }

    bool IsFullyCollapsed(Dictionary<Vector2Int, bool> dict) {
        foreach (var kv in dict) {
            if (!kv.Value) return false;
        }
        return true;
    }

    //GETS TILES WITH RULES FROM EXAMPLEIMAGE
    List<Tile> GetTiles(Texture2D exampleImage) {
        Dictionary<Vector2Int, Color> examplePixelMap = GetPixelMap(exampleImage);
        List<Color> colors = new List<Color>();
        List<Tile> tilesToReturn = new List<Tile>();
        foreach (var pixel in examplePixelMap) {
            if (!colors.Contains(pixel.Value)) {
                colors.Add(pixel.Value);
                tilesToReturn.Add(new Tile(pixel.Value, examplePixelMap));
            }
        }

        return tilesToReturn;
    }

    //CONVERTS IMAGE TO DICTIONARY
    Dictionary<Vector2Int, Color> GetPixelMap(Texture2D image) {
        Dictionary<Vector2Int, Color> dict = new Dictionary<Vector2Int, Color>();
        for (int x = 0; x < image.width; x++) {
            for (int y = 0; y < image.height; y++) {
                dict.Add(new Vector2Int(x, y), image.GetPixel(x, y));
            }
        }

        return dict;
    }

    //CREATE SPRITE FROM DICTIONARY
    Sprite CreateSprite(Dictionary<Vector2Int, Color> map, Vector2Int size) {
        Texture2D tex = new Texture2D(size.x, size.y);
        
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                tex.SetPixel(x, y, map[new Vector2Int(x, y)]);
        }
        }
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
    }

    Vector2Int[] GetAdjacentPositions(Vector2Int pos) {
        List<Vector2Int> directions = new List<Vector2Int> {
            {new Vector2Int(1, 0)}, //right
            {new Vector2Int(-1, 0)}, //left
            {new Vector2Int(0, 1)}, //bottom
            {new Vector2Int(0, -1)} //top
        };

        Vector2Int[] positions = new Vector2Int[4];
        int i = 0;
        foreach (Vector2Int direction in directions) {
            positions[i] = pos + direction;
            i++;
        }

        return positions;
    }

}

    
