using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [System.Serializable]
    public class ServerResponse
    {
        public bool success;
        public string message;
        public int score;
    }

    [Header("Элементы интерфейса")]
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;
    
    [Header("Экраны")]
    public GameObject authPanel;
    public GameObject gamePanel;

    public static string currentUser;
    public static int startScore;

    private string serverUrl = "http://127.0.0.1:3000";

    public void RegisterUser() => StartCoroutine(SendAuthRequest("/register"));
    public void LoginUser() => StartCoroutine(SendAuthRequest("/login"));

    IEnumerator SendAuthRequest(string endpoint)
    {
        string login = loginInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Заполните все поля!";
            yield break;
        }

        statusText.text = "Связь с сервером...";
        
        string fullUrl = $"{serverUrl}{endpoint}?login={UnityWebRequest.EscapeURL(login)}&password={UnityWebRequest.EscapeURL(password)}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                statusText.text = "Ошибка сети: " + webRequest.error;
            }
            else
            {
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(webRequest.downloadHandler.text);

                if (response.success)
                {
                    if (endpoint == "/login")
                    {
                        currentUser = login;
                        startScore = response.score;
                        StartGame();
                    }
                    else statusText.text = "Регистрация успешна! Войдите в аккаунт.";
                }
                else statusText.text = response.message;
            }
        }
    }

    private void StartGame()
    {
        authPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}