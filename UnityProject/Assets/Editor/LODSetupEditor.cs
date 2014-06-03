using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class LevelScriptEditor : EditorWindow
{
    [MenuItem("Window/My Window")]
    static void ShowWindow () {
        EditorWindow.GetWindow (typeof(LevelScriptEditor));
    }

    private GameObject gameObject;

    private float[] lodHeight = {0.75f, 0.9f, 1.0f};

    void OnGUI () {
        GUILayout.Label ("Base Settings", EditorStyles.boldLabel);

        lodHeight[0] = EditorGUILayout.Slider("LOD0", lodHeight[0], 0, 1);
        lodHeight[0] = Mathf.Clamp(lodHeight[0], 0, lodHeight[1]);
        lodHeight[1] = EditorGUILayout.Slider("LOD1", lodHeight[1], 0, 1);
        lodHeight[1] = Mathf.Clamp(lodHeight[1], lodHeight[0], lodHeight[2]);
        lodHeight[2] = EditorGUILayout.Slider("LOD2", lodHeight[2], 0, 1);
        lodHeight[2] = Mathf.Clamp(lodHeight[2], lodHeight[1], 1);

        //gameObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", gameObject, typeof(GameObject));

        if (GUILayout.Button("Create LOD Groups"))
        {
            //Go through children and add appropiate LOD Groups
            //TryAddingLODGroupForTransform(gameObject.transform);


            foreach (Transform item in Selection.transforms)
            {
                TryAddingLODGroupForTransform(item);
            }
        }

    }

    void TryAddingLODGroupForTransform(Transform target)
    {
        if (target.GetComponent<LODGroup>())
        {
            DestroyImmediate(target.GetComponent<LODGroup>());
        }

        if (HasLODChildren(target))
        {
            //This guy has children with "LOD" in his name
            AddLODGroupForTransform(target);
        }
        TryAddingLODGroupForTransformsChildren(target);
    }

    void TryAddingLODGroupForTransformsChildren(Transform target)
    {
        if (target.childCount == 0)
            return;

        foreach (Transform item in target)
        {
            TryAddingLODGroupForTransform(item);
        }
    }

    void AddLODGroupForTransform(Transform target)
    {
        LODGroup lodGroup = target.gameObject.AddComponent<LODGroup>();
        LOD[] lods = LODChildCount(target);
        lodGroup.SetLODS(lods);
        lodGroup.RecalculateBounds();
    }

    LOD[] LODChildCount(Transform target)
    {
        Queue<Transform> LODChildren = new Queue<Transform>();
        foreach (Transform item in target)
        {
            if (item.name.Contains("LOD"))
            {
                LODChildren.Enqueue(item);
            }
        }
        LOD[] lods = new LOD[LODChildren.Count];

        for (int i = 0; i < lods.Length; i++)
        {
            Renderer[] renderers = new Renderer[1];
            renderers[0] = LODChildren.Dequeue().renderer;
            lods[i] = new LOD(1.0f-lodHeight[i], renderers);
            //1f / (i + 3)
        }

        return lods;
    }

    bool HasLODChildren(Transform target)
    {
        foreach (Transform item in target)
        {
            if (item.name.Contains("LOD"))
            {
                return true;
            }
        }
        return false;
    }
}
