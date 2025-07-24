using UnityEngine;

public class CReactiveInit : MonoBehaviour
{
    void Start()
    {
        MemoryComponent memory = GetComponent<MemoryComponent>();
        if (memory == null) return;

        float[] cueVector = new float[10];
        for (int i = 0; i < cueVector.Length; i++)
            cueVector[i] = Random.Range(-0.5f, 0.5f);

        memory.LogReverbEntry("reactive", cueVector, "init", new string[] { "startup" }, 0.8f, "priming");
    }
}