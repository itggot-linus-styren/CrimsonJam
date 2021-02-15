using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scroller : MonoBehaviour
{

    public new SpriteRenderer renderer;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        scoreText.text = "score\n" + PlayerPrefs.GetInt("score").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.SetVector("_ScrollSpeed", new Vector2(10f, 0));
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
