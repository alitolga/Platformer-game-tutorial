using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    
    public int offsetX = 2;

    // these are used for checking if we need to instantiate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    
    public bool reverseScale = false; // used it the object is not tilable

    public float spriteWidth; // the width of our element
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        // does it still need buddies? if not do nothing
        if (!hasALeftBuddy || !hasARightBuddy)
        {
            // Camera width'in yarısı
            // calculate the cameras horizontal extend (half the width) of what the camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;
            
            // checking if we can se the edge of element and calling MakeNewBuddy if we can
            if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasALeftBuddy) 
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }

        }

        // A function that creates a buddy on the side required
        void MakeNewBuddy (int rightOrLeft)
        {
            // Calculating the new position of our buddy
            Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
            // Instantiating our new buody and storing him in a variable
            Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

            // if not tilable reverse the x of the object to get rid of ugly seams
            if (reverseScale)
            {
                newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
            }

            newBuddy.parent = myTransform.parent;
            if(rightOrLeft > 0)
            {
                newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
            }
            else
            {
                newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
            }

        }


    }
}
