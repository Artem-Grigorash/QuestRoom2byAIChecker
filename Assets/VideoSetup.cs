using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoSetup : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string videoFileName;

    private void Awake()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoFileName);
    }
}