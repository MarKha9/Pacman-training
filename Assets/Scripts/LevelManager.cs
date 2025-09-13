using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LightController lightController;
    public Transform checkpointsParent;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private int currentExpectedCheckpoint = 0;
    private int currentLevel = 0;
    private Vector3 playerStartPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        // Находим все чекпоинты на уровне
        checkpoints.Clear();
        foreach (Transform child in checkpointsParent)
        {
            Checkpoint checkpoint = child.GetComponent<Checkpoint>();
            if (checkpoint != null)
            {
                checkpoints.Add(checkpoint);
            }
        }

        // Находим стартовую позицию игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStartPosition = player.transform.position;
        }

        // Начинаем последовательность лампочки
        currentExpectedCheckpoint = 0;
        lightController.StartLevelSequence(currentLevel);
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        int[] expectedSequence = lightController.GetCurrentSequenceColors();

        if (currentExpectedCheckpoint < expectedSequence.Length &&
            checkpoint.colorIndex == expectedSequence[currentExpectedCheckpoint])
        {
            // Правильная точка
            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position);
            currentExpectedCheckpoint++;

            // Проверяем, завершен ли уровень
            if (currentExpectedCheckpoint >= expectedSequence.Length)
            {
                LevelComplete();
            }
        }
        else
        {
            // Неправильная точка
            AudioSource.PlayClipAtPoint(wrongSound, Camera.main.transform.position);
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerStartPosition;
        }
    }

    private void LevelComplete()
    {
        lightController.StopSequence();

        // Здесь показываем экран с элементом иллюстрации
        ShowMemoryFragment(currentLevel);

        // Переходим к следующему уровню
        currentLevel++;
        if (currentLevel < 7)
        {
            Invoke("LoadNextLevel", 3f); // Задержка перед загрузкой следующего уровня
        }
        else
        {
            // Конец игры
            Invoke("ShowEnding", 3f);
        }
    }

    private void ShowMemoryFragment(int levelIndex)
    {
        // Реализация показа фрагмента памяти/иллюстрации
        Debug.Log("Показан фрагмент памяти для уровня " + levelIndex);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ShowEnding()
    {
        // Показ финальной заставки
        Debug.Log("Игра завершена!");
    }
}