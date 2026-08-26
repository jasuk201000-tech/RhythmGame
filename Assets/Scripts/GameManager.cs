using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool StartPlaying;
    public BeatScroller theBS;

    public static GameManager instance;

    // --- Accuracy (replaces score) ---
    // Weight each hit tier contributes toward accuracy.
    public float perfectWeight = 1.0f;   // 100%
    public float greatWeight = 0.66f;  // ~66%
    public float goodWeight = 0.33f;  // ~33%
    // Miss contributes 0.

    float earnedPoints;   // sum of weights actually earned
    int notesPlayed;      // every judged note (hit or miss)
    public float currentAccuracy; // 0–100, for display / other systems

    // --- Combo & multiplier (unchanged) ---
    public int currentCombo;
    public int Currentmultiplier;
    public int multiplierTracker;
    public int[] multiplierThreshold;

    public TMP_Text accuracyText;   // was scoreText
    public TMP_Text multiText;
    public TMP_Text comboText;

    void Awake()
    {
        instance = this;              // Awake so instance is ready before any note
    }

    void Start()
    {
        Currentmultiplier = 1;
        UpdateAccuracyText();         // shows 100% (or 0 notes) at start
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

    // Called by every successful hit; weight depends on the tier.
    void RegisterHit(float weight)
    {
        earnedPoints += weight;
        notesPlayed++;

        // combo / multiplier progression
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

    public void PerfectHit() { Debug.Log("Perfect"); RegisterHit(perfectWeight); }
    public void GreatHit() { Debug.Log("Great"); RegisterHit(greatWeight); }
    public void GoodHit() { Debug.Log("Good"); RegisterHit(goodWeight); }

    public void NoteMissed()
    {
        Debug.Log("Missed");

        notesPlayed++;                // a miss still counts as a note played (0 earned)
        currentCombo = 0;            // reset combo
        Currentmultiplier = 1;
        multiplierTracker = 0;

        RefreshUI();
    }

    void RecalculateAccuracy()
    {
        // earned / maximum possible, as a percentage
        if (notesPlayed > 0)
            currentAccuracy = (earnedPoints / notesPlayed) * 100f;
        else
            currentAccuracy = 100f;   // nothing played yet
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