using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState
{
    public int rating {
        get {
            if (PlayerPrefs.HasKey("rating"))
            {
                return  PlayerPrefs.GetInt("rating");

            }
            else
            {
                PlayerPrefs.SetInt("rating", 0);
                return PlayerPrefs.GetInt("rating");
            }
 
        }
        set {
            PlayerPrefs.SetInt("rating", value);
        }
    }// player rank  -pref

  
 
   public List<int> highScore { get; set; }
  

    public bool refreshingData { get; set; }

    public enum GameSeq
    {
        loading,
        main,
        gameplay,
        result
    }
    public enum GamePlaySeq
    {
        startQuestion,
        playing,
        timeout,
        checkAnswer,
        endQuestion
        
    }
    public GameSeq curretState { get ; set; }
    public GameSeq nexState { get; set; }
    public  GameState()
    {

        curretState = GameSeq.loading;
    }//contrcut



}//gameState




public class GameManager : MonoBehaviour {

    [Header("Window")]
    public GameObject window_loading;
    public GameObject window_main;
    public GameObject window_gameplay;
    public GameObject window_result;
    public GameObject window_disconnected;

    [Header("Panel")]
    public GameObject panel_result;
    public GameObject panel_highScore;

    [Header("Other")]
    public GameObject eventSystem;
    public float animation_time;

    [HideInInspector]
    public GameState gameState;

    private QuizManager quizManager;

    // Use this for initialization
    void Awake () {
        Init();
   
    }//start

    public void Init()
    {
        gameState = new GameState();

        quizManager = GameObject.Find("[QuizManager]").GetComponent<QuizManager>();

        OnChangeSeq(GameState.GameSeq.loading, GameState.GameSeq.main);


    }

    // Update is called once per frame
    void Update () {

        OnRefreshingData();
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Application.LoadLevel("PreScene");

        //}
	}//update

    

    public void EnableInput(bool enable)
    {
        eventSystem.SetActive(enable);
  
    }//enableInput

#region Button
    public void Button_Start()
    {
        MoveWindow(window_loading, animation_time);
        SetToggle(window_loading, 0);

        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);

        NextSequnce(GameState.GameSeq.gameplay);
    }//button action

    public void Button_OkayResult()
    {
        MoveWindow(window_loading, animation_time);
        SetToggle(window_loading, 0);

        SetToggle(window_result, 0.5f);

        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);

        gameState.curretState = GameState.GameSeq.loading;
        OnChangeSeq(GameState.GameSeq.loading, GameState.GameSeq.main);
    
        
    }//button action

    public void Button_Restart()
    {
        AudioPlayer.Instance.StopBGM();
        Application.LoadLevel("ThisIsQuiz");
    }//button action

#endregion

    private void NextSequnce(GameState.GameSeq nextSeq)
    {
     
       if (nextSeq == GameState.GameSeq.main)
        {
            gameState.curretState = GameState.GameSeq.main;
            SetToggle(window_main, 0);


            AudioPlayer.Instance.PlayBGM(AudioPlayer.Instance.bgm_theme);
   
                gameState.highScore = FireBaseProvider.Instance.GetScore();

            for (int i = 0; i < 4; i++)
                panel_highScore.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text
                    = gameState.highScore[i].ToString();

                SetToggle(window_loading, animation_time);
                MoveWindow(window_loading, animation_time);
                
                


        }//
        else if (nextSeq == GameState.GameSeq.gameplay)
        {
            gameState.curretState = GameState.GameSeq.gameplay ;

            AudioPlayer.Instance.PlayBGM(AudioPlayer.Instance.bgm_gameplay,2);

            quizManager.GameStart();
           
            MoveWindow(window_loading, animation_time, 2);
            SetToggle(window_loading, animation_time*2 + 2);
           

            SetToggle(window_main,  2);
            SetToggle(window_gameplay,  2);

 


        }//
   
    }//s
    void OnChangeSeq(GameState.GameSeq currentState, GameState.GameSeq nextState)
    {
        gameState.refreshingData = true;
        gameState.nexState = nextState;

        FireBaseProvider.Instance.snapshot = null;
        FireBaseProvider.Instance.RefreshData();
        if (currentState == GameState.GameSeq.loading)
        {
            if (window_loading.activeSelf == false)
                window_loading.SetActive(true);

        }//loading
    }//


    public void OnResult(string result,int score)
    {
        SetToggle(window_gameplay, 0.5f);
        SetToggle(window_result, 0);
       

        AudioPlayer.Instance.StopBGM();
        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_result);

        panel_result.transform.GetChild(1).GetComponent<Text>().text = result;
        panel_result.transform.GetChild(2).GetComponent<Text>().text = score.ToString();

        FireBaseProvider.Instance.SetScore(score, gameState.highScore);
       
    }//show result

    #region Animation
    public void SetToggle(GameObject obj,float delay)
    {
        if(delay==0)
            obj.SetActive(!obj.activeSelf);
        else
        StartCoroutine(DelaySetActive(obj, delay));
    }//setActive

    private IEnumerator DelaySetActive(GameObject obj,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(!obj.activeSelf);
    }//
    public void MoveWindow(GameObject obj, float time)
    {
        if (obj.activeSelf) {
            LeanTween.moveY(obj, -Screen.height/2, time).setEase(LeanTweenType.easeInOutQuart);
        }
        else
        {
            LeanTween.moveY(obj, Screen.height / 2, time).setEase(LeanTweenType.easeInOutQuart);
        }


    }//moveWindow
    public void MoveWindow(GameObject obj, float time,float delay)
    {
   
        if (obj.activeSelf)
        {
            LeanTween.moveY(obj, -Screen.height / 2, time).setEase(LeanTweenType.easeInOutQuart).setDelay(delay);
        }
        else
        {
            LeanTween.moveY(obj, Screen.height / 2, time).setEase(LeanTweenType.easeInOutQuart).setDelay(delay);
        }


    }//moveWindow
    #endregion

    private float refreshTimeout;
    private void OnRefreshingData()
    {
        if (gameState.refreshingData)
        {
            refreshTimeout += Time.deltaTime;
        
            if (FireBaseProvider.Instance.snapshot != null && refreshTimeout >1.5f)
            {
                NextSequnce(gameState.nexState);
                gameState.refreshingData = false;
                refreshTimeout = 0;
            }//get updated data



            if (refreshTimeout > 5)
            {
                window_disconnected.SetActive(true);
                gameState.refreshingData = false;
            }//null
        }//subscripe snapshot

    }//

    



}//GameManager
