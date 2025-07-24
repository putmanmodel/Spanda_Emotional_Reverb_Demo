using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public SpandaNPC NPC_B, NPC_A, NPC_C, NPC_D, NPC_E, NPC_F;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("🎬 Starting scripted emotional cascade demo...");
            StartCoroutine(RunScriptedCascade());
        }
    }

    System.Collections.IEnumerator RunScriptedCascade()
    {
        // B initiates emotional pulse
        NPC_B.ReceiveSpike("Director", GenerateSpike(1.0f), "Scripted B Spike", 1.0f);
        yield return new WaitForSeconds(1.5f);

        // A reacts to B
        NPC_A.ReceiveSpike("B", GenerateSpike(0.8f), "Scripted A Trigger", 1.0f);
        yield return new WaitForSeconds(1.5f);

        // C reacts to A
        NPC_C.ReceiveSpike("A", GenerateSpike(0.7f), "Scripted C Reaction", 1.0f);
        yield return new WaitForSeconds(1.5f);

        // D reacts to C
        NPC_D.ReceiveSpike("C", GenerateSpike(0.6f), "Scripted D Spillover", 1.0f);
        yield return new WaitForSeconds(1.5f);

        // E reacts to D
        NPC_E.ReceiveSpike("D", GenerateSpike(0.6f), "Scripted E Echo", 1.0f);
        yield return new WaitForSeconds(1.5f);

        // F reacts to E
        NPC_F.ReceiveSpike("E", GenerateSpike(0.9f), "Scripted F Intervention", 1.0f);
        yield return null;
    }

    private float[] GenerateSpike(float strength)
    {
        float[] spike = new float[10];
        for (int i = 0; i < spike.Length; i++)
        {
            spike[i] = Random.Range(-1f, 1f) * strength;
        }
        return spike;
    }
}