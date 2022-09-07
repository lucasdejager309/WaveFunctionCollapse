using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField] Texture2D exampleImage;
    List<Tile> tilePallete = new List<Tile>();
    Dictionary<Vector2Int, Tile> tileDict = new Dictionary<Vector2Int, Tile>();
    Dictionary<Vector2Int, bool> isTakenDict = new Dictionary<Vector2Int, bool>();
    Dictionary<Vector2Int, List<Tile>> possibleTileDict = new Dictionary<Vector2Int, List<Tile>>();

    [SerializeField] SpriteRenderer spriteRenderer;

    //TEMP
    void Start() {
        tilePallete = GetTiles(exampleImage);

        spriteRenderer.sprite = CreateSprite(CreateNewPixelMap(GetTiles(exampleImage), new Vector2Int(exampleImage.width, exampleImage.height)), new Vector2Int(exampleImage.width, exampleImage.height));
        
        //Recreate Sprite
        //CreateSprite(GetPixelMap(exampleImage), new Vector2Int(exampleImage.width, exampleImage.height));
    }
 
    //CREATE PIXELMAP FROM TILES
    Dictionary<Vector2Int, Color> CreateNewPixelMap(List<Tile> tiles, Vector2Int size) {

        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                isTakenDict.Add(new Vector2Int(x, y), false);
                possibleTileDict.Add(new Vector2Int(x, y), tiles);
            }
        }

        while (!IsFullyCollapsed(isTakenDict)) {
            Vector2Int posToCollapse;
            
            if (!isTakenDict.ContainsValue(true)) {
                //first position
                posToCollapse = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
                CollapseTile(posToCollapse);
            } else {
                break;

                //get position with least possible tiles
                //do something with possibleTileDict!!

                //collapse position

                //continue
            }
        }


        return TileToColorDict(tileDict);
    }

    Dictionary<Vector2Int, Color> TileToColorDict(Dictionary<Vector2Int, Tile> dict) {
        Dictionary<Vector2Int, Color> dictToReturn = new Dictionary<Vector2Int, Color>();
        foreach (var entry in dict) {
            dictToReturn.Add(entry.Key, entry.Value.color);
        }
        return dictToReturn;
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
            if (isTakenDict.ContainsKey(adjPos)) {
                if (isTakenDict[adjPos]) {
                    Tile adjTile = tileDict[adjPos];
                    Dictionary<Color, List<Vector2Int>> rules = adjTile.GetRules();

                    foreach (var rule in rules) {
                        if (!rule.Value.Contains(-(pos - adjPos))) {
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
            
        }

        return PickTileOnRarity(possibleTiles);
    }

    Tile PickTileOnRarity(List<Tile> tiles) {
        Tile pickedTile = null;
        float probabilitySum = 0;
        
        //get sum of probabilities
        foreach(Tile currentObject in tiles) {
            probabilitySum += currentObject.frequency;
        }

        //generate random number
        float randomFloat = Random.Range(0, probabilitySum+1);


        foreach(Tile currentObject in tiles) {
            if (randomFloat > 0) {
                randomFloat -= currentObject.frequency;
                pickedTile = currentObject;
            } else break;
        }

        if (pickedTile == null) {
            pickedTile = tiles[tiles.Count-1];
        }
     
        return pickedTile;
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
                if (map.ContainsKey(new Vector2Int(x, y))) {
                    tex.SetPixel(x, y, map[new Vector2Int(x, y)]);
                } else {
                    tex.SetPixel(x, y, Color.white);
                }       
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

    
