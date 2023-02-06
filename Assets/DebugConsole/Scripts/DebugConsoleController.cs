using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleController : MonoBehaviour
{
    [SerializeField] private GameObject _logItemPrefab;

    [Header("Debug Console")]
    [SerializeField] private GameObject _scrollView;
    [SerializeField] private Transform _consoleContent;
    private ScrollRect _scrollRect;

    [Header("Error Display")]
    [SerializeField] private GameObject _errorPanel;
    [SerializeField] private Text _error;
    [SerializeField] private Text _errorContent;
    [SerializeField] private Button _closeErrorBtn;

    [Header("Hide Button")]
    [SerializeField] private Text _hideBtnText;
    [SerializeField] private Button _hideBtn;

    private bool _even; //used to alternate log item background color
    private bool _wasErrorPanelOpen; //track whether to open the error panel with the console or not
    private bool _isOpen = true; //track the current console's state

    private void Start()
    {
        Application.logMessageReceived += ConsoleLog;
        _closeErrorBtn.onClick.AddListener(HideErrorPanel);
        _hideBtn.onClick.AddListener(HideConsole);
        _errorPanel.SetActive(false);
        _scrollRect = _scrollView.GetComponent<ScrollRect>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Application.logMessageReceived -= ConsoleLog;
        _closeErrorBtn.onClick.RemoveListener(HideErrorPanel);
        _hideBtn.onClick.RemoveListener(HideConsole);
    }

    private void ConsoleLog(string logString, string stackTrace, LogType type)
    {
        logString = $"[{DateTime.Now:HH:mm:ss}] {logString}";

        GameObject logItem = Instantiate(_logItemPrefab, _consoleContent);
        LogItemController lic = logItem.GetComponent<LogItemController>();

        lic.Setup(logString, stackTrace, type, _even);
        if (type is LogType.Error)
            lic.onDisplayError += () => { DisplayError(logString, stackTrace); };

        // Keep the scroll bar at the bottom if it's already at the bottom
        if (_scrollRect != null && _scrollRect.verticalNormalizedPosition <= 0.05f)
            StartCoroutine(ScrollToBottom()); //need to wait a frame to update the scrollbar

        _even = !_even;
    }

    private void DisplayError(string logString, string stackTrace)
    {
        _error.text = logString;
        _errorContent.text = stackTrace;
        _errorPanel.SetActive(true);
    }

    private void HideErrorPanel()
    {
        _errorPanel.SetActive(false);
        _error.text = "";
        _errorContent.text = "";
    }

    private void HideConsole()
    {
        if (_isOpen)
        {
            // Hiding the console
            _scrollView.SetActive(false);
            _wasErrorPanelOpen = _errorPanel.activeInHierarchy;
            _errorPanel.SetActive(false);
            _hideBtnText.text = "^";
        }
        else
        {
            // Opening the console
            _scrollView.SetActive(true);
            _errorPanel.SetActive(_wasErrorPanelOpen);
            _hideBtnText.text = "_";
        }

        _isOpen = !_isOpen;
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
