using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] playerTokens;

    public GameObject player1TokenUI;
    public GameObject player2TokenUI;

    [SerializeField] Canvas player1WinScreen;
    [SerializeField] Canvas player2WinScreen;
    [SerializeField] Canvas drawScreen;

    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    public LineRenderer lineRenderer;

    private bool gameOver = false;

    private int currentPlayer = 0;
    private int totalMoves = 0; 
    void Start()
    {
        HighlightCurrentPlayer();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                PlaceToken(hit.collider.gameObject);
            }
        }
    }

    void PlaceToken(GameObject cell)
    {
        //determine the column of clicked cell
        int column = (int)Mathf.Round(cell.transform.position.x);

        for (int row = 0; row < 6; row++)
        {
            GameObject targetCell = GameObject.Find($"Cell ({column},{row})");
            
            //check if the cell is empty
            if (targetCell.transform.childCount == 0)
            {
                Vector3 startPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y + 6, targetCell.transform.position.z);
                GameObject token = Instantiate(playerTokens[currentPlayer], startPosition, Quaternion.identity, targetCell.transform);

               
                MoveTokenWithDOTween(token, targetCell.transform.position);

                WinChecker winChecker = GetComponent<WinChecker>();
                winChecker.UpdateGrid(column, row, currentPlayer + 1);
                // Increment total moves
                totalMoves++; 

                if (winChecker.CheckForWinFromCell(column, row, currentPlayer + 1))
                {
                    Debug.Log($"Player {currentPlayer + 1} wins!");
                    HighlightWinningCells(winChecker.GetWinningCells(), currentPlayer + 1);
                    StartCoroutine(HandleWin(currentPlayer));
                    gameOver = true;
                }
                else if (totalMoves >= 42) 
                {
                    StartCoroutine(HandleDraw());
                    gameOver = true;
                }
                else
                {
                    currentPlayer = (currentPlayer + 1) % playerTokens.Length;
                    HighlightCurrentPlayer();
                }

                break;
            }
        }
    }

  
    IEnumerator HandleWin(int winningPlayer)
    {
        yield return new WaitForSeconds(2f);

        if (winningPlayer == 0)
        {
            UIManager.instance.SwitchScreen(GameScreens.Win1);
            lineRenderer.enabled = false;
        }
        else
        {
            UIManager.instance.SwitchScreen(GameScreens.Win2);
            lineRenderer.enabled = false;
        }
    }

    IEnumerator HandleDraw()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.SwitchScreen(GameScreens.Draw);
    }

    void MoveTokenWithDOTween(GameObject token, Vector3 finalPosition)
    {
        float moveSpeed = 15f;
        float distance = Vector3.Distance(token.transform.position, finalPosition);
        float duration = distance / moveSpeed;

        token.transform.DOMove(finalPosition, duration).SetEase(Ease.Linear);
    }

    void HighlightWinningCells(List<Vector2Int> winningCells, int player)
    {
        Debug.Log("Highlighting winning cells");
        Color highlightColor = Color.red;

        foreach (Vector2Int cellPos in winningCells)
        {
            GameObject cell = GameObject.Find($"Cell ({cellPos.x},{cellPos.y})");

            if (cell.transform.childCount > 0)
            {
                var spriteRenderer = cell.transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Debug.Log($"Highlighting cell at position: {cellPos}");
                    spriteRenderer.color = highlightColor;
                }
              
            }
        }
        DrawWinningLine(winningCells);
    }

    void HighlightCurrentPlayer()
    {
        if (currentPlayer == 0)
        {
            player1TurnPanel.SetActive(true);
            player2TurnPanel.SetActive(false);
        }
        else
        {
            player1TurnPanel.SetActive(false);
            player2TurnPanel.SetActive(true);
        }
    }

    //void DrawWinningLine(List<Vector2Int> winningCells)
    //{
    //    if (winningCells.Count > 1)
    //    {
    //        lineRenderer.enabled = true;

    //        lineRenderer.positionCount = winningCells.Count;

    //        for (int i = 0; i < winningCells.Count; i++)
    //        {
    //            Vector2Int cellPos = winningCells[i];
    //            GameObject cell = GameObject.Find($"Cell ({cellPos.x},{cellPos.y})");
    //            if (cell != null)
    //            {
    //                Vector3 cellPosition = cell.transform.position;
    //                // Adjust the z-position to ensure the line is visible
    //                cellPosition.z -= 0.1f;
    //                lineRenderer.SetPosition(i, cellPosition);
    //            }
    //        }
    //    }
    //}

    void DrawWinningLine(List<Vector2Int> winningCells)
    {
        if (winningCells.Count > 1)
        {
            StartCoroutine(AnimateLineDrawing(winningCells));
        }
    }

    IEnumerator AnimateLineDrawing(List<Vector2Int> winningCells)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = winningCells.Count;

        Vector3[] positions = new Vector3[winningCells.Count];
        for (int i = 0; i < winningCells.Count; i++)
        {
            Vector2Int cellPos = winningCells[i];
            GameObject cell = GameObject.Find($"Cell ({cellPos.x},{cellPos.y})");
            if (cell != null)
            {
                Vector3 cellPosition = cell.transform.position;
                // Adjust the z-position to ensure the line is visible
                cellPosition.z -= 0.1f;
                positions[i] = cellPosition;
            }
        }

        // Initialize line renderer positions
        for (int i = 0; i < positions.Length; i++)
        {
            // Set all positions to start initially
            lineRenderer.SetPosition(i, positions[0]); 
        }

        float totalDuration = 1.0f;
        // Duration per segment
        float segmentDuration = totalDuration / (positions.Length - 1); 

        // Animate each segment of the line
        for (int i = 1; i < positions.Length; i++)
        {
            Vector3 startPosition = positions[i - 1];
            Vector3 endPosition = positions[i];

            yield return DOTween.To(() => startPosition, x => UpdateLineSegment(i, x), endPosition, segmentDuration)
                                .SetEase(Ease.Linear)
                                .WaitForCompletion();
        }
    }

    void UpdateLineSegment(int index, Vector3 position)
    {
        lineRenderer.SetPosition(index, position);
    }

}


