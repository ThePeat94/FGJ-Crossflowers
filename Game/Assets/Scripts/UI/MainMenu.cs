using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_StartMenu;
    [SerializeField] private GameObject m_SelectPlayMode;

    public void BtnPlay_Click()
    {
        this.m_StartMenu.SetActive(false);
        this.m_SelectPlayMode.SetActive(true);        
    }

    public void BtnQuit_Click()
    {
        Application.Quit();
    }

    public void BtnHowTo_Click()
    {

    }

    public void BtnCredits_Click()
    {

    }

    public void BtnStoryMode_Click()
    {
        SceneManager.LoadScene(2);
    }

    public void BtnRandomMode_Click()
    {
        SceneManager.LoadScene(1);
    }

    public void BtnBackToStartScreen_Click()
    {
        this.m_SelectPlayMode.SetActive(false); 
        this.m_StartMenu.SetActive(true);        
    }
}
