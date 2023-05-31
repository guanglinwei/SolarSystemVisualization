using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetInitializer))]
public class PlanetInitializerEditor : Editor
{
    private PlanetInitializer planetInitializer;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        planetInitializer = (PlanetInitializer) target;

        if (GUILayout.Button("Create Planets"))
        {
            planetInitializer.InitPlanets();

            foreach (GravityObject o in FindObjectsOfType<GravityObject>())
            {
                EditorUtility.SetDirty(o);
            }
        }
    }
}
