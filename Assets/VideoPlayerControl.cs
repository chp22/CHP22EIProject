using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void StartVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
        }
    }
}