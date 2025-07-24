using UnityEditor;
using UnityEngine;

public class GlowOrbSpawner : MonoBehaviour
{
    [MenuItem("Tools/Spawn GlowOrbs")]
    public static void SpawnGlowOrbs()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Select an NPC GameObject first.");
            return;
        }

        GameObject npc = Selection.activeGameObject;
        Transform parent = npc.transform;

        for (int i = 0; i < 10; i++)
        {
            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.name = $"GlowOrb_{i}";
            orb.transform.SetParent(parent);
            orb.transform.localPosition = new Vector3(0f, 2f + i * 0.3f, 0f);
            orb.transform.localRotation = Quaternion.identity;
            orb.transform.localScale = Vector3.one * 0.2f;

            var renderer = orb.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = Color.white; // or randomize for debug
        }

        Debug.Log("✨ 10 GlowOrbs spawned under selected GameObject.");
    }
}
