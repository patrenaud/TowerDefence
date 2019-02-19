using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Tower Defense Parameters")]
    private const int MAX_LEVELS = 100; //100 level maximum.
    public int GetMaxLevels
    {
        get { return MAX_LEVELS; }
    }

    private int m_CurrentLevel = 0;
    public int CurrentLevel
    {
        get { return m_CurrentLevel; }
    }

    [SerializeField]
    private LevelGenerator m_LevelGenerator;

    [Header("Scene Management Parameters")]
    [Tooltip("Is there a scene transition during the scene loading.")]
    [SerializeField]
    private bool m_HasTransitionScene = false;

    [SerializeField]
    private float m_MinSceneTransitionTime = 1f;

    [SerializeField]
    private GameObject m_LoadingScreen;

    private int m_SceneIndexRef; //To keep a track of the wanted scene when doing a scene transition.

    protected override void Awake()
    {
        base.Awake();
        m_LoadingScreen.SetActive(false);
    }

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RegisterEvent(EventID.ChangeLevel, ChangeLevel);
        }

        EventManager.Instance.DispatchEvent(EventID.ChangeLevel, EScenes.BaseScene);
    }

    //Call the level generator to build the current level
    public void GenerateLevel()
    {
        m_CurrentLevel++;
        m_LevelGenerator.GenerateLevel();
    }



    private void OnLoadingDone(Scene i_Scene, LoadSceneMode i_Mode)
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.DispatchEvent(EventID.UpdateMusic, SceneManager.GetActiveScene().buildIndex);
        }

        //On enleve la fct de la liste de fct appelees par l'event OnLoadingDone de Unity.
        SceneManager.sceneLoaded -= OnLoadingDone;

        //Stop Animation Loading Screen
        m_LoadingScreen.SetActive(false);

        //Now that the scene is loaded if we have scene transition, load the real scene now.
        if (m_HasTransitionScene && SceneManager.GetActiveScene().buildIndex != m_SceneIndexRef)
        {
            if (m_MinSceneTransitionTime > 0f)
            {
                StartCoroutine(WaitForTransition());
            }
            else
            {
                LoadSceneAtIndex(m_SceneIndexRef);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex == (int)EScenes.BaseScene)
        {
            GenerateLevel();
        }
    }

    private void ChangeLevel(object a_Scene)
    {
        //a_Scene as EScene type or int (build index) type.

        if (m_HasTransitionScene)
        {
            m_SceneIndexRef = (int)a_Scene;

            //Load the transition scene
            LoadSceneAtIndex((int)EScenes.TransitionScene);
        }
        else
        {
            LoadSceneAtIndex((int)a_Scene);
        }
    }

    private void LoadSceneAtIndex(int a_Index)
    {
        m_LoadingScreen.SetActive(true);
        SceneManager.LoadScene(a_Index);
        //Ajoute la fonction OnLoadingDone dans la liste de fonction appelees par (l'action/l'event SceneLoaded de Unity).
        SceneManager.sceneLoaded += OnLoadingDone;
    }

    private IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(m_MinSceneTransitionTime);

        LoadSceneAtIndex(m_SceneIndexRef);
    }
}
