using UnityEngine;

public class PigGameManager : MonoBehaviour
{
    public Animator pigAnimator;
    public BirdController birdController;
    public GameObject startButton;
    public GameObject exitButton;
    public RoadGenerator roadGenerator; // Ссылка на RoadGenerator
    public Rigidbody pigRigidbody; // Ссылка на Rigidbody свинки
    public float forwardImpulse = 5f; // Импульс для движения вперед

    private bool isGameStarted = false;

    void Start()
    {
        // Изначально отключаем управление свинкой
        birdController.enabled = false;

        // Очищаем параметр StartGame, чтобы избежать автоматического запуска анимации
        pigAnimator.ResetTrigger("StartGame");
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            startButton.SetActive(false);
            exitButton.SetActive(false);

            // Запускаем анимацию подпрыгивания и разворота свинки
            pigAnimator.SetTrigger("StartGame");

            // После завершения анимации включаем управление свинкой
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