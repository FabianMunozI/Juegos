using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;
using System;

[RequireComponent(typeof(Animator))]

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;
    public static TransitionManager Instance
    {
        get
        {
            if (instance==null){
                instance = Instantiate(Resources.Load<TransitionManager>("TransitionManager"));
            }

            return instance;
        }
    }

    //Escenas
    public const string SCENE_NAME_MAIN_MENU = "MainScene";
    public const string SCENE_NAME_GAME_CITY = "Ciudad";
    public const string SCENE_NAME_GAME_FOREST = "Bosque";
    public const string SCENE_NAME_GAME_DESERT = "FabProcedural";
    public const string SCENE_NAME_GAME_ARTIC = "Artico";
    public const string SCENE_NAME_GAME_BEACH = "Playa";
    
    public const string SCENE_NAME_GAME_MISIONPLANTA = "PlantaTratamientoAgua";
    public const string SCENE_NAME_GAME_TP_PLANTA = "TP_PlantaTratamiento";
    public const string SCENE_NAME_GAME_TP_EOLICA = "MisionEolica";

    //Menu
    public const string SCENE_NAME_GAME_MENU = "Menu";
    public const string SCENE_NAME_GAME_MENUCIUDAD = "MenuCiudad";
    public const string SCENE_NAME_GAME_MENUBOSQUE = "MenuBosque";
    public const string SCENE_NAME_GAME_MENUARTICO = "MenuArtico";
    public const string SCENE_NAME_GAME_MENUPLAYA = "MenuPlaya";
    public const string SCENE_NAME_GAME_MENUDESIERTO = "MenuDesierto";
    //Procedural
    public const string SCENE_NAME_GAME_INTENTO = "intento";
    public const string SCENE_NAME_GAME_PROCEDURALNEGRO = "ProceduralNegro";
    public const string SCENE_NAME_GAME_PROCEDURALARTICO = "Polo";
    public const string SCENE_NAME_GAME_PROCEDURALPLAYA = "PlayaProcedural";
    public const string SCENE_NAME_GAME_PROCEDURALDESIERTO = "FabProcedural";
    


    public Slider progressSlider;
    public TextMeshProUGUI progressLabel;
    public TextMeshProUGUI transitionInformationLabel;
    [Multiline]
    public string[] gameInformation = new string[0];

    private Animator m_Anim;
    //private int HashShowAnim = Animator.StringToHash("Show");


    private void Awake() {
        if(instance==null){
            instance=this;
            Init();
        }
        else if( instance!= this){
            Destroy(gameObject);
        }
    }

    void Init(){
        m_Anim = GetComponent<Animator>();

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName){
        StartCoroutine(LoadCoroutine(sceneName));
    }

    IEnumerator LoadCoroutine(string sceneName){
        m_Anim.SetBool("HasShowAnim", true);

        if( transitionInformationLabel != null)
            transitionInformationLabel.text = gameInformation[UnityEngine.Random.Range(0, gameInformation.Length -1)];

        UpdateProgressValue(0);   

        yield return new WaitForSeconds(1.3f);
        var sceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!sceneAsync.isDone){
            UpdateProgressValue(sceneAsync.progress);

            yield return null;

        }
        UpdateProgressValue(1);
        m_Anim.SetBool("HasShowAnim", false);
        //Debug.Log("entre");

    }

    void UpdateProgressValue(float progressValue){
        if (progressSlider != null){
            progressSlider.value = progressValue;
        }
        if(progressLabel.text != null){
            progressLabel.text = $"{(int)(progressValue * 100)}%";
        }

    }

}
