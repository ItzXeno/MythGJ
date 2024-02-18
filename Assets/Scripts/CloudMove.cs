using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private Camera cam;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        speed += 1f * Time.deltaTime;
        if (isOffscreen())
        {
            Destroy(gameObject);
        }
    }

    bool isOffscreen()
    {
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        return screenPosition.y < 0;
    }
}
