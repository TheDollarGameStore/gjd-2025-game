using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private PieceColor color;

    [HideInInspector] public int column;

    [SerializeField] private GameObject piecePrefab;

    private bool moving = true;

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            return;
        }

        transform.position += (Vector3)Vector2.down * GridManager.instance.gameSpeed * Time.deltaTime;

        if (transform.position.y < GridManager.instance.paddle.transform.position.y - 3.5f)
        {
            Piece newPiece = Instantiate(piecePrefab, transform.position, Quaternion.identity).GetComponent<Piece>();

            if (GridManager.instance.AddToGrid(newPiece, column, true))
            {
                Destroy(gameObject);
            }
            else
            {
                //TODO: Game Over
                moving = false;
            }
        }
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
