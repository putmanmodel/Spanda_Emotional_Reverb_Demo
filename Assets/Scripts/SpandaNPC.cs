using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MemoryComponent))]
[RequireComponent(typeof(AffinityModule))]
[RequireComponent(typeof(PersonalBubbleModule))]
public class SpandaNPC : MonoBehaviour
{
    public MemoryComponent memory;
    private AffinityModule affinityModule;

    [Header("Base Body Color")]
    public Color baseColor = Color.white;
    public Renderer bodyRenderer;

    [Header("Glow Orb Settings")]
    public GameObject glowOrbPrefab;
    public Transform glowOrbAnchor;
    private GlowOrbController[] glowOrbControllers;
    private bool isJiggling = false;

    [Header("Axis Labels")]
    public string[] axisZones = new string[10]
    {
        "Love/Hate", "Harmony/Conflict", "Joy/Despair", "Courage/Fear", "Trust/Suspicion",
        "Loyalty/Betrayal", "Attraction/Repulsion", "Intimacy/Distance", "Peace/Chaos", "Gratitude/Resentment"
    };

    private Vector3? targetPosition = null;
    public float moveSpeed = 1.5f;
    public float distressThreshold = 3.5f;
    public Transform safeTarget;

    private Vector3 originalScale;

    private void OnEnable()
    {
        CleanupExistingLabels();
        CreateFloatingLabel();
    }

    private void CleanupExistingLabels()
    {
        Transform existing = transform.Find("NameLabel");
        if (existing != null)
            DestroyImmediate(existing.gameObject);
    }

    private void CreateFloatingLabel()
    {
        GameObject labelObj = new GameObject("NameLabel");
        labelObj.transform.SetParent(transform, false);
        labelObj.transform.localPosition = new Vector3(0, 1.5f, 0);

        var textMesh = labelObj.AddComponent<TMPro.TextMeshPro>();
        textMesh.text = gameObject.name;
        textMesh.fontSize = 3f;
        textMesh.color = baseColor;
        textMesh.outlineColor = Color.black;
        textMesh.outlineWidth = 0.25f;
        textMesh.alignment = TMPro.TextAlignmentOptions.Center;
    }

    void Start()
    {
        memory = GetComponent<MemoryComponent>();
        affinityModule = GetComponent<AffinityModule>();
        originalScale = transform.localScale;

        switch (gameObject.name)
        {
            case "NPC_A": baseColor = new Color(1f, 0.7f, 0.7f); break;
            case "NPC_B": baseColor = new Color(0.6f, 0.6f, 1f); break;
            case "NPC_C": baseColor = new Color(0.6f, 1f, 0.6f); break;
            case "NPC_D": baseColor = new Color(1f, 1f, 0.6f); break;
            case "NPC_E": baseColor = new Color(1f, 0.6f, 1f); break;
            case "NPC_F": baseColor = new Color(1f, 0.8f, 0.4f); break;
        }

        if (bodyRenderer != null)
            bodyRenderer.material.color = baseColor;

        if (memory.currentVector == null || memory.currentVector.Length != axisZones.Length)
            memory.currentVector = new float[axisZones.Length];

        if (gameObject.name == "NPC_A")
        {
            memory.currentVector = new float[] { -0.3f, 0.1f, -0.4f, 0.2f, 0f, -0.1f, 0.3f, -0.2f, 0.1f, -0.3f };
            memory.RememberTag("startled");
        }

        if (gameObject.name == "NPC_B")
        {
            memory.currentVector = new float[] { 0.6f, -0.4f, 0.2f, -0.3f, 0.1f, 0f, -0.2f, 0.2f, -0.1f, 0.5f };
            memory.RememberTag("bully");
            Invoke(nameof(SpikeNPC_A), 2f);
        }

        if (gameObject.name == "NPC_C")
        {
            memory.currentVector = new float[axisZones.Length];
            if (safeTarget != null)
            {
                Vector3 towardA = GameObject.Find("NPC_A").transform.position - transform.position;
                targetPosition = transform.position + towardA.normalized * 2.5f;
                Debug.Log("[Init] NPC_C entering scene toward NPC_A");
            }
        }

        if (gameObject.name == "NPC_D")
        {
            memory.currentVector = new float[]
            {
                0.9f, 0.8f, 0.9f, 0.7f, 0.8f, 0.8f, 0.7f, 0.8f, 0.9f, 0.8f
            };
            memory.RememberTag("safe");
            memory.RememberTag("protector");
            Debug.Log($"[NPC_D Vector Init] {string.Join(", ", memory.currentVector.Select(v => v.ToString("F2")))}");
        }

        if (glowOrbPrefab != null && glowOrbAnchor != null)
        {
            glowOrbControllers = new GlowOrbController[axisZones.Length];
            for (int i = 0; i < axisZones.Length; i++)
            {
                GameObject orbInstance = Instantiate(glowOrbPrefab, glowOrbAnchor);
                orbInstance.transform.localScale = Vector3.one * 0.2f;
                
                var orbRenderer = orbInstance.GetComponent<Renderer>();
                if (orbRenderer != null)
                    orbRenderer.material = new Material(orbRenderer.material);

                var controller = orbInstance.GetComponent<GlowOrbController>();
                if (controller != null)
                {
                    controller.orbitCenter = this.transform;
                    controller.axisIndex = i;
                    controller.zone = axisZones[i];
                    controller.startAngleDeg = i * (360f / axisZones.Length);
                    glowOrbControllers[i] = controller;
                }
            }
        }

        Invoke(nameof(UpdateGlowOrbs), 0.2f);
    }

