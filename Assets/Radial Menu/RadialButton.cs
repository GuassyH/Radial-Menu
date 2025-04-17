using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour
{

    public bool HoverOver;
    public string ButtonInfo;
    public Button.ButtonClickedEvent clickEvent;
    public Button.ButtonClickedEvent altClickEvent;

    Material material;

    public Color baseColor;
    public Color highlightColor;

    RadialMesh thisMesh;
    
    // Start is called before the first frame update
    void Awake()
    {
        // thisMesh = this.GetComponent<RadialMesh>();
        // material = thisMesh.material;

        this.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        material = this.GetComponent<MeshRenderer>().material;
        material.color = material.color;

        if(HoverOver){
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * 1.1f, 8 * Time.deltaTime);    
            material.color = Color.Lerp(material.color, highlightColor, 8 * Time.deltaTime);
        }
        else if (!HoverOver){
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, 8 * Time.deltaTime);    
            material.color = Color.Lerp(material.color, baseColor, 8 * Time.deltaTime);
        }
    }
}
