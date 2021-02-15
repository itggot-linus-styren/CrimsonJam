using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Sprite[] sprites;
    public new SpriteRenderer renderer;
    public float animSpeed = 0.25f;

    float counter = 0;
    int sprite;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > animSpeed)
        {
            counter = 0;
            sprite = ++sprite;

            if (sprite >= sprites.Length)
            {
                Destroy(gameObject);
            }
        }

        if (sprite < sprites.Length) renderer.sprite = sprites[sprite];
    }
}
