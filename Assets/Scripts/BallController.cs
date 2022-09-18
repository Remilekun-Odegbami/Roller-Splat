using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb; // a component that takes care of movement
    public ParticleSystem explosionParticle;


    public float speed = 15;
    private float moveBall = 0.5f;

    public int minSwipeRecognition = 500; // gets the direction the mouse should move before the ball moves to prevent it from moving on mouse hover or screen touch

    private bool isMoving;

    private Vector3 direction;
    private Vector3 nextCollisionPosition;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;


    private void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = speed * direction;
        }

        // to get the position of the ball and the ground by creating a sphere when it is above the ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f); // the code reads: get ball position, go down half of the ball position and the sphere should be 5% of the ball size.

        // while loop to change the ground color whwn the ball touches it
        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if (nextCollisionPosition != Vector3.zero) // statement to check if the ball hit a block or wall

        {
            // if ball hits a wall trigger the particle effect
            explosionParticle.Play();

            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1) // if the ball is not moving in x, y or z axis because it hit wall
            {
                isMoving = false;
                direction = Vector3.zero; // travel direction = 0
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isMoving) // if the ball continues to move, ignore the above if statement
        {
            return;
        }

        if (Input.GetMouseButton(0)) // if the mouse is pressed down or clicked
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // get the x and y coordinate of where the mouse currently is

            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                // if the square root of the mouse is less, ignore and make the ball remain in position
                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize(); // normalize gets the direction not the distance.

                // If the swipe is up/down
                if (currentSwipe.x > -moveBall && currentSwipe.x < moveBall)
                {
                    // Go up/down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if (currentSwipe.y > -moveBall && currentSwipe.y < moveBall)
                {
                    // Go left/right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

            }

            swipePosLastFrame = swipePosCurrentFrame; // make the last position the current position
        }

        if (Input.GetMouseButtonUp(0)) // if the mouse is released
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 ballDirection)
    {
        direction = ballDirection;

        RaycastHit hit; // checks the object the ball collides with
        if (Physics.Raycast(transform.position, ballDirection, out hit, 100f)) // the position, direction, did we hit something?, how far?
        {
            nextCollisionPosition = hit.point;
        }

        isMoving = true;
    }


}

