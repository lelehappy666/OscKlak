using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AudioStackUI : MonoBehaviour
{

    public bool isMove=false;
    private void Awake()
    {
        AddTriggerLister(gameObject,EventTriggerType.PointerDown,Test);
        AddTriggerLister(gameObject,EventTriggerType.PointerUp,dsa);    
    }

    private void Update()
    {
        print(Camera.main.ScreenToWorldPoint(Input.mousePosition).x/10);
        if(isMove)
        {
            print(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            GetComponent<RectTransform>().pivot=new Vector2((-Camera.main.ScreenToWorldPoint(Input.mousePosition).x/10), GetComponent<RectTransform>().pivot.y);
        }
    }
    public void Test(BaseEventData data)
    {
        print("按下");
        isMove=!isMove;
    }
    public void dsa(BaseEventData data)
    {
        print("songkai1");
         isMove=!isMove;
    }

    /// <summary>
    /// EventTrigger监听事件
    /// </summary>
    /// <param name="obj">绑定EvertTrigger物体</param>
    /// <param name="eventTriggerType">EventTrigger事件类型</param>
    /// <param name="action">绑定事件</param>
    public void AddTriggerLister(GameObject obj,EventTriggerType eventTriggerType,UnityAction<BaseEventData> action)
    {
        EventTrigger eventTrigger=obj.GetComponent<EventTrigger>();
        if(eventTrigger==null)
        {
            eventTrigger=obj.AddComponent<EventTrigger>();
        }
        if(eventTrigger.triggers.Count==0)
        {
            eventTrigger.triggers=new List<EventTrigger.Entry>();
        }

        UnityAction<BaseEventData> callBack=new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry=new EventTrigger.Entry();
        entry.eventID=eventTriggerType;
        entry.callback.AddListener(callBack);
        eventTrigger.triggers.Add(entry);
    }

    
}
