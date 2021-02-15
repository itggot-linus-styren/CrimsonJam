using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public new SpriteRenderer renderer;
    public Transform player;
    public float animSpeed = 0.25f;
    public float asteroidSpeed = 1f;
    public float minResetModifier = 0.5f;
    public float maxResetModifier = 3f;
    public bool stop = true;

    Vector2 position;
    float counter = 0;
    int sprite;    
    float cooldown = 2f;
    float cooldownCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        position = transform.position;
    }

    public void Stop()
    {
        stop = true;
        cooldown = Random.Range(minResetModifier, maxResetModifier);
        cooldownCounter = 0;
        position.x = 1;
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > animSpeed)
        {
            counter = 0;
            sprite = ++sprite % 4;
        }

        if (stop)
        {
            if (cooldownCounter > cooldown)
            {
                stop = false;                
                position.y = player.position.y;
            } else
            {
                cooldownCounter += Time.deltaTime;
            }
            
        } else
        {
            position.x -= asteroidSpeed * Time.deltaTime;

            if (position.x < -1)
            {
                Stop();
            }

            transform.position = position;

            renderer.sprite = sprites[sprite];
        }
    }
}
