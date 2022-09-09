using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] Texture2D exampleImage;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector2Int renderSize;

    void Start() {
        spriteRenderer.sprite = Sprite.Create(exampleImage, new Rect(0.0f, 0.0f, exampleImage.width, exampleImage.height), new Vector2(0.5f, 0.5f), 100f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            spriteRenderer.sprite = WaveFunctionCollapse.GenerateFromImage(exampleImage, new Vector2Int(renderSize.x, renderSize.y));
        }
    }
}
