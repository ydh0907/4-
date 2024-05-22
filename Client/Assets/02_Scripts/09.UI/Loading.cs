using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loadingImg;

    private RectTransform rect;
    private float currentTime;

    private void Awake()
    {
        rect = loadingImg.GetComponent<RectTransform>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        float degrees = GetDegrees();
        if (degrees >= 360)
        {
            currentTime = 0;
            degrees = GetDegrees();
        }
        rect.rotation = Quaternion.Euler(0, 0, -degrees);
    }

    private float GetDegrees()
    {
        float degrees = Mathf.Lerp(0, 360, currentTime / 2);
        return degrees;
    }
}
