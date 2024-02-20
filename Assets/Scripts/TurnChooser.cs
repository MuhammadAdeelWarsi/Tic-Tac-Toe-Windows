using UnityEngine;

public class TurnChooser : MonoBehaviour
{
    public string firstTurnHolder;

    
    private void OnTriggerEnter2D(Collider2D colliderInfo) 
    {
        firstTurnHolder = (colliderInfo.tag == "PlayerOne") ? "PlayerOne" : "PlayerTwo";
    }   
}