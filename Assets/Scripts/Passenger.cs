using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Sprite[] sprites; // Assign this array in the inspector

    private void Awake()
    {
        AssignRandomSprite();
    }

    private void AssignRandomSprite()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null && sprites.Length > 0)
        {
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
