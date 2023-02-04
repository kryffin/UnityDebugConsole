using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogItemController : MonoBehaviour
{
    [Header("Text Colors")]
    [SerializeField] private Color _logTextColor;
    [SerializeField] private Color _warningTextColor;
    [SerializeField] private Color _errorTextColor;

    [Header("Background Colors")]
    [SerializeField] private Color _backgroundColor1;
    [SerializeField] private Color _backgroundColor2;

    [Header("Links")]
    [SerializeField] private Text _text;
    [SerializeField] private RawImage _backgroundImage;
    [SerializeField] private Button _displayErrorBtn;

    [HideInInspector] public UnityAction onDisplayError;

    public void Setup(string logString, string stackTrace, LogType type, bool even)
    {
        _text.text = logString;

        switch (type)
        {
            default:
            case LogType.Log:
                _text.color = _logTextColor;
                break;

            case LogType.Warning:
                _text.color = _warningTextColor;
                break;

            case LogType.Error:
                _text.color = _errorTextColor;
                _displayErrorBtn.gameObject.SetActive(true);
                _displayErrorBtn.onClick.AddListener(() => { onDisplayError?.Invoke(); });
                break;
        }

        if (even)
            _backgroundImage.color = _backgroundColor1;
        else
            _backgroundImage.color = _backgroundColor2;
    }

    private void OnDestroy()
    {
        _displayErrorBtn.onClick.RemoveAllListeners();
    }
}
