using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleTester : MonoBehaviour
{
    [SerializeField] private InputField _logInputField;

    [Header("Buttons")]
    [SerializeField] private Button _logBtn;
    [SerializeField] private Button _logWarningBtn;
    [SerializeField] private Button _logErrorBtn;

    private void Start()
    {
        _logBtn.onClick.AddListener(Log);
        _logWarningBtn.onClick.AddListener(LogWarning);
        _logErrorBtn.onClick.AddListener(LogError);
    }

    private void Log()
    {
        Debug.Log(string.IsNullOrEmpty(_logInputField.text) ? "Default log message" : _logInputField.text);
    }

    private void LogWarning()
    {
        Debug.LogWarning(string.IsNullOrEmpty(_logInputField.text) ? "Default log warning message" : _logInputField.text);
    }

    private void LogError()
    {
        Debug.LogError(string.IsNullOrEmpty(_logInputField.text) ? "Default log error message" : _logInputField.text);
    }
}
