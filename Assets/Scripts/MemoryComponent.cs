using UnityEngine;
using System.Linq;
using TMPro;

public class MemoryComponent : MonoBehaviour
{
    public float[] baseline;
    public float[] currentVector;
    public int axisCount = 10;

    private MemoryLogUI logScroller;

    void Awake()
    {
        if (logScroller == null)
            logScroller = FindObjectOfType<MemoryLogUI>();

        baseline = new float[axisCount];
        currentVector = new float[axisCount];
        for (int i = 0; i < axisCount; i++)
            baseline[i] = currentVector[i];

        InitCascadeMatrix();
    }

    public bool HasEmotionalTag(string tag)
    {
        // Implement your logic here
        return false;
    }

    void InitCascadeMatrix()
    {
        // Placeholder for future logic
    }

    public void SetLastSource(GameObject source)
    {
        // Optional
    }

    public void RememberTag(string tag)
    {
        // Optional
    }

    public void LogReverbEntry(
        string sourceName,
        float[] delta,
        string reason,
        string[] deltaShorthand,
        float importance = 1f,
        string context = "default"
    )
    {
        Debug.Log($"[🧪 TRACE] LogReverbEntry called for: {sourceName} → {gameObject.name}");

        if (logScroller == null)
            logScroller = FindObjectOfType<MemoryLogUI>();

        if (logScroller == null)
        {
            Debug.LogWarning("LogReverbEntry: No MemoryLogUI found.");
            return;
        }

        float totalDelta = delta.Sum(Mathf.Abs);
        string deltaSummary = string.Join(",", deltaShorthand);

        Debug.Log($"[🧪 TRACE] Sending to AppendLog: delta={totalDelta:F2}, tags={deltaSummary}");

        logScroller.AppendLog(
            gameObject.name,
            totalDelta,
            deltaSummary,
            sourceName,
            reason
        );

        Debug.Log($"📘 [{sourceName} → {gameObject.name}] Δ+{totalDelta:F2} | {reason} | {deltaSummary}");
    }
}