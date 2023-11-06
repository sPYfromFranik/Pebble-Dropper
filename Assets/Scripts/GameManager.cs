using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] GameObject endButtons;
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
    string playerName = "Noname";
    BestScore bestScore = new BestScore();

    public class BestScore
    {
        public string playerName;
        public int score;
    }
    // Start is called before the first frame update
    void Start()
    {
        PreparePebble();
        if(MainManager.Instance != null)//in case the game is launched from this scene directly (during testing)
        {
            playerName = MainManager.Instance.playerName;
        }
        string saveFilePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(saveFilePath))//load data if there is such data
        {
            string json = File.ReadAllText(saveFilePath);
            bestScore = JsonUtility.FromJson<BestScore>(json);
            UpdateBestScore();
        }
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

    void LateUpdate()
    {
        if (pebbleFalling && player.transform.position.y + camHorizontalOffset <= camVerticalConstraint && player.transform.position.y + camHorizontalOffset >= -camVerticalConstraint) // constraints, so that came does not go above or below the texture
            cam.transform.position = new Vector3(0, player.transform.position.y + camHorizontalOffset, -22);
    }
    /// <summary>
    /// Resetting camera, pebble position and rotation, visibility and values of all the necessary texts
    /// </summary>
    public void PreparePebble()
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

    /// <summary>
    /// Move the pebble left and right, until a player presses space bar
    /// </summary>
    void MovePebbleHorizontally()
    {
        player.transform.Translate(Vector3.right * playerMovementDirection * movementSpeed * Time.deltaTime);
        if (player.transform.position.x < -playerHorizontalConstraint)
            playerMovementDirection = 1;
        if (player.transform.position.x > playerHorizontalConstraint)
            playerMovementDirection = -1;
    }
    /// <summary>
    /// Let the pebble fall and update the lives left
    /// </summary>
    void DropPebble()
    {
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 25), Random.Range(0, 25), Random.Range(0, 25)), ForceMode.Impulse); //set random rotation, for more random outcome when hitting objects
        pebbleFalling = true;
        lives--;
        livesText.SetText("Pebbles: " + lives);
    }
    /// <summary>
    /// Display the score that was achieved in this fall and update the total score
    /// </summary>
    /// <param name="scoreIncrease">Score achieved in this fall</param>
    public void UpdateScore(int scoreIncrease)
    {
        levelScoreText.gameObject.SetActive(true);
        levelScoreText.SetText("+" + (int)(scoreIncrease * playerController.scoreMultiplier / 10));
        score = score + (int)(scoreIncrease * playerController.scoreMultiplier / 10);
        scoreText.SetText("Score: " + score);
        audioSource.PlayOneShot(finishSound);
        playerController.scoreMultiplier = 10;
    }
    /// <summary>
    /// Display a gameover text, score and restart button
    /// </summary>
    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        levelScoreText.gameObject.transform.localScale = Vector3.one;
        levelScoreText.SetText("YOUR SCORE: " + score);
        endButtons.SetActive(true);
        if (bestScore.score < score)
        {
            bestScore.playerName = MainManager.Instance.playerName;
            bestScore.score = score;
            UpdateBestScore();
            string json = JsonUtility.ToJson(bestScore);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayBumpSound()
    {
        audioSource.PlayOneShot(bounceSound);
    }
    /// <summary>
    /// If somehow ball gets out of playable zone - add extra life and reset the pebble
    /// </summary>
    public void OutOfBoundaries()
    {
        levelScoreText.gameObject.SetActive(true);
        levelScoreText.gameObject.transform.localScale = Vector3.one;
        levelScoreText.SetText("Oops. Pebble got out of boundaries. \n Here is an extra pebble for you.");
        playerController.scoreMultiplier = 10;
        lives++;
        livesText.SetText("Pebbles: " + lives);
        StartCoroutine(NextPebble());
    }
    /// <summary>
    /// Wait for 5 seconds to let the player read the text then reset the pebble with PreparePebble() method
    /// </summary>
    public IEnumerator NextPebble()
    {
        yield return new WaitForSeconds(5);
        PreparePebble();
    }
    /// <summary>
    /// Updates the best score text at the bottom of the screen
    /// </summary>
    public void UpdateBestScore()
    {
        bestScoreText.gameObject.SetActive(true);
        bestScoreText.SetText("Best score by " + bestScore.playerName + ": " + bestScore.score);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
