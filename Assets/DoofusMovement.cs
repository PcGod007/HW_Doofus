using UnityEngine;

public class DoofusMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float raycastDistance = 1.5f;

    private bool gameOverTriggered = false;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveX, 0f, moveZ);

        if (!IsOnPlatform())
        {
            if (!gameOverTriggered)
            {
                gameOverTriggered = true;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                    StopAllCoroutines();
                }
                else
                {
                    Debug.LogError("GameManager instance is not assigned.");
                }
            }
        }
    }

    bool IsOnPlatform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            return hit.collider.CompareTag("Platform");
        }
        return false;
    }
}
