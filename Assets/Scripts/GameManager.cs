using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefeb;
    public GameObject playerPrefeb;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highscoreText;
    public AudioSource audioSource;

    public GameObject panalMenu;
    public GameObject panalPlay;
    public GameObject panalLevelCompleted;
    public GameObject panalGameOver;

    public GameObject[] levels;

    public static GameManager Instance{ get; private set; }

    public enum State {MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;
    GameObject _currentball;
    GameObject _currentLevel;
    bool _isSwitchingState;
    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value;
            scoreText.text = "SCORE: " + _score;
        }
       
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set { _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }
    }


    public void PlayClicked() {
        SwitchState(State.INIT);
    
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void SwitchState(State newState, float delay = 0) {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay) {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState) {

        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                panalMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panalPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                if (_currentLevel != null) {
                    Destroy(_currentLevel);
                }
                Instantiate(playerPrefeb);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentball);
                Destroy(_currentLevel);
                Level++;
                SwitchState(State.LOADLEVEL, 2f);
                panalLevelCompleted.SetActive(true);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("highscore")) {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panalGameOver.SetActive(true);
                break;
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (_currentball == null) {
                    if (Balls > 0) {
                       _currentball = Instantiate(ballPrefeb);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);

                    }
                }
                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState) {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown) {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState() {
        switch (_state)
        {
            case State.MENU:
                panalMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panalLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panalPlay.SetActive(false);
                panalGameOver.SetActive(false);
                break;
        }

    }
    
}
