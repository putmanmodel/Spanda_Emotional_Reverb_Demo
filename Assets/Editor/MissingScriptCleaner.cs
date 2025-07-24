using UnityEngine;
using UnityEditor;

public static class MissingScriptCleaner
{
    [MenuItem("Tools/Clean Missing Scripts from Selection")]
    public static void CleanSelection()
    {
        foreach (var go in Selection.gameObjects)
            RemoveMissingScripts(go);
        Debug.Log("Missing scripts cleaned.");
    }

    private static void RemoveMissingScripts(GameObject go)
    {
        // SerializedObject lets us poke into the GameObject's raw data
        var so = new SerializedObject(go);
        var comps = so.FindProperty("m_Component");
        int removedCount = 0;

        for (int i = comps.arraySize - 1; i >= 0; i--)
        {
            var element = comps.GetArrayElementAtIndex(i);
            // An objectReferenceInstanceIDValue of 0 means "no script"
            if (element.FindPropertyRelative("component").objectReferenceInstanceIDValue == 0)
            {
                comps.DeleteArrayElementAtIndex(i);
                removedCount++;
            }
        }

        if (removedCount > 0)
        {
            so.ApplyModifiedProperties();
            Debug.Log($"-- Removed {removedCount} missing scripts from {go.name}");
        }

        // Recurse into children
        foreach (Transform child in go.transform)
            RemoveMissingScripts(child.gameObject);
    }
}
