using System;
using UnityEngine;

[Serializable]
public class EmotionalReverbEntry
{
    public int id;
    public float timestamp;            // Unity time
    public string dsl;                 // e.g., "V,A,$,A,$"
    public float salience;            // importance of the beat
    public float[] deviation;         // delta from baseline (current - baseline)
    public string impression;         // 2–5 word symbolic label
    public string[] tags;             // flexible anchors (e.g., "confession", "anger")
}