using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	[SerializeField]
	Text results;

	void Start() {
		if (GameManager.Instance.WonGame) {
			results.text = "Congratulations!\nYou fought back the waves of sea creatures and protected the castle.";
			GetComponentInChildren<Image> ().color = new Color(51f / 255f, 81f/255f, 217f/255f);;
		} else {
			results.text = "The castle has fallen!\nBut all is not lost.  We can rebuild it and try again.  Will you join us?";
			GetComponentInChildren<Image> ().color = new Color (1f / 255f, 34f / 255f, 86 / 255f);
		}
		Destroy (GameManager.Instance.gameObject);
	}
	public void Quit() {
		Application.Quit ();
	}

	public void MainMenu() {
		SceneManager.LoadScene ("JDTestScene");
	}
}
