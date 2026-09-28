using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Hide continue button if no save exists
        if(!SaveSystem.Instance.HasSave())
            continueButton.SetActive(false);
    }

    public GameObject continueButton;

    public void PlayNew()
    {
        SaveSystem.Instance.DeleteSave();
        StatTracker.Instance.level = 1;
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SaveSystem.Instance.loadingFromSave = true;

        int level = SaveSystem.Instance.GetSavedLevel();
        SceneManager.LoadScene(level);
    }
}