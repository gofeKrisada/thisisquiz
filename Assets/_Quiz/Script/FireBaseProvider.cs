using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class FireBaseProvider : Singleton<FireBaseProvider> {

    [HideInInspector]
    public DataSnapshot snapshot;
    DatabaseReference rootRef;


 
    private List<int> qPool;
    private string currectDiff;
   
    public List<string>  GetQuestion(int playerRating)
    {

        int result = ((int)Random.Range(0, playerRating ))/100;
        string difficulty="";
        if (result <= 0) difficulty = "easy";
        else if (result == 1) difficulty = "medium";
        else if (result >= 2) difficulty = "hard";

        if ( qPool==null || qPool.Count <= 0 )
        {
            qPool = new List<int>();
            for(int i=0;i< snapshot.Child(difficulty).ChildrenCount; i++)
            {
                qPool.Add(i);
            }
            currectDiff = difficulty;
        }//create pool
        
        int qRand = (int)Random.Range(0, qPool.Count);
        int qIndex = qPool[qRand];
        qPool.RemoveAt(qRand);
            //(int)Random.Range(0, snapshot.Child(difficulty).ChildrenCount);

        List<string> retList = new List<string>();
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("question").Value.ToString());
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("rank").Value.ToString());
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("correct").Value.ToString());
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("wrong1").Value.ToString());
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("wrong2").Value.ToString());
        retList.Add(snapshot.Child(currectDiff).Child(qIndex.ToString()).Child("wrong3").Value.ToString());

        return retList;
        
    }//




    public void RefreshData()
    {
        snapshot = null;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://quizclash-dbca1.firebaseio.com/");
        rootRef = FirebaseDatabase.DefaultInstance.RootReference;


        rootRef = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance).GetReferenceFromUrl("https://quizclash-dbca1.firebaseio.com/");
        rootRef.GetValueAsync().ContinueWith(task =>
        {

            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {

                snapshot = task.Result;

            }
        });
    }//refresh

    public  List<int> GetScore()
    {


      
        List<int> highscore = new List<int>();

           
       
        highscore.Add(int.Parse(snapshot.Child("highscore").Child("0").Value.ToString()));
        highscore.Add(int.Parse(snapshot.Child("highscore").Child("1").Value.ToString()));
        highscore.Add(int.Parse(snapshot.Child("highscore").Child("2").Value.ToString()));
        highscore.Add(int.Parse(snapshot.Child("highscore").Child("3").Value.ToString()));

        return highscore;

    }//get score

   public void SetScore(int score, List<int> highscore)
    {
        for(int i = 0; i < highscore.Count; i++)
        {
            if (score > highscore[i])
            {
                highscore.Insert(i, score);
                highscore.RemoveAt(highscore.Count-1);
                break;
            }
        }//for
        for (int i = 0; i < highscore.Count; i++)
            rootRef.Child("highscore").Child(i.ToString()).SetValueAsync(highscore[i]);
    }//setScore

}//class
