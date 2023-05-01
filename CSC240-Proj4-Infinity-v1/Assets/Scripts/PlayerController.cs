using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    private IEnumerator DeathCo;
    private bool Fallen = false;

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
        DeathCo = Respawn(CountDown);

        // displays appropriate texts
        SetLeveltext();
        SetCountText();
        winTextObject.SetActive(false);
        deathTextObject.SetActive(false);
        updateTextObject.SetActive(false);
    }

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (count >= 7 && level == 2 && !Door2.GetComponent<DoorOpenClose>().Open)
        {
            updateText.text = "You hear a door open.";
            updateTextObject.SetActive(true);
            Door2.GetComponent<DoorOpenClose>().Open = true;
            Invoke("HideUpdate", 7.0f);
        }
        if(count >= 9 && level == 2 && !level2Complete)
        {
            level2Complete = true;
            updateText.text = "You think you have collected enough stars! Take them to the refueling bucket.";
            updateTextObject.SetActive(true);
            Invoke("HideUpdate", 4.0f);
        }
        if(gameObject.transform.position.y <= -50 && !Fallen)
        {
            Fallen = true;
            Death(3)
;        }
        
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
        if(other.gameObject.CompareTag("Laser"))
        {
            Death(1);
        }
        if(other.gameObject.CompareTag("Mine"))
        {
            Death(2);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
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
        rb.velocity = new Vector3(0, 0, 0);
        rb.freezeRotation = true;
        transform.position = new Vector3(0, 51, -120);
        winTextObject.SetActive(true);
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

    void Death(int Cause = 0) // 0 = Default/Invalid Death, 1 = Laser, 2 = Mine, 3 = Void.
    {
        if(Cause == 1)
        {
            rb.freezeRotation = true;
            speed = 0;
            deathText.text = "You Got Hit By A Laser! \n [You Lost]";
            deathTextObject.SetActive(true);
            StartCoroutine(DeathCo);
        }
        if(Cause == 2)
        {
            rb.freezeRotation = true;
            speed = 0;
            deathText.text = "You Set Off A Mine! \n [You Lost]";
            deathTextObject.SetActive(true);
            StartCoroutine(DeathCo);
        }
        if(Cause == 3)
        {
            rb.freezeRotation = true;
            speed = 0;
            deathText.text = "You Fell Into Space! \n [You Lost]";
            deathTextObject.SetActive(true);
            StartCoroutine(DeathCo);
        }
    }

    public IEnumerator Respawn(float CountDown)
    {
        for(float Timer = CountDown; Timer > 0; Timer--)
        {
            updateText.text = "You Will Respawn In: " + CountDown.ToString();
            updateTextObject.SetActive(true);
            CountDown -= 1.0f;
            Debug.Log($"{CountDown}");
            yield return new WaitForSeconds(1.0f);
        }
        
        updateTextObject.SetActive(false);
        deathTextObject.SetActive(false);
        Reset();
    }
}