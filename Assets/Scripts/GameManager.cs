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
    public int scorePerGoodNote = 100; // generalised score may change later

    public int scorePerGreatNote = 125; // score for great note
    public int scorePerPerfectNote = 150; // score for perfect note

    public int currentCombo; // current combo count

    public int Currentmultiplier;
    public int multiplierTracker;

    public int[] multiplierThreshold;

    public TMP_Text scoreText;

    public TMP_Text multiText;

    public TMP_Text comboText; //combo text to display the current combo count

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
       
       // multiText.text = "Multiplier: x" + Currentmultiplier; // update multiplier display
       // currentScore += scorePerNote * Currentmultiplier;


        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
        // update score display and exception handling if scoreText is not assigned
    }

    public void GoodHit()
    { 
        currentScore += scorePerGoodNote * Currentmultiplier;
        NoteHit();
        currentCombo++;
    }


    public void GreatHit()
    { 
        currentScore += scorePerGreatNote * Currentmultiplier;  
        NoteHit();
        currentCombo++;
    }

    public void PerfectHit()
    {
        
        currentScore += scorePerPerfectNote * Currentmultiplier;
        NoteHit();
        currentCombo++;
    }


    public void NoteMissed()
    {
        Debug.Log("Missed");

        Currentmultiplier = 1;
        multiplierTracker = 0;
        comboText.text = "Combo: " + currentCombo; // reset combo display



        multiText.text = "Multiplier: x" + Currentmultiplier; // update multiplier display
    }
}
