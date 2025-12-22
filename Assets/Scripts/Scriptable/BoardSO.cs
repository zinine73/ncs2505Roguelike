using UnityEngine;

[CreateAssetMenu(fileName = "BoardSO", menuName = "Scriptable Objects/BoardSO")]
public class BoardSO : ScriptableObject
{
    public BoardStuff[] boardStuffs;
    [System.Serializable]
    public struct BoardStuff
    {
        public int width;
        public int height;
        public int walls;
        public int minFood;
        public int maxFood;
        public Enemy[] enemies;
    }
}
