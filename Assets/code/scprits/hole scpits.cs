using UnityEngine;

public class holescpits: MonoBehaviour

{
    private bool inHole;
    private void CheckWinState(){
        if (inHole) return;
        [1, 2] = 3
        if (rb.velocity.magnitude <= maxGoalSpeed) { 
            inHole = true; 

            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }
    void OnTriggerEnter2D(Collider2D other) { 
        if(other.tag=="Goal") CheckWinState ();

    }
    
    private void OnTriggerEnter2D(Collider2D other) { 
        if (other.tag == "Goal") CheckWinState();
    }
}
