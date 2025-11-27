using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField]
    private Vector2 Size = new Vector2(16f, 9f);

    [SerializeField]
    private GameObject fishPrefab = null;

    [Range(0, 300)]
    [SerializeField]
    private int SpawningCount;

    private Fish[] fishes = null;

    private void Start()
    {
        fishes = new Fish[SpawningCount];
        for (int i = 0; i < SpawningCount; i++)
        {
            GameObject fishInstance = Instantiate(fishPrefab, transform);
            fishInstance.gameObject.name = $"Fish {System.Guid.NewGuid()}";
            fishes[i] = fishInstance.GetComponent<Fish>();
        }
    }

    private void LateUpdate()
    {
        // Loop around out of bound fishes.
        int fishesCount = fishes.Length;
        for (int i = 0; i < fishesCount; i++)
        {
            Fish fish = fishes[i];
            Vector3 position = fish.transform.localPosition;

            // Left border?
            if (position.x < -Size.x * 0.5f)
            {
                position.x += Size.x;
            }
            // Right border?
            else if (position.x > Size.x * 0.5f)
            {
                position.x -= Size.x;
            }

            // Top border?
            if (position.y > Size.y * 0.5f)
            {
                position.y -= Size.y;
            }
            // Bottom border?
            if (position.y <  -Size.y * 0.5f)
            {
                position.y += Size.y;
            }

            fish.transform.localPosition = position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Size);
    }
}