using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioStackUIAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    public Text audioStackTimelineSuperscript;

    public Image audioStackTimelineChildImage;
    void Start()
    {
        audioStackTimelineSuperscript=GetComponentInChildren<Text>();
        audioStackTimelineChildImage=GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
