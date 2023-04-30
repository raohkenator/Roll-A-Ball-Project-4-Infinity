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
    public float CountDown = 5;

    // external objects
    private GameObject Door1;
    private GameObject Door2;
    private GameObject FinalStar;
    private GameObject FinalTrigger;

    private Rigidbody rb;
    public int count;
    private float movementX;
    private float movementY;
    private IEnumerator GameWinCo;

    void Start()
    {
        // obtain external objects
        rb = GetComponent<Rigidbody>();
        deathText = deathTextObject.GetComponent<TextMeshProUGUI>();
        updateText = updateTextObject.GetComponent<TextMeshProUGUI>();

        Door1 = GameObject.Find("Door1");
        Door2 = GameObject.Find("Door2");
        FinalStar = GameObject.Find("Final Star");
        FinalTrigger = GameObject.FindWithTag("WinTrigger");

        // hides appropriate objects
        FinalStar.SetActive(false);

        // default values
        count = 0;
        level = 1;
        GameWinCo = GameWin(CountDown);

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
            level1Complete = true;
            updateText.text = "You hear a door open.";
            updateTextObject.SetActive(true);
            Door1.GetComponent<DoorOpenClose>().Open = true;
            Invoke("HideUpdate", 7.0f);
        }
        if (count >= 6 && level == 2 && !Door2.GetComponent<DoorOpenClose>().Open)
        {
            updateText.text = "You hear a door open.";
            updateTextObject.SetActive(true);
            Door2.GetComponent<DoorOpenClose>().Open = true;
            Invoke("HideUpdate", 7.0f);
        }
        if(count >= 8 && level == 2 && !level2Complete)
        {
            level2Complete = true;
            updateText.text = "You think you have collected enough stars! Take them to the refueling bucket.";
            updateTextObject.SetActive(true);
            Invoke("HideUpdate", 4.0f);
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
        if(other.gameObject.CompareTag("WinTrigger") && level2Complete)
        {
            if(!FinalStar.activeSelf)
            {
                FinalTrigger.SetActive(false);
                FinalStar.SetActive(true);
                StartCoroutine(GameWinCo);
                Debug.Log("Works");
            }
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

    void HideUpdate()
    {
        updateTextObject.SetActive(false);
    }

    void Win()
    {
        Debug.Log("Win Condition Met");
        transform.position = new Vector3(0, 50, -120);
    }
    public IEnumerator GameWin(float CountDown)
    {
        for(float Timer = CountDown; Timer > 0; Timer--)
        {
            updateText.text = "Spaceship Will Resume Flight In: " + CountDown.ToString();
            updateTextObject.SetActive(true);
            CountDown -= 1.0f;
            Debug.Log($"{CountDown}");
            yield return new WaitForSeconds(1.0f);
        }
        
        updateTextObject.SetActive(false);
        Win();
    }
}
