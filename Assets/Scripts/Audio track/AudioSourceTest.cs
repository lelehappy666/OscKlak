using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTest : MonoBehaviour
{
    // Start is called before the first frame update
    // public AudioSource audioSource;
    // public float[] spectrumData=new float[8192];
    // void Start()
    // {
    //     audioSource.GetSpectrumData(spectrumData,0,FFTWindow.BlackmanHarris);
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    public FFTWindow fftWindow = FFTWindow.Rectangular;
    public int sampleSection = 256;
    public float[] spectrum;
    public float[] spectrumLogValues;


    public float sampleInterval = 0.1f;
    public int logLevel = 0;
    public int dropSpeed = 1;
    public int maxHeight = 50;


    private float currentSampleTime = 0;
    private float[] currDropSpeeds;


    void Update( )
    {
        if(currentSampleTime < sampleInterval)
        {
            currentSampleTime += Time.deltaTime;
            return;
        }
        else
        currentSampleTime -= sampleInterval;


        if(spectrum == null || spectrum.Length !=sampleSection)
        spectrum = new float[sampleSection];
        if(spectrumLogValues == null || spectrumLogValues.Length !=sampleSection)
        spectrumLogValues = new float[sampleSection];
        if(currDropSpeeds == null || currDropSpeeds.Length !=sampleSection)
        currDropSpeeds = new float[sampleSection];


        AudioListener.GetSpectrumData( spectrum, 0, fftWindow);


        for( int i = 1; i < spectrum.Length-1; i++ )
        {
            float currentVal = 0;
            if(logLevel == 0)
            currentVal = spectrum[i];
            if(logLevel < 0)
            currentVal = Mathf.Clamp(spectrum[i]*(maxHeight+i*i), 0, maxHeight);
            else
            currentVal = -Mathf.Log(spectrum[i], logLevel);


            if(spectrumLogValues[i] == 0 || currentVal - spectrumLogValues[i] >= Mathf.Abs(currDropSpeeds[i]))
            { 
                currDropSpeeds[i] = 0;
                spectrumLogValues[i] = currentVal;
            }
            else
            {
                spectrumLogValues[i] = Mathf.Clamp(spectrumLogValues[i]- currDropSpeeds[i], 0, maxHeight);
                currDropSpeeds[i] += dropSpeed * Time.deltaTime;
            }
        }
    }


    void OnDrawGizmos()
    {
        for( int i = 1; i < spectrumLogValues.Length-1; i++)
        Gizmos.DrawCube(new Vector3(i*1, 0.5f * spectrumLogValues[i], 0), new Vector3(1, spectrumLogValues[i], 1));
    }
}
