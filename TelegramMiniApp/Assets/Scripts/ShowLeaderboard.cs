using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ShowLeaderboard : MonoBehaviour
{
    [System.Serializable]
    public class Player
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class BoardData
    {
        public Player[] players;
    }

    [Header("Куда выводить результат")]
    public TMP_Text textDisplay;
    string url = "http://127.0.0.1:3000/leaderboard";

    public void DisplayTop5()
    {
        textDisplay.text = "Загрузка лидеров...";
        StartCoroutine(FetchDataFromServer());
    }

    IEnumerator FetchDataFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success) textDisplay.text = "<color=red>Ошибка соединения: " + request.error + "</color>";
            else
            {
                string jsonResponse = request.downloadHandler.text;
                BoardData data = JsonUtility.FromJson<BoardData>(jsonResponse);
                textDisplay.text = "<b>TOP 5 LEADERS:</b>\n\n";
                for (int i = 0; i < data.players.Length; i++) textDisplay.text += $"{i + 1}. <b>{data.players[i].name}</b> - {data.players[i].score}\n";
            }
        }
    }
}
