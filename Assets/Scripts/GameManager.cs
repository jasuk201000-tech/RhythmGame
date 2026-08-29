using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // music
    public AudioSource theMusic;
    public BeatScroller theBS;
    public bool StartPlaying;

    // accuracy weighting similar to osu and maimai
    public float perfectWeight = 1.0f;
    public float greatWeight = 0.66f;
    public float goodWeight = 0.33f;

    float earnedPoints;
    int notesPlayed;
    public float currentAccuracy;

    // combo and multiplier manager
    public int currentCombo;
    public int longestCombo;
    public int Currentmultiplier;
    public int multiplierTracker;
    public int[] multiplierThreshold;

    // UI text
    public TMP_Text accuracyText;
    public TMP_Text multiText;
    public TMP_Text comboText;

   // tallying notes
    public int TotalNotes;
    public int goodHits;
    public int greatHits;
    public int perfectHits;
    public int missHits;

    // pass threshold
    public float passThreshold = 50f;   // accuracy % needed to pass

    // pause panel
    public GameObject pausePanel;       
    public bool isPaused;

    // string for scene switching
    public string resultsSceneName = "Ranking Display";
    public string gameplaySceneName = "Round1";

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
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }

    // registering hits with respective accuracy
    void RegisterHit(float weight)
    {
        earnedPoints += weight;
        notesPlayed++;

        currentCombo++;
        if (currentCombo > longestCombo)
            longestCombo = currentCombo;

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
        CheckSongComplete();
    }

    public void PerfectHit() { Debug.Log("Perfect"); perfectHits++; RegisterHit(perfectWeight); }
    public void GreatHit() { Debug.Log("Great"); greatHits++; RegisterHit(greatWeight); }
    public void GoodHit() { Debug.Log("Good"); goodHits++; RegisterHit(goodWeight); }

    // note missed class
    public void NoteMissed()
    {
        Debug.Log("Missed");
        missHits++;
        notesPlayed++;
        currentCombo = 0;
        Currentmultiplier = 1;
        multiplierTracker = 0;

        RefreshUI();
        CheckSongComplete();
    }

    // accuracy arithmetic
    void RecalculateAccuracy()
    {
        if (notesPlayed > 0)
            currentAccuracy = (earnedPoints / notesPlayed) * 100f;
        else
            currentAccuracy = 100f;
    }

    // UI refresh after each note
    void RefreshUI()
    {
        RecalculateAccuracy();
        UpdateAccuracyText();

        if (comboText != null) comboText.text = "Combo: " + currentCombo;
        if (multiText != null) multiText.text = "Multiplier: x" + Currentmultiplier;
    }

    // updates accuracy each time a note is hit or missed
    void UpdateAccuracyText()
    {
        if (accuracyText != null)
            accuracyText.text = "Accuracy: " + currentAccuracy.ToString("F1") + "%";
    }

    // checks if song has complete through the number of notes played and total notes in the song
    void CheckSongComplete()
    {
        if (songFinished) return;
        if (notesPlayed >= TotalNotes && TotalNotes > 0)
        {
            songFinished = true;
            SongComplete();
        }
    }

    void SongComplete()
    {
        ResultsData.perfects = perfectHits;
        ResultsData.greats = greatHits;
        ResultsData.goods = goodHits;
        ResultsData.misses = missHits;
        ResultsData.accuracy = currentAccuracy;
        ResultsData.longestCombo = longestCombo;
        ResultsData.hasPassed = currentAccuracy >= passThreshold;

        Time.timeScale = 1f;   
        SceneManager.LoadScene(resultsSceneName);
    }

    // pausegame method
    public void PauseGame()
    {
        Debug.Log("PauseGame called | pausePanel is " + (pausePanel == null ? "NULL" : pausePanel.name));
        isPaused = true;
        Time.timeScale = 0f;
        if (theMusic != null) theMusic.Pause();
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {

        isPaused = false;
        Time.timeScale = 1f;
        if (theMusic != null) theMusic.UnPause();
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void RestartSong()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        Application.Quit();   // this will change once linked the group project (RPG)
        
    }
}