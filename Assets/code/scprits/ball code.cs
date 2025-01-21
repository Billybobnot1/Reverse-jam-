using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public int nextscene;
    public Rigidbody2D rb;
    public LineRenderer lr;

    public float maxPower = 10f; 
    public float power = 2f; 
    public float maxGoalSpeed = 100f;

    private bool isDragging;
    private bool inHole;

    public Text Par, Strokes;
    [SerializeField] int strokes =0;
    public int par =4;

    private GameObject instantiatedClub; // Instance of the club
    private Vector2 swingStartPos;       // Start position of the swing
    private float swingProgress;         // Progress of the swing (0 to 1)
    private float swingSpeed = 4f;       // Speed of the swing

    private void Update()
    {
        HandlePlayerInput();

        if (instantiatedClub != null)
        {
            PerformSwing();
        }
    }

    private void HandlePlayerInput()
    {
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (ItemHoldManager.currentItem != null)
        {
            Par.text = "Par: " + par;
            Strokes.text = "Strokes: " + strokes;
            power = ItemHoldManager.currentItem.pow;

            if (Input.GetMouseButtonDown(0) && distance <= 0.5f)
                StartSwing(inputPos);

            if (Input.GetMouseButtonUp(0) && isDragging)
                ReleaseSwing(inputPos);
        }
    }

    private void StartSwing(Vector2 mousePos)
    {
        isDragging = true;

        if (ItemHoldManager.currentItem.prefab != null)
        {
            // Instantiate the club and set its starting position
            instantiatedClub = Instantiate(ItemHoldManager.currentItem.prefab, mousePos, Quaternion.identity);
            swingStartPos = mousePos;
            swingProgress = 0f; // Reset swing progress
        }
    }
    private bool isReturningToBall; // Flag to check if the club is returning to the ball

    private void ReleaseSwing(Vector2 releasePos)
    {
        isDragging = false;

        if (instantiatedClub != null)
        {
            // Start moving the club towards the ball before applying force
            isReturningToBall = true;
        }
        strokes++;
    }

    private void PerformSwing()
    {
        if (instantiatedClub == null) return;

        if (isReturningToBall)
        {
            // Move the club towards the ball
            instantiatedClub.transform.position = Vector3.MoveTowards(
                instantiatedClub.transform.position,
                transform.position,
                swingSpeed * Time.deltaTime
            );

            // Rotate the club to point towards the ball
            Vector2 direction = (Vector2)transform.position - (Vector2)instantiatedClub.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            instantiatedClub.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Check if the club has reached the ball
            if (Vector2.Distance(instantiatedClub.transform.position, transform.position) < 0.1f)
            {
                // Destroy the club and apply force to the ball
                Destroy(instantiatedClub);
                instantiatedClub = null;
                isReturningToBall = false;

                // Apply force to the ball after the club reaches it
                Vector2 dir = (Vector2)transform.position - swingStartPos;
                rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
            }
            return;
        }

        // Swing motion logic (if applicable during dragging)
        if (isDragging)
        {
            Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swingStartPos = inputPos; // Dynamically update swing start position

            // Increment swing progress
            swingProgress += Time.deltaTime * swingSpeed;

            // Calculate sine wave motion
            float sineValue = Mathf.Sin(swingProgress); // Oscillates between -1 and 1
            float swingFactor = (sineValue + 1f) / 2f; // Normalize to a range of 0 to 1

            // Interpolate between mouse position and ball position
            Vector2 currentPos = Vector2.Lerp(swingStartPos, transform.position, swingFactor);

            // Update the club's position and rotation
            instantiatedClub.transform.position = currentPos;
            Vector2 direction = (Vector2)transform.position - currentPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            instantiatedClub.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void CheckWinState(){ 
        Debug.Log ("checked");
        if (inHole) return;
        //[1, 2] = 3;
        if (rb.velocity.magnitude <= maxGoalSpeed) { 
            Debug.Log("test");
            inHole = true; 

            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);

            SceneManager.LoadScene (nextscene);
        }

    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.tag=="Goal") CheckWinState ();

    }
    
}

