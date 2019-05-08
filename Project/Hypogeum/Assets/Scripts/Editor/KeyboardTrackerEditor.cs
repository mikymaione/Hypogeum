using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardTracker))]
public class KeyboardTrackerEditor : Editor
{
    public override void OnInspectorGUI() {

        KeyboardTracker kt = target as KeyboardTracker;
        EditorGUILayout.LabelField("Axis", EditorStyles.boldLabel);
        
        if (kt.axisKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No axis defined in InputManager", MessageType.Info);
        } else
	    {
            SerializedProperty prop = serializedObject.FindProperty("axisKeys");
            for (int i = 0; i < kt.axisKeys.Length; i++)
			{
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Axis " + i));
			}
	    }

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        if (kt.buttonKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No buttons defined in InputManager", MessageType.Info);
        } else
	    {
            for (int i = 0; i < kt.buttonKeys.Length; i++)
			{
                kt.buttonKeys[i] = (KeyCode)EditorGUILayout.EnumPopup("Button " + i, kt.buttonKeys[i]);
			}
	    }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
