using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

public class BeatMapController : MonoBehaviour
{

    public List<GameObject> beatmap;
    //Manually linked
    public GameObject boxPrefab;
    public float time = 0f;
    public float bpm = 120f;
    public string rawMapName;
    public float tileDistance = 1f;
    public float delayByBeats = 0f;
    public float notePreempt = 8f;
    public GameObject audioSource;

    private IEnumerator startTrack;
    private string[] rawmap;
    
    private float curSumBeats = 0f;
    private float secondsPerBeat;
    private float dspStartTime;
    private float songPosition;
    private float songPositionInBeats;
    private int curIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        bpm = Data.instance.bpm;   
        rawMapName = Data.instance.MapFileName;

        //curSumBeats += delayByBeats - notePreempt - 0.1f;
        curSumBeats = -notePreempt;

        beatmap = new List<GameObject>();
        TextAsset rawMapAsset = Resources.Load(rawMapName) as TextAsset;
        string rawMapString = rawMapAsset.text;
        rawmap = Regex.Split(rawMapString, Environment.NewLine);

        //rawmap = System.IO.File.ReadAllLines("Resources/" + rawmap + ".txt");
        foreach (string line in rawmap)
        {
            string[] parameter = line.Split(' ');

            if (parameter[0].Length > 1 && parameter[0].Substring(0, 2) == "//")
            {

            }
            else if (parameter[0].ToLower() == "wait")
            {
                curSumBeats += float.Parse(parameter[1]);
            }
            else
            {
                string paraIdentity = parameter[0].ToLower();
                float paraDuration = float.Parse(parameter[1]);
                Vector3 paraDirection = Vector3.zero;
                if (parameter[2] == "u")
                    paraDirection = Vector3.up * 1.1f;
                if (parameter[2] == "d")
                    paraDirection = Vector3.down * 1.1f;
                if (parameter[2] == "r")
                    paraDirection = Vector3.right;
                if (parameter[2] == "l")
                    paraDirection = Vector3.left;
                
                beatmap.Add(MakeBeatBox(paraIdentity, paraDuration, paraDirection));
                curSumBeats += paraDuration;
            }
        }

        secondsPerBeat = 60.0f / bpm;
        dspStartTime = (float) AudioSettings.dspTime;
        //audioSource.GetComponent<AudioController>().Play();

        /* Old implementation
        startTrack = StartTrack();
        StartCoroutine(startTrack);
        */

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Level Selection");
        }

        //Debug.Log(audioSource.GetComponent<AudioSource>().pitch);
        songPosition = (float) AudioSettings.dspTime - dspStartTime;
        songPositionInBeats = songPosition / secondsPerBeat;
        
        if (curIndex < beatmap.Count)
        {
            float notePosition = beatmap[curIndex].GetComponent<BeatBoxController>().GetBeatPosition();

            if (songPositionInBeats >= notePosition)
            {
                beatmap[curIndex].SetActive(true);
                curIndex++;
            }
        }

        if (songPosition > audioSource.GetComponent<AudioSource>().clip.length)
        {
            //Debug.Log("stop playing");
            SceneManager.LoadScene("GameOver");
        }

    }

    public GameObject MakeBeatBox(string identity, float duration, Vector3 direction)
    {
        transform.position += direction * tileDistance;
        GameObject createdBox = Instantiate(boxPrefab, transform.position, Quaternion.identity);
        createdBox.GetComponent<BeatBoxController>().SetIdentity(identity);
        createdBox.GetComponent<BeatBoxController>().SetDuration(duration);
        //direction memeber may not be needed
        createdBox.GetComponent<BeatBoxController>().SetDirection(direction);
        createdBox.GetComponent<BeatBoxController>().SetBeatPosition(curSumBeats);
        createdBox.GetComponent<BeatBoxController>().SetBPM(bpm);
        createdBox.GetComponent<BeatBoxController>().beatLeadUp = notePreempt;
        createdBox.SetActive(false);
        //script for more advanced tile coloring, see function in beatbox
        //createdBox.GetComponent<BeatBoxController>().SetTileType(totalBeats);
        return createdBox;
    }

    public GameObject GetBeatBox(int index)
    {
        return beatmap[index];
    }

    private IEnumerator StartTrack()
    {
        //reminder that the subtraction from the delay here accounts for the time it takes a beat box controller to activate after being spawned.
        audioSource.GetComponent<AudioController>().Play();
        yield return new WaitForSeconds(((60f/bpm) * (delayByBeats - 1.5f)) - 6f);
        int index = 0;
        while (index < beatmap.Count)
        {
            beatmap[index].SetActive(true);
            float beatNumber = beatmap[index].GetComponent<BeatBoxController>().GetDuration();
            yield return new WaitForSeconds((60f/bpm)*beatNumber);
            index++;
        }
    }
}
