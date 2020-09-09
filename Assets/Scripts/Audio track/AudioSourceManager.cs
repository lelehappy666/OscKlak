using System.Security.Principal;
using System.Net.Mime;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class AudioSourceManager : MonoBehaviour
{
    // Start is called before the first frame update
    [FoldoutGroup("ListFiles")]
    [InfoBox("文件列表")]
    [Tooltip("音频储存路径")]
    public string path;

    [FoldoutGroup("ListFiles")]
    [Tooltip("音频数组")]
    public List<string> audioSources;

    [FoldoutGroup("ListFiles")]
    [Tooltip("音频数组实例化模板")]
    public Image examplePrefabImage;


    [FoldoutGroup("ListFiles")]
    [Tooltip("实例化模板父物体位置")]
    public Transform examplePrefabParent;


    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴间距控制器")]
    public GridLayoutGroup gridLayoutGroup;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴间隔数组")]
    public List<GameObject> audioStackTimes;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴间距UI模板")]
    public GameObject audioStackUI;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴数量")]
    public int audioStackCount;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴缩放比例")]
    public float audioStackSpaceX;

    void Start()
    {
        LoadAudioSources(path);
        AudioStackUiProduction();
    }

    // Update is called once per frame
    void Update()
    {
        AudioSourceStackIntervalChange();
        print(gridLayoutGroup.spacing.x+"------");
    }

    /// <summary>
    /// 初始加载指定路径音频文件
    /// </summary>
    /// <param name="path">路径</param>
    public void LoadAudioSources(string path)
    {
        path=Application.streamingAssetsPath;
        if(Directory.Exists(path))
        {
            DirectoryInfo directoryInfo=new DirectoryInfo(path);
            FileInfo[] fileInfos=directoryInfo.GetFiles("*",SearchOption.AllDirectories);
            for(int i=0;i<fileInfos.Length;i++)
            {
                if(fileInfos[i].Name.Split('.').Length<3)
                {
                    audioSources.Add(fileInfos[i].Name);
                    var examplesPrefabImages=Instantiate(examplePrefabImage);
                    examplesPrefabImages.transform.SetParent(examplePrefabParent);
                    examplesPrefabImages.name=fileInfos[i].Name;
                    examplesPrefabImages.GetComponentInChildren<Text>().text=fileInfos[i].Name;
                    //点击事件
                    examplesPrefabImages.GetComponentInChildren<Button>().onClick.AddListener(delegate{OnClickBtnExamplePrefabImages();});
                }
            }
        }
    }

    public void  AudioStackUiProduction()
    {
        for(int i=0;i<audioStackCount;i++)
        {
            //音轨UI间隔
            var auioStackUIs=Instantiate(audioStackUI);
            auioStackUIs.GetComponentInChildren<Text>().text=i.ToString();
            auioStackUIs.name=auioStackUIs.name+"_"+i;
            auioStackUIs.gameObject.transform.SetParent(gridLayoutGroup.transform);
            audioStackTimes.Add(auioStackUIs);
        }
    } 
    public void OnClickBtnExamplePrefabImages()
    {
        
    }

    public void AudioSourceStackIntervalChange()
    {
        switch  (gridLayoutGroup.spacing.x)
        {
            case 45:
                audioStackSpaceX=2.38f;
                for(int i=0;i<audioStackTimes.Count;i++)
                {
                    if(i%5!=0)
                    {
                        audioStackTimes[i].GetComponentInChildren<Text>().enabled=false;
                        audioStackTimes[i].GetComponentsInChildren<RectTransform>()[2].sizeDelta=new Vector2(1.537476f,12.227f);
                    }
                }
            break;

            case 21.19999f:
                audioStackSpaceX=1f;
                for(int i=0;i<audioStackTimes.Count;i++)
                {
                    if(i%10!=0)
                    {
                        audioStackTimes[i].GetComponentInChildren<Text>().enabled=false;
                        audioStackTimes[i].GetComponentsInChildren<RectTransform>()[2].sizeDelta=new Vector2(1.537476f,12.227f);
                    }
                }
            break;

            case 4.199989f:
                 audioStackSpaceX=0;
                 print(gridLayoutGroup.spacing.x+"------");
                for(int i=0;i<audioStackTimes.Count;i++)
                {
                    if(i%20!=0)
                    {
                        audioStackTimes[i].GetComponentInChildren<Text>().enabled=false;
                        audioStackTimes[i].GetComponentsInChildren<RectTransform>()[2].sizeDelta=new Vector2(1.537476f,6.11f);
                    }
                }
            break;
        }
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            if(Input.GetAxis("Mouse ScrollWheel")>0)
            {
                if(gridLayoutGroup.spacing.x<1015)
                {
                    gridLayoutGroup.spacing=new Vector2(gridLayoutGroup.spacing.x+audioStackSpaceX,0);
                }
            }
            else
            {
                if(gridLayoutGroup.spacing.x>5)
                {
                    gridLayoutGroup.spacing=new Vector2(gridLayoutGroup.spacing.x-audioStackSpaceX,0);
                }
            }
        }
    }

}
