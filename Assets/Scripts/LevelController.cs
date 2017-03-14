using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    static GameController _gameController;

    private float _levelCompletion = 0;

    private bool _musicPlaying = true;
    private const bool levelDebug = true;

    // The amount of actions each player starts with, could be unique for each level.
    public int StartActionOne = 3;
    public int StartActionTwo = 3;

    public GameObject canvas;
    private Slider p1Health;
    private Slider p2Health;

    // Use this for initialization
    void Start()
    {
        //if (_gameController == null)
        //{
        //    _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //}

        _levelCompletion = 0;

        if (canvas == null && FindObjectOfType(typeof(Canvas)) == null)
        {
            canvas = (GameObject) Instantiate(Resources.Load("UI/BlankCanvas"));
            Debug.Log("Canvas instantiated");
        }
        else if (canvas == null)
        {
            canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;
            Debug.Log("canvas linked");
        }

        if(SceneManager.GetActiveScene().name != "OpeningScene")
        {
            // Programatically instantiate player health UI
            p1Health = ((GameObject) Instantiate(Resources.Load("UI/P1HealthBar"))).GetComponent<Slider>();
            p1Health.transform.SetParent(canvas.transform);
            p2Health = ((GameObject)Instantiate(Resources.Load("UI/P2HealthBar"))).GetComponent<Slider>();
            p2Health.transform.SetParent(canvas.transform);

            // TODO: finalize clean positioning
            Vector3 bottomLeft = new Vector3(p2Health.GetComponent<RectTransform>().rect.width / 2, 10, 0);
            Vector3 bottomRight = new Vector3(canvas.GetComponent<RectTransform>().rect.width - p2Health.GetComponent<RectTransform>().rect.width / 2, 10, 0);

            p1Health.GetComponent<RectTransform>().position = bottomLeft;
            p2Health.GetComponent<RectTransform>().position = bottomRight;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                Debug.Log("Initializing health for " + player.name);
                UpdatePlayerHealth(player.GetComponent<PlayerMovementController>().player, player.GetComponent<PlayerMovementController>().healthPoints);
            }
        }
    }

    void Update()
    {
        // On level complete
        if (_levelCompletion >= 100)
        {
            if (_gameController != null)
				_gameController.GotoScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (levelDebug)
        {
            if (Input.GetButtonDown("Start"))
            {
                if (_gameController != null)
					_gameController.GotoScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (Input.GetButtonDown("ToggleMusic"))
            {
                AudioSource music = GetComponent<AudioSource>();
                if (music != null)
                {
                    if (_musicPlaying) music.Stop();
                    else music.Play();
                    _musicPlaying = !_musicPlaying;
                }
            }
        }
    }

    /**
     *  Progresses through the level.
     */

    public void ProgressLevel(float percent)
    {
        _levelCompletion += percent;
    }

    public void ResetLevel()
    {
        _levelCompletion = 0;
        if (_gameController != null)
			_gameController.GotoScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdatePlayerHealth(PlayerID id, float hp)
    {
        if (id == PlayerID.P1)
            p1Health.value = hp / 100;
        else if (id == PlayerID.P2)
            p2Health.value = hp / 100;
    }
}