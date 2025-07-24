using TMPro;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpandaNPC))]
public class FloatingNameLabel : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public float fontSize = 10f;

    private Camera mainCam;
    private GameObject labelObj;
    private TextMeshPro textMesh;
    private SpandaNPC npc;

    void Start()
    {
        if (!Application.isPlaying) return;
        StartCoroutine(DelayedLabelSetup());
    }

    IEnumerator DelayedLabelSetup()
    {
        yield return null;
        mainCam = Camera.main;
        npc = GetComponent<SpandaNPC>();

        CleanupExistingLabels();
        CreateLabel();
    }

    void CleanupExistingLabels()
    {
        var existing = transform.Find("NameLabel");
        if (existing != null)
        {
            Destroy(existing.gameObject);
        }
    }

    void CreateLabel()
    {
        labelObj = new GameObject("NameLabel");
        labelObj.transform.SetParent(transform, false);
        labelObj.transform.localPosition = offset;

        // ✅ Add the missing TextMeshPro component
        textMesh = labelObj.AddComponent<TextMeshPro>();

        textMesh.text = $"<b>{gameObject.name}</b>";
        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.enableCulling = false;
        textMesh.richText = true;

        // Bright text color
        textMesh.color = Color.red;

        // Outline
        textMesh.outlineColor = Color.black;
        textMesh.outlineWidth = 0.5f;

        // Soft shadow for depth
        textMesh.enableWordWrapping = false;
        textMesh.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
        textMesh.fontSharedMaterial.SetFloat("_UnderlaySoftness", 0.3f);
        textMesh.fontSharedMaterial.SetFloat("_UnderlayDistance", 0.25f);
        textMesh.fontSharedMaterial.SetColor("_UnderlayColor", Color.black);
    }

    void Update()
    {
        if (labelObj == null || mainCam == null) return;
        labelObj.transform.rotation = Quaternion.LookRotation(labelObj.transform.position - mainCam.transform.position);
    }
}