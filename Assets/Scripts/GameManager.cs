using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Camera cam;
    [SerializeField] float camVerticalConstraint;
    [SerializeField] float playerHorizontalConstraint;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI levelScoreText;
    [SerializeField] Button restarButton;
    [SerializeField] AudioClip bounceSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerController playerController;
    [SerializeField] TextMeshProUGUI levelScoreMultiplier;
    int camHorizontalOffset = 0;
    bool pebbleFalling;
    int playerMovementDirection = 1;
    int movementSpeed = 25;
    int score = 0;
    public int lives = 2;
    // Start is called before the first frame update
    void Start()
    {
        PreparePebble();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !pebbleFalling)
            DropPebble();
    }
    private void FixedUpdate()
    {
        if (!pebbleFalling)
            MovePebbleHorizontally();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (pebbleFalling && player.transform.position.y + camHorizontalOffset <= camVerticalConstraint && player.transform.position.y + camHorizontalOffset >= -camVerticalConstraint) // constraints, so that came does not go above or below the texture
            cam.transform.position = new Vector3(0, player.transform.position.y + camHorizontalOffset, -22);
    }

    public void PreparePebble() //resetting camera, pebble and all the necessary texts
    {
        player.transform.position = new Vector3(0, 66, -0.75f);
        cam.transform.position = new Vector3(0, camVerticalConstraint, -22);
        pebbleFalling = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        levelScoreText.gameObject.transform.localScale = Vector3.one * 3;
        levelScoreText.gameObject.SetActive(false);
        levelScoreMultiplier.SetText("Level Score Mulitplier: 1.0");
    }

    void MovePebbleHorizontally()
    {
        player.transform.Translate(Vector3.right * playerMovementDirection * movementSpeed * Time.deltaTime);
        if (player.transform.position.x < -playerHorizontalConstraint)
            playerMovementDirection = 1;
        if (player.transform.position.x > playerHorizontalConstraint)
            playerMovementDirection = -1;
    }

    void DropPebble()
    {
        player.GetComponent<Rigidbody>().useGravity = true; //enable falling
        pebbleFalling = true;
        player.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 25), Random.Range(0, 25), Random.Range(0, 25)), ForceMode.Impulse); //set random rotation, for more random outcome of hitting objects
        lives--;
        livesText.SetText("Pebbles: " + lives);
    }

    public void UpdateScore(int scoreIncrease) //display score achieved in this play and update total score
    {
        levelScoreText.gameObject.SetActive(true);
        levelScoreText.SetText("+" + (int)(scoreIncrease * playerController.scoreMultiplier / 10));
        score = score + (int)(scoreIncrease * playerController.scoreMultiplier / 10);
        scoreText.SetText("Score: " + score);
        audioSource.PlayOneShot(finishSound);
        playerController.scoreMultiplier = 10;
    }

    public IEnumerator GameOver() //display a gameover text, score and restart button
    {
        yield return new WaitForSeconds(5);
        levelScoreText.gameObject.transform.localScale = Vector3.one;
        levelScoreText.SetText("YOUR SCORE: " + score);
        restarButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayBumpSound()
    {
        audioSource.PlayOneShot(bounceSound);
    }

    public void OutOfBoundaries() //if somehow ball gets out of playable zone - add extra life and reset the pebble
    {
        levelScoreText.gameObject.SetActive(true);
        levelScoreText.gameObject.transform.localScale = Vector3.one;
        levelScoreText.SetText("Oops. Out of boundaries. \n Here is an extra pebble for you.");
        playerController.scoreMultiplier = 10;
        lives++;
        livesText.SetText("Pebbles: " + lives);
        StartCoroutine(NextPebble());
    }

    public IEnumerator NextPebble() //delay to let the player read whatever text there is to read, before resetting the pebble
    {
        yield return new WaitForSeconds(5);
        PreparePebble();
    }
}
