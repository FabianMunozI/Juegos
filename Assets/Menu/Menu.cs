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
        if(!Equals(SceneManager.GetActiveScene().name, "MenuArtico")){
            if(!PlayerPrefs.HasKey("seedArtico")){
                int seed = Random.Range(0,10000);
                PlayerPrefs.SetInt("seedArtico", seed);
                PlayerPrefs.Save();
            }
        }else { // estoy en la scena menu desierto y presiono regenerar desierto
            int seed = Random.Range(0,10000);
            PlayerPrefs.SetInt("seedArtico", seed);
            PlayerPrefs.Save();
        }
        
        SceneManager.LoadScene("MenuArtico");
    }
    public void MenuButtonDesierto ()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
         // numero entre 0 y 10000 ( exclusivo el 2do)
        if(!Equals(SceneManager.GetActiveScene().name, "MenuDesierto")){
            if(!PlayerPrefs.HasKey("seedDesierto")){
                int seed = Random.Range(0,10000);
                PlayerPrefs.SetInt("seedDesierto", seed);
                PlayerPrefs.Save();
            }
        }else { // estoy en la scena menu desierto y presiono regenerar desierto
            int seed = Random.Range(0,10000);
            PlayerPrefs.SetInt("seedDesierto", seed);
            PlayerPrefs.Save();
        }
        
        SceneManager.LoadScene("MenuDesierto");
    }
    public void MenuButtonPlaya ()
    {
         if(!Equals(SceneManager.GetActiveScene().name, "MenuPlaya")){
            if(!PlayerPrefs.HasKey("seedPlaya")){
                int seed = Random.Range(0,10000);
                PlayerPrefs.SetInt("seedPlaya", seed);
                PlayerPrefs.Save();
            }
        }else { 
            int seed = Random.Range(0,10000);
            PlayerPrefs.SetInt("seedPlaya", seed);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("MenuPlaya");
    }
    public void BotonTutorial (){
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_TUTORIAL);
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
        //SceneManager.LoadScene("PlayaProcedural");
    }
    public void ProceduralDesierto()
    {
        //Decidir que escena vamos a cargar lol
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALDESIERTO);
        //SceneManager.LoadScene("intento");
    }
}