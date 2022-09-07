using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] Texture2D exampleImage;
    [SerializeField] SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer.sprite = WaveFunctionCollapse.GenerateFromImage(exampleImage, new Vector2Int(exampleImage.width, exampleImage.height));
    }
}
