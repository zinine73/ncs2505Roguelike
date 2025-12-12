using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager {get; private set;}
    public UIDocument UIDoc;

    Label foodLabel;
    int foodAmount = 100;

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
        foodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        ChangeFood(0);
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        foodLabel.text = $"Food:{foodAmount:000}";
    }
}
