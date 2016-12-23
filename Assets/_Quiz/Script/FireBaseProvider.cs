using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FireBaseProvider : Singleton<FireBaseProvider> {


    DataSnapshot snapshot;
    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://quizclash-dbca1.firebaseio.com/");

        DatabaseReference rootRef = FirebaseDatabase.DefaultInstance.RootReference;
        rootRef.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                 snapshot = task.Result;
                Debug.Log(task.Result.Child("1").Child("correct").Value);
             }
        });

  


    }//start
    Dictionary<string, int> dictionary =  new Dictionary<string, int>();
    //  [SerializeField]
   
    public string GetQuestion(int playerRating)
    {

        int result = ((int)Random.Range(0, playerRating ))/100;
        string difficulty="";
        if (result <= 0) difficulty = "easy";
        else if (result == 1) difficulty = "medium";
        else if (result >= 2) difficulty = "hard";

        return (string)snapshot.Child(difficulty)
            .Child(Random.Range(0, snapshot.Child(difficulty).ChildrenCount).ToString())
            .Child("question").Value;
    }//


   


}//class
