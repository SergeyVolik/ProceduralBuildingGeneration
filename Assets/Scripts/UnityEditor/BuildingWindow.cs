using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UnityEditor
{
    class BuildingWindow : EditorWindow
    {

        [MenuItem("Window/BuildingCreator")]
        public static void ShowWindow()
        {
            GetWindow<BuildingWindow>("BuildingCreator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Building Settings", EditorStyles.boldLabel);

            if (GUILayout.Button("Create"))
            {
                
            }
        }
    }
}
