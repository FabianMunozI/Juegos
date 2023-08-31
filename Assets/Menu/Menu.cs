using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void MenuButtonBosque ()
    {
        SceneManager.LoadScene("MenuBosque");
    }
    public void MenuButtonCiudad ()
    {
        SceneManager.LoadScene("MenuCiudad");
    }
    // Update is called once per frame
    public void OnQuitButton ()
    {
        Application.Quit();
    }

    public void OnBackButton ()
    {
        SceneManager.LoadScene("Menu");
    }
    public void GameStart()
    {
        //Decidir que escena vamos a cargar lol
        SceneManager.LoadScene("Bosque");
    }
    public void GameStart2()
    {
        //Decidir que escena vamos a cargar lol
        SceneManager.LoadScene("Ciudad");
    }
    public void ProceduralBosque()
    {
        //Decidir que escena vamos a cargar lol
        SceneManager.LoadScene("intento");
    }
    public void ProceduralCiudad()
    {
        //Decidir que escena vamos a cargar lol
        SceneManager.LoadScene("ProceduralNegro");
    }
}