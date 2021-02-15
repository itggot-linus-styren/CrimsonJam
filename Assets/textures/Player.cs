using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Player : MonoBehaviour
{

    public Sprite[] sprites;
    public new SpriteRenderer renderer;
    public Renderer bg;
    public Enemy enemy;
    public GameObject secondBullet;
    public Asteroid asteroid;
    public TextMeshProUGUI scoreText;
    public float speed = 2f;
    public float yLimit = 0.48f;
    public float bgScrollSpeed = 50f;
    public float minPlayerX = -0.25f;
    public float maxPlayerX = 0.5f;
    public float score = 0;

    Sprite forward, up, down;

    Vector2 position;
    float counter;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        forward = sprites[0];
        up = sprites[1];
        down = sprites[2];

        position = transform.position;
    }

    void CheckDifficulty()
    {
        if (score >= 2500 && score <= 5000)
        {
            bgScrollSpeed = 20f;
            enemy.speed = 1.5f;
            enemy.maxShootModifier = 2f;
            enemy.maxStopTimeModifier = 2f;
            enemy.spawnPositionModifier = -3f;
        } else if (score >= 5000 && score <= 10000)
        {
            bgScrollSpeed = 30f;
            enemy.speed = 2f;
            enemy.maxShootModifier = 1.5f;
            enemy.maxStopTimeModifier = 1.5f;
            asteroid.asteroidSpeed = 1.5f;
            asteroid.maxResetModifier = 2f;
            secondBullet.SetActive(true);
            enemy.spawnPositionModifier = -2.5f;
        } else if (score >= 10000 && score <= 15000)
        {
            bgScrollSpeed = 40f;
            enemy.speed = 2.5f;
            enemy.maxShootModifier = 1f;
            enemy.maxStopTimeModifier = 1f;
            enemy.bulletSpeed = 2f;
            asteroid.asteroidSpeed = 2f;
            asteroid.maxResetModifier = 1.5f;
            enemy.spawnPositionModifier = -1.5f;
        } else if (score >= 15000)
        {
            bgScrollSpeed = 50f;
            enemy.speed = 3f;
            enemy.maxShootModifier = 0.5f;
            enemy.minStopTimeModifier = 0.0f;
            enemy.maxStopTimeModifier = 0.25f;
            asteroid.asteroidSpeed = 2.5f;
            asteroid.maxResetModifier = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDifficulty();
        counter += Time.deltaTime;
        if (counter > 0.1)
        {
            counter = 0;
            score += 5 * Mathf.Floor(enemy.speed);
        }

        scoreText.text = score.ToString();
        bg.material.SetVector("_ScrollSpeed", new Vector2(bgScrollSpeed, 0));

        float input = Input.GetAxis("Vertical");

        //if (Mathf.Abs(input - 0) < Mathf.Epsilon && Mathf.Abs(Input.GetAxis("Horizontal")) - 0 > Mathf.Epsilon)
        //{
        //    input = -Input.GetAxis("Horizontal");
        //}

        if (input == 0)
        {
            renderer.sprite = forward;
        } else if (input > 0)
        {
            renderer.sprite = up;
            position.y += speed * Time.deltaTime;
        } else
        {
            renderer.sprite = down;
            position.y -= speed * Time.deltaTime;
        }

        if (Mathf.Abs(position.y) > yLimit)
        {
            position.y = yLimit * (Mathf.Abs(position.y) / position.y);
        }

        input = Input.GetAxis("Horizontal");

        if (input > 0)
        {
            position.x += speed * Time.deltaTime;
        } else if (input < 0)
        {
            position.x -= speed * Time.deltaTime;
        }

        if (position.x > maxPlayerX) position.x = maxPlayerX;
        else if (position.x < minPlayerX) position.x = minPlayerX;
            
        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (dead) return;
        PlayerPrefs.SetInt("score", (int) score);
        dead = true;
        Debug.Log("EXPLOSWION" + col.gameObject.name);
        SceneManager.LoadScene(1);
    }
}
