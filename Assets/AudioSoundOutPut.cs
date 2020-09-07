using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{ 
    [AddComponentMenu("Klak/Wiring/AudioSoundOutPut")]
    public class AudioSoundOutPut : NodeBase
    {
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        bool isaudioSource=true;

        float _originalScale;
        [Inlet]
        public float audioSource_Volume
        {
            set
            {
                if(audioSource==null || !enabled) return;
                if(isaudioSource) 
                audioSource.volume=value;
            }
        }

        void OnEnable()
        {
            if(audioSource!=null)
            {
                _originalScale=audioSource.volume;
            }
        }

        void OnDisable()
        {
            if(audioSource!=null)
            {
                audioSource.volume=_originalScale;
            }
        }
    }   
}