    void SpikeNPC_A()
    {
        GameObject target = GameObject.Find("NPC_A");
        if (target != null)
        {
            float[] spike = new float[10] { -0.8f, -0.7f, -0.6f, -0.6f, -0.5f, -0.5f, -0.6f, -0.4f, -0.3f, -0.7f };
            var targetNPC = target.GetComponent<SpandaNPC>();
            targetNPC?.ReceivePulse(spike, this.gameObject);
            Debug.Log("💥 NPC_B spiked NPC_A");

            Collider[] hits = Physics.OverlapSphere(target.transform.position, 3f);
            foreach (var hit in hits)
            {
                if (hit.gameObject == target) continue;
                var nearby = hit.GetComponent<SpandaNPC>();
                if (nearby != null)
                {
                    nearby.ReceivePulse(targetNPC.GetEmotionalVector(), target);
                    Debug.Log($"🌊 NPC_A emitted pulse to {nearby.name}");
                }
            }
        }
    }

    void Update()
    {
        if (targetPosition.HasValue)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, step);
            if (Vector3.Distance(transform.position, targetPosition.Value) < 0.1f)
                targetPosition = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float[] delta = new float[axisZones.Length];
            string[] symbols = new string[axisZones.Length];

            for (int i = 0; i < axisZones.Length; i++)
            {
                delta[i] = Random.Range(-1f, 1f);
                symbols[i] = delta[i] > 0.5f ? "A" : delta[i] < -0.5f ? "V" : "$";
                memory.currentVector[i] = Mathf.Clamp(memory.currentVector[i] + delta[i], -1f, 1f);
            }

            memory.LogReverbEntry("manual", delta, "space test", symbols, 1f, "manual");
            UpdateGlowOrbs();
        }
    }
