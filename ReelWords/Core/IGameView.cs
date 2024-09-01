namespace ReelWords.Core;

public interface IGameView
{
    public void Init(int reelsCount);
    public void UpdateScore(int newScore);
    public void UpdateStatus(string status);
    public void AnimateShift(int index, char letter);
    public void EndGame(string message);
}