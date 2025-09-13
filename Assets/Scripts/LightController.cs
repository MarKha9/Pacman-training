using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [System.Serializable]
    public class LightSequence
    {
        public Color[] colors;
        public float[] durations;
    }

    public LightSequence[] levelSequences; // Последовательности для каждого уровня
    public Image lightImage; // Ссылка на Image компонент лампочки
    public Color defaultColor = new Color(0.608f, 0.608f, 0.608f); // 9b9b9b
    public float defaultDuration = 3f;

    private int currentLevel = 0;
    private int currentStep = 0;
    private bool isPlayingSequence = false;

    private void Start()
    {
        // Инициализируем цвета из задания
        Color yellow = new Color(1f, 0.914f, 0f);     // ffe900
        Color blue = new Color(0.067f, 0.573f, 0.855f);   // 1192da
        Color pink = new Color(0.91f, 0f, 0.506f);    // e80081
        Color green = new Color(0.067f, 0.855f, 0.11f);   // 11da1c

        // Создаем последовательности для каждого уровня согласно ТЗ
        levelSequences = new LightSequence[7];

        // Уровень 1: 1, Д
        levelSequences[0] = new LightSequence()
        {
            colors = new Color[] { yellow, defaultColor },
            durations = new float[] { 1f, defaultDuration }
        };

        // Уровень 2: 1,2, Д
        levelSequences[1] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, defaultColor },
            durations = new float[] { 1f, 1f, defaultDuration }
        };

        // Уровень 3: 1,2,3, Д
        levelSequences[2] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, pink, defaultColor },
            durations = new float[] { 1f, 1f, 1f, defaultDuration }
        };

        // Уровень 4: 1,2,3,2, Д
        levelSequences[3] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, pink, blue, defaultColor },
            durations = new float[] { 1f, 1f, 1f, 1f, defaultDuration }
        };

        // Уровень 5: 1,2,3,2,4, Д
        levelSequences[4] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, pink, blue, green, defaultColor },
            durations = new float[] { 1f, 1f, 1f, 1f, 1f, defaultDuration }
        };

        // Уровень 6: 1,2,3,2,4,1, Д
        levelSequences[5] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, pink, blue, green, yellow, defaultColor },
            durations = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, defaultDuration }
        };

        // Уровень 7: 1,2,3,2,4,1,2, Д
        levelSequences[6] = new LightSequence()
        {
            colors = new Color[] { yellow, blue, pink, blue, green, yellow, blue, defaultColor },
            durations = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, defaultDuration }
        };
    }

    public void StartLevelSequence(int levelIndex)
    {
        currentLevel = levelIndex;
        currentStep = 0;
        isPlayingSequence = true;
        PlayNextStep();
    }

    private void PlayNextStep()
    {
        if (!isPlayingSequence || currentLevel >= levelSequences.Length) return;

        LightSequence sequence = levelSequences[currentLevel];

        if (currentStep < sequence.colors.Length)
        {
            lightImage.color = sequence.colors[currentStep];
            Invoke("PlayNextStep", sequence.durations[currentStep]);
            currentStep++;
        }
        else
        {
            isPlayingSequence = false;
        }
    }

    public void StopSequence()
    {
        isPlayingSequence = false;
        CancelInvoke("PlayNextStep");
        lightImage.color = defaultColor;
    }

    public int[] GetCurrentSequenceColors()
    {
        // Возвращает массив индексов цветов текущей последовательности (без дефолтного)
        if (currentLevel >= levelSequences.Length) return new int[0];

        LightSequence sequence = levelSequences[currentLevel];
        int[] colorIndices = new int[sequence.colors.Length - 1]; // -1 чтобы исключить дефолтный

        for (int i = 0; i < colorIndices.Length; i++)
        {
            // Здесь нужно преобразовать Color в индекс (1-4)
            // Это упрощенная версия - в реальности нужно сравнивать цвета
            colorIndices[i] = i % 4 + 1;
        }

        return colorIndices;
    }
}