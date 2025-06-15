using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private PieceColor color;

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)Vector2.down * GridManager.instance.gameSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GridManager.instance.paddle.AddPiece(color))
            {
                Destroy(gameObject);
            }
        }
    }
}
