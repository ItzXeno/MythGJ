using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDebug : MonoBehaviour
{
    // Start is called before the first frame update
    private int health = 10;
    public GameObject debugGameObject;
    PassengerSpawner spawner;
    void Start()
    {
        spawner = debugGameObject.GetComponent<PassengerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            spawner.MakePassengerFallOff();
            health--;
        }
    }
}
