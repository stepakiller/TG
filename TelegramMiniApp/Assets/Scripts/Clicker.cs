using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Clicker : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText; 
    private int score;
    private string serverUrl = "http://127.0.0.1:3000/save";

    void OnEnable()
    {
        score = AuthManager.startScore;
        UpdateUI();
    }

    public void OnClick()
    {
        score++;
        UpdateUI();
        StartCoroutine(SaveScoreToServer());
    }

    void UpdateUI()
    {
        counterText.text = score.ToString();
    }

    IEnumerator SaveScoreToServer()
    {
        string url = $"{serverUrl}?login={UnityWebRequest.EscapeURL(AuthManager.currentUser)}&score={score}";
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
        }
    }
}