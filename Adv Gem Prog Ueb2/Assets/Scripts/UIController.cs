using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public static UIController Instance;

    [Header("Health and Mannabars")]
    public RectTransform healthbarPlayer1;
    public RectTransform mannabarPlayer1;
    public RectTransform healthbarPlayer2;
    public RectTransform mannabarPlayer2;

    [Header("Points & Wins")]
    public Text player1Points;
    public Text player2Points;
    public GameObject roundStartEndText;


    public void Awake() // wir setzen sicher dass es immer existier aber immer nur eins
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance); // es kann passieren wenn wir eine neue Scene laden dass immer noch eine Instanz existiert
        }
        else
        {
            Instance = this;
        }
    }

   

    public void UpdatePlayerPoints(int playerNumber, int points)
    {
        if (playerNumber == 1) player1Points.text = points.ToString();
        if (playerNumber == 2) player2Points.text = points.ToString();
    }

    public void RoundStart()
    {
        roundStartEndText.SetActive(true);
        roundStartEndText.GetComponent<Text>().text = "Round Starts";
        StartCoroutine("HideRoundText");
    }

    public void Draw()
    {
        roundStartEndText.SetActive(true);
        roundStartEndText.GetComponent<Text>().text = "Draw";
        StartCoroutine("HideRoundText");
    }

    public void PlayerWinsRound(int playerNumber)
    {
        roundStartEndText.SetActive(true);
        if (playerNumber == 1) roundStartEndText.GetComponent<Text>().text = "Player 1 Wins the Round";
        else if (playerNumber == 2) roundStartEndText.GetComponent<Text>().text = "Player 2 Wins the Round";
        StartCoroutine("HideRoundText");
    }

    public void PlayerWinsGame(int playerNumber)
    {
        roundStartEndText.SetActive(true);
        roundStartEndText.GetComponent<Text>().fontSize = 21;
        if (playerNumber == 1) roundStartEndText.GetComponent<Text>().text = "Player 1 Wins, hit r to restart, esc to go back";
        else if (playerNumber == 2) roundStartEndText.GetComponent<Text>().text = "Player 2 Wins, hit r to restart, esc to go back";
        StopCoroutine("HideRoundText");
    }

    IEnumerator HideRoundText()
    {
        yield return new WaitForSeconds(4f);
        roundStartEndText.SetActive(false);
    }

    public void UpdateMannaBar(int playerNumber, int value)
    {
        if (playerNumber == 1)
        {
            mannabarPlayer1.sizeDelta = new Vector2(value, mannabarPlayer1.sizeDelta.y);
        }
        else if (playerNumber == 2)
        {
            mannabarPlayer2.sizeDelta = new Vector2(value, mannabarPlayer2.sizeDelta.y);
        }
        else
        {
            Debug.Log("wrong player number-updateMannaBar");
        }
    }

    public void UpdateHealthBar(int playerNumber, int value)
    {
        if (playerNumber == 1)
        {
            healthbarPlayer1.sizeDelta = new Vector2(value, healthbarPlayer1.sizeDelta.y);
        }
        else if (playerNumber == 2)
        {
            healthbarPlayer2.sizeDelta = new Vector2(value, healthbarPlayer2.sizeDelta.y);
        }
        else
        {
            Debug.Log("wrong player number- updateHealth");
        }
    }
}