public void ReceivePulse(float[] sourceVector, GameObject sourceNPC)
{
    if (memory == null || affinityModule == null) return;
    memory.SetLastSource(sourceNPC);

    var bubble = GetComponent<PersonalBubbleModule>();
    if (bubble == null || !bubble.CanReact()) return;

    var myVector = memory.currentVector;
    float distance = Vector3.Distance(transform.position, sourceNPC.transform.position);
    if (distance > bubble.bubbleRadius) return;

    bubble.MarkReacted();

    // NPC_C healing from NPC_D
    if (name == "NPC_C" && sourceNPC.name == "NPC_D")
    {
        for (int i = 0; i < myVector.Length; i++)
        {
            float healDelta = (sourceVector[i] - myVector[i]) * 0.3f;
            myVector[i] = Mathf.Clamp(myVector[i] + healDelta, -1f, 1f);
        }

       // memory.LogReverbEntry(
       //   sourceNPC.name,
       //   sourceVector,
       //   "healed",
       //   Enumerable.Repeat("H", myVector.Length).ToArray(),
       //   1f,
       //   "warmth"
       // );

        StartCoroutine(JiggleEffect());
        UpdateGlowOrbs();
        return;
    }

    // NPC_C repel from A/B
    if (name == "NPC_C" && (sourceNPC.name == "NPC_A" || sourceNPC.name == "NPC_B"))
    {
        memory.RememberTag("distressed");
        if (safeTarget != null)
        {
            Vector3 dir = (safeTarget.position - transform.position).normalized;
            targetPosition = safeTarget.position - dir * 1.0f;

            var warmth = safeTarget.GetComponent<DEmitWarmth>();
            if (warmth != null)
                warmth.EmitWarmth(gameObject);
        }

        float[] deltaCopy = sourceVector.ToArray();
        string[] symbols = new string[deltaCopy.Length];
        for (int i = 0; i < deltaCopy.Length; i++)
            symbols[i] = deltaCopy[i] > 0.05f ? "A"
                           : deltaCopy[i] < -0.05f ? "V"
                           : "$";

        memory.LogReverbEntry(
            sourceNPC.name,
            deltaCopy,
            "pulse",
            symbols,
            1f,
            "bubble"
        );
    }

    // Generic emotional spill-over
    for (int i = 0; i < myVector.Length; i++)
    {
        float mod = sourceVector[i] * (1f - bubble.pulseResistance);
        mod *= bubble.receptivityIndex;
        if (bubble.pulseReinforcementFactor > 0f)
            mod += sourceVector[i] * bubble.pulseReinforcementFactor;

        myVector[i] = Mathf.Clamp(myVector[i] + mod, -1f, 1f);
    }

    StartCoroutine(JiggleEffect());
    UpdateGlowOrbs();
}

// —————— Continue with the rest of your methods unchanged ——————
private IEnumerator JiggleEffect()
{
    if (isJiggling) yield break;
    isJiggling = true;

    Vector3 originalPosition = transform.position;
    float jiggleAmount = 0.1f;
    float duration = 0.2f;
    float speed = 20f;

    float elapsed = 0f;
    while (elapsed < duration)
    {
        float offset = Mathf.Sin(elapsed * speed) * jiggleAmount;
        transform.position = originalPosition + new Vector3(0, offset, 0);
        elapsed += Time.deltaTime;
        yield return null;
    }

    transform.position = originalPosition;
    isJiggling = false;
}

private void UpdateGlowOrbs()
{
    Debug.Log($"[Orbs] Updating {glowOrbControllers.Length} orbs. First value: {memory.currentVector[0]:F2}");
    if (glowOrbControllers == null || memory == null || memory.currentVector == null) return;

    for (int i = 0; i < glowOrbControllers.Length; i++)
    {
        try
        {
            glowOrbControllers[i]?.SetValue(memory.currentVector[i]);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"GlowOrb {i} error: {ex.Message}");
        }
    }
}

public void ReceiveSpike(string sender, float[] delta, string label = "scripted", float importance = 1.0f)
{
    if (memory == null) return;

    string[] symbolic = new string[delta.Length];
    for (int i = 0; i < delta.Length; i++)
        symbolic[i] = delta[i] > 0.05f ? "A" : delta[i] < -0.05f ? "V" : "$";

    memory.LogReverbEntry(sender, delta, label, symbolic, importance, "demo");
    Debug.Log($"[Cascade Spike] {name} received spike from {sender} | Δ={delta.Sum(Mathf.Abs):F2} | {string.Join(",", symbolic)}");
}

public float[] GetEmotionalVector()
{
    return memory?.currentVector ?? new float[axisZones.Length];
}
}