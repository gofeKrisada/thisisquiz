using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState
{
   public int correctCount { get; set; }
   public int wrongCount { get; set; }
   public int difficult { get; set; }

    public  GameState()
    {
        correctCount = 0;
        wrongCount = 0;
        difficult = 0;
    }

}//gameState


public class GameManager : MonoBehaviour {

    GameState gameState;
    // Use this for initialization
    void Start () {
        gameState = new GameState();
     
	}//start
	
	// Update is called once per frame
	void Update () {
		
	}//update

    private float timer;
    public float baseTimer;

    public GameObject eventSystem;
    public GameObject UI_timerBar;

    public void EnableInput(bool enable)
    {
        eventSystem.SetActive(enable);
  
    }//enableInput

    void OnQuestionStart()
    {
        timer = baseTimer;
        EnableInput(true);

        GetQuestion();


    }//OnQuestionStart

    void OnQuestionEnd()
    {
        EnableInput(false);
        if (gameState.correctCount >= 5)
        {
            GameEnd();
        }//correct 5
        else if (gameState.wrongCount >= 3)
        {
            GameEnd();
        }//wrong 3

    }//OnQuestionEnd



    void QuestionTimer()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UI_timerBar.GetComponent<Image>().fillAmount = 1 - timer / baseTimer;
        }
        else
        {
            timer = 0;
            gameState.wrongCount += 1;
            OnQuestionEnd();

        }// question timeout


    }//gameTimer

    void CheckAnswer(bool correctAnswer)
    {

        if (correctAnswer)
        {
            gameState.correctCount++;
            AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_correct);
        }//correct
        else
        {
            gameState.wrongCount++;
            AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_wrong);
  
        }//wrong


    }//checkAnswer


    void GetQuestion()
    {

    }//get Question

    void GameEnd()
    {

    }//GameEnd

}//GameManager
