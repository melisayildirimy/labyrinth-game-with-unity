using UnityEngine;
using UnityEngine.UI;

public class ColorChangingSlider : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;

    private Color redColor = Color.red;
    private Color yellowColor = Color.yellow;
    private Color greenColor = Color.green;


    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        UpdateSliderColors(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateSliderColors(value);
    }

    private void UpdateSliderColors(float value)
    {
        if (value >= 0.5f)
        {
            fillImage.color = Color.Lerp(yellowColor, greenColor, (value - 0.5f) / 0.5f);
        }
        else if (value >= 0.25f)
        {
            fillImage.color = Color.Lerp(redColor, yellowColor, (value - 0.25f) / 0.25f);
        }
        else
        {
            fillImage.color = Color.Lerp(Color.black, redColor, value / 0.25f);
        }
    }
}
