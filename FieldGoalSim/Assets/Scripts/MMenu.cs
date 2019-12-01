using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMenu : MonoBehaviour {

    //public GameObject HTP;

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    //public void ExitGame()
    //{
    //    Application.Quit();
    //}

    //public void htpDisplay()
    //{
    //    HTP.SetActive(true);
    //}

    //public void Return()
    //{
    //    HTP.SetActive(false);
    //}
}
