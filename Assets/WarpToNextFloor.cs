using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpToNextFloor : MonoBehaviour
{
    [SerializeField] Light warpLight;
    [SerializeField] int floorLevelIndex = 0;
    [SerializeField] float warpDelay = 2f;
    [SerializeField] float minIntensity = 2500f;
    [SerializeField] float maxIntensity = 5000f;
    float time;
    int playerCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount < 2)
            {
                time = 0;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerCount > 2)
        {
            time += Time.deltaTime;
            warpLight.intensity = Mathf.Lerp(warpLight.intensity, maxIntensity, Time.deltaTime);
            if (time >= warpDelay)
            {
                time = 0;
                Warp();
            }
        }
        else
        {
            time = 0;
            warpLight.intensity = Mathf.Lerp(warpLight.intensity, minIntensity, Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount < 2)
            {
                time = 0;
            }
        }
    }
    void Warp()
    {
        GameEvents.FloorLevel++;
        SceneManager.LoadScene(floorLevelIndex);
    }
}
