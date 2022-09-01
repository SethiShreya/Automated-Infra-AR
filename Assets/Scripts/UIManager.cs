using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Transform TimeLeftBlock;
    public TextMeshProUGUI scoreText;
    private int scores=0;
    public GameObject gameOver;
    public TextMeshProUGUI gameOverScoreText;
    private SoundManager soundManager;
    public AudioClip gameOverClip;
    public bool isGameOver=false;
    public BackGroundMusic groundMusic;
    public BlockHolder blockHolder;

    private void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        blockHolder = GameObject.FindObjectOfType<BlockHolder>();
        groundMusic = GameObject.FindObjectOfType<BackGroundMusic>();
        StartCoroutine(TimeLeft());
    }

    IEnumerator TimeLeft()
    {
        int time = 0;
        while (time <= 120)
        {
            yield return new WaitForSeconds(1);
            TimeLeftBlock.localScale -= new Vector3(0.041666f, 0);
            time++;
           
        }
        GameOver();
        groundMusic.StopBgMusic();
        blockHolder.DeleteAllChildren();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayAgain();
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    void GameOver()
    {
        soundManager.GetComponent<SoundManager>().SelectAudio(clip: gameOverClip);
        gameOver.SetActive(true);
        gameOverScoreText.text = scores.ToString();
    }

    public void IncreaseScore(int multiples=10)
    {
        scores  += multiples;
        scoreText.text = scores.ToString();
    }
}
