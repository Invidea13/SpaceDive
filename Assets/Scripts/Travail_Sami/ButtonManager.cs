using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public GameObject[] menuUI;
    public GameObject[] optionUI;
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OptionSelect()
    {
        menuUI[0].SetActive(false);
        menuUI[1].SetActive(false);
        menuUI[2].SetActive(false);
        optionUI[0].SetActive(true);
    }

    public void MenuReturn()
    {
        menuUI[0].SetActive(true);
        menuUI[1].SetActive(true);
        menuUI[2].SetActive(true);
        optionUI[0].SetActive(false);
    }
}
