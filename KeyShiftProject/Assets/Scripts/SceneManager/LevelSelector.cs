using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    public GameObject[] levels = new GameObject[3];

    private int currentSelection = 0;

    public AudioSource enter;
    public AudioSource key;

    public Canvas TextCanvas;
    public Canvas DiffSelection;

    private int currentLevel;

    public AudioSource[] musicList;
    public string[,] namesList;
    public int[] bpmList;

    public GameObject LSbg;
    public GameObject DSbg;

    public Text easyTutorial;

    public AudioSource escapeClip;

    private GameObject musManager;

    private void Start()
    {
        TextCanvas.enabled = true;
        DiffSelection.enabled = false;
        LSbg.SetActive(true);
        DSbg.SetActive(false);
        namesList = new string[3, 3] { {"Tutorial_Easy", "Tutorial_Beat", "Tutorial_Hard" }, { "Lv1_Easy", "Lv1_Beat", "Lv1_Hard" }, { "Lv2_Easy", "Lv2_Beat", "Lv2_Hard" } };
        bpmList = new int[3] {132, 138, 156};
        easyTutorial.text = "easy";
        musManager = GameObject.FindGameObjectWithTag("Music");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            enter.Play();
            DSbg.SetActive(true);
            LSbg.SetActive(true);

            if (DiffSelection.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    GameObject.FindGameObjectWithTag("Music").GetComponent<MusManager>().StopMusic();
                    ChooseScene();
                }
            }
            else
            {
                currentLevel = currentSelection;
            }

            if (currentLevel == 0)
            {
                easyTutorial.text = "Tutorial";
            }

            DiffSelection.enabled = true;
            TextCanvas.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            key.Play();
            currentSelection = (currentSelection + 1 + levels.Length) % levels.Length;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            key.Play();
            currentSelection = (currentSelection - 1 + levels.Length) % levels.Length;
        }
        transform.position = new Vector3(transform.position.x, levels[currentSelection].transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeClip.Play();
            if (TextCanvas.enabled)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else {
                TextCanvas.enabled = true;
                DiffSelection.enabled = false;
                LSbg.SetActive(true);
                DSbg.SetActive(false);
            }
        }
    }

    private void ChooseScene()
    {
        Data.instance.music = musicList[currentLevel].clip;
        Data.instance.MapFileName = namesList[currentLevel, currentSelection];
        Data.instance.bpm = bpmList[currentLevel];
        musManager.GetComponent<MusManager>().StopMusic();
        SceneManager.LoadScene("GamePlay");
    }
}
