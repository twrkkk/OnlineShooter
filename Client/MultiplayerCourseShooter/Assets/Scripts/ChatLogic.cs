using UnityEngine;
using UnityEngine.UI;

public class ChatLogic : MonoBehaviour
{
    [SerializeField] private ChatUI _chatUI;
    [SerializeField] private InputField _input;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Controller _controller;
    private bool _isPrinting;

    private void Update()
    {
        if (_isPrinting == false && Input.GetKeyDown(KeyCode.Return))
        {
            ActivateInput();
        }
    }
    private void OnEnable()
    {
        _input.onSubmit.AddListener(SendMessage);
        _sendButton.onClick.AddListener(SendMessage);
        DeactivateInput();
    }

    private void OnDisable()
    {
        _input.onSubmit.RemoveListener(SendMessage);
        _sendButton.onClick.RemoveListener(SendMessage);
    }

    public void ActivateInput()
    {
        _controller.IsPrinting = true;
        _input.gameObject.SetActive(true);
        _sendButton.gameObject.SetActive(true);
        _isPrinting = true;
        _input.ActivateInputField();
        _input.Select();
    }

    public void DeactivateInput()
    {
        _controller.IsPrinting = false;
        _input.gameObject.SetActive(false);
        _sendButton.gameObject.SetActive(false);
        Invoke("DeactivatePrinting", 0.2f);
    }

    public void SendMessage(string message)
    {
        bool empty = string.IsNullOrEmpty(message);

        if (!empty)
            MultiplayerManager.Instance.SendMessage("chat", message);

        _input.text = "";
        DeactivateInput();
    }

    public void SendMessage()
    {
        string message = _input.text;
        if (string.IsNullOrEmpty(message)) return;

        MultiplayerManager.Instance.SendMessage("chat", message);

        _input.text = "";
        DeactivateInput();
    }

    public void ShowNewMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        _chatUI.ApplyMessage(message);
    }

    private void DeactivatePrinting()
    {
        _isPrinting = false;
    }
}
