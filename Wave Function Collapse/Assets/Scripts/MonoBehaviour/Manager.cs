using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] Texture2D exampleImage;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector2Int renderSize;

    void Start() {   
        spriteRenderer.sprite = WaveFunctionCollapse.GenerateFromImage(exampleImage, new Vector2Int(renderSize.x, renderSize.y));
    }
}
