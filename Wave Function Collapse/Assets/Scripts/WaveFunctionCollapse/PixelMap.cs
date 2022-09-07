using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelMap
{
    public static Sprite CreateSprite(Dictionary<Vector2Int, Color> map, Vector2Int size) {
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

    public static Dictionary<Vector2Int, Color> GetPixelMap(Texture2D _image) {
        Dictionary<Vector2Int, Color> dict = new Dictionary<Vector2Int, Color>();
        for (int x = 0; x < _image.width; x++) {
            for (int y = 0; y < _image.height; y++) {
                dict.Add(new Vector2Int(x, y), _image.GetPixel(x, y));
            }
        }

        return dict;
    }
}
