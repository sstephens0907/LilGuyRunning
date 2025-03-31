using UnityEngine;

public class script : MonoBehaviour
{
    private float length, startPosition;
    public GameObject camera;
    public float parallaxEffect;
    void Start()
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
   
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float distance = (camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z); 
        
        if (temp > startPosition + length) startPosition += length * 2;
        else if (temp < startPosition - length) startPosition -= length * 2;
    }
}

