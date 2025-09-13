using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int colorIndex; // 1-4 соответствуют цветам лампочки
    public int sequenceOrder; // Порядок в последовательности

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.CheckpointReached(this);
        }
    }
}