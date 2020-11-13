using UnityEngine;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    [SerializeField] private int score;
    [SerializeField] private int maxScore;
    [SerializeField] private int target;
    private int star;
    [SerializeField] private float timeGameplay;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private Camera camera;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        CheckCondition();
        DecreaseTime();
    }

    public PlayerBehaviour GetPlayer()
    {
        return player;
    }

    public Transform GetCamera()
    {
        return camera.transform;
    }

    public int Score { get => score; set => score = value; }
    public int MaxScore { get => maxScore; set => maxScore = value; }
    public int Star { get => star; set => star = value; }
    public float TimeGameplay { get => timeGameplay; set => timeGameplay = value; }

    public void AddScore(int _score)
    {
        score += _score;
    }

    public void CheckCondition()
    {
        if (Score >= target) star = 3;
        else if (Score >= target / 2) star = 2;
        else if (Score >= target / 4) star = 1;
        else star = 0;
    }

    public void DecreaseTime()
    {
        if (timeGameplay > 1) timeGameplay -= Time.deltaTime;
    }

    public void SaveData()
    {
        if (GameVariables.ACTIVE_LEVEL != null)
        {
            GameVariables.ACTIVE_SECTION_DATA.levelInfo.First(e => e.levelID == GameVariables.ACTIVE_LEVEL.levelID).coinCollect = GameVariables.ACTIVE_LEVEL.coinCollect;
            GameVariables.ACTIVE_LEVEL = null;
        }
    }

    public bool CheckScore()
    {
        return (score >= maxScore) ? true : false;
    }

    public bool IsNull()
    {
        return (Star == 0) ? true : false;
    }
}
