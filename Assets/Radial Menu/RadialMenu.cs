using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UI;

[ExecuteAlways]
public class RadialMenu : MonoBehaviour
{

    [Header("Radial Menu")]
    [Range(1, 25)] public int testNumOptions;
    //[Min(2)] public int resolution = 20; 
    [Min(0.05f)] public float thickness = 0.5f;
    public float radius = 1;
    
    [Header("MenuButtons")]
    public Material material;
    public Canvas canvas;
    public TextMeshProUGUI infoText;

    float[] angles;
    float strideStep;

    RadialMesh[] meshes;
    public List<RadialButton> Buttons;

    RaycastHit2D hit;
    Ray ray;



    private void Start() {
        CreateCanvas();
    }

    void Update()
    {
        Interaction();
    }



    void Interaction(){

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);

        foreach (RadialButton button in Buttons){   button.HoverOver = false;   }

        if(hit && hit.transform.GetComponent<RadialButton>()){

            RadialButton currentButton = hit.transform.GetComponent<RadialButton>();
            currentButton.HoverOver = true;

            infoText.text = currentButton.ButtonInfo;

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                currentButton.clickEvent.Invoke();
                CloseWheel();
            }
            if(Input.GetKeyDown(KeyCode.Mouse1)){
                currentButton.altClickEvent.Invoke();
                CloseWheel();
            }
        
        }

    }



    public void OpenWheel(int numOptions, List<string> infoTexts, List<UnityAction> actions){
        List<UnityAction> altActions = new List<UnityAction>(){ };
        CreateMeshes(numOptions, infoTexts, actions, altActions);
    }
    public void OpenWheel(int numOptions, List<string> infoTexts, List<UnityAction> actions, List<UnityAction> altActions){
        CreateMeshes(numOptions, infoTexts, actions, altActions);
    }

    public void CloseWheel(){
        //ClearMenu();
    }





    void CreateMeshes(int numOptions, List<string> infoTexts, List<UnityAction> actions, List<UnityAction> altActions){

        ClearMenu();
        CreateAngles(numOptions);

        meshes = new RadialMesh[numOptions];

        for (int i = 0; i < numOptions; i++){
            GameObject radialButtonObj = new GameObject();
            radialButtonObj.transform.name = "Button: " + (i+1).ToString();
            radialButtonObj.transform.SetParent(this.transform);

            RadialButton radialButton = radialButtonObj.AddComponent<RadialButton>();
            RadialMesh radialMesh = radialButtonObj.AddComponent<RadialMesh>();

            radialMesh.angleStride = strideStep;
            radialMesh.angle = angles[i];
            radialMesh.resolution = 100/numOptions;
            radialMesh.thickness = thickness;
            radialMesh.radius = radius;
            radialMesh.material = material;

            // radialButton.clickEvent = new Button.ButtonClickedEvent();
            // radialButton.altClickEvent = new Button.ButtonClickedEvent();

            // radialButton.clickEvent.RemoveAllListeners();
            // radialButton.altClickEvent.RemoveAllListeners();

            // radialButton.clickEvent.AddListener(new UnityAction( () => Debug.Log(this.name + " has been left-clicked") ));
            // radialButton.altClickEvent.AddListener(new UnityAction( () => Debug.Log(this.name + " has been right-clicked") ));
           
            // Set info text
            if(i < infoTexts.Count){    radialButton.ButtonInfo = infoTexts[i]; }
            else{   radialButton.ButtonInfo = radialButton.name;    }
            
            

            Buttons.Add(radialButton);
            meshes[i] = radialMesh;
        }
    }


    void CreateAngles(int numOptions){
        strideStep = 360f / numOptions;
        angles = new float[numOptions];

        for (int i = 0; i < numOptions; i++){
            angles[i] = (-strideStep * i) + 90f;
        }

    } 

    void ClearMenu(){
        // if(meshes != null){ foreach (RadialMesh mesh in meshes){    EditorApplication.delayCall += () => { DestroyImmediate(mesh.gameObject); };    }   }

        foreach (RadialMesh mesh in this.transform.GetComponentsInChildren<RadialMesh>()){
            if(mesh != this.transform){
                EditorApplication.delayCall += () => { DestroyImmediate(mesh.gameObject); };
            }
        }
        

        Buttons.Clear();
        
    }

    void CreateCanvas(){
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.transform.localPosition = Vector3.zero;
    }

}
