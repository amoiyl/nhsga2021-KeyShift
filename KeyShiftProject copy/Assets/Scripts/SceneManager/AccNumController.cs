using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccNumController : MonoBehaviour
{

    public Text accuracyNum;
    public Slider accuracySlider;

    void Start()
    {
        accuracyNum.text = "0%";
    }

    void Update()
    {
        accuracyNum.text = (Mathf.Round(accuracySlider.GetComponent<AccuracyBar>().GetAccuracyPoints() * 100f)) + "%";
    }
}
