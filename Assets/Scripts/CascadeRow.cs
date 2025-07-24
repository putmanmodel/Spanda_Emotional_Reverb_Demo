using UnityEngine;

[System.Serializable]
public class CascadeRow
{
    public float[] values;

    public CascadeRow(int size)
    {
        values = new float[size];
        for (int i = 0; i < size; i++)
            values[i] = 0f;
    }

    public CascadeRow(float[] values, string[] tags, string source, float importance)
    {
        this.values = values;
        // You can store or ignore the other fields depending on your needs
        // Example:
        // this.tags = tags;
        // this.source = source;
        // this.importance = importance;
    }
}