using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTextController : MonoBehaviour
{
    private float quadratic = 0f;
    private float time = 0f;

    public float offsetY;
    public float offsetX;
    public float factorY;
    public Sprite[] spriteList;
    //0 miss, 1 good, 2 perfect, 3 oops
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            Destroy(gameObject);
        }
        quadratic = 4 * time * (1 - time);
        transform.position = new Vector3(gameObject.transform.parent.position.x + offsetX, gameObject.transform.parent.position.y + offsetY + (quadratic * factorY), transform.position.z);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, quadratic);
    }

    public void SetImage(int index)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[index];
        if (index == 3)
        {
            gameObject.transform.localScale = new Vector3(0.28f, 0.28f, 0.28f);
        }
    }
}
