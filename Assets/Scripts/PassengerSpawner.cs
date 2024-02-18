using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public int rows = 5;
    public int columns = 2;
    public float spacing = 1.0f; // Adjust the spacing to your preference
    private GameObject[,] passengers; // Use a 2D array to represent the grid

    private void Start()
    {
        passengers = new GameObject[columns, rows]; // Initialize the array
        SpawnPassengers();
    }

    private void SpawnPassengers()
    {
        passengers = new GameObject[columns, rows]; // Re-initialize the array for safety

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate the position for each passenger
                Vector3 position = new Vector3(col * spacing, row * -spacing, 0); // Negative for y to go down

                // Instantiate the passenger
                GameObject newPassenger = Instantiate(passengerPrefab, position, Quaternion.identity, transform);
                passengers[col, row] = newPassenger; // Store the passenger in the array

                // If the passenger is on the left side (col == 0), flip the sprite
                SpriteRenderer spriteRenderer = newPassenger.GetComponent<SpriteRenderer>();
                if (col == 0 && spriteRenderer != null)
                {
                    spriteRenderer.flipX = true; // Flip the sprite to face right
                }
            }
        }
    }

    // Call this method to make a passenger fall off
    public void MakePassengerFallOff()
    {
        // Find a passenger that is still active to fall off
        for (int i = 0; i < columns * rows; i++)
        {
            int col = i % columns;
            int row = i / columns;
            GameObject passenger = passengers[col, row];

            if (passenger != null && passenger.activeSelf)
            {
                passenger.GetComponent<Passenger>().FallOff();
                passengers[col, row] = null; // Set the array element to null after making the passenger fall off
                break; // Only remove one passenger per call
            }
        }
    }
}
