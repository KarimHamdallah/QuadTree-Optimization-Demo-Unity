using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display_Script : MonoBehaviour
{
    public TextMeshProUGUI CountText;
    public Toggle Toggle;
    public Player_Script Player;

    private void Start()
    {
        Toggle.onValueChanged.AddListener(UseQuadTreeOptimization);
    }

    void OnEnable()
    {
        Player_Script.OnDistanceCountChanged += UpdateDisplay;
    }

    void OnDisable()
    {
        Player_Script.OnDistanceCountChanged -= UpdateDisplay;
    }

    void UpdateDisplay(int value)
    {
        CountText.text = value.ToString();
    }

    void UseQuadTreeOptimization(bool value)
    {
        Player.SetUseQuadTreeOptimization(value);
    }
}
