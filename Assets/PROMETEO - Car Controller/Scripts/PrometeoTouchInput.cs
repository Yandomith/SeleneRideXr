using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrometeoTouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool changeScaleOnPressed = false;
    [HideInInspector]
    public bool buttonPressed = false;
    RectTransform rectTransform;
    Vector3 initialScale;
    float scaleDownMultiplier = 0.85f;

    void Start(){
      rectTransform = GetComponent<RectTransform>();
      initialScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData){
      ButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData){
      ButtonUp();
    }

    public void ButtonDown(){
      buttonPressed = true;
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale * scaleDownMultiplier;
      }
    }

    public void ButtonUp(){
      buttonPressed = false;
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale;
      }
    }

}
