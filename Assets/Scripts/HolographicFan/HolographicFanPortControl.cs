using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class HolographicFanPortControl : MonoBehaviour
{
    //串口名
    public string portName = "COM3";//串口名
    public int baudRate = 19200;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    private SerialPort serialPort;
    void Start()
    {
        OpenPort();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// 打开串口
    /// </summary>
    public void OpenPort()
    {
        serialPort=new SerialPort(portName,baudRate,parity,dataBits,stopBits);
        try
        {
            //打开串口
            serialPort.Open();
            Debug.Log("打开成功");
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }

    }

    /// <summary>
    /// 关闭串口
    /// </summary>
    public void ClosePort()
    {
        serialPort.Close();
        Debug.Log("关闭串口");
    }

    /// <summary>
    /// 发送命令
    /// </summary>
    /// <param name="message"></param>
    public void SenMessage(string message)
    {
        string mesg=message;
        byte[] cmd=new byte[1024*1024*3];
        cmd=Convert16(mesg);
        //发送
        if(serialPort.IsOpen)
        {
            print("发送----------"+cmd);
            serialPort.Write(cmd,0,cmd.Length);
        }
    }

    /// <summary>
    /// 16进制转换
    /// </summary>
    /// <param name="strTex"></param>
    /// <returns></returns>
    public byte[] Convert16(string strTex)
    {
        strTex = strTex.Replace(" ", "");
        byte[] bText = new byte[strTex.Length / 2];
        for (int i = 0; i < strTex.Length / 2; i++)
        {
            bText[i] = Convert.ToByte(Convert.ToInt32(strTex.Substring(i * 2, 2), 16));
        }
        return bText;
    }

    public void OnClickBtnSendMessage(string message)
    {
        SenMessage(message);
    }

    private void OnApplicationQuit()
    {
        ClosePort();
    }
}
