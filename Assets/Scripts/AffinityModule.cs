using UnityEngine;

public class AffinityModule : MonoBehaviour
{
    /// <summary>
    /// Calculates emotional affinity between two tone vectors.
    /// Returns a value between -1 (opposed) and +1 (identical).
    /// </summary>
    public float CalculateAffinity(float[] vectorA, float[] vectorB)
    {
        if (vectorA == null || vectorB == null || vectorA.Length != vectorB.Length)
            return 0f;

        float dot = 0f;
        float magA = 0f;
        float magB = 0f;

        for (int i = 0; i < vectorA.Length; i++)
        {
            dot += vectorA[i] * vectorB[i];
            magA += vectorA[i] * vectorA[i];
            magB += vectorB[i] * vectorB[i];
        }

        if (magA == 0 || magB == 0)
            return 0f;

        return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB));
    }

    /// <summary>
    /// Optional: amplify incoming delta by affinity multiplier.
    /// For use in propagation logic.
    /// </summary>
    public float[] ApplyAffinityInfluence(float[] delta, float affinityMultiplier)
    {
        float[] adjusted = new float[delta.Length];
        for (int i = 0; i < delta.Length; i++)
        {
            adjusted[i] = delta[i] * affinityMultiplier;
        }
        return adjusted;
    }
} 
