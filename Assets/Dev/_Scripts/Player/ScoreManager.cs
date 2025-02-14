using TMPro;

public class ScoreManager
{
    private TextMeshProUGUI _scoreText;
    private int _score = 0;

    public void SetScoreText(TextMeshProUGUI text)
    {
        _scoreText = text;
        UpdateScoreText();
    }

    public void AddScore(int point)
    {
        _score = point;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = $"{_score}";
    }
}
