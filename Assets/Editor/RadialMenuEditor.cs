using UnityEngine.Events;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RadialMenu))]
public class RadialMenuEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        RadialMenu radialMenu = (RadialMenu)target;

        List<UnityAction> actions = new List<UnityAction>(){ () => Debug.Log("Main Action for Button 1"),  () => Debug.Log("Main Action for Button 2") };
        List<string> info = new List<string>(){ "this is the first", "this is the second" };

        if(GUILayout.Button("Create Wheel")){
            radialMenu.OpenWheel(radialMenu.testNumOptions, info, actions);
        }    
    }
    
}