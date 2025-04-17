using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
public class RadialMenu : MonoBehaviour
{

    [Header("Radial Menu")]
    [Range(1, 25)] public int testNumOptions;
    [Min(0.05f)] public float thickness = 0.5f;
    public float radius = 1;
    
    [Header("MenuButtons")]
    public Material material;
    public Canvas canvas;
    public TextMeshProUGUI infoText;
    public Color baseColor;
    public Color highlightColor;

    float[] angles;
    float strideStep;

    RadialMesh[] meshes;
    public List<RadialButton> Buttons;

    RaycastHit2D hit;
    Ray ray;

    int resolution; 

    public bool WheelOpen;

    private void Start() {
        CreateCanvas();
        CloseWheel();
    }

    void Update()
    {
        Interaction();
    }



    void Interaction(){


        if(Buttons.Count > 0){
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);
            
            foreach (RadialButton button in Buttons){   button.HoverOver = false;   }

            if(hit && hit.transform.GetComponent<RadialButton>()){

                RadialButton currentButton = hit.transform.GetComponent<RadialButton>();
                currentButton.HoverOver = true;

                infoText.text = currentButton.ButtonInfo;

                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    currentButton.clickEvent.Invoke();
                }
                if(Input.GetKeyDown(KeyCode.Mouse1)){
                    currentButton.altClickEvent.Invoke();
                }
            
            }
        }

    }



    public void OpenWheel(int numOptions, List<string> infoTexts, List<UnityAction> actions){
        List<UnityAction> altActions = new List<UnityAction>(){ };
        CreateWheel(numOptions, infoTexts, actions, altActions);
        WheelOpen = true;
    }
    public void OpenWheel(int numOptions, List<string> infoTexts, List<UnityAction> actions, List<UnityAction> altActions){
        CreateWheel(numOptions, infoTexts, actions, altActions);
        WheelOpen = true;
    }



    void CreateWheel(int numOptions, List<string> infoTexts, List<UnityAction> actions, List<UnityAction> altActions){
        CreateMeshes(numOptions);
        AssignButtons(infoTexts, actions, altActions);
    }
    


    void CreateMeshes(int numOptions){

        ClearMenu();

        canvas.gameObject.SetActive(true);

        strideStep = 360f / numOptions;
        resolution = 100 / numOptions;
        angles = new float[numOptions];

        for (int i = 0; i < numOptions; i++){
            angles[i] = (-strideStep * i) + 90f;
        }

        meshes = new RadialMesh[numOptions];

        for (int i = 0; i < numOptions; i++){
            GameObject radialButtonObj = new GameObject();
            radialButtonObj.transform.name = "Button: " + (i+1).ToString();
            radialButtonObj.transform.SetParent(this.transform);

            RadialButton radialButton = radialButtonObj.AddComponent<RadialButton>();
            RadialMesh radialMesh = radialButtonObj.AddComponent<RadialMesh>();

            radialButton.baseColor = baseColor;
            radialButton.highlightColor = highlightColor;
            radialButton.radialMenu = this;

            radialMesh.angleStride = strideStep;
            radialMesh.angle = angles[i];
            radialMesh.resolution = resolution;
            radialMesh.thickness = thickness;
            radialMesh.radius = radius;
            radialMesh.material = material;
            radialMesh.color = baseColor;

            Buttons.Add(radialButton);
            radialMesh.CreateMesh();

            meshes[i] = radialMesh;

        }
    }



    void AssignButtons(List<string> infoTexts, List<UnityAction> actions, List<UnityAction> altActions){
        for (int i = 0; i < Buttons.Count; i++)
        { 
            Buttons[i].clickEvent = new Button.ButtonClickedEvent();
            Buttons[i].altClickEvent = new Button.ButtonClickedEvent();

            Buttons[i].clickEvent.AddListener( () => Debug.Log("Left Click on") );
            Buttons[i].altClickEvent.AddListener( () => Debug.Log("Right Click on") );

            if(i < infoTexts.Count){    Buttons[i].ButtonInfo = infoTexts[i]; }
            else{   Buttons[i].ButtonInfo = Buttons[i].name;    }
        }
    }





    void ClearMenu(){
        // if(meshes != null){ foreach (RadialMesh mesh in meshes){    EditorApplication.delayCall += () => { DestroyImmediate(mesh.gameObject); };    }   }

        foreach (RadialButton button in this.transform.GetComponentsInChildren<RadialButton>()){
            if(button != this.transform){
                EditorApplication.delayCall += () => { DestroyImmediate(button.gameObject); };
            }
        }
        
        Buttons.Clear();
        meshes = new RadialMesh[0];
    }



    public void CloseWheel(){
        // ClearMenu();
        WheelOpen = false;
        canvas.gameObject.SetActive(false);
    }




    void CreateCanvas(){
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.transform.localPosition = Vector3.zero;
    }

}
