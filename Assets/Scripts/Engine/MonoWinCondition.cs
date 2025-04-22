using UnityEngine;

public abstract class MonoWinCondition : MonoBehaviour
{
    public bool IsWinConditionPassed() => GetConditionPassedDays >= Constants.WinConditionLength[DifficultySelector.SelectedDifficulty];
    protected int GetConditionPassedDays { get; set; }
    protected abstract void SetConditionPassedDays();
}
