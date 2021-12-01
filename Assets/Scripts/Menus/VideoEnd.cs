using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEnd : MonoBehaviour
{
    [SerializeField] private VideoPlayer mVideoPlayer;

    private void Awake()
    {
        Cursor.visible = false;
        mVideoPlayer.loopPointReached += OnMovieFinished;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            mVideoPlayer.Stop();
            AudioManager.instance.Play("HeartBGM");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnMovieFinished(VideoPlayer player)
    {
        player.Stop();
        AudioManager.instance.Play("HeartBGM");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
