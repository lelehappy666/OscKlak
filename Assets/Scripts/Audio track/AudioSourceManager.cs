using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;

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
    public GameObject audioStackUITiemLine;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴数量")]
    public int audioStackCount;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨时间轴缩放比例")]
    public float audioStackSpaceX;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨UI模板")]
    public Image audioStackUI;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音轨UI模板父物体")]
    public Transform audioStackUIParent;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音频模板")]
    public AudioSource audioSource;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("是否在时间轴创建音频")]
    public bool isCreatAudioSourceUI=false;

    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音频数组")]
    public List<AudioSource> audioSourcesTemplates ;

    
    [FoldoutGroup("AudioStackUI")]
    [Tooltip("音频UI数组")]
    public List<Image> audioSourcesUITemplates ;
    public int coefficient;


    void Start()
    {
        LoadAudioSources(path);
        AudioStackUiProduction();
    }

    // Update is called once per frame
    void Update()
    {
        AudioSourceStackIntervalChange();
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
                    examplesPrefabImages.GetComponentInChildren<Button>().onClick.AddListener(delegate{OnClickBtnExamplePrefabImages(audioStackUI,audioStackUIParent,examplesPrefabImages.name);});
                }
            }
        }
    }

    /// <summary>
    /// 实例化音轨时间轴线
    /// </summary>
    public void  AudioStackUiProduction()
    {
        for(int i=0;i<audioStackCount;i++)
        {
            //音轨UI间隔
            var auioStackUIs=Instantiate(audioStackUITiemLine);
            auioStackUIs.GetComponentInChildren<Text>().text=i.ToString();
            auioStackUIs.name=auioStackUIs.name+"_"+i;
            auioStackUIs.gameObject.transform.SetParent(gridLayoutGroup.transform);
            audioStackTimes.Add(auioStackUIs);
        }
    }

    /// <summary>
    /// 点击音乐资源实例化音轨UI
    /// </summary>
    /// <param name="audioStackUI"></param>
    /// <param name="audioStackUIParent"></param>
    /// <param name="audioStackUIName"></param>
    public void OnClickBtnExamplePrefabImages(Image audioStackUI,Transform audioStackUIParent,string audioStackUIName)
    {
        isCreatAudioSourceUI=true;
        var audioStackUIs=Instantiate(audioStackUI);
        audioSourcesUITemplates.Add(audioStackUIs);
        audioStackUIs.transform.SetParent(audioStackUIParent);
        audioStackUIs.name=audioStackUIName;
        audioStackUIs.GetComponentInChildren<Text>().text=audioStackUIs.name.Split('.')[0];
        // audioStackUIs.
        LoadAudioFile(Application.streamingAssetsPath+"/"+audioStackUIName,audioStackUIName,audioStackUIs);
        // audioStackUI.GetComponent<Button>().OnSelect();
    }

    /// <summary>
    /// 鼠标滚轮滚动时音轨间隔变化
    /// </summary>
    public void AudioSourceStackIntervalChange()
    {
        switch  (gridLayoutGroup.spacing.x)
        {
            case 45:
                audioStackSpaceX=2;
                if(Input.GetAxis("Mouse ScrollWheel")>0)
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,24.77252f),5,false,1);
                    audioStackSpaceX=5;
                }
                else
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,12.227f),5,true,1);
                }
            break;

            case 25f:
                audioStackSpaceX=1f;
                if(Input.GetAxis("Mouse ScrollWheel")>0)
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,24.77252f),10,false,5);   
                    audioStackSpaceX=2;
                }
                else
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,12.227f),10,true,5);   
                }
            break;

            case 5f:
                audioStackSpaceX=0.5f;
                if(Input.GetAxis("Mouse ScrollWheel")>0)
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,12.227f),20,false,10);   
                    audioStackSpaceX=1;
                }
                else
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,6.11f),20,true,10);
                }
            break;

            case 3.5f:
                audioStackSpaceX=0.1f;
                if(Input.GetAxis("Mouse ScrollWheel")>0)
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,6.11f),40,false,20);   
                    audioStackSpaceX=0.5f;
                }
                else
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,3.05f),40,true,20);   
                }
            break;
            case 1.700001f:
            print("到了");
                audioStackSpaceX=0f;
                if(Input.GetAxis("Mouse ScrollWheel")>0)
                {
                    AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,3.05f),60,false,40);   
                    audioStackSpaceX=0.1f;
                }
                else
                {
                     AudioSourceStackUIChange(audioStackTimes,new Vector2(1.537476f,1.025f),60,true,40);   
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
                if(gridLayoutGroup.spacing.x>=1.800001f)
                {
                    gridLayoutGroup.spacing=new Vector2(gridLayoutGroup.spacing.x-audioStackSpaceX,0);
                }
            }
            if(isCreatAudioSourceUI)
            {
                for(int i=0;i<audioSourcesUITemplates.Count;i++)
                {
                    if(!audioSourcesUITemplates[i].GetComponent<ImageDrap>().isDrag)
                    {
                        audioSourcesUITemplates[i].GetComponent<RectTransform>().sizeDelta=new Vector2(audioSourcesTemplates[i].GetComponent<AudioClipAttribute>().audioClipLenth*gridLayoutGroup.spacing.x,36.73944f);
                        audioSourcesUITemplates[i].GetComponent<RectTransform>().anchoredPosition3D =new Vector3(audioSourcesTemplates[i].GetComponent<AudioClipAttribute>().audioClipLenth*gridLayoutGroup.spacing.x/2, audioSourcesUITemplates[i].GetComponent<RectTransform>().anchoredPosition3D.y,0);
                    }
                    else
                    {
                       audioSourcesUITemplates[i].GetComponent<RectTransform>().sizeDelta=new Vector2(audioSourcesTemplates[i].GetComponent<AudioClipAttribute>().audioClipLenth*gridLayoutGroup.spacing.x,36.73944f);
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// 音轨UI变化
    /// </summary>
    /// <param name="audioStackTimes">音轨间隔UI数组</param>
    /// <param name="changeRectTransformSizeDalta">变换后的被缩小的间距</param>
    /// <param name="interval">忽略的数字系数</param>
    /// <param name="isSllowDown">是否向下滑</param>
    /// <param name="intervalUp">向上滑动系数</param>
    public void AudioSourceStackUIChange(List<GameObject> audioStackTimes,Vector2 changeRectTransformSizeDalta,float intervalDown,bool isSllowDown,float intervalUp)
    {
         for(int i=0;i<audioStackTimes.Count;i++)
            {
                if(isSllowDown)
                {
                    if(i%intervalDown!=0)
                    {
                        audioStackTimes[i].GetComponent<AudioStackUIAttribute>().audioStackTimelineSuperscript.enabled=false;
                        audioStackTimes[i].GetComponentsInChildren<RectTransform>()[2].sizeDelta=changeRectTransformSizeDalta;
                        audioStackTimes[i].GetComponent<AudioStackUIAttribute>().audioStackTimelineChildImage.enabled=false;
                    }
                }
                else
                {
                    if(i%intervalUp==0)
                    {
                        audioStackTimes[i].GetComponent<AudioStackUIAttribute>().audioStackTimelineSuperscript.enabled=true;
                        audioStackTimes[i].GetComponentsInChildren<RectTransform>()[2].sizeDelta=changeRectTransformSizeDalta;
                        audioStackTimes[i].GetComponent<AudioStackUIAttribute>().audioStackTimelineChildImage.enabled=true;
                    }
                }
            }
    }
    
    /// <summary>
    /// 点击音轨UI加载外部WAV音频创建AudioSource
    /// </summary>
    /// <param name="url"></param>
    /// <param name="audioName"></param>
    /// <returns></returns>
    IEnumerator LoadWavAudioFile(string url,string audioName,Image audioStackUIs)
    {
         WWW music = new WWW(url);
        yield return music;
        AudioClip lamusic = music.GetAudioClipCompressed(true, AudioType.WAV);
        AudioSource audioSources=Instantiate(audioSource);
        audioSourcesTemplates.Add(audioSources);
        audioSources.transform.SetParent(gameObject.transform);
        audioSources.name=audioName;
        audioSources.clip=lamusic;
        //保留一位
        string audioClipLength=String.Format("{0:F1}",audioSources.clip.length);
        audioSources.GetComponent<AudioClipAttribute>().audioClipLenth=float.Parse(audioClipLength);
        print(audioSources.name+"--------------"+audioSources.GetComponent<AudioClipAttribute>().audioClipLenth+"----------"+audioClipLength);
        // audioStackUIParent.GetComponent<GridLayoutGroup>().cellSize=new Vector2(float.Parse(audioClipLength)*10*gridLayoutGroup.spacing.x,39);
        audioStackUIs.GetComponent<RectTransform>().sizeDelta=new Vector2(float.Parse(audioClipLength)*gridLayoutGroup.spacing.x,36.73944f);
        audioStackUIs.GetComponent<RectTransform>().anchoredPosition3D =new Vector3(float.Parse(audioClipLength)*gridLayoutGroup.spacing.x/2,237.3f-coefficient*70,0);
        coefficient=coefficient+1;
    }

    /// <summary>
    /// 加载音频文件在时间轴创建音频UI
    /// </summary>
    /// <param name="url"></param>
    /// <param name="audioName"></param>
    public void LoadAudioFile(string url,string audioName,Image audioStackUIs)
    {
        print(url);
        StartCoroutine(LoadWavAudioFile(url,audioName,audioStackUIs));
    }

}
