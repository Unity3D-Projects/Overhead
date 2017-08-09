﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardControllerScript : MonoBehaviour {

    [SerializeField] GameObject playerScoreCanvas;
    [SerializeField] GameObject connectionErrorCanvas;
    [SerializeField] GameObject offlineScoreCanvas;
    [SerializeField] GameObject leaderBoadCanvas;
    [SerializeField] GameObject leaderBoadCanceledCanvas;
    [SerializeField] GameObject list;
    [SerializeField] GameObject listC;

    private GameObject nameList;
    private GameObject waveList;
    private GameObject scoreList;
    private GameObject nameListC;
    private GameObject waveListC;
    private GameObject scoreListC;
    private HighScores highScores;
	private DeathManager deathManager;
	private Fading fading;
	private bool cancel = false; 
	private float wave;
	private float score;
	private string name;

    public void SetPlayerScoreCanvasActive(bool setActive)
    {
        playerScoreCanvas.SetActive(setActive);
    }

    public void SetConnectionErrorCanvas(bool setActive)
    {
        connectionErrorCanvas.SetActive(setActive);
    }

    public void SetOfflineScoreCanvas(bool setActive)
    {
        offlineScoreCanvas.SetActive(setActive);
    }

    public void SetleaderBoadCanvas(bool setActive)
    {
        leaderBoadCanvas.SetActive(setActive);
    }

    public void SetleaderBoadCanceledCanvas(bool setActive)
    {
        leaderBoadCanceledCanvas.SetActive(setActive);
    }

    public void GetInput(string _name){
		name = _name;
		Debug.Log (name);
		UploadHighscore();
	}

	/// <summary>
	/// Determines whether this instance cancel with no error.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel with no error; otherwise, <c>false</c>.</returns>
	public void CancelWithNoError(){
		cancel = true;
		fading.DisappearPlayerScoreCanvas ();
		fading.AppearLeaderBoadCanceledCanvas ();
	}

	/// <summary>
	/// Determines whether this instance cancel due to error.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel due to error; otherwise, <c>false</c>.</returns>
	public void CancelDueToError(){
		cancel = true;
		fading.DisappearConnectionErrorMessageCanvas ();
		fading.AppearOfflineScoreCanvas ();
	}

	/// <summary>
	/// Connections the error.
	/// </summary>
	/// <param name="ErrorMessage">Error message.</param>
	public void ConnectionError(string ErrorMessage){
		Debug.Log ("Upload failed: " + ErrorMessage);
		GameObject.Find("ErrorMessage").gameObject.GetComponent<Text>().text = ("Error message: " + ErrorMessage);
	}

	public void UploadHighscore(){
        Debug.Log((int)score);
        fading.DisappearPlayerScoreCanvas();
		highScores.AddNewHighscore (name, (int)score);
	}

	public void OpenLeaderBoard()
    {
        if(cancel == false)
        {
            fading.AppearLeaderBoardCanvas();
            SetHighScoreBoard();
        }
        else
        {
            fading.AppearLeaderBoadCanceledCanvas();
            SetHighScoreBoardCancelled();
        }

	}

    public void SetWave(float _wave){
		wave = _wave;
	}

	public void SetScore(float _score){
		score = _score;
	}

	private void Start(){
		highScores = GameObject.Find ("GameMaster").GetComponent<HighScores> ();
		deathManager = GameObject.Find ("GameMaster").GetComponent<DeathManager> ();
		fading = GameObject.Find ("GameMaster").GetComponent<Fading> ();

        SetleaderBoadCanvas(true);
        SetOfflineScoreCanvas(true);
        nameList = GameObject.Find("NameList");
        waveList = GameObject.Find("WaveList");
        scoreList = GameObject.Find("ScoreList");
        nameListC = GameObject.Find("NameListC");
        waveListC = GameObject.Find("WaveListC");
        scoreListC = GameObject.Find("ScoreListC");
        SetleaderBoadCanvas(false);
        SetOfflineScoreCanvas(false);
    }

    private void SetHighScoreBoard()
    {
        PlayerDataCanvas[] playerDataCanvas = highScores.GetPlayerDataCanvas();

        for (int i = 0; i < 7; i ++)
        {
            nameList.transform.GetChild(i).gameObject.GetComponent<Text>().text = playerDataCanvas[i].GetPlayerName();
            scoreList.transform.GetChild(i).gameObject.GetComponent<Text>().text = playerDataCanvas[i].GetScore();
            waveList.transform.GetChild(i).gameObject.GetComponent<Text>().text = playerDataCanvas[i].GetWave();
        }



        //must have the system to verify if the player is a new highscore

    }

    private void SetHighScoreBoardCancelled()
    {
        PlayerDataCanvas[] playerDataCanvas = highScores.GetPlayerDataCanvas();


    }
}