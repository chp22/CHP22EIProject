#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NeoclassicalMuseumBuilder))]
public class NeoclassicalMuseumBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NeoclassicalMuseumBuilder builder = (NeoclassicalMuseumBuilder)target;
        if (GUILayout.Button("Generate Museum"))
        {
            builder.GenerateMuseum();
        }
    }
}
#endif
