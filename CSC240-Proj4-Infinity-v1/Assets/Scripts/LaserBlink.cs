using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlink : MonoBehaviour
{
    public float BlinkDelay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Unhide", 1f);
    }

    void Unhide()
    {
        gameObject.SetActive(true);

        Invoke("Hide", BlinkDelay);
    }

    void Hide()
    {
        gameObject.SetActive(false);
        Invoke("Unhide", BlinkDelay);
    }
}
