using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public int rows = 5;
    public int columns = 2;
    public float spacing = 1.0f; // Adjust the spacing to your preference

    private void Start()
    {
        SpawnPassengers();
    }

    private void SpawnPassengers()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate the position for each passenger
                Vector3 position = new Vector3(col * spacing, row * -spacing, 0); // Negative for y to go down

                // Instantiate the passenger
                GameObject newPassenger = Instantiate(passengerPrefab, position, Quaternion.identity, transform);

                // If the passenger is on the left side (col == 0), flip the sprite
                if (col == 0)
                {
                    SpriteRenderer spriteRenderer = newPassenger.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.flipX = true; // Flip the sprite to face right
                    }
                }
            }
        }
    }
}
