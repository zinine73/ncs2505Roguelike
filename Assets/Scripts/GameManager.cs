using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager {get; private set;}
    public UIDocument UIDoc;

    VisualElement gameOverPanel;
    Label gameOverMessage;
    Label foodLabel;
    int foodAmount = 20;
    int currentLevel = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;
        foodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        gameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        gameOverMessage = gameOverPanel.Q<Label>("GameOverMessage");
        StartNewGame();
    }

    public void StartNewGame()
    {
        gameOverPanel.style.visibility = Visibility.Hidden;
        currentLevel = 0;
        foodAmount = 20;
        NewLevel();
        ChangeFood(0);
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        foodLabel.text = $"Food:{foodAmount:000}";
        if (foodAmount <= 0)
        {
            PlayerController.GameOver();
            gameOverPanel.style.visibility = Visibility.Visible;
            gameOverMessage.text = 
                $"Game Over!\n\nYou traveled through {currentLevel} levels";
        }
    }

    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
        currentLevel++;
    }
}
