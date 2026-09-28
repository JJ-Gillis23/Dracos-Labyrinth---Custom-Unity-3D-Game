using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI persistentText;
    public TextMeshProUGUI achivementText;
    public GameObject keyUIItemPrefab;
public Transform keyInventoryPanel;
    private Dictionary<string, GameObject> keyUIItems = new Dictionary<string, GameObject>();

    public static PlayerUI Instance { get; private set; }

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ShowAchievement("Level " + StatTracker.Instance.level);
    }

    // Update is called once per frame
    public void UpdatePromptText(string message)
    {
        promptText.text = message;
    } 
    public void UpdateTimerText(string message)
    {
        timerText.text = message;
    }
    public void UpdatePersistentText(string message)
    {
        persistentText.text = message;
    }
    public void UpdateAchievementText(string message)
    {
        achivementText.text = message;
    }
    public void ShowAchievement(string message)
    {
        StopCoroutine("ClearAchievement");
        UpdateAchievementText(message);
        StartCoroutine("ClearAchievement");
    }

    IEnumerator ClearAchievement()
    {
        yield return new WaitForSeconds(3f);
        UpdateAchievementText(string.Empty);
    }
    public void ShowPersistentText(string message)
    {
        StopCoroutine("ClearPersistentText");
        UpdatePersistentText(message);
        StartCoroutine("ClearPersistentText");
    }

    IEnumerator ClearPersistentText()
    {
        yield return new WaitForSeconds(3f);
        UpdatePersistentText(string.Empty);
    }
    public void AddKeyUI(string keyID, int uses)
    {
        if (keyUIItems.ContainsKey(keyID)) 
        {
            // Already exists, just update it
            return;
        }

        GameObject item = Instantiate(keyUIItemPrefab, keyInventoryPanel);
        keyUIItems[keyID] = item;
    }

    public void RemoveKeyUI(string keyID)
    {
        if (keyUIItems.ContainsKey(keyID))
        {
            Destroy(keyUIItems[keyID]);
            keyUIItems.Remove(keyID);
        }
    }
    public void ClearAllKeysUI()
    {
        foreach (var item in keyUIItems.Values)
        {
            Destroy(item);
        }
        keyUIItems.Clear();
    }
}
