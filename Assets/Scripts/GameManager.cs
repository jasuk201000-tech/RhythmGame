using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class GameManager : MonoBehaviour
{

    public AudioSource theMusic;

    public bool StartPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore; // initially set to to zero

    public int scorePerNote = 100; // generalised score may change later

    public int Currentmultiplier;
    public int multiplierTracker;

    public int[] multiplierThreshold;

    public TMP_Text scoreText;

    public TMP_Text multiText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        scoreText.text = "Score: 0"; // initial score display

        Currentmultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartPlaying)
        { 
            if(Input.anyKeyDown)
            {
                StartPlaying = true;
                theBS.HasStarted = true;
                theMusic.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit on time");

        if(Currentmultiplier - 1 < multiplierThreshold.Length)
        {
            multiplierTracker++;
            if (multiplierThreshold[Currentmultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                Currentmultiplier++;
            }
        }
       
        multiText.text = "Multiplier: x" + Currentmultiplier; // update multiplier display
        currentScore += scorePerNote * Currentmultiplier;


        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
        // update score display and exception handling if scoreText is not assigned
    }

    public void NoteMissed()
    {
        Debug.Log("Missed");
    }
}
