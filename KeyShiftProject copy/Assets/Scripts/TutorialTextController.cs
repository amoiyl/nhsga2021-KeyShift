using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
    public string introText;
    private string introCurrentText = "";
    public Text introTutorialText;

    public string breakText;
    private string breakCurrentText = "";
    public Text breakTutorialText;

    public GameObject background1;
    public GameObject background2;

    public float charDelay = 0.02f;

    void Start()
    {
        background1.SetActive(false);
        background2.SetActive(false);
        if (Data.instance.MapFileName.Equals("Tutorial_Easy"))
        {
            background2.SetActive(true);
            StartCoroutine(IntroTyper());
        }
    }

    IEnumerator IntroTyper()
    {
        for (int i = 1; i <= introText.Length; i++)
        {
            introCurrentText = introText.Substring(0, i);
            introTutorialText.text = introCurrentText;
            yield return new WaitForSeconds(charDelay);
        }
        yield return new WaitForSeconds(6);
        introTutorialText.enabled = false;
        background2.SetActive(false);

        yield return new WaitForSecondsRealtime(15); //15 seconds since start of music
        background1.SetActive(true);
        for (int i = 1; i <= breakText.Length; i++)
        {
            breakCurrentText = breakText.Substring(0, i);
            breakTutorialText.text = breakCurrentText;
            yield return new WaitForSeconds(charDelay);
        }
        yield return new WaitForSeconds(5);
        breakTutorialText.enabled = false;
        background1.SetActive(false);
    }
} 
