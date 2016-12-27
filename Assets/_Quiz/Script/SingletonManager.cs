using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour {

   
    public GameObject gameM;
    public GameObject quizM;
       public GameObject audioP;

    // Use this for initialization
    void Awake () {
        //OnExist(gameM);
       // OnExist(quizM);
        OnExist(audioP);

      //  GameManager.Instance.Init();
    }//start

    private void OnExist(GameObject obj)
    {
        if (GameObject.Find("(singleton) " + obj.name) != null)
        {
            Destroy(obj);
        }
        else {
            obj.SetActive(true);
        }
    }//exist?
	
	
}//class
