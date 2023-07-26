using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{
    public void StartRun(string url, Action<string> OnSuccess, Action<string> Error = null) => StartCoroutine(Run(url, OnSuccess, (s) => { Debug.Log("error " + s); }));

    private IEnumerator Run(string url, Action<string> success, Action<string> error = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                error?.Invoke(www.error);
            else
                success?.Invoke(www.downloadHandler.text);
        }

        //using or www.Dispose();
    }
}
