using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public Text accuracyValue;
    public Text scoreValue;

    public SpriteRenderer letterGrade;

    public Sprite A;
    public Sprite B;
    public Sprite C;
    public Sprite D;
    public Sprite S;

    public Text CSMAC;

    public AudioSource escapeClip;

    void Start()
    {
        accuracyValue.text = "Accuracy: " + Data.instance.accuracy;
        scoreValue.text = "Score: " + Data.instance.score;

        if (Data.instance.accuracy < 70) {
            letterGrade.sprite = D;
            CSMAC.text = "Come see me after class.";
        }
        else if (Data.instance.accuracy < 80)
        {
            CSMAC.text = "Suggestion: try paying attention";
            letterGrade.sprite = C;
        }
        else if (Data.instance.accuracy < 90)
        {
            CSMAC.text = "Better than randomly guessing I presume.";
            letterGrade.sprite = B;
        }
        else if (Data.instance.accuracy < 95)
        {
            CSMAC.text = "Lucky break.";
            letterGrade.sprite = A;
        }
        else if (Data.instance.accuracy < 100)
        {
            CSMAC.text = "Pretty sure you cheated";
            letterGrade.sprite = S;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeClip.Play();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
