using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    public void ApplyMessage(string text)
    {
        _text.text += $"\n[{DateTime.Now.ToString("HH:mm:ss")}] {text}";    
    }
}
