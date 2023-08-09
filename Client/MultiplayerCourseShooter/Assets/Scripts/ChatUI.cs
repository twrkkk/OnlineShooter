using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private GameObject _messagePrefab;
    [SerializeField] private Transform _container;
    public void CreateMessage(string text)
    {
        GameObject newMessage = Instantiate(_messagePrefab, _container);
        newMessage.GetComponent<Text>().text = text;    
    }
}
