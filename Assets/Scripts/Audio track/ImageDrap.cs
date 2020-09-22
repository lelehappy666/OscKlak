using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ImageDrap : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    public bool isDrag=false;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖拽");
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        rectTransform.position = new Vector3(pos.x,rectTransform.position.y,0);
        isDrag=true;
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束拖拽");
    }
 
}
 
