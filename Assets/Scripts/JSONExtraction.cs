using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class PlayerData
{
    public int speed;
}

[Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}

[Serializable]
public class Data
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

public class JSONExtraction : MonoBehaviour
{
    private Data jsonData;

    public Data GetJsonData()
    {
        return jsonData;
    }

    private string url = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            jsonData = JsonUtility.FromJson<Data>(json);

            Debug.Log("Data loaded successfully");

            //UpdateData();
        }
        else
        {
            Debug.LogError("Failed to load data: " + request.error);
        }
    }

    //private void UpdateData()
    //{
    //    if (jsonData != null)
    //    {
    //        Debug.Log("Player Speed: " + jsonData.player_data.speed);
    //        Debug.Log("Min destroy Time: " + jsonData.pulpit_data.min_pulpit_destroy_time);
    //        Debug.Log("Max destroy Time: " + jsonData.pulpit_data.max_pulpit_destroy_time);
    //        Debug.Log("Spawn Time: " + jsonData.pulpit_data.pulpit_spawn_time);
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to load data");
    //    }
    //}
}
