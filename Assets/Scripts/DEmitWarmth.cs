using UnityEngine;
using System.Collections;

public class DEmitWarmth : MonoBehaviour
{
    public float[] warmthPulse = new float[10]
    {
        1.2f, // 0 - Love/Hate (strong)
        0.9f, // 1 - background
        0.9f, // 2 - background
        1.2f, // 3 - Courage/Fear (strong)
        0.9f, // 4 - background
        0.9f, // 5 - background
        1.2f, // 6 - Attraction/Repulsion (strong)
        0.9f, // 7 - background
        1.2f, // 8 - Peace/Chaos (strong)
        1.2f  // 9 - Gratitude/Resentment (strong)
    };

    public float pulseRadius = 2f;      // focused range
    public float pulseInterval = 0.5f;  // fast pulse for quick healing

    private float lastPulseTime;

    void Start()
    {
        InvokeRepeating(nameof(EmitAreaWarmth), 1f, pulseInterval);
    }

    public void EmitWarmth(GameObject target)
    {
        if (Time.time - lastPulseTime < pulseInterval) return;

        var targetNPC = target.GetComponent<SpandaNPC>();
        if (targetNPC != null)
        {
            Debug.Log($"💗 NPC_D emits warmth to {target.name}");
            targetNPC.ReceivePulse(warmthPulse, this.gameObject);
            lastPulseTime = Time.time;
        }
    }

    private void EmitAreaWarmth()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pulseRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            var npc = hit.GetComponent<SpandaNPC>();
            if (npc != null)
            {
                npc.ReceivePulse(warmthPulse, this.gameObject);
                Debug.Log($"🌞 NPC_D radiates warmth to {npc.name}");
            }
        }
    }
}