using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    private void Awake()
    {
        if (Instance != this) // Already an instance exists
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
