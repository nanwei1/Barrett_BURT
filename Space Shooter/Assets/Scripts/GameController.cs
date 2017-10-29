using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public GUIText scoreText;
	private int score;
	public GUIText restartText;
	public GUIText gameOverText;

	private bool gameOver;
	private bool restart;

	// Use this for initialization
	void Start () {
		score = 0;
		UpdateScore ();
		restartText.text = "";
		gameOverText.text = "";
		StartCoroutine (SpawnWaves ());	
		gameOver = false;
		restart = false;
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i=0; i<hazardCount; i++){
				Vector3 spawnPosition = new Vector3(Random.Range(-6,6), spawnValues.y, spawnValues.z);
				Quaternion spawnRotaion = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotaion);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	public void GameOver(){
		gameOver = true;
		gameOverText.text = "GAME OVER";
	}

	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Debug.Log ("You need to manually restart the game!");
				// Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}
