using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int scoreMultiplier = 10;
    [SerializeField] TextMeshProUGUI levelScoreMultiplier;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) //if the player hits a bumper or a sidewall - play the sound and increase multiplier
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().PlayBumpSound();
            scoreMultiplier ++;
            levelScoreMultiplier.SetText("Level Score Mulitplier: " + (float) scoreMultiplier/10); 
            /* score multiplier is intionally set as an integer and the divided by 10, because using float and increasing it by .1
             sometimes results in values like 1.8999999 instead of 1.9 */
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayingArea")) //if the player leaves the game area somehow - add extra pebble and reset it
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().OutOfBoundaries(); 
        }
    }
}
