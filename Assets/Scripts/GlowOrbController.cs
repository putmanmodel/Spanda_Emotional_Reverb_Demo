using UnityEngine;

public class GlowOrbController : MonoBehaviour
{
    public Transform orbitCenter;
    public int axisIndex;
    public string zone;
    public float orbitRadius = 0.5f;
    public float orbitSpeed = 20f;
    public float startAngleDeg = 0f;
    public bool enableDebug = false;

    private float angle;
    private float currentValue = 0f;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;
    private int colorID;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        colorID = Shader.PropertyToID("_Color");
        angle = startAngleDeg;
    }

    void Update()
    {
        if (orbitCenter == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;

        Vector3 flatOffset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * orbitRadius;
        float yOffset = currentValue * 0.15f;
        Vector3 offset = flatOffset + new Vector3(0f, yOffset, 0f);
        transform.position = orbitCenter.position + offset;
    }

    public void SetValue(float value)
    {
        currentValue = Mathf.Clamp(value, -1f, 1f);

        Color c = GetGradientColor(currentValue);
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor(colorID, c);
        rend.SetPropertyBlock(propBlock);

        float intensity = Mathf.Abs(currentValue);
        transform.localScale = Vector3.one * Mathf.Lerp(0.1f, 0.4f, intensity);

        if (enableDebug)
        {
            Debug.Log($"[Orb-{zone}] axis {axisIndex} value={currentValue:F2}, color={c}, scale={transform.localScale.x:F2}");
        }
    }

    Color GetGradientColor(float value)
{
    value = Mathf.Clamp(value, -1f, 1f);

    if (value < -0.5f)
        return Color.Lerp(Color.blue, new Color(0f, 0.5f, 1f), value + 1f); // Deep blue → Sky blue
    if (value < 0f)
        return Color.Lerp(new Color(0f, 0.5f, 1f), Color.green, value + 0.5f); // Sky blue → Green
    if (value < 0.5f)
        return Color.Lerp(Color.green, Color.yellow, value + 0.0f); // Green → Yellow
    return Color.Lerp(Color.yellow, Color.red, (value - 0.5f) * 2f); // Yellow → Red
}
}