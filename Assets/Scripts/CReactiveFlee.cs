using UnityEngine;

[RequireComponent(typeof(SpandaNPC))]
public class CReactiveFlee : MonoBehaviour
{
    public Transform safeTarget; // Assign NPC_D in inspector
    public float escapeSpeed = 2.5f;
    public float fleeThreshold = 0.4f;

    private SpandaNPC npc;
    private MemoryComponent memory;
    private bool isFleeing = false;

    void Start()
    {
        npc = GetComponent<SpandaNPC>();
        memory = GetComponent<MemoryComponent>();
    }

    void Update()
    {
        if (isFleeing && safeTarget != null)
        {
            Vector3 direction = (safeTarget.position - transform.position).normalized;
            transform.position += direction * escapeSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, safeTarget.position) < 1.0f)
            {
                Debug.Log("[Flee] NPC_C reached safe zone (NPC_D)");
                isFleeing = false;
                memory.RememberTag("safe");
            }
        }
    }

    public void EvaluatePulse(float[] sourceVector, GameObject sourceNPC)
    {
        if (isFleeing) return;

        float deltaMagnitude = 0f;
        foreach (float val in sourceVector)
            deltaMagnitude += Mathf.Abs(val);

        if (deltaMagnitude > fleeThreshold * sourceVector.Length)
        {
            Debug.Log($"[Flee Triggered] NPC_C overwhelmed by {sourceNPC.name} | Δ={deltaMagnitude:F2}");
            memory.RememberTag("distressed");
            isFleeing = true;

            // 💗 Trigger warmth from NPC_D
            GameObject safeHaven = GameObject.Find("NPC_D"); // or assign directly
            if (safeHaven != null)
            {
                DEmitWarmth warmth = safeHaven.GetComponent<DEmitWarmth>();
                if (warmth != null)
                    warmth.EmitWarmth(gameObject); // NPC_C is the one needing comfort
            }
        }
    }
}