using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    public int score=0;
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.UpdateScore(score);//update the score and show some text
            if (gameManager.lives > 0)
            {
                StartCoroutine(gameManager.NextPebble()); //delay to let the player read the text and then reset the pebble
            } 
            else
            {
                StartCoroutine(gameManager.GameOver()); //delay to let the player read current drop score and the show the game over text, score and restart button
            }
        }
    }
}
