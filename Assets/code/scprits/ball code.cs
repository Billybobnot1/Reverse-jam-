using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour { 

// [Header ("References")] 
public Rigidbody2D rb; 
public LineRenderer lr;

// [Header ("Attributes")] 
public float maxPower = 10f; 
public float power = 2f; 
public float maxGoalSpeed = 100f; 

private bool isDragging; 
private bool inHole; 

private void Update() { 
    PlayerInput(); 

}

private void PlayerInput () { 
    Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    float distance = Vector2.Distance(transform.position, inputPos);

    if (Input.GetMouseButtonDown(0) && distance <=0.5f) DragStart();
    if (Input.GetMouseButton(0) && isDragging) DragChange(); 
    if (Input.GetMouseButtonUp(0) && isDragging) DragRelease(inputPos); 
 
}

public void DragStart () { 
isDragging = true; 
}
private void DragChange() {} 
private void DragRelease(Vector2 pos) {
    float distance = Vector2.Distance((Vector2)transform.position, pos);
    isDragging = false; 

    if (distance < 1f) { 
        return; 
    }
    Vector2 dir = (Vector2)transform.position - pos; 

    rb.linearVelocity = Vector2.ClampMagnitude(dir * power, maxPower);
}
    private void CheckWinState(){ 
        Debug.Log ("checked");
        if (inHole) return;
        //[1, 2] = 3;
        if (rb.linearVelocity.magnitude <= maxGoalSpeed) { 
            Debug.Log("test");
            inHole = true; 

            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.tag=="Goal") CheckWinState ();

    }
    
}

