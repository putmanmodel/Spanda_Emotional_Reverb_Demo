using TMPro;
using UnityEngine;
using UnityEngine.UI; // ← REQUIRED for LayoutRebuilder

public class MemoryLogUI : MonoBehaviour
{
    public static MemoryLogUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI logText;
    [Range(5, 100)]
    [SerializeField] private int maxLines = 30;

    private readonly System.Collections.Generic.Queue<string> logLines = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject); // ensures only one persists
    }

    public void AppendLog(string npcName, float delta, string deltaShorthand, string sourceName, string reason)
    {
        if (logText == null)
        {
            Debug.LogWarning("MemoryLogUI: logText is not assigned.");
            return;
        }

        string line = $"<color=white>[{sourceName} → <b>{npcName}</b>] Δ+{delta:F2} | {reason} | {deltaShorthand}</color>";
        logLines.Enqueue(line);

        while (logLines.Count > maxLines)
            logLines.Dequeue();

        logText.text = string.Join("\n", logLines);

        // 🔍 Debug dump of full log text
        Debug.Log("[AppendLog] Full text:\n" + logText.text);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(logText.rectTransform.parent as RectTransform);
    }
}