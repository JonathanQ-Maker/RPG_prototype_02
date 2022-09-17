using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssetManager))]
public class AssetManagerEditor : Editor
{
    AssetManager manager;
    SerializedObject managerObject;
    bool prefabCollapsed, materialCollapsed;

    private void OnEnable()
    {
        manager = (AssetManager)target;
        managerObject = serializedObject;
    }
    public override void OnInspectorGUI()
    {
        SerializedProperty prefabs = managerObject.FindProperty("prefabs");

        prefabCollapsed = EditorGUILayout.Foldout(prefabCollapsed, "Prefabs");
        if (prefabCollapsed)
        {
            for (int i = 0; i < prefabs.arraySize; i++)
            {
                SerializedProperty prefab = prefabs.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                manager.types[i] = (PrefabType)EditorGUILayout.EnumPopup(manager.types[i]);
                prefab.objectReferenceValue = EditorGUILayout.ObjectField(prefab.objectReferenceValue, typeof(GameObject), true);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Remove"))
                {
                    manager.types.RemoveAt(i);
                    manager.prefabs.RemoveAt(i);
                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (manager.prefabs.Count < Enum.GetNames(typeof(PrefabType)).Length && GUILayout.Button("New Prefab"))
            {
                manager.types.Add(PrefabType.Null);
                manager.prefabs.Add(null);
            }
        }

        SerializedProperty materials = managerObject.FindProperty("materials");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        materialCollapsed = EditorGUILayout.Foldout(materialCollapsed, "Materials");
        if (materialCollapsed)
        {
            for (int i = 0; i < materials.arraySize; i++)
            {
                SerializedProperty material = materials.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                manager.materialTypes[i] = (MaterialType)EditorGUILayout.EnumPopup(manager.materialTypes[i]);
                material.objectReferenceValue = EditorGUILayout.ObjectField(material.objectReferenceValue, typeof(Material), true);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Remove"))
                {
                    manager.materialTypes.RemoveAt(i);
                    manager.materials.RemoveAt(i);
                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (manager.materials.Count < Enum.GetNames(typeof(MaterialType)).Length && GUILayout.Button("New Material"))
            {
                manager.materialTypes.Add(MaterialType.Null);
                manager.materials.Add(null);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
