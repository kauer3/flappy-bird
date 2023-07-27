using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D bird;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    float lastPosition;
    float currentPosition;
    bool wingUp;
    GameObject wing;
    private Animation falling;
    private Animation flyLeft;
    private Animation flyRight;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        wing = GameObject.FindGameObjectWithTag("Wing");
        
        falling = wing.gameObject.GetComponent<Animation>();
        flyLeft = wing.transform.GetChild(0).gameObject.GetComponent<Animation>();
        flyRight = wing.transform.GetChild(1).gameObject.GetComponent<Animation>();

        lastPosition = bird.transform.position.y;
        wingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bird.transform.position.y > 18 || bird.transform.position.y < -17)
        {
            logic.gameOver();
            birdIsAlive = false;
        } else if (Input.GetKeyDown (KeyCode.Space) && birdIsAlive)
        {
            bird.velocity = Vector2.up * flapStrength;
            flyLeft.Play("FlyLeft");
            flyRight.Play("FlyRight");
        }

        currentPosition = bird.transform.position.y;
        if (currentPosition > lastPosition && !wingUp)
        {
            wingUp = true;
            wing.transform.localScale = new Vector3(1, 1, 1);
            falling.Stop("Falling");
        } else if (currentPosition < lastPosition && wingUp)
        {
            wingUp = false;
            wing.transform.localScale = new Vector3(1, -1, 1);
            falling.Play("Falling");
        }
        lastPosition = currentPosition;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }
}
