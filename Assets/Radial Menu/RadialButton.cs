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

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(HoverOver){
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * 1.1f, 8 * Time.deltaTime);    
            //sliceSprite.color = Color.Lerp(sliceSprite.color, hoverColor, 8 * Time.deltaTime);
        }
        else if (!HoverOver){
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, 8 * Time.deltaTime);    
            //sliceSprite.color = Color.Lerp(sliceSprite.color, normalColor, 8 * Time.deltaTime);
        }
    }
}
