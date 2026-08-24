using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public AudioSource theMusic;

    public bool StartPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore; // initially set to to zero

    public int scorePerNote = 100; // generalised score may change later

    public Text scoreText;

    public Text multiText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
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

        currentScore += scorePerNote;

        scoreText.text = "Score:" + currentScore;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed");
    }
}
