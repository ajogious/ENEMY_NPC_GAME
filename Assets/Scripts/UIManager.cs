using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject instructionPanel;
    public Slider playerHealthBar;
    public Slider enemyHealthBar;

    public void ToggleInstructions()
    {
        instructionPanel.SetActive(!instructionPanel.activeSelf);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
