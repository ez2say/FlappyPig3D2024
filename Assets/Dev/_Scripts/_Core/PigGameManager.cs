using UnityEngine;
using TMPro;

public class PigGameManager : MonoBehaviour
{
    public Animator pigAnimator;
    public BirdController birdController;
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject apple;

    public TextMeshProUGUI text;
    public RoadGenerator roadGenerator;
    public Rigidbody pigRigidbody;
    public float forwardImpulse = 5f;

    private bool isGameStarted = false;

    void Start()
    {
        birdController.enabled = false;

        pigAnimator.ResetTrigger("StartGame");
        
        text.gameObject.SetActive(false);

        apple.SetActive(false);
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            startButton.SetActive(false);
            exitButton.SetActive(false);

            pigAnimator.SetTrigger("StartGame");

            Invoke("EnableBirdController", pigAnimator.GetCurrentAnimatorStateInfo(0).length);

            text.gameObject.SetActive(true);

            apple.SetActive(true);

            roadGenerator.StartRoadGeneration();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void EnableBirdController()
    {
        birdController.enabled = true;

        pigAnimator.enabled = false;

        birdController.SetScoreText(text);
    }
}