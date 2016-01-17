using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

	Dictionary<int, Combination> game;

	bool isPCTurn;
	bool isUserTurn;
	bool isResult;
	bool isNextLevel;
	bool isGameOver;
	bool isContinue;
	int numberOfTones;
	int points;

	[SerializeField]
	Text turnIndicator;
	[SerializeField]
	Text pointsIndicator;
	[SerializeField]
	Text endTurn;
	[SerializeField]
	Button endTurnButton;
	[SerializeField]
	Button continueButton;
	[SerializeField]
	Button resetButton;

	[SerializeField]
	GameObject closeInstructionsBtn;
	[SerializeField]
	GameObject instructions;

	[SerializeField]
	Text roundCounter;
	[SerializeField]
	GameObject roundCounterBG;


	List<string> pcMoves;
	List<string> playerMoves;
	// Use this for initialization
	void Start () {
		isPCTurn = true;
		isUserTurn = false;
		isResult = false;
		isNextLevel = false;
		isGameOver = false;
		isContinue = false;
		points = 0;
		game = new Dictionary<int, Combination>();
		numberOfTones = 1;
		continueButton.gameObject.SetActive(false);
		resetButton.gameObject.SetActive(false);
	}

	IEnumerator Controller(){
		roundCounter.gameObject.SetActive(true);
		roundCounterBG.SetActive(true);
		roundCounter.text = "Ready? "+Environment.NewLine+"Number of Tones = "+numberOfTones+""+Environment.NewLine+"Start!";
		yield return new WaitForSeconds(5f);
		roundCounter.gameObject.SetActive(false);
		roundCounterBG.SetActive(false);
		Debug.Log(numberOfTones.ToString());
		if(isPCTurn){
			PlayerController.Instance.DisableButtons();
			endTurnButton.gameObject.SetActive(false);
			SampleController.Instance.isReady = false;
			turnIndicator.text = "Turn: <color=red>PC</color>";
			pcMoves = GenerateSequence(numberOfTones);
			while(!SampleController.Instance.isReady){
				yield return null;
			}
			numberOfTones++;
			isPCTurn = false;
			isUserTurn = true;
			DebugList(pcMoves, "PC");
		}
		if(isUserTurn){
			//Debug.Log("Waiting for User");
			//CheckResult();
			PlayerController.Instance.EnableButtons();
			endTurnButton.gameObject.SetActive(true);
			PlayerController.Instance.RestartPlayerSequence();
			turnIndicator.text = "Turn: <color=green>User</color>";
		}
	}

	void DebugList(List<string> listToPrint, string turn){
		var result = String.Join(", ", listToPrint.ToArray());
		Debug.Log(turn+" "+result);
	}
	// Update is called once per frame
	void Update () {

	
	}

	public void CloseInstructions(){
		instructions.SetActive(false);
		closeInstructionsBtn.SetActive(false);
		StartCoroutine(Controller());
	}

	public void EndTurn(){
		isPCTurn = true;
		isUserTurn = false;
		playerMoves = PlayerController.Instance.GetPlayerSequence();
		DebugList(playerMoves, "Player");
		if(CheckResult(pcMoves,playerMoves).isWin){
			points = points + ((numberOfTones - 1) * 100);
			pointsIndicator.text = "Points <color=yellow>"+points+"</color>";
			StartCoroutine(Controller ());
		}else{
			endTurnButton.enabled = false;
			endTurn.text = "Game Over";
			pointsIndicator.text = "Points <color=yellow>"+points+"</color>";
			roundCounter.gameObject.SetActive(true);
			roundCounterBG.SetActive(true);
			continueButton.gameObject.SetActive(true);
			resetButton.gameObject.SetActive(true);
			roundCounter.text = "<color=red>GAME OVER</color>"+Environment.NewLine+"Total Points = "+points;
			var hiScore = 0;
			if(PlayerPrefs.HasKey("hiscore")){
				hiScore = PlayerPrefs.GetInt("hiscore");
			}
			if(points > hiScore){
				PlayerPrefs.SetInt("hiscore", points);
				roundCounter.text = roundCounter.text + Environment.NewLine + "<color=magenta> You have set a new personal record!</color>";
			}
		}
	}

	public void StartAgain(){
		endTurnButton.enabled = true;
		endTurn.text = "End Turn";
		roundCounter.gameObject.SetActive(false);
		roundCounterBG.SetActive(false);
		continueButton.gameObject.SetActive(false);
		resetButton.gameObject.SetActive(false);
		numberOfTones = 1;
		StartCoroutine(Controller());
    }

	public void BackToTitle(){
		Application.LoadLevel("Intro");
	}

	IEnumerator WaitForNextTurn(Action next){
		yield return null;
		next();
	}

	public Combination CheckResult(List<string> pcMoves, List<string> userMov){
		return new Combination(pcMoves, userMov);
	}

	public List<string> GenerateSequence(int numberOfTones){
		return SampleController.Instance.GenerateTones(numberOfTones);
	}




}

public class Combination{

	public List<string> moves;
	public bool         isWin;
	public List<string> userMoves;

	public Combination(){
		moves = new List<string>();
		isWin = false;
		userMoves = new List<string>();
	}

	public Combination(List<string> pcMoves, List<string> userMov){
		moves = pcMoves;
		userMoves = userMov;
		isWin = pcMoves.SequenceEqual(userMov);
	}

}
