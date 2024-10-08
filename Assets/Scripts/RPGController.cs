using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RPGController : MonoBehaviour
{
    public float speed;
    private Vector2 movement;

    public Tilemap map;
    public Transform targetPosition;
    public LayerMask UnwalkableLayer;
    public LayerMask MoveableLayer;
    public TileBase targetTiles;

    public GameManager gameManagerScript;

    Animator animator;

    private HashSet<GameObject> blocksOnTargetTiles = new HashSet<GameObject>();


    private void Awake()
    {
        targetPosition.position = transform.position;
        InitializeMovableBlocks();
        animator = GetComponent<Animator>();
    }

    private void InitializeMovableBlocks()
    {
        GameObject[] movableBlocks = GameObject.FindGameObjectsWithTag("MovableBlock");
        gameManagerScript.SetTotalMovableBlocks(movableBlocks.Length);

        foreach (GameObject block in movableBlocks)
        {
            if (IsBlockOnTargetTile(block))
            {
                blocksOnTargetTiles.Add(block);
                gameManagerScript.IncrementBlocksOnTargetTiles();
            }
        }
    }

    void Update()
    {
        CheckAllMovableBlocks();

        RestartCheck();

        Animate();

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f &&
            !Physics2D.OverlapCircle(targetPosition.position + new Vector3(movement.x, movement.y, 0f), 0.1f, UnwalkableLayer))
        {
            Collider2D IsColliding = Physics2D.OverlapCircle(targetPosition.position + new Vector3(movement.x, movement.y, 0f), 0.1f, MoveableLayer);
            if (IsColliding)
            {
                if(!(Physics2D.OverlapCircle(targetPosition.position + new Vector3(2 * movement.x, 2 * movement.y, 0f), 0.1f, UnwalkableLayer) ||
                    Physics2D.OverlapCircle(targetPosition.position + new Vector3(2 * movement.x, -2 * movement.y, 0f), 0.1f, MoveableLayer)))
                    {
                        targetPosition.position = new Vector3(targetPosition.position.x + movement.x, targetPosition.position.y + movement.y, 0f);
                        Debug.Log("Settiing position based on a double overlap");
                        IsColliding.gameObject.transform.position = new Vector3(targetPosition.position.x + movement.x, targetPosition.position.y + movement.y, 0f);
                    }
            }
            else 
            {
                targetPosition.position = new Vector3(targetPosition.position.x + movement.x, targetPosition.position.y + movement.y, 0f);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        if (movement.x != 0 && movement.y != 0)
        {
            movement = Vector2.zero;
        }
    }

    void CheckAllMovableBlocks()
    {
        GameObject[] movableBlocks = GameObject.FindGameObjectsWithTag("MovableBlock");
        foreach (GameObject block in movableBlocks)
        {
            bool IsOnTargetTile = IsBlockOnTargetTile(block);

            if (IsOnTargetTile && !blocksOnTargetTiles.Contains(block))
            {
                Debug.Log("The block is on the target tile");
                blocksOnTargetTiles.Add(block);
                gameManagerScript.IncrementBlocksOnTargetTiles();
            }
            else if (!IsOnTargetTile && blocksOnTargetTiles.Contains(block))
            {
                Debug.Log("The block is not on any target tile");
                blocksOnTargetTiles.Remove(block);
                gameManagerScript.DecrementBlocksOnTargetTiles();
            }
        }
    }

    bool IsBlockOnTargetTile(GameObject block)
    {
        Vector3Int blockPosition = map.WorldToCell(block.transform.position);
        return map.GetTile(blockPosition) == targetTiles;
    }

    void RestartCheck()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    void Animate()
    {
        animator.SetFloat("MovementX", movement.x);
        animator.SetFloat("MovementY", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);
    }
}