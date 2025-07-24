using UnityEngine;

public class PersonalBubbleModule : MonoBehaviour
{
    [Header("Bubble Settings")]
    public float bubbleRadius = 2.5f;

    [Range(-1f, 1f)]
    public float receptivityIndex = 0f; // -1 = Rebel, 0 = Ignore, 1 = Mimic

    [Header("Modes")]
    public bool fightOrFlightMode = true;
    public bool socialButterflyMode = false;

    [Header("Pulse Gate Thresholds")]
    public float pulseReactionThreshold = 0.6f; // replaces fightThreshold
    public float pulseEscapeThreshold = 0.4f;   // replaces fleeThreshold

    [Header("Pulse Behavior")]
    [Range(0f, 1f)]
    public float pulseResistance = 0.2f;
    [Range(0f, 1f)]
    public float pulseReinforcementFactor = 0.3f;

    [Header("Cooldown (seconds)")]
    public float reactionCooldown = 1.0f;
    private float lastReactionTime = -99f;

    public bool CanReact()
    {
        return Time.time - lastReactionTime >= reactionCooldown;
    }

    public void MarkReacted()
    {
        lastReactionTime = Time.time;
    }
}