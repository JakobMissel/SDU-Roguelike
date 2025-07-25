using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    
    private float _pollingTime = 1f;
    private float _time;
    private int _frameCount;

    void Update()
    {
        _time += Time.deltaTime;

        _frameCount++;

        if(_time >= _pollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount / _time);
            fpsText.text = "FPS: " + frameRate.ToString();

            _time -= _pollingTime;
            _frameCount = 0;
        }

    }
}
