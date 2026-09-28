using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [Header("Stats")]
    public TMP_Text timeText;
    public TMP_Text itemsText;
    public TMP_Text camerasText;

    [Header("Leaderboard")]
    public TMP_Text topTime1Text;
    public TMP_Text topTime2Text;
    public TMP_Text topTime3Text;

    [Header("Buttons")]
    public Button playAgainButton;
    public Button mainMenuButton;

    void Start()
    {
        StatTracker.Instance.StopTimer();
        StatTracker.Instance.SaveTopTime();

        // Show this run's stats
        timeText.text = "Time: " + StatTracker.Instance.GetFormattedTime(StatTracker.Instance.elapsedTime);
        itemsText.text = "Items Collected: " + StatTracker.Instance.itemsCollected;
        camerasText.text = "Cameras Destroyed: " + StatTracker.Instance.camerasDestroyed;

        //Display top 3 times
        float[] topTimes = StatTracker.Instance.GetTopTimes();

        topTime1Text.gameObject.SetActive(topTimes[0] >= 0);
        if (topTimes[0] >= 0) topTime1Text.text = "1. " + StatTracker.Instance.GetFormattedTime(topTimes[0]);

        topTime2Text.gameObject.SetActive(topTimes[1] >= 0);
        if (topTimes[1] >= 0) topTime2Text.text = "2. " + StatTracker.Instance.GetFormattedTime(topTimes[1]);

        topTime3Text.gameObject.SetActive(topTimes[2] >= 0);
        if (topTimes[2] >= 0) topTime3Text.text = "3. " + StatTracker.Instance.GetFormattedTime(topTimes[2]);

        // Buttons
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);
    }

    void PlayAgain()
    {
        // Reset stats and go back to level 1
        StatTracker.Instance.camerasDestroyed = 0;
        StatTracker.Instance.itemsCollected = 0;
        StatTracker.Instance.elapsedTime = 0f;
        SceneManager.LoadScene(1);
    }

    void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}