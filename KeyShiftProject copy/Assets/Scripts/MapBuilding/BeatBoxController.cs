using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BeatBoxController : MonoBehaviour
{
    public string identity = "";
    public float duration = 0f;
    public Vector3 direction = Vector3.zero;
    public TextMeshProUGUI label;
    public GameObject indicator;
    public float scaleSpeed = 0.5f;
    public bool isCorrectTime = false;
    public float correctDuration = 1f;
    public bool passed = false;
    public SpriteRenderer tileSprite;
    public float beatPosition = 0f;

    private float bpm;
    public float beatLeadUp = 0f;
    private float secondsLeadUp;
    public float beatStayUp = 2f;

    public Sprite redTile;
    public Sprite orangeTile;
    public Sprite yellowTile;
    public Sprite blueTile;
    public Sprite missTile;
    // red = quarter,  yellow = eight, blue = 16th, orange = misc
    public GameObject hitEffectObject;
    public SpriteRenderer hitEffect;
    public Sprite hitEffect1;
    public Sprite hitEffect2;


    private IEnumerator correctPhase;
    private float brightness = 0f;
    private float indicatorSize = 2f;
    private float indicatorColor = 0f;
    private float sizeDifference;

    private float localTime = 0f;
    private float percentageLeadUp = 0f;
    private int pointValue = 100;
    private float startTime = 0f;
    private bool leadUpDone = false;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        correctPhase = CorrectPhase();
        tileSprite.color = new Color (0f, 0f, 0f, 0f);  
        secondsLeadUp = (60f / bpm) * beatLeadUp; 
        sizeDifference = indicatorSize - 0.6f;
        correctDuration = (60f/bpm) * beatStayUp;
    }

    // Update is called once per frame
    void Update()
    {
        localTime += Time.deltaTime;

        if (!passed)
        {
            if (!leadUpDone)
            {
                percentageLeadUp = localTime / secondsLeadUp;
                //brightness = 0.5f * percentageLeadUp + 0.3f;
                brightness = (percentageLeadUp * (-percentageLeadUp + 2)) * 0.8f;
                tileSprite.color = new Color(1f, 1f, 1f, brightness);
                label.color = new Color(0f, 0f, 0f, brightness);

                if (percentageLeadUp >= 0.8f)
                {
                    float fractionalPercentage = (percentageLeadUp - 0.8f)/(0.2f);
                    float curIndicatorSize = indicatorSize - (sizeDifference * fractionalPercentage);
                    indicator.transform.localScale = new Vector3(curIndicatorSize, curIndicatorSize, 0f);

                    indicatorColor = 0.5f + fractionalPercentage * 0.5f;
                    indicator.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, indicatorColor);

                    if (percentageLeadUp >= 0.9f)
                    {
                        isCorrectTime = true;
                        pointValue = Math.Min((int) Mathf.Ceil(fractionalPercentage * 100f), 100);

                    }
                }
                if (percentageLeadUp >= 1f)
                {
                    indicator.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                    startTime = localTime;
                    StartCoroutine(correctPhase);
                }
            }
            else
            {
                //percentageLeadUp = (localTime - startTime) / correctDuration;
                //pointValue = Math.Min(((int) Mathf.Ceil((1 - percentageLeadUp)*100)) + 10, 100);
            }
        }
    }

    public void SetIdentity(string setIdentity)
    {
        identity = setIdentity;
        label.SetText(setIdentity.ToUpper());
    }

    public string GetIdentity()
    {
        return identity;
    }

    public void SetDuration(float setDuration)
    {
        duration = setDuration;
        if (duration == 1f)
        {
            tileSprite.sprite = redTile;
            hitEffect.color = new Color(3f/255f, 56f/255f, 243f/255f);
        }
        else if (duration == 0.5f)
        {
            tileSprite.sprite = yellowTile;
            hitEffect.color = new Color(245f/255f, 5f/255f, 144f/255f);
        }
        else if (duration == 0.25f)
        {
            tileSprite.sprite = blueTile;
            hitEffect.color = new Color(246f/255f, 255f/255f, 38f/255f);
        }
        else
        {
            tileSprite.sprite = orangeTile;
            hitEffect.color = new Color(55f/255f, 205f/255f, 244f/255f);
        }
    }

    //works as intended but doesn't look good, may need more advanced logic?
    public void SetTileType(float total)
    {
        float type = (float) (total - Math.Truncate(total));
        if (type == 0f)
        {
            tileSprite.sprite = redTile;
        }
        else if (type == 0.5f)
        {
            tileSprite.sprite = yellowTile;
        }
        else if (type == 0.25f || type == 0.75f)
        {
            tileSprite.sprite = blueTile;
        }
        else
        {
            tileSprite.sprite = orangeTile;
            hitEffect.color = new Color(55f/255f, 205f/255f, 244f/255f);
        }
    }

    public float GetDuration()
    {
        return duration;
    }

    public void SetDirection(Vector3 setDir)
    {
        direction = setDir;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    private IEnumerator CorrectPhase()
    {
        //Debug.Log("Phase active");
        tileSprite.color = Color.white;
        leadUpDone = true;
        //sprite.color = new Color(91f, 215f, 238f, 1f);
        //sprite.color = new Color(91f/255, 215f/255, 238f/255, 1f);
        yield return new WaitForSeconds(correctDuration);
        isCorrectTime = false;
        leadUpDone = false;
        passed = true;
        if (isHit)
            tileSprite.color = Color.gray;
        //tileSprite.color = Color.gray;
        //Debug.Log("Phase done");
    }

    private IEnumerator ExplodeAnim()
    {
        hitEffect.sprite = hitEffect1;
        yield return new WaitForSeconds(30f/bpm);
        hitEffect.sprite = hitEffect2;
        //hitEffectObject.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
        yield return new WaitForSeconds(30f/bpm);
        hitEffect.enabled = false;
    }

    public bool IsCorrectTime()
    {
        return isCorrectTime;
    }

    public bool IsPassed()
    {
        return passed;
    }

    public void MarkHit()
    {
        isHit = true;
        StartCoroutine("ExplodeAnim");
        label.SetText("");
    }

    public void MarkMissed()
    {
        tileSprite.sprite = missTile;
        label.SetText("");
    }

    public void SetBeatPosition(float setPosition)
    {
        beatPosition = setPosition;
    }

    public float GetBeatPosition()
    {
        return beatPosition;
    }

    public void SetBPM(float setBPM)
    {
        bpm = setBPM;
    }

    public int GetPointValue()
    {
        return pointValue;
    }
}