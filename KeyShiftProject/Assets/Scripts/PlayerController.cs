using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject mapPlacer;
    public int curIndex = 0;
    public Vector3 targetPos;
    public float speed = 1f;
    private float curSpeed;
    public bool active = true;
    public GameObject bmc;
    public AudioClip hitSound;
    public AudioClip missSound;

    private int numberOfBeats = 0;
    private int deletionTail = 20;
    private int beatsHit = 0;

    public Slider accuracyBar;
    //private Animator anim;
    private AudioSource soundEffects;

    public Text pointScore; 
    private int points = 0;
    public Text pointAdd;
    private float pointAddAlpha = 0f;

    public GameObject reactionImagePrefab;
    public Vector3 reactionPosition;
    //public Sprite hitImage;
    //public Sprite missImage;
    //public Sprite perfectImage;

    //public GameObject background;

    //animation handling
    public SpriteRenderer render;
    public Sprite[] playerIdle;
    public Sprite[] playerJump;
    public Sprite[] playerDance;
    public Sprite[] playerDash;
    private float secondsPerJumpFrame;
    private float secondsPerDashFrame;
    private float size;

    public AudioSource escapeClip;

    public enum State
    {
        Idle,
        Jump,
        Dance,
        Pull
    }
    public State anim;

    private float bpm;
    private float animTime;
    private int idleIndex = 0;
    private int jumpIndex = 0;
    private int dashIndex = 0;
    private int danceIndex = 0;
    private float facingDir = 1f;
    private float curNoteDuration;


    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        numberOfBeats = mapPlacer.GetComponent<BeatMapController>().beatmap.Count;
        //anim = transform.GetChild(1).GetComponent<Animator>();
        soundEffects = transform.GetChild(2).GetComponents<AudioSource>()[1];
        bpm = bmc.GetComponent<BeatMapController>().bpm;
        //anim.SetFloat("bpm", bpm/60f);
        //background.SetActive(false);
        anim = State.Idle;
        curSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeClip.Play();
            SceneManager.LoadScene("LevelSelection");
        }

        Data.instance.score = points;
        if (curIndex < numberOfBeats)
        {
            GameObject nextBeatBox = mapPlacer.GetComponent<BeatMapController>().GetBeatBox(curIndex);
            string nextLetter = nextBeatBox.GetComponent<BeatBoxController>().GetIdentity();
            bool isCorrectPhase = nextBeatBox.GetComponent<BeatBoxController>().IsCorrectTime();
            bool isPassed = nextBeatBox.GetComponent<BeatBoxController>().IsPassed();
            if (Input.GetKeyDown(nextLetter) && isCorrectPhase)
            {
                anim = State.Jump;
                soundEffects.PlayOneShot(hitSound, 0.2f);
                targetPos = nextBeatBox.transform.position;
                nextBeatBox.GetComponent<BeatBoxController>().MarkHit();
                curIndex++;
                beatsHit++;

                //get speed at which player must travel to arrive at next tile on time

                /* duration dependent solution
                curNoteDuration = nextBeatBox.GetComponent<BeatBoxController>().GetDuration();
                float secondsToTravel = (30f/bpm) * curNoteDuration;
                secondsPerJumpFrame = secondsToTravel / playerJump.Length;
                secondsPerDashFrame = secondsToTravel / playerDash.Length;
                float distanceToTravel = Vector3.Distance(transform.position, targetPos);
                curSpeed = distanceToTravel/secondsToTravel;
                */
                float secondsToTravel = 30f/bpm;
                secondsPerJumpFrame = secondsToTravel / playerJump.Length;
                secondsPerDashFrame = secondsToTravel / playerDash.Length;
                curNoteDuration = nextBeatBox.GetComponent<BeatBoxController>().GetDuration();
                curSpeed = GetResultingSpeed(curNoteDuration);

                
                int pointsToAdd = nextBeatBox.GetComponent<BeatBoxController>().GetPointValue();
                points += pointsToAdd;
                pointScore.text = points.ToString();
                pointAdd.text = "+" + pointsToAdd.ToString();
                pointAddAlpha = 1f;
                
                if (pointsToAdd == 100)
                    CreateReaction(2);
                else
                    CreateReaction(1);
                
                accuracyBar.GetComponent<Slider>().value = (float) points / (numberOfBeats * 100);
                Data.instance.accuracy = Mathf.Round(accuracyBar.GetComponent<Slider>().value * 100f);
                
                //Debug.Log(nextBeatBox.GetComponent<BeatBoxController>().GetDirection().x);
                if (nextBeatBox.GetComponent<BeatBoxController>().GetDirection().x == -1f)
                {
                    //Debug.Log("left detected");
                    facingDir = -1f;
                }
                else if (nextBeatBox.GetComponent<BeatBoxController>().GetDirection().x == 1f)
                {
                    facingDir = 1f;
                }
            }
            else if (Input.anyKeyDown && !Input.GetKeyDown(nextLetter) && isCorrectPhase)
            {
                CreateReaction(3);
            }
            else if (isPassed)
            //else if (isPassed || (Input.anyKeyDown && !Input.GetKeyDown(nextLetter) && isCorrectPhase))
            {
                targetPos = nextBeatBox.transform.position;
                //Destroy(nextBeatBox);
                nextBeatBox.GetComponent<BeatBoxController>().MarkMissed();
                curIndex++;
                CreateReaction(0);
                //StartCoroutine(displayText(missText));
                curSpeed = speed;
                anim = State.Pull;
                soundEffects.PlayOneShot(missSound, 0.2f);
            }
        }
        else
        {
            anim = State.Dance;
        }
        if (pointAddAlpha > 0f)
        {
            pointAddAlpha -= Mathf.Min(0.5f * Time.deltaTime, pointAddAlpha);
            pointAdd.color = new Color(0f, 0f, 0f, pointAddAlpha);
        }

        if (curIndex >= deletionTail)
        {
            Destroy(mapPlacer.GetComponent<BeatMapController>().GetBeatBox(curIndex-deletionTail));
        }

        //transform.position = Vector3.MoveTowards(transform.position, targetPos, curSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, curSpeed * Time.deltaTime);
        

        //animation
        RenderAnim();
    }

    IEnumerator displayText(Text text)
    {
        //background.SetActive(true);
        text.enabled = true;
        yield return new WaitForSeconds(0.3f);
        text.enabled = false;
        //background.SetActive(false);
    }

    public void CreateReaction(int index)
    {
        GameObject createdReaction = Instantiate(reactionImagePrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform.GetChild(0));
        createdReaction.GetComponent<ReactionTextController>().SetImage(index);
    }

    public float GetResultingSpeed(float dur)
    {
        if (dur == 0.25)
        {
            return 10f;
        }
        return 6f;
    }
    
    public void RenderAnim()
    {
        //float size = 1f;
        animTime += Time.deltaTime;
        if (anim == State.Idle)
        {
            if (animTime >= 30f/bpm)
            {
                size = 1f;
                render.sprite = playerIdle[idleIndex];
                idleIndex = (idleIndex + 1) % playerIdle.Length;
                animTime = 0f;
            }
            //Debug.Log("idel anim");
        }
        else if (anim == State.Jump && curIndex < numberOfBeats)
        {
            //size = 1f;
            //Debug.Log("jump anim");
            if (curNoteDuration > 0.25f)
            {
                //size = 0.55f;
                if (jumpIndex < playerJump.Length)
                {
                    if (animTime >= secondsPerJumpFrame)
                    {
                        size = 0.55f;
                        render.sprite = playerJump[jumpIndex];
                        float x = (float) jumpIndex/playerJump.Length;
                        Vector3 placeholder = transform.GetChild(0).position;
                        transform.GetChild(1).position = new Vector3(placeholder.x, placeholder.y + 0.3298286f + 0.4f * x * (1f - x), 0f);
                        jumpIndex++;
                        animTime = 0f;
                    }
                }
                else
                {
                    size = 1f;
                    render.sprite = playerIdle[0];
                    Vector3 placeholder = transform.GetChild(0).position;
                    transform.GetChild(1).position = new Vector3(placeholder.x, placeholder.y + 0.3298286f, 0f);
                    jumpIndex = 0;
                    animTime = 0f;
                    anim = State.Idle;
                }
            }
            else
            {
                //size = 1f;
                if (dashIndex < playerDash.Length)
                {
                    //if (animTime >= secondsPerDashFrame)
                    if (animTime >= 0.1f)
                    {
                        if (transform.position != targetPos && dashIndex == 5)
                        {
                            dashIndex = 4;
                        }
                        else
                        {
                            dashIndex++;
                            animTime = 0f;
                        }
                        size = 1f;
                        render.sprite = playerDash[dashIndex];
                    }
                }
                else
                {
                    size = 1f;
                    render.sprite = playerIdle[0];
                    dashIndex = 0;
                    animTime = 0f;
                    anim = State.Idle;
                }
            }
        }
        else if (anim == State.Dance)
        {
            //size = 1f;
            if (animTime >= 15f/bpm)
            {
                size = 1f;
                render.sprite = playerDance[danceIndex];
                danceIndex = (danceIndex + 1) % playerDance.Length;
                animTime = 0f;
            }
        }
        else if (anim == State.Pull)
        {
            if (dashIndex < playerDash.Length)
            {
                if (animTime >= 0.1f)
                {
                    size = 1f;
                    render.sprite = playerDash[dashIndex];
                    if (transform.position != targetPos && dashIndex == 6)
                    {
                        dashIndex = 4;
                    }
                    else
                    {
                        dashIndex++;
                        animTime = 0f;
                    }
                }
            }
            else
            {
                size = 1f;
                render.sprite = playerIdle[0];
                dashIndex = 0;
                animTime = 0f;
                anim = State.Idle;
            }
        }

        transform.GetChild(1).localScale = new Vector3(size * facingDir, size, size);
    }
}
