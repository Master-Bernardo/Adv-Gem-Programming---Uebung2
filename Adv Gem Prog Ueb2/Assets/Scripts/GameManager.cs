using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public static GameManager Instance;

    //players
    public GameObject player1Spawnpoint;
    public GameObject player1Prefab;
    private GameObject currentPlayer1Object;

    public GameObject player2Spawnpoint;
    public GameObject player2Prefab;
    private GameObject currentPlayer2Object;

    //cameras
    public GameObject mainCamera;
    public GameObject player1Camera;
    public GameObject player2Camera;


    private State state;
    private bool player1Alive;
    private bool player2Alive;
    private int player1Points = 0;
    private int player2Points = 0;

    //sound 
    [Space(10)]
    [Header("Sound")]
    [SerializeField]
    private AudioClip roundEndsSound;
    [SerializeField]
    private AudioClip winGameSound;
    [SerializeField]
    private AudioClip roundStartsSound;

    private AudioSource audioSource;

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
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Use this for initialization
    void Start() {
        state = State.INGAME;
        BeginnRound();
	}

    private enum State
    {
        INGAME,
        GAMEOVER
    }

    public void Update()
    {
        if(state == State.GAMEOVER)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
        }

    }

    private void BeginnRound()
    {
        state = State.INGAME;
        if (currentPlayer1Object != null) Destroy(currentPlayer1Object);
        if (currentPlayer2Object != null) Destroy(currentPlayer2Object);

        currentPlayer1Object = Instantiate(player1Prefab, player1Spawnpoint.transform.position, player1Spawnpoint.transform.rotation);
        currentPlayer2Object = Instantiate(player2Prefab, player2Spawnpoint.transform.position, player2Spawnpoint.transform.rotation);

        player1Alive = true;
        player2Alive = true;

        //assign Cameras
        mainCamera.GetComponent<Camera2PersonMain>().SetPlayer1Cam(player1Camera,currentPlayer1Object);
        mainCamera.GetComponent<Camera2PersonMain>().SetPlayer2Cam(player1Camera,currentPlayer2Object);

        UIController.Instance.RoundStart();

        audioSource.clip = roundStartsSound;
        audioSource.Play();
    }

    private void EndRound()
    {
        Debug.Log("round Ended");
        if (!player1Alive && player2Alive)
        {
            player2Points++;
            UIController.Instance.UpdatePlayerPoints(2, player2Points);
            player2Alive = false;
            UIController.Instance.PlayerWinsRound(2);
        }
        else if (player1Alive && !player2Alive)
        {
            player1Points++;
            UIController.Instance.UpdatePlayerPoints(1, player1Points);
            player1Alive = false;
            UIController.Instance.PlayerWinsRound(1);
        }
        else
        {
            //draw
            UIController.Instance.Draw();
            player1Alive = false;
            player2Alive = false;
        }
        //change the Win Text, display it


        //Make Enumerator to beginn next Round after 5 Seconds
        if (player1Points < 3 && player2Points < 3)
        {
            StartCoroutine("BeginnRoundDelayed");
            audioSource.clip = roundEndsSound;
            audioSource.Play();
        }
        else
        {
            state = State.GAMEOVER;
            if (player1Points == 3) UIController.Instance.PlayerWinsGame(1);
            if (player2Points == 3) UIController.Instance.PlayerWinsGame(2);
            audioSource.clip = winGameSound;
            audioSource.Play();
        }

      
    }

    IEnumerator BeginnRoundDelayed()
    {
        yield return new WaitForSeconds(6.5f);
        BeginnRound();
    }

    IEnumerator EndRoundDelayed()
    {
        yield return new WaitForSeconds(2f);
        EndRound();
    }

    public void PlayerDied(int playerNumber)
    {
        if (playerNumber == 1) player1Alive = false;
        else if (playerNumber == 2) player2Alive = false;
        else Debug.Log("wrong Player Number - playerDieds");

        if (state == State.INGAME)
        {
            StartCoroutine("EndRoundDelayed");
            state = State.GAMEOVER;
        }

    }

}
