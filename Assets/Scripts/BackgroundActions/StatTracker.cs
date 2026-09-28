using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public static StatTracker Instance { get; private set; }

    public int camerasDestroyed = 0;
    public int itemsCollected = 0;
    public int level = 0;

    public float elapsedTime = 0f;
    private bool timerRunning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            level = 0;
            camerasDestroyed = 0;
            itemsCollected = 0;
            elapsedTime = 0f;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void CameraDestroyed() => camerasDestroyed++;
    public void ItemCollected() => itemsCollected++;
    public void LevelCompleted() => level++;

    public string GetFormattedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes}:{seconds:00}.{milliseconds:000}";
    }

    public void SaveTopTime()
    {
        float t1 = PlayerPrefs.GetFloat("TopTime1", float.MaxValue);
        float t2 = PlayerPrefs.GetFloat("TopTime2", float.MaxValue);
        float t3 = PlayerPrefs.GetFloat("TopTime3", float.MaxValue);

        if (elapsedTime < t1)
        {
            PlayerPrefs.SetFloat("TopTime3", t2);
            PlayerPrefs.SetFloat("TopTime2", t1);
            PlayerPrefs.SetFloat("TopTime1", elapsedTime);
        }
        else if (elapsedTime < t2)
        {
            PlayerPrefs.SetFloat("TopTime3", t2);
            PlayerPrefs.SetFloat("TopTime2", elapsedTime);
        }
        else if (elapsedTime < t3)
        {
            PlayerPrefs.SetFloat("TopTime3", elapsedTime);
        }

        PlayerPrefs.Save();
    }

    public float[] GetTopTimes()
    {
        return new float[]
        {
            PlayerPrefs.GetFloat("TopTime1", -1f),
            PlayerPrefs.GetFloat("TopTime2", -1f),
            PlayerPrefs.GetFloat("TopTime3", -1f)
        };
    }
}