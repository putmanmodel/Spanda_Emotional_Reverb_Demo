using UnityEngine;

public enum DecisionState { Neutral, Cry, Fight, Collapse }

public class DecisionEngine : MonoBehaviour
{
    public float cryThreshold = 0.25f;
    public float fightThreshold = 0.45f;
    public float collapseThreshold = 0.75f;

    public DecisionState currentState = DecisionState.Neutral;
    public float currentMagnitude = 0f;

    private Renderer npcRenderer;
    private Color neutralColor = Color.white;
    private bool rendererReady = false;

    void Awake()
    {
        npcRenderer = GetComponent<Renderer>();
        if (npcRenderer == null)
            npcRenderer = GetComponentInChildren<Renderer>();

        if (npcRenderer != null)
        {
            neutralColor = npcRenderer.material.color;
            rendererReady = true;
        }
    }

    public void Evaluate(float[] inputVector)
    {
        float magnitude = 0f;
        foreach (float val in inputVector)
            magnitude += Mathf.Abs(val);
        magnitude /= inputVector.Length;

        currentMagnitude = magnitude;

        if (magnitude >= collapseThreshold && currentState != DecisionState.Collapse)
            TriggerState(DecisionState.Collapse, magnitude);
        else if (magnitude >= fightThreshold && magnitude < collapseThreshold && currentState != DecisionState.Fight)
            TriggerState(DecisionState.Fight, magnitude);
        else if (magnitude >= cryThreshold && magnitude < fightThreshold && currentState != DecisionState.Cry)
            TriggerState(DecisionState.Cry, magnitude);
        else if (magnitude < cryThreshold && currentState != DecisionState.Neutral)
            TriggerState(DecisionState.Neutral, magnitude);
    }

    public void TriggerState(DecisionState newState, float intensity)
    {
        currentState = newState;

        Debug.Log($"💥 Decision Triggered: DecisionEngine enters {newState} state at intensity {intensity:F2}");
        Debug.Log($"🧠 DecisionEngine magnitude: {intensity:F2} | State: {newState}");

        if (rendererReady)
        {
            switch (newState)
            {
                case DecisionState.Cry: npcRenderer.material.color = Color.blue; break;
                case DecisionState.Fight: npcRenderer.material.color = Color.red; break;
                case DecisionState.Collapse: npcRenderer.material.color = Color.gray; break;
                case DecisionState.Neutral: npcRenderer.material.color = neutralColor; break;
            }
        }
    }
}