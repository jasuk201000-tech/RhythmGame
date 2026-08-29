using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text perfectDisplay;
    public TMP_Text greatDisplay;
    public TMP_Text goodDisplay;
    public TMP_Text missDisplay;
    public TMP_Text accuracyDisplay;
    public TMP_Text longestComboDisplay;
    public TMP_Text rankingDisplay;
    public TMP_Text passFailDisplay;

    public string gameplaySceneName = "Round1";

    void Start()
    {
        Time.timeScale = 1f;

        perfectDisplay.text = "Perfects: " + ResultsData.perfects;
        greatDisplay.text = "Greats: " + ResultsData.greats;
        goodDisplay.text = "Goods: " + ResultsData.goods;
        missDisplay.text = "Misses: " + ResultsData.misses;
        accuracyDisplay.text = "Accuracy: " + ResultsData.accuracy.ToString("F1") + "%";
        longestComboDisplay.text = "Longest combo: " + ResultsData.longestCombo;
        rankingDisplay.text = GetRank(ResultsData.accuracy);

        if (passFailDisplay != null)
            passFailDisplay.text = ResultsData.hasPassed ? "PASS" : "FAIL";
    }

    string GetRank(float acc)
    {
        if (acc >= 95f) return "S";
        if (acc >= 85f) return "A";
        if (acc >= 75f) return "B";
        if (acc >= 50f) return "C";
        if (acc >= 40f) return "D";
        return "F";
    }

    public void RetrySong()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        // or SceneManager.LoadScene("MainMenu");
    }
}