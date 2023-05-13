using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
   
    public const float yPosition = 0.04f;

    public GameObject chessPiece;
    public GameObject cameraRotator;

    // Positions
    private GameObject[,] positions = new GameObject[8, 8];

    // Teams
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject[] playerBlack = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        cameraRotator = GameObject.Find("CameraRotator");
        playerWhite = new GameObject[]
        {
            Create("White_Rook", 0, 0), Create("White_Knight", 1, 0), Create("White_Bishop",2,0), Create("White_Queen", 3, 0),
            Create("White_King", 4, 0), Create("White_Bishop",5,0), Create("White_Knight", 6, 0), Create("White_Rook", 7, 0),
            Create("White_Pawn", 0, 1), Create("White_Pawn", 1, 1), Create("White_Pawn", 2, 1), Create("White_Pawn", 3, 1),
            Create("White_Pawn", 4, 1), Create("White_Pawn", 5, 1),  Create("White_Pawn", 6, 1),  Create("White_Pawn", 7, 1),

        };

        playerBlack = new GameObject[]
       {
            Create("Black_Rook", 0, 7), Create("Black_Knight", 1, 7), Create("Black_Bishop",2,7), Create("Black_Queen", 3, 7),
            Create("Black_King", 4, 7), Create("Black_Bishop",5,7), Create("Black_Knight", 6, 7), Create("Black_Rook", 7, 7),
            Create("Black_Pawn", 0, 6), Create("Black_Pawn", 1, 6), Create("Black_Pawn", 2, 6), Create("Black_Pawn", 3, 6),
            Create("Black_Pawn", 4, 6), Create("Black_Pawn", 5, 6),  Create("Black_Pawn", 6, 6),  Create("Black_Pawn", 7, 6),

       };



        for (int i = 0; i < playerWhite.Length; i++) 
        {
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }
    }

    

    public GameObject Create(string name, int x, int z) 
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0f,yPosition, 0f), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetZBoard(z);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj) 
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(),cm.GetZBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y) 
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y) 
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y) 
    {
        if(x < 0 || y < 0 || x >= positions.GetLength(0) || y>= positions.GetLength(1)) 
        {
            return false;
        }
        return true;
    }

    public string GetCurrentPlayer() 
    {
        return currentPlayer;
    }

    public bool IsGameOver() 
    {
        return gameOver;
    }

    public void NextTurn() 
    {
        if(currentPlayer == "white") 
        {
            currentPlayer = "black";
            cameraRotator.GetComponent<CameraRotate>().RotateCamera();

        }
        else 
        {
            cameraRotator.GetComponent<CameraRotate>().RotateCamera();
            currentPlayer = "white";
        }
    }

    public void Winner(string playerWinner) 
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TextMeshProUGUI>().text = playerWinner + " is the winner";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void Update()
    {
        if(gameOver == true && Input.GetMouseButtonDown(0)) 
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }

        if (currentPlayer == "black")
        {
            List<GameObject> movablePieces = new List<GameObject>();

            // Find all black pieces with valid moves
            foreach (GameObject piece in playerBlack)
            {
                if (piece == null)
                {
                    continue;
                }
                Chessman cm = piece.GetComponent<Chessman>();
                cm.DestroyMovePlates();
                cm.InitiateMovePlates();
                if (GameObject.FindGameObjectsWithTag("MovePlate").Length > 0)
                {
                    movablePieces.Add(piece);
                }
            }

            if (movablePieces.Count > 0)
            {
                // Move a random piece
                int randomPieceIndex = Random.Range(0, movablePieces.Count);
                Chessman cm = movablePieces[randomPieceIndex].GetComponent<Chessman>();
               
                cm.DestroyMovePlates();
                cm.InitiateMovePlates();
                MovePlate[] movePlates = FindObjectsOfType<MovePlate>();

                int MoveIndex = 0;

                for (int i = 0; i < movePlates.Length; i++)
                {
                    if (movePlates[i].attack == true) 
                    {
                        MoveIndex = i;
                        break;
                    }
                    else 
                    {
                        MoveIndex = Random.Range(0, movePlates.Length);
                        break;
                    }
                }
                
       
                
                GameObject targetPiece = GetPosition(movePlates[MoveIndex].matrixX, movePlates[MoveIndex].matrixZ);
                DestroyImmediate(targetPiece);
                MovePiece(cm, movePlates[MoveIndex]);
            }
            else
            {
                // No valid moves for black, skip turn
                NextTurn();
            }
        }

        

    }

    private void MovePiece(Chessman cm, MovePlate mp)
    {
        SetPositionEmpty(cm.GetXBoard(), cm.GetZBoard());
        cm.SetXBoard(mp.matrixX);
        cm.SetZBoard(mp.matrixZ);
        cm.SetCoords();
        SetPosition(cm.gameObject);
        cm.DestroyMovePlates();
        //moveSounds.GetComponent<MoveSounds>().Sounds();
        NextTurn();
        cm.pawnFirstMove = false;
    }





}
 