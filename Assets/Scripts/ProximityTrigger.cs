using UnityEngine;

[RequireComponent(typeof(SpandaNPC))]
public class ProximityTrigger : MonoBehaviour
{
    private SpandaNPC selfNPC;
    private MemoryComponent selfMemory;

    private void Awake()
    {
        selfNPC = GetComponent<SpandaNPC>();
        selfMemory = GetComponent<MemoryComponent>();

        if (selfMemory == null)
            Debug.LogError($"{name} missing MemoryComponent.");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger] {gameObject.name} was approached by {other.name}");

        SpandaNPC self = GetComponent<SpandaNPC>();
        SpandaNPC otherNPC = other.GetComponent<SpandaNPC>();

        if (self == null || otherNPC == null) return;

        // Self receives pulse from other
        self.ReceivePulse(otherNPC.GetEmotionalVector(), other.gameObject);

        // Check if *self* has reactive behavior
        CReactiveFlee selfFlee = GetComponent<CReactiveFlee>();
        if (selfFlee != null)
            selfFlee.EvaluatePulse(otherNPC.GetEmotionalVector(), other.gameObject);

        // OTHER receives pulse from self
        otherNPC.ReceivePulse(self.GetEmotionalVector(), gameObject);

        // Check if *other* has reactive behavior
        CReactiveFlee otherFlee = other.GetComponent<CReactiveFlee>();
        if (otherFlee != null)
            otherFlee.EvaluatePulse(self.GetEmotionalVector(), gameObject);
    }
}