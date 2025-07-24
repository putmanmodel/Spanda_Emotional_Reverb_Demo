using UnityEngine;
using System.Collections;

public class SceneFreezeManager : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 5f;

    private void Awake()
    {
        Time.timeScale = 0f;
        Debug.Log("⏸️ Scene frozen...");
        StartCoroutine(UnfreezeAfterDelay());
    }

    private IEnumerator UnfreezeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1f;
        Debug.Log("▶️ Scene unfrozen after delay.");
    }
}