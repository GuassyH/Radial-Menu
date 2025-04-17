using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestScriptForOpening : MonoBehaviour
{

    public RadialMenu radialMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q) && !radialMenu.WheelOpen){
            List<string> infoTexts = new List<string>(){ "One", "Two", "Three", "Four" };
            List<UnityAction> actions = new List<UnityAction>(){ () => Debug.Log("Press") };
            
            radialMenu.OpenWheel(4, infoTexts, actions);
        }
        else if(!Input.GetKey(KeyCode.Q) && radialMenu.WheelOpen){
            radialMenu.CloseWheel();
        }
    }
}
