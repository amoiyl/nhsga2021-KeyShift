using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccuracyBar : MonoBehaviour
{
    public Slider accuracyBar;
    public BeatMapController bmc;

    private void Start()
    {
        Data.instance.accuracy = 0f;
    }

    public void AddAccuracyPoints()
    {
        Data.instance.accuracy += (100 / bmc.beatmap.Count);
        accuracyBar.value = Data.instance.accuracy / 100f;
    }

    public float GetAccuracyPoints()
    {
        return accuracyBar.value;
    }
}
