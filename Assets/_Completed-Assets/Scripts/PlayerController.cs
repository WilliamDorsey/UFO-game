using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Text score;
    public Text lives;
    public Text winLoseText;

    private int count;
    private int totalCount;
    private int lifeCount;

    private Rigidbody2D rb2d;
    private GameObject[] enemies;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        count = 0;                            //Displayed score
        totalCount = 0;                       //Pickup count for determining when next level starts
        lifeCount = 3;                        //Lives
        winLoseText.text = "";
        SetScore ();
    }

    //Quit Application
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    //Player Movement
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);
    }

    //When player collides with an object
    void OnTriggerEnter2D(Collider2D other)
    {
        //Pick up pickup
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            totalCount++;
        }

        //Player hits a bad pickup
        if (other.gameObject.CompareTag("BadPickup"))
        {
            other.gameObject.SetActive(false);
            lifeCount--;
            count--;
        }

        SetScore();

    }

    //Display current score
    private void SetScore()
    {
        score.text = ($"Score: {count}");
        lives.text = ($"Lives: {lifeCount}");

        //Teleports player after all pickups on level 1
        if (totalCount == 12)
        {
            transform.position = new Vector3(0, -60, 0);
            rb2d.velocity = Vector3.zero;
            totalCount++;
        }

        //Win state
        if (totalCount >= 22)
        {
            winLoseText.text = "The laser is charged up, William. We win!";

            //Destroy all bad pickups
            enemies = GameObject.FindGameObjectsWithTag("BadPickup");
            for (var i = 0; i < enemies.Length; i++)
                Destroy(enemies[i]);
        }

        //Death state
        if (lifeCount <= 0)
        {
            winLoseText.fontStyle = FontStyle.Bold;
            winLoseText.color = Color.red;
            winLoseText.text = "You Lose";
            rb2d.velocity = Vector3.zero;
            rb2d.isKinematic = true;
        }
    }
}
