using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    [SerializeField]
    public GameObject[] selections = new GameObject[3];

    private int currentSelection = 0;

    public AudioSource enter;
    public AudioSource key;

    public GameObject musmanager;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusManager>().PlayMusic();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            enter.Play();
            ChooseScene();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            key.Play();
            currentSelection = (currentSelection + 1 + selections.Length) % selections.Length;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            key.Play();
            currentSelection = (currentSelection - 1 + selections.Length) % selections.Length;
        }
        transform.position = new Vector3(transform.position.x, selections[currentSelection].transform.position.y, transform.position.z);
    }

    private void ChooseScene()
    {
        switch (currentSelection)
        {
            case 0:
                SceneManager.LoadScene("LevelSelection"); //level selection scene scene
                break;
            case 1:
                SceneManager.LoadScene("SettingsMenu"); //settings canvas
                break;
            case 2:
                Application.Quit(); //quit game
                break;
            default:
                return;
        }
    }
}
