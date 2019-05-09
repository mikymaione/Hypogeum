/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardTracker))]
public class KeyboardTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var kt = target as KeyboardTracker;
        EditorGUILayout.LabelField("Axis", EditorStyles.boldLabel);

        if (kt.axisKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No axis defined in InputManager", MessageType.Info);
        }
        else
        {
            var prop = serializedObject.FindProperty("axisKeys");

            for (var i = 0; i < kt.axisKeys.Length; i++)
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Axis " + i));
        }

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        if (kt.buttonKeys.Length == 0)
        {
            EditorGUILayout.HelpBox("No buttons defined in InputManager", MessageType.Info);
        }
        else
        {
            for (var i = 0; i < kt.buttonKeys.Length; i++)
                kt.buttonKeys[i] = (KeyCode)EditorGUILayout.EnumPopup("Button " + i, kt.buttonKeys[i]);
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

}