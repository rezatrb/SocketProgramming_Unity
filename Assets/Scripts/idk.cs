using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

public class idk : MonoBehaviour
{
    SerialPort SP;
    //public GameObject go;
    public bool dataExist = false;
    public string ReceivedText, returnText = "0";
    public int pp = 0;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            SP = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            SP.Handshake = Handshake.None;
            SP.ReadTimeout = 5000;
            SP.WriteTimeout = 5000;
            SP.Encoding = ascii;
            SP.Open();
            Debug.Log("Serial port opened.");
        }
        catch(Exception err)
        {
            Debug.Log(err.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        if (SP != null && SP.IsOpen)
        {
            SP.Close();
            Debug.Log("Serial port closed.");
        }
    }
    public void SPread()
    {
        if (SP.IsOpen && SP.BytesToRead > 0)
        {
            dataExist = true;
            string data = SP.ReadLine();
            Debug.Log("Received data: " + data);
            returnText = data;

            for(int i =0;i<20;i++)
            {
                if (Int16.Parse(data) >= i * 12.75 && Int16.Parse(data) <= (i + 1) * 12.75)
                {
                    //Debug.Log(i.ToString());
                    pp = i;
                }
            }
        }
        else
        {
            dataExist = false;
        }
    }    
}
