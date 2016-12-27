using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestQuestion {

    QuizManager quizM;
    [SetUp]
    public void Init()
    {
        quizM = new QuizManager("hard");
        if(quizM.difficult=="easy")
        quizM.quesitonRank = 10;
       else if (quizM.difficult == "medium")
            quizM.quesitonRank = 20;
       else if (quizM.difficult == "hard")
            quizM.quesitonRank = 30;
    }


    [Test]
	public void AddScore_OnCorrect() {
        //Arrange


        //Act
        quizM.AddScore();
        
		//Assert

		Assert.AreEqual(quizM.quesitonRank, quizM.score);
	}
    [Test]
    public void DeductScore_OnWrong()
    {
        //Arrange


        //Act
        quizM.SubstractScore();

        //Assert

        Assert.AreEqual(-quizM.quesitonRank/2, quizM.score);
    }





}//class
