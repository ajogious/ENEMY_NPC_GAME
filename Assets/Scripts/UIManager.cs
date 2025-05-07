using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject instructionPanel;

    [Header("Health Bars")]
    public Slider playerHealthBar;
    public Slider enemyHealthBar;

    // Toggle the instructions panel on/off
    public void ToggleInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(!instructionPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Instruction panel not assigned in UIManager.");
        }
    }

    // Resume game time
    public void StartGame()
    {
        Time.timeScale = 1f;
    }

    // Pause game time
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // Quit application
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    // Optionally update health bar UI from external calls
    public void UpdatePlayerHealth(float value)
    {
        if (playerHealthBar != null)
            playerHealthBar.value = value;
    }

    public void UpdateEnemyHealth(float value)
    {
        if (enemyHealthBar != null)
            enemyHealthBar.value = value;
    }
}
