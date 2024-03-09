using UnityEngine;

[CreateAssetMenu(fileName = "GameOverDescriptor", menuName = "Level System/Game Over Descriptor")]
public class GameOverDescriptor : ScriptableObject
{
    public int amountPassed;
    public int amountDetroyed;
}
