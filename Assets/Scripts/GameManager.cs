using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject arm1, arm2, pausePanel, activePanel, winPanel,losePanel,ReadyPanel,drawPanel;
    public Text PauseText, DiffText, TimeText;
    private int Difficulty = 10;
    private int  count = 0, ppower=0;
    private float GameTime = 30f , SettTime = 30f, totalRot= 0f, Force = -10;
    public idk SerialScipt;
    public Slider ts;
    public AudioSource AS;
    public AudioSource BGSound;
    public bool doing = false, soundplaying = true, isplay = true;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        ts.maxValue = GameTime;
        ts.value = GameTime;
        ReadyPanel.SetActive(true);
        Invoke("doidoit", 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) 
            return;
        if (doing )
        {
            if (isplay)
            {
                if (SerialScipt.dataExist)
                    ppower = SerialScipt.pp;
                Force = ppower - Difficulty;
                GameTime -= Time.deltaTime;
                ts.value = GameTime;
                count++;
                SerialScipt.SPread();
                if (SerialScipt.dataExist)
                    ppower = SerialScipt.pp;
                Force = ppower - Difficulty;
                if (GameTime < 0.01f)
                {
                    if (arm1.transform.rotation.z < 0)
                        losePanel.SetActive(true);
                    else if (arm1.transform.rotation.z > 0)
                        winPanel.SetActive(true);
                    else
                        drawPanel.SetActive(true);
                }

                Rot();
                Wincheck();
                Controls();
            }
        }
    }
    public void Controls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if(pausePanel.activeSelf == false)
            //    if (Input.GetKeyDown(KeyCode.Escape))
            //        Quit();
            PauseButton();              
        }
        if (Input.GetKey(KeyCode.T))
            Quit();
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (soundplaying)
                stopsound();
            else
                startsound();
        }    
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Wincheck()
    {
        if(totalRot>=90)
        {
            Time.timeScale = 0.0f;
            isplay = false;
            winPanel.SetActive(true);

        }
        else if(totalRot <=-90)
        {
            Time.timeScale = 0.0f;
            isplay = false;
            losePanel.SetActive(true);

        }
    }
    public void Rot()
    {
        arm1.transform.Rotate(0, 0, Force/100);
        arm2.transform.Rotate(0, 0, -Force/100);
        totalRot = totalRot + Force/100;
    }
    void PauseGame()
    {
        Time.timeScale = 0.0f;
    }
    void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
    public void PauseButton()
    {
        PauseGame();
        pausePanel.SetActive(true);
        DiffText.text = "Difficulty(?/ 20) : " + Difficulty.ToString();
        TimeText.text = "Time : " + SettTime.ToString();
        if (Input.GetKey(KeyCode.Escape))
            Quit();
        Time.timeScale = 0.0f;
    }

    public void DiffAddButton()
    {
        if (Difficulty < 20)
            Difficulty++;
        DiffText.text = "Difficulty(?/ 20) : " + Difficulty.ToString();
    }
    public void DiffReduceButton()
    {
        if (Difficulty >1)
            Difficulty--;
        DiffText.text = "Difficulty(?/ 20) : " + Difficulty.ToString();
    }
    public void TimeAddButton()
    {
        if (SettTime < 60)
            SettTime++;
        TimeText.text = "Time : " + SettTime.ToString();
        GameTime = SettTime;
    }
    public void TimeReduceButton()
    {
        if (SettTime > 10)
            SettTime--;
        TimeText.text = "Time : " + SettTime.ToString();
        GameTime = SettTime;
    }
    public void ReturnButton()
    {
        pausePanel.SetActive(false);
        activePanel.SetActive(true);
        ts.maxValue = GameTime;
        ts.value = GameTime;
        arm1.transform.Rotate(0, 0, -totalRot);
        arm2.transform.Rotate(0, 0, totalRot);
        totalRot = 0;
        Time.timeScale = 1.0f;
        isplay = true;
    }
    public void pAgainButton()
    {
        ts.maxValue = GameTime;
        ts.value = GameTime;
        arm1.transform.Rotate(0, 0, -totalRot);
        arm2.transform.Rotate(0, 0, totalRot);
        totalRot = 0;
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        activePanel.SetActive(true);
        Time.timeScale = 1.0f;
        isplay = true;
        //losePanel.SetActive(false);
        //winPanel.SetActive(false);
        //Time.timeScale = 1.0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void doidoit()
    {
        doing = true;
        ReadyPanel.SetActive(false);
        AS.Play();
    }
    public void stopsound()
    {
        soundplaying = false;
        BGSound.Stop();
    }
    public void startsound()
    {
        soundplaying = true;
        BGSound.Play();
    }
}
