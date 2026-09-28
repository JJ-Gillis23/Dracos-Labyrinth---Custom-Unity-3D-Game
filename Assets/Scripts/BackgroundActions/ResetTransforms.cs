using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class ResetTransforms : MonoBehaviour
{
    [ContextMenu("Round All Child Positions")]
    void RoundAllChildren()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            Vector3 p = child.localPosition;
            child.localPosition = new Vector3(
                Mathf.Round(p.x),
                Mathf.Round(p.y),
                Mathf.Round(p.z)
            );
        }
    }
}
#endif