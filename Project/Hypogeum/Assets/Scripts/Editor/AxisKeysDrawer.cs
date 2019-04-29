using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AxisKeys))]
public class AxisKeysDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        //ensure override works on entire property
        EditorGUI.BeginProperty(position, label, property);

        //don't indent
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        //label
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        //set position rects
        Rect posLabel = new Rect(position.x + 85, position.y, 15, position.height);
        Rect posField = new Rect(position.x + 115, position.y, 50, position.height);
        Rect negLabel = new Rect(position.x + 180, position.y, 15, position.height);
        Rect negField = new Rect(position.x + 210, position.y, 50, position.height);

        //set labels
        GUIContent posGUI = new GUIContent("+");
        GUIContent negGUI = new GUIContent("-");

        //draw fields
        EditorGUI.LabelField(posLabel, posGUI);
        EditorGUI.PropertyField(posField, property.FindPropertyRelative("positive"), GUIContent.none);
        EditorGUI.LabelField(negLabel, negGUI);
        EditorGUI.PropertyField(negField, property.FindPropertyRelative("negative"), GUIContent.none);

        //reset
        EditorGUI.indentLevel = indent;

        //end property
    }
}
