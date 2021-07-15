using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class testVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    
    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        rawImage = this.GetComponent<RawImage>();
    }
    
    void Update()
    {
        if (videoPlayer.texture == null)
        {
            return;
        }

        //把VideoPlayerd的视频渲染到UGUI的RawImage
        rawImage.texture = videoPlayer.texture;

    }
}
