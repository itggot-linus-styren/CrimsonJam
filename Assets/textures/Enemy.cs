using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite[] sprites;
    public new SpriteRenderer renderer;
    public float speed = 1.5f;
    public float bulletSpeed = 1f;
    public float yLimit = 0.48f;
    public Transform bullet;
    public Player player;
    public Asteroid asteroid;
    public GameObject explosionPrefab;
    public float minShootModifier = 0.5f;
    public float maxShootModifier = 3f;
    public float minStopTimeModifier = 0.5f;
    public float maxStopTimeModifier = 3f;
    public float spawnPositionModifier = -1f;

    Sprite forward, up, down;

    Vector2 initialPosition;
    Vector2 position;
    Vector2 bulletPosition;
    bool shooting = false;
    bool stopped = false;
    bool canShoot = false;
    float cooldown = 1f;
    float cooldownCounter = 0;
    float stopCooldown = 0;
    float stopCooldownCounter = 0;    

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        forward = sprites[0];
        up = sprites[1];
        down = sprites[2];

        initialPosition = transform.position;

        position = transform.position;
        position.x = spawnPositionModifier;
        bulletPosition.y = 1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (stopped)
        {
            renderer.sprite = forward;

            if (stopCooldownCounter > stopCooldown)
            {
                stopped = false;
            }
            else
            {
                stopCooldownCounter += Time.deltaTime;
            }
        } else
        {
            if (Mathf.Abs(transform.position.y - player.transform.position.y) < speed * Time.deltaTime)
            {
                position.y = player.transform.position.y;    
                canShoot = true;
                transform.position = position;
            }

            if (Mathf.Abs(transform.position.y - player.transform.position.y) < Mathf.Epsilon)
            {
                renderer.sprite = forward;
            }
            else if (transform.position.y < player.transform.position.y)
            {
                renderer.sprite = up;
                position.y += speed * Time.deltaTime;
            }
            else
            {
                renderer.sprite = down;
                position.y -= speed * Time.deltaTime;
            }

            if (Mathf.Abs(position.y) > yLimit)
            {
                position.y = yLimit * (Mathf.Abs(position.y) / position.y);
            }

            if (Mathf.Abs(transform.position.x - initialPosition.x) > speed * Time.deltaTime)
            {
                position.x += speed * Time.deltaTime;
                transform.position = position;
                canShoot = false;
            } else
            {
                position.x = initialPosition.x;
            }

            if (shooting)
            {
                stopped = true;
                stopCooldown = Random.Range(minStopTimeModifier, maxStopTimeModifier);
                stopCooldownCounter = 0;
            }
        }

        // SHOOTING

        if (!shooting)
        {
            if (!stopped && cooldownCounter > cooldown && canShoot)
            {
                shooting = true;
                bulletPosition = transform.position;
                bulletPosition.y += 0.035f;
                bulletPosition.x += 0.035f;
                stopped = true;
            }
            else if (!stopped)
            {
                cooldownCounter += Time.deltaTime;
            }

        }
        else
        {
            bulletPosition.x += bulletSpeed * Time.deltaTime;

            if (bulletPosition.x > 1)
            {
                shooting = false;
                cooldown = Random.Range(minShootModifier, maxShootModifier);
                cooldownCounter = 0;
            }
        }

        bullet.transform.position = bulletPosition;
        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (asteroid.stop || col.gameObject != asteroid.gameObject) return;
        player.score += 1000;
        Debug.Log("EXPLOSWION" + col.gameObject.name);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, null);
        position.x = spawnPositionModifier;
        transform.position = position;
        asteroid.Stop();
    }
}
