using System.Collections.Generic;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField]
    private Vector2 Size = new Vector2(16f, 9f);

    [SerializeField]
    private GameObject fishPrefab = null;

    [SerializeField]
    private Camera myCamera;
    private FishTank [] fishes = null;
    private List<Fish> fishList = new List<Fish>();

    [Range(0, 300)]
    [SerializeField]
    private int SpawningCount;

    private Fish[] fishes = null;

    private void Start()
    {
        fishes = new Fish[SpawningCount];
        for (int i = 0; i < SpawningCount; i++)
        {
            CreateFish(Vector3.zero);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = myCamera.ScreenToWorldPoint(mousePosition);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, 1f);

            for (int i = 0; i < fishesThatWillBeDestroyed.Length; i++)
            {
                colliders2D fishesThatWillBeDestroyed = fishesThatWillBeDestroyed[i];
            }
        }
    }
   
    private void CreateFish(Vector3 WorldPosition)
    
    {
          GameObject fishInstance = Instantiate(fishPrefab, transform);
            fishInstance.gameObject.name = $"Fish {System.Guid.NewGuid()}";
            fishList.Add(fishInstance.GetComponent<Fish>());
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = myCamera.ScreenToWorldPoint(mousePosition);
        }
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -myCamera.transform.position.z;
            Vector3 worldPosition = myCamera.ScreenToWorldPoint(mousePosition);

            foreach (Fish fish in fishes)
            {
                Vector3 direction = worldPosition - fish.transform.position;
                fish.velocity += direction.normalized * 0.1f;
            }
        }
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