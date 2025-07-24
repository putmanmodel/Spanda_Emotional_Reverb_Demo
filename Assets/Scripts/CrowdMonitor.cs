using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CrowdMonitor : MonoBehaviour
{
    [Tooltip("Set NPCs to monitor. Assign in Inspector or via code.")]
    public List<SpandaNPC> trackedNPCs = new List<SpandaNPC>();

    [Tooltip("Index of critical axes to monitor (e.g., Anger = 2, Hate = 6)")]
    public int[] alertAxes = new int[] { 2, 6 }; // Joy/Despair, Attraction/Repulsion

    [Tooltip("Threshold value above which Quarantine is triggered")]
    public float quarantineThreshold = 0.75f;

    [Tooltip("Optional: log every quarantine hit")]
    public bool debugLogs = true;

    // Tracks which (npc, axis) pairs are currently in quarantine
    private HashSet<string> activeQuarantines = new HashSet<string>();

    void Update()
{
    for (int i = 0; i < trackedNPCs.Count; i++)
    {
        var npc = trackedNPCs[i];
        if (npc == null || npc.memory == null) continue;

        float[] vector = npc.memory.currentVector;

        // 🔒 Quarantine Logic (unchanged)
        foreach (int axis in alertAxes)
        {
            if (axis >= vector.Length) continue;

            float absVal = Mathf.Abs(vector[axis]);
            string key = $"{npc.name}:{axis}";

            if (absVal >= quarantineThreshold)
            {
                if (!activeQuarantines.Contains(key))
                {
                    activeQuarantines.Add(key);
                    TriggerQuarantine(npc, axis, absVal);
                }
            }
            else
            {
                activeQuarantines.Remove(key);
            }
        }

        // 🧠 Memory Echo Logic (NEW)
        for (int j = 0; j < trackedNPCs.Count; j++)
        {
            if (i == j) continue;

            var other = trackedNPCs[j];
            if (other == null || other == npc) continue;

            float dist = Vector3.Distance(npc.transform.position, other.transform.position);
            if (dist < 3f) // 🧠 Trigger radius
            {
                var memory = npc.GetComponent<SpandaMemory>();
                if (memory != null)
                {
                    memory.TryTriggerEcho(other.gameObject);
                }
            }
        }
    }
}

    void TriggerQuarantine(SpandaNPC npc, int axis, float value)
    {
        if (debugLogs)
        {
            Debug.Log($"🚨 Quarantine Triggered: {npc.name} | Axis {axis} ({npc.axisZones[axis]}) = {value:F2}");
        }

        // Future idea: mark NPC visually, trigger camera, or apply tag
    }

    [ContextMenu("Auto-Populate NPCs")]
    public void AutoPopulate()
    {
        trackedNPCs = FindObjectsOfType<SpandaNPC>().ToList();
        Debug.Log($"📦 Auto-populated {trackedNPCs.Count} NPCs into CrowdMonitor.");
    }
}