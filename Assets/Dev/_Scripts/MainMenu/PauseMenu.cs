using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private GameObject _deathPanel;
    
    private BirdController _birdController;
    private bool isPaused = false;

    private TextMeshProUGUI _scoreText;

    public void Initialize(BirdController birdController, ScoreManager scoreManager)
    {
        _birdController = birdController;
        _birdController.RegisterDeathListener(() => 
        {
            int currentScore = scoreManager.GetScore();
            ShowDeathPanel(currentScore);
        });

        _scoreText = _deathPanel.GetComponentInChildren<TextMeshProUGUI>(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        TimeStop();

        pauseMenuPanel.SetActive(true);

        isPaused = true;
    }
    
    public void DeathPanel()
    {
        _deathPanel.SetActive(false);
    }

    private void TimeStop()
    {
        Time.timeScale = 0f;
    }

    private void TimeResume()
    {
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        TimeResume();

        pauseMenuPanel.SetActive(false);

        isPaused = false;
    }

    public void RestartGame()
    {
        TimeResume();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSettings()
    {
       _settingsPanel.SetActive(!_settingsPanel.activeSelf);
    }

    public void BackToMainMenu()
    {
        TimeResume();

        SceneManager.LoadScene("Level_v1.0");
    }

    public void ShowDeathPanel(int score)
    {
        TimeStop();
        _settingsPanel.SetActive(false);
        _deathPanel.SetActive(true);
        _scoreText.text = $"{score}";
    }
}