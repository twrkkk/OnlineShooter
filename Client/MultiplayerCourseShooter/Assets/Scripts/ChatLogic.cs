using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ChatLogic : MonoBehaviour
{
    [SerializeField] private ChatUI _chatUI;
    [SerializeField] private InputField _input;
    [SerializeField] private Button _sendButton;

    private void OnEnable()
    {
        _sendButton.onClick.AddListener(SendMessage);
    }

    private void OnDisable()
    {
        _sendButton.onClick.RemoveListener(SendMessage);
    }

    public void SendMessage()
    {
        string message = _input.text;
        if (string.IsNullOrEmpty(message)) return;

        MultiplayerManager.Instance.SendMessage("chat", message);

        ShowNewMessage(message);
        _input.text = "";
    }

    public void ShowNewMessage(string message)
    {
        if(string.IsNullOrEmpty(message)) return;
        _chatUI.CreateMessage(message);
    }
}
