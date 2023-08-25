using Navigation.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Navigation.Editor
{
    [CustomEditor(typeof(NavigationMap))]
    public class TacticMapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Create Nav Data"))
            {
                var map = serializedObject.targetObject.GetComponent<NavigationMap>();
                
                map.GenerateNavData();
            }
            
            serializedObject.Update();
        }
    }
}