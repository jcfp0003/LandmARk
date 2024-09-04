//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(TokenActionsControl))]
//[CanEditMultipleObjects]
//public class ListTokenActionsEditor : Editor
//{
//    private SerializedProperty TokenAvailActions;

//    void OnEnable()
//    {
//        TokenAvailActions = serializedObject.FindProperty("TokenAvailActions");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
        
//        serializedObject.ApplyModifiedProperties();
//    }
//}
