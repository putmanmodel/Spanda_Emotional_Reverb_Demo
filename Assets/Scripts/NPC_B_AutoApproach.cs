using UnityEngine;

public class NPC_B_AutoApproach : MonoBehaviour
{
    public string targetName = "NPC_A";
    public float speed = 1.5f;
    public float triggerDistance = 2.0f;
    public bool hasTriggered = false;

    private Transform target;
    private MemoryComponent memory;

    void Start()
    {
        GameObject targetObj = GameObject.Find(targetName);
        if (targetObj != null)
            target = targetObj.transform;

        memory = GetComponent<MemoryComponent>();
    }

    void Update()
    {
        if (target == null || hasTriggered) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > triggerDistance)
        {
            // Move toward NPC_A
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // Close enough — trigger emotional pulse
            float[] delta = new float[10];
            delta[0] = -0.3f; // Love/Hate = spike toward hate
            delta[3] = -0.4f; // Courage/Fear = fear spike

            memory.SetLastSource(gameObject);
            memory.LogReverbEntry("approached-too-close", delta, "Unwanted proximity", new string[] { "boundary", "threat" }, 0.9f, "flinch");

            hasTriggered = true;
        }
    }
}