using Unity.VisualScripting;

using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{

    private PlayerController playerController;
    public GameObject passengerPrefab;
    public int rows = 5;
    public int columns = 2;
    public float spacing = 1.0f; // Adjust the spacing to your preference
    private GameObject[,] passengers; // Use a 2D array to represent the grid

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
            // Use the player's health to determine the number of passengers
            int health = playerController.Health;
            int rows = 5; // Assuming you still want to arrange them in 5 rows
            int columns = Mathf.CeilToInt((float)health / rows); // Calculate columns needed

            passengers = new GameObject[columns, rows]; // Initialize the array based on health
            SpawnPassengers(health, rows, columns);
        }
        else
        {
            Debug.LogError("PlayerController not found on parent GameObject");
        }
    }

    private void SpawnPassengers(int health, int rows, int columns)
    {
        // Calculate an offset to center or move the grid of passengers more to the left
        float xOffset = -(columns * spacing / 2) + (spacing / 2); // Adjust this calculation as needed

        int passengerCount = 0; // Keep track of how many passengers have been instantiated

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (passengerCount >= health) // Stop spawning if we've hit the health count
                    return;

                // Use the xOffset to adjust the position of each passenger
                Vector3 position = new Vector3(col * spacing + xOffset, row * -spacing, 0);

                // Instantiate the passenger
                GameObject newPassenger = Instantiate(passengerPrefab, position, Quaternion.identity, transform);
                passengers[col, row] = newPassenger; // Store the passenger in the array

                // If the passenger is on the left side (col == 0), flip the sprite
                SpriteRenderer spriteRenderer = newPassenger.GetComponent<SpriteRenderer>();
                if (col == 0 && spriteRenderer != null)
                {
                    spriteRenderer.flipX = true; // Flip the sprite to face right
                }

                passengerCount++;
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
                //passenger.GetComponent<BoxCollider2D>().enabled = false;
                passengers[col, row] = null; // Set the array element to null after making the passenger fall off
                break; // Only remove one passenger per call
            }
        }
    }
}
