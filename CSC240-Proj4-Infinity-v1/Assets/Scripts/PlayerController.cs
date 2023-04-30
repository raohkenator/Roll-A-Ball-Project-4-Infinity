using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour

{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI countText;

    // conditional texts
    public GameObject deathTextObject;
    public GameObject updateTextObject;
    public GameObject winTextObject;

    private TextMeshProUGUI deathText;
    private TextMeshProUGUI updateText;

    // level complete bool
    private bool level1Complete; // holds value representing completion of level 1 objectives
    private bool level2Complete; // holds value representing completion of level 2 objectives

    public float speed = 10; // controls the movement speed of player
    public int level = 0;

    private GameObject Door1;
    private GameObject Door2;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        deathText = deathTextObject.GetComponent<TextMeshProUGUI>();
        updateText = updateTextObject.GetComponent<TextMeshProUGUI>();

        Door1 = GameObject.Find("Door1");
        Door2 = GameObject.Find("Door2");

        // default values
        count = 0;
        level = 1;

        // displays appropriate texts
        SetLeveltext();
        SetCountText();
        winTextObject.SetActive(false);
        deathTextObject.SetActive(false);
        updateTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        if(count >= 11 && level == 1 && !level1Complete)
        {
            level1Complete = false;
            updateText.text = "You hear a door open.";
            updateTextObject.SetActive(true);
            Door1.GetComponent<DoorOpenClose>().Open = true;
        }
        if(count >= 8 && level == 2)
        {

        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if(other.gameObject.CompareTag("Level2Trigger"))
        {
            count = 0;
            level = 2;
            updateTextObject.SetActive(false);
            SetCountText();
            SetLeveltext();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 2)
        {
            winTextObject.SetActive(true);
        }
    }

    void SetLeveltext()
    {
        levelText.text = "Level " + level.ToString();
    }
}
