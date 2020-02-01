using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_test : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 10f;

    bool carrying = false;
    Vector3 movementDirection;




    // Start is called before the first frame update
    void Start()
    {
        // at start, movement direction is a zero-vector (0,0).
        // Vector3 variable can be given a value that is a Vector2. This results in a Vector3 with the X and Y of the 2-dimenional vector, and a Z-value of 0.
        movementDirection = new Vector2();

    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyboardInput();

        if (movementDirection.magnitude == 0)
            Debug.Log("Not moving");
        else
            Debug.Log("Moving!");

        transform.Translate( movementDirection * Time.deltaTime * movementSpeed);
        

    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, movementDirection * 2f, Color.red, 1.2f,false);
        RaycastHit2D[] results;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, 1f, 8 );
        if (hit.collider != null && hit.collider.tag == "bounds")
        {
            ChangeDirection("stop");

        }
    }

    void CheckKeyboardInput()

        // Input.GetAxis returns a value between -1 and 1
        // Horizontal axis is left (-1) to right (1), allowing for smooth control with joysticks, D-pad, etc
        // GetAxis also smooths arrow key inputs
    {
        if( Input.GetAxis("Horizontal") < -0.5f )
        {
            ChangeDirection("left");
        }
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            ChangeDirection("right");
        }
        if (Input.GetAxis("Vertical") > 0.5f)
        {
            ChangeDirection("up");
        }
        if (Input.GetAxis("Vertical") < -0.5f)
        {
            ChangeDirection("down");
        }

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            ChangeDirection("stop");
        }
    }

    /// <summary>
    /// Return a normalized Vector2 pointing up, down, left or right; or a 0-length vector.
    /// </summary>
    /// <param name="newDirection">String, expects up, down, left, right or stop</param>
    /// <returns></returns>
    Vector2 ChangeDirection(string newDirection) {
        Debug.Log("Changing movement direction to \'" + newDirection +"\'");

        newDirection = newDirection.ToLower();
        switch (newDirection)
        {
            case "up":
                movementDirection = new Vector2(0f, 0.5f);
                break;
            case "down":
                movementDirection = new Vector2(0f, -0.5f);
                break;
            case "left":
                movementDirection = new Vector2(-1f, 0f);
                break;
            case "right":
                movementDirection = new Vector2(1f, 0f);
                break;
            case "stop":
                movementDirection = new Vector2(0f, 0f);
                break;
            default:
                movementDirection = new Vector2(0f,0f);
                break;
        }
        return movementDirection;
    }

}
