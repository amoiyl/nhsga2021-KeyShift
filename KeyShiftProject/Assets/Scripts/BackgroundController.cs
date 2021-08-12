using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject layer1;
    public GameObject layer2;
    public float layer1Speed = 0f;
    public float layer2Speed = 0f;

    public Color tutorialColor;
    public Color level1Color;
    public Color level2Color;

    // Start is called before the first frame update
    void Start()
    {
        string songName = Data.instance.MapFileName;
        songName = songName.Substring(0,3);
        SpriteRenderer render = layer2.GetComponent<SpriteRenderer>();

        if (songName == "Tut")
        {
            render.color = tutorialColor;
        }
        else if (songName == "Lv1")
        {
            render.color = level1Color;
        }
        else if (songName == "Lv2")
        {
            render.color = level2Color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        layer1.transform.eulerAngles -= new Vector3(0f, 0f, layer1Speed * Time.deltaTime);
        layer2.transform.eulerAngles -= new Vector3(0f, 0f, layer2Speed * Time.deltaTime);
    }
}
