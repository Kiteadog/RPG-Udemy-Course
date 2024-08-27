using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect_X;
    [SerializeField] private float parallaxEffect_Y;

    private float position_X;
    private float length_X;
    private float position_Y;
    private float length_Y;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length_X = GetComponent<SpriteRenderer>().bounds.size.x;
        position_X = transform.position.x;
        length_Y = GetComponent<SpriteRenderer>().bounds.size.y;
        position_Y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved_X = cam.transform.position.x * (1 - parallaxEffect_X);
        float distanceToMove_X = cam.transform.position.x * parallaxEffect_X;
        float distanceMoved_Y = cam.transform.position.y * (1 - parallaxEffect_Y);
        float distanceToMove_Y = cam.transform.position.y * parallaxEffect_Y;

        transform.position = new Vector3(position_X + distanceToMove_X, position_Y + distanceToMove_Y);

        if (distanceMoved_X > position_X + length_X)
            position_X += length_X;
        else if (distanceMoved_X < position_X - length_X)
            position_X -= length_X;

        if (distanceMoved_Y > position_Y + length_Y)
            position_Y += length_Y;
        else if (distanceMoved_Y < position_Y - length_Y)
            position_Y -= length_Y;
    }
}
