using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void MenuButtonBosque ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUBOSQUE);
        SceneManager.LoadScene("MenuBosque");
    }
    public void MenuButtonCiudad ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
        SceneManager.LoadScene("MenuCiudad");
    }
    public void MenuButtonArtico ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
        SceneManager.LoadScene("MenuArtico");
    }
    public void MenuButtonDesierto ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
        SceneManager.LoadScene("MenuDesierto");
    }
    public void MenuButtonPlaya ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
        SceneManager.LoadScene("MenuPlaya");
    }
    // Update is called once per frame
    public void OnQuitButton ()
    {
        Application.Quit();
    }

    public void OnBackButton ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENU);
        SceneManager.LoadScene("Menu");
    }
    public void GameStart()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_FOREST);
        //SceneManager.LoadScene("Bosque");
    }
    public void GameStart2()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_CITY);
        //SceneManager.LoadScene("Ciudad");
    }
    public void ProceduralBosque()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_INTENTO);
        //SceneManager.LoadScene("intento");
    }
    public void ProceduralCiudad()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALNEGRO);
        //SceneManager.LoadScene("ProceduralNegro");
    }
    public void ProceduralArtico()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALARTICO);
        //SceneManager.LoadScene("intento");
    }
    public void ProceduralPlaya()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALPLAYA);
        //SceneManager.LoadScene("intento");
    }
    public void ProceduralDesierto()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALDESIERTO);
        //SceneManager.LoadScene("intento");
    }
}