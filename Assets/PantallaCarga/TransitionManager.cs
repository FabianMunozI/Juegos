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

    public const string SCENE_NAME_MAIN_MENU = "MainScene";
    public const string SCENE_NAME_GAME_CITY = "Ciudad";
    public const string SCENE_NAME_GAME_FOREST = "Bosque";
    public const string SCENE_NAME_GAME_DESERT = "Desierto";
    public const string SCENE_NAME_GAME_ARTIC = "Artico";
    public const string SCENE_NAME_GAME_BEACH = "Playa";

    public const string SCENE_NAME_GAME_MENU = "Menu";
    public const string SCENE_NAME_GAME_MENUCIUDAD = "MenuCiudad";
    public const string SCENE_NAME_GAME_MENUBOSQUE = "MenuBosque";
    public const string SCENE_NAME_GAME_INTENTO = "intento";


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
            progressLabel.text = $"{progressValue * 100}%";
        }

    }

}
