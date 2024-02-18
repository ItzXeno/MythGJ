using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public GameObject[] clouds;
    private Camera cam;
    private float camWidth, camHeight, cornerX, cornerY;

    private void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        StartCoroutine(spawnClouds());
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator spawnClouds()
    {
        cornerX = Random.Range(-camWidth/2, camWidth/2);
        cornerY = cam.orthographicSize + 1f;
        Instantiate(clouds[Random.Range(0,clouds.Length)], new Vector3(cornerX, cornerY + Random.Range(0f, 5f), transform.position.z), transform.rotation);
       yield return new WaitForSeconds(Mathf.Clamp(Random.Range(4f, 8f) -(GameObject.Find("Player").GetComponent<PlayerController>().timeAlive * .1f), .2f, 8f));
        StartCoroutine(spawnClouds());
    }
}
