using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButtonPress : MonoBehaviour
{
    public GameObject ButtonBase;
    public GameObject Laser1;
    public GameObject laser2;

    // Start is called before the first frame update
    void Start()
    {
        ButtonBase.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
