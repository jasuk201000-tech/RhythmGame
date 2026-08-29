using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool StartPlaying;
    public BeatScroller theBS;

    public static GameManager instance;

    // accuracy weighting (similar to osu or maimai)
    public float perfectWeight = 1.0f;   // 100%
    public float greatWeight = 0.66f;  // ~66%
    public float goodWeight = 0.33f;  // ~33%

    float earnedPoints;   // sum of weights actually earned
    int notesPlayed;      // every judged note (hit or miss)
    public float currentAccuracy;

    // combo and multiplier settings
    public int currentCombo;
    public int Currentmultiplier;
    public int multiplierTracker;
    public int[] multiplierThreshold;

    public TMP_Text accuracyText;
    public TMP_Text multiText;
    public TMP_Text comboText;

    // note tally
    public int TotalNotes;
    public int goodHits;
    public int greatHits;
    public int perfectHits;
    public int missHits;

   

    bool songFinished;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Currentmultiplier = 1;
        UpdateAccuracyText();

        TotalNotes = FindObjectsByType<NoteObject>(FindObjectsInactive.Include).Length;
    }

    void Update()
    {
        if (!StartPlaying)
        {
            if (Input.anyKeyDown)
            {
                StartPlaying = true;
                theBS.HasStarted = true;
                theMusic.Play();
            }
        }
    }

    // called by every successful hit; weight depends on the tier
    void RegisterHit(float weight)
    {
        earnedPoints += weight;
        notesPlayed++;

        // combo / multiplier progression (combo owned here, once)
        currentCombo++;
        if (Currentmultiplier - 1 < multiplierThreshold.Length)
        {
            multiplierTracker++;
            if (multiplierThreshold[Currentmultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                Currentmultiplier++;
            }
        }

        RefreshUI();
        
    }

    public void PerfectHit()
    {
        Debug.Log("Perfect");
        perfectHits++;
        RegisterHit(perfectWeight);
    }

    public void GreatHit()
    {
        Debug.Log("Great");
        greatHits++;
        RegisterHit(greatWeight);
    }

    public void GoodHit()
    {
        Debug.Log("Good");
        goodHits++;
        RegisterHit(goodWeight);
    }

    public void NoteMissed()
    {
        Debug.Log("Missed");

        missHits++;
        notesPlayed++;
        currentCombo = 0;          // reset combo on a miss
        Currentmultiplier = 1;
        multiplierTracker = 0;

        RefreshUI();
        
    }

    void RecalculateAccuracy()
    {
        // earned / notes played so far, as a percentage (live display)
        if (notesPlayed > 0)
            currentAccuracy = (earnedPoints / notesPlayed) * 100f;
        else
            currentAccuracy = 100f;
    }

    void RefreshUI()
    {
        RecalculateAccuracy();
        UpdateAccuracyText();

        if (comboText != null)
            comboText.text = "Combo: " + currentCombo;
        if (multiText != null)
            multiText.text = "Multiplier: x" + Currentmultiplier;
    }

    void UpdateAccuracyText()
    {
        if (accuracyText != null)
            accuracyText.text = "Accuracy: " + currentAccuracy.ToString("F1") + "%";
    }
}