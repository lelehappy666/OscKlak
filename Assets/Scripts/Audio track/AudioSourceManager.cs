using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string path;
    public List<string> audioSources;
    void Start()
    {
        LoadAudioSources(path);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                }
            }
        }
    }
}
