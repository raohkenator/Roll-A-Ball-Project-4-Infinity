using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButtonPress : MonoBehaviour
{
    public GameObject ButtonPushed;
    public GameObject Laser1;
    public GameObject Laser2;

    // Start is called before the first frame update
    void Start()
    {
        ButtonPushed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        Laser1.SetActive(false);
        Laser2.SetActive(false);
        gameObject.SetActive(false);
        ButtonPushed.SetActive(true);
    }
}
