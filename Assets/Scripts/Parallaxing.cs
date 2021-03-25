using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds; // List of all backgrounds to be parallaxed
    private float[] parallaxScales; // The proportion of camera's movement to move the backgrounds by
    public float smoothing = 1f; // How smooth the parallaxing is going to be

    private Transform cam; // reference to the main cameras transform
    private Vector3 previousCamPos; // the position of the camera in the previous frame

    // Called before start. Great for references.
    private void Awake()
    {
        cam = Camera.main.transform;

    }

    // Use this for initializations
    void Start()
    {
        // The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current x position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target oısition which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;

    }
}
