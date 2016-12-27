
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz
{
    public string question { get; set; }
    public List<string> choices { get; set; }
    public int correctIndex { get; set; }
    public int rank;

    public int[] SortQuiz()
    {
        List<int> choiceList = new List<int>();
        for (int i = 2; i < 6; i++)
            choiceList.Add(i);

        correctIndex = -1;
        int[] cSort = new int[4];
        for (int i = 0; i < 4; i++)
        {
            int cRand = (int)Random.Range(0, choiceList.Count);
            cSort[i] = choiceList[cRand];

            if (cRand == 0 && correctIndex < 0)
                correctIndex = i;

            choiceList.RemoveAt(cRand);
        }//int
       
        return cSort;

    }//sort

}//class quiz



public class QuizManager : MonoBehaviour  {

    public QuizManager()
    {

    }//
    public QuizManager(string diff)
    {
        difficult = diff;
     
    }//



    [Header("Label")]
    public GameObject label_question;
    public GameObject label_choice_a;
    public GameObject label_choice_b;
    public GameObject label_choice_c;
    public GameObject label_choice_d;
    public GameObject label_score;



    private float timer;
    [Header("Timer")]
    public GameObject UI_timerBar;
    public float baseTimer;

    [Header("UI")]
    public GameObject UI_life;



    private GameManager gameManager;
    private int correctChoice=-1;
    private int wrongCount;
    private int correctCount;


    [HideInInspector]
    public int score;
    [HideInInspector]
    public int quesitonRank;
    [HideInInspector]
    public string difficult;

    void Start()
    {
        gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();
    }//start

    // Update is called once per frame
    void Update () {
        QuestionTimer();
	}//update

    public void GameStart()
    {
        wrongCount = 0;
        correctCount = 0;
        correctChoice = -1;
        onTimer = false;

        score = 0;
        label_score.GetComponent<Text>().text = "SCORE : " + score;

        timer = 0;

        for (int i = 0; i < 3; i++)
            UI_life.transform.GetChild(i)
            .GetChild(0).gameObject.SetActive(true);

        OnQuestionStart(2.55f);
    }//game start

    private void OnQuestionStart()
    {
        timer = baseTimer;
        onTimer = true;
       // gameManager.EnableInput(true);

        GetQuestion(gameManager.gameState.rating);


    }//OnQuestionStart
    private void OnQuestionStart(float delay)
    {
        timer = baseTimer+delay;
        onTimer = true;
        //gameManager.EnableInput(true);

        GetQuestion(gameManager.gameState.rating);


    }//OnQuestionStart + Delay

    private void OnQuestionEnd()
    {
      //  gameManager.EnableInput(false);
        if (correctCount >= 5)
        {
            onTimer = false;
            GameEnd("YouWin!");
        }//correct 5
        else if (wrongCount >= 3)
        {
            onTimer = false;
            GameEnd("YouLost!");
        }//wrong 3
        else
        {
            OnQuestionStart();
        }

    }//OnQuestionEnd

    private bool onTimer;
    private void QuestionTimer()
    {
        if (onTimer)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UI_timerBar.GetComponent<Image>().fillAmount = 1 - timer / baseTimer;
            }
            else
            {
                timer = 0;
                UI_life.transform.GetChild(2 - wrongCount)
                .GetChild(0).gameObject.SetActive(false);

                wrongCount ++;
                OnQuestionEnd();

                AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_wrong);
          
            }// question timeout
        }//timer on

    }//gameTimer

    public void CheckAnswer(bool correctAnswer)
    {

        if (correctAnswer)
        {
            for (int i = 0; i < 3; i++)
                UI_life.transform.GetChild(i)
                .GetChild(0).gameObject.SetActive(true);

            wrongCount = 0;

            correctCount++;
            AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_correct);

            gameManager.gameState.rating += quesitonRank;
            AddScore();
            
        }//correct
        else
        {
            UI_life.transform.GetChild(2 - wrongCount)
             .GetChild(0).gameObject.SetActive(false);
           correctCount = 0;
           wrongCount++;
            AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_wrong);

            gameManager.gameState.rating -= quesitonRank/2;
            SubstractScore();
            

        }//wrong

        label_score.GetComponent<Text>().text = "SCORE : " + score;

        OnQuestionEnd();
    }//checkAnswer

    public void AddScore()
    {

        score += quesitonRank;

       
    }//calculate score
    public void SubstractScore()
    {

            score -= quesitonRank / 2;

    }//calculate score


    private void GetQuestion(int rating)
    {
  
        List<string> retList = FireBaseProvider.Instance.GetQuestion(rating);

        Quiz quiz = new Quiz();
        quiz.question = retList[0];
        quiz.rank = int.Parse(retList[1]);

        // Add Choices
        quiz.choices = new List<string>();
        int[] cSort = quiz.SortQuiz();
        for (int i = 0; i < 4; i++) 
        quiz.choices.Add(retList[cSort[i]]);


        this.quesitonRank = quiz.rank;
        this.correctChoice = quiz.correctIndex;
   
        UIHandler(quiz);
    }//get Question

    private void UIHandler(Quiz quiz)
    {

        //Question
        label_question.GetComponent<Text>().text = quiz.question;
        //Choices 
        label_choice_a.GetComponent<Text>().text = quiz.choices[0];
        label_choice_b.GetComponent<Text>().text = quiz.choices[1];
        label_choice_c.GetComponent<Text>().text = quiz.choices[2];
        label_choice_d.GetComponent<Text>().text = quiz.choices[3];
    }//UI


    private void GameEnd(string result)
    {
        gameManager.OnResult(result, score);
        
    }//GameEnd

    #region Button_choice
    public void Button_A()
    {
        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);
        if (correctChoice == 0)
            CheckAnswer(true);
        else 
            CheckAnswer(false);

        }//choice A
    public void Button_B()
    {
        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);
        if (correctChoice == 1)
            CheckAnswer(true);
        else
            CheckAnswer(false);

    }//choice B
    public void Button_C()
    {
        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);
        if (correctChoice == 2)
            CheckAnswer(true);
        else
            CheckAnswer(false);

    }//choice C
    public void Button_D()
    {
        AudioPlayer.Instance.Play(AudioPlayer.Instance.sfx_button);
        if (correctChoice == 3)
            CheckAnswer(true);
        else
            CheckAnswer(false);

    }//choice D
    #endregion

}//class
