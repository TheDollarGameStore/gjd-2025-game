using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private List<GameObject> pieces;

    [HideInInspector] public List<Piece> currentPieces;

    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite fullSprite;

    [SerializeField] private AudioClip catchSound;

    private void Start()
    {
        currentPieces = new List<Piece>();

        UpdateHitbox();
    }

    public void PlayPiece()
    {
        if (currentPieces.Count == 0)
        {
            return;
        }

        if (GridManager.instance.AddToGrid(currentPieces[currentPieces.Count - 1], GridManager.instance.paddlePos, true))
        {
            currentPieces[currentPieces.Count - 1].gameObject.transform.parent = null;
            currentPieces.RemoveAt(currentPieces.Count - 1);
            UpdateHitbox();
        }
    }

    public bool AddPiece(PieceColor color)
    {
        if (currentPieces.Count == 5)
        {
            return false;
        }

        SoundManager.instance.PlayNormal(catchSound);

        Piece newPiece = Instantiate(pieces[(int)color], transform.position + (Vector3)(Vector2.up * 7 * (currentPieces.Count + 1)), Quaternion.identity, transform).GetComponent<Piece>();
        newPiece.UpdateYPos(currentPieces.Count + 1);
        currentPieces.Add(newPiece);
        transform.position += (Vector3)Vector2.down * 2f;

        if (currentPieces.Count == 5)
        {
            sr.sprite = fullSprite;
        }

        UpdateHitbox();

        return true;
    }

    // Update is called once per frame
    void UpdateHitbox()
    {
        boxCollider.size = new Vector2(16, 7 + (currentPieces.Count > 0 ? 3.5f : 0));
        boxCollider.offset = new Vector2(0, (currentPieces.Count > 0 ? -3.25f : -1.5f) + (currentPieces.Count * 7f));
    }
}
