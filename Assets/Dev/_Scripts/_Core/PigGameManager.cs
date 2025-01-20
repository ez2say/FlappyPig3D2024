using UnityEngine;

public class PigGameManager : MonoBehaviour
{
    public Animator pigAnimator;
    public BirdController birdController;
    public GameObject startButton;
    public GameObject exitButton;
    public RoadGenerator roadGenerator;
    public Rigidbody pigRigidbody;
    public float forwardImpulse = 5f;

    private bool isGameStarted = false;

    void Start()
    {
        birdController.enabled = false;

        pigAnimator.ResetTrigger("StartGame");
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

            // Запускаем генерацию дороги
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
    }
}