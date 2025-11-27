using System.Collections.Generic;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField]
    private Vector2 Size = new Vector2(16f, 9f);

    [SerializeField]
    private GameObject fish = null;

    [Range(0, 300)]
    [SerializeField]
    private int SpawningCount;

    [SerializeField]
    private Camera myCamera;

    private List<Fish> fishes = new List<Fish>();

    private void Start()
    {
        for (int i = 0; i < SpawningCount; i++)
        {
            CreateFish(Vector3.zero);
        }
    }

    private void CreateFish(Vector3 worldPosition)
    {
        GameObject fishInstance = Instantiate(fish, transform);
        fishInstance.gameObject.name = $"Fish {System.Guid.NewGuid()}";
        fishInstance.transform.position = worldPosition;
        fishes.Add(fishInstance.GetComponent<Fish>());
    }

    private void LateUpdate()
    {
        // Add fish?
        if (Input.GetMouseButton(0))
        {
            // Get mouse position
            Vector3 mousePosition = Input.mousePosition;

            // Project into world space
            mousePosition = myCamera.ScreenToWorldPoint(mousePosition);

            // Instantiate new fish.
            CreateFish(mousePosition);
        }

        // Remove fish?
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse position
            Vector3 mousePosition = Input.mousePosition;

            // Project into world space
            mousePosition = myCamera.ScreenToWorldPoint(mousePosition);

            // Get list of fish(es)
            Collider2D[] fishesThatWillBeDestroyed = Physics2D.OverlapCircleAll(mousePosition, 1f);

            // Destroy fish(es)
            for (int i = 0; i < fishesThatWillBeDestroyed.Length; i++)
            {
                // Get single fish collider
                Collider2D fishThatWillBeDestroyed = fishesThatWillBeDestroyed[i];

                // Get its component
                Fish removedFish = fishThatWillBeDestroyed.GetComponent<Fish>();

                // Remove from list
                fishes.Remove(removedFish);

                // Bye bye!
                Destroy(removedFish.gameObject);
            }
        }

        // Loop around out of bound fishes.
        int fishesCount = fishes.Count;
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