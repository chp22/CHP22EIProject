using UnityEngine;
using UnityEngine.UI; // Required for Button
using UnityEngine.Video; // Required for VideoPlayer

public class PlayVideoButton : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Drag the Video Player GameObject here in the Inspector

    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }
}