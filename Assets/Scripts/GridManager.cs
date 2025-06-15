using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    public static GridManager instance = null;

    [SerializeField] private Vector2Int gridSize;

    [SerializeField] private Vector2Int tileSize;

    private Tile[,] tiles;

    [HideInInspector] public int paddlePos;

    [SerializeField] public Paddle paddle;

    [SerializeField] private float movementRepeatCooldown;

    private bool repeatReady;

    [SerializeField] public float gameSpeed;
    [SerializeField] private float spawnFrequencyBase;

    [SerializeField] private List<GameObject> dropPieces;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();

        paddlePos = Mathf.RoundToInt(gridSize.x / 2f);

        paddle.gameObject.transform.position = new Vector2(tiles[paddlePos, 0].transform.position.x, transform.position.y + ((gridSize.y + 1) * tileSize.y));

        Invoke("SpawnDrop", CalculateSpawnFrequency());
    }

    float CalculateSpawnFrequency()
    {
        return spawnFrequencyBase / gameSpeed;
    }

    private void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            paddlePos--;
            repeatReady = false;
            CancelInvoke("RepeatReady");
            Invoke("RepeatReady", movementRepeatCooldown);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            paddlePos++;
            repeatReady = false;
            CancelInvoke("RepeatReady");
            Invoke("RepeatReady", movementRepeatCooldown);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            paddle.PlayPiece();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (repeatReady)
            {
                paddlePos--;
                repeatReady = false;
                CancelInvoke("RepeatReady");
                Invoke("RepeatReady", movementRepeatCooldown);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (repeatReady)
            {
                paddlePos++;
                repeatReady = false;
                CancelInvoke("RepeatReady");
                Invoke("RepeatReady", movementRepeatCooldown);
            }
        }

        if (paddlePos < 0)
        {
            paddlePos = 0;
        }

        if (paddlePos > gridSize.x - 1)
        {
            paddlePos = gridSize.x - 1;
        }

        paddle.gameObject.transform.position = Vector2.Lerp(paddle.gameObject.transform.position, new Vector2(tiles[paddlePos, 0].transform.position.x, transform.position.y + ((gridSize.y + 1) * tileSize.y)), 10f * Time.deltaTime);
    }

    void GenerateGrid()
    {
        tiles = new Tile[gridSize.x, gridSize.y];

        Vector2 totalGridSize = new Vector2(gridSize.x * tileSize.x, gridSize.y * tileSize.y);
        Vector2 startPosition = new Vector2(-totalGridSize.x / 2f + tileSize.x / 2f, 0);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 spawnPosition = startPosition + new Vector2(x * tileSize.x, y * tileSize.y);
                GameObject tileObj = Instantiate(tilePrefab, transform);
                tileObj.transform.localPosition = spawnPosition;

                Tile tile = tileObj.GetComponent<Tile>();
                tiles[x, y] = tile;
            }
        }
    }

    void RepeatReady()
    {
        repeatReady = true;
    }

    void SpawnDrop()
    {
        Instantiate(dropPieces[Random.Range(0, dropPieces.Count)], new Vector2(tiles[Random.Range(0, gridSize.x), 0].transform.position.x, 106f), Quaternion.identity);
        Invoke("SpawnDrop", CalculateSpawnFrequency());
    }

    public bool AddToGrid(Piece piece, int column, bool trail)
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            if (tiles[column, y].piece == null)
            {
                if (trail)
                {
                    piece.TrailEffect(new Vector2(tiles[column, 0].transform.position.x, piece.transform.position.y), tiles[column, y].transform.position);
                }

                tiles[column, y].piece = piece;
                piece.gameObject.transform.position = tiles[column, y].transform.position;
                piece.gameObject.transform.parent = null;
                piece.UpdateYPos(y + 1);
                piece.StartCoroutine(piece.FlashEffect());
                return true;
            }
        }

        return false;
    }
}
