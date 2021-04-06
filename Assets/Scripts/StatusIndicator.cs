using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private TMP_Text healthText;

    private void Start()
    {
        if (healthBarRect == null)
            Debug.LogError("Status Indicator: No health bar object referenced!");
        if (healthText == null)
            Debug.LogError("Status Indicator: No health text object referenced!");
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float) _cur / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";

        Image image = healthBarRect.GetComponent<Image>();
        if (_cur <= 0.25 * _max)
            image.color = new Color(255, 0, 0);  //Health Bar colour = red if health = 25% of max
        else if (_cur <= 0.5 * _max)
            image.color = new Color(229, 207, 0); //Health Bar colour = yellow if health = 50% of max
    }



}
