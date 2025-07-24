using UnityEngine;

public class SpandaMemory : MonoBehaviour
{
    public MemoryComponent memory;

    void Start()
    {
        if (memory == null)
            memory = GetComponent<MemoryComponent>();

        float[] delta = new float[10];
        for (int i = 0; i < delta.Length; i++)
            delta[i] = Random.Range(-1f, 1f);

        memory.LogReverbEntry("auto-dsl", delta, "startup pulse", new string[] { "spike" }, 1.0f, "init");
    }

    public void TryTriggerEcho(GameObject source)
    {
        Debug.Log($"TryTriggerEcho called by {source.name} — no behavior defined yet.");
    }
}