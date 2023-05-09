using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movePlate;

    // Positions
    private int xBoard = -1;
    private int zBoard = -1;

    // player color
    private string player;
    private bool isAI = false;

    // References for all chess models
    public Mesh king, queen, knight, bishop, rook, pawn;


    public bool pawnFirstMove = true;


    public void Activate() 
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        
    SetCoords();

        switch (this.name) 
        {
            case "White_Rook": this.GetComponent<MeshFilter>().mesh = rook; player = "white"; break;
            case "White_Knight": this.GetComponent<MeshFilter>().mesh = knight; player = "white"; break;
            case "White_Bishop": this.GetComponent<MeshFilter>().mesh = bishop; player = "white"; break;
            case "White_King": this.GetComponent<MeshFilter>().mesh = king; player = "white"; break;
            case "White_Queen": this.GetComponent<MeshFilter>().mesh = queen; player = "white"; break;
            case "White_Pawn": this.GetComponent<MeshFilter>().mesh = pawn; player = "white"; break;

            case "Black_Rook":
                this.GetComponent<MeshFilter>().mesh = rook;
                this.BlackFigure();
                player = "black";
                break;
            case "Black_Knight":
                this.GetComponent<MeshFilter>().mesh = knight;
                this.BlackFigure();
                player = "black";
                break;

            case "Black_Bishop":
                this.GetComponent<MeshFilter>().mesh = bishop;
                this.BlackFigure();
                player = "black";
                break;

            case "Black_King":
                this.GetComponent<MeshFilter>().mesh = king;
                this.BlackFigure();
                player = "black";
                break;
            case "Black_Queen":
                this.GetComponent<MeshFilter>().mesh = queen;
                this.BlackFigure();
                player = "black";
                break;
            case "Black_Pawn":
                this.GetComponent<MeshFilter>().mesh = pawn;
                this.BlackFigure();
                player = "black";
                break;

        }
    }

    public void BlackFigure() 
    {
        Material black = Resources.Load<Material>("Materials/Black");
        this.GetComponent<Renderer>().material = black;
        this.transform.Rotate(0f, 180f, 0f, Space.World);

    }

    public void SetCoords() 
    {
        float x = xBoard;
        float z = zBoard;

        x *= 0.18f;
        z *= 0.18f;

        x += -0.63f;
        z += -0.63f;
        this.transform.position = new Vector3(x,0.04f,z);
    }

    public int GetXBoard() { return xBoard; }
    public int GetZBoard() {  return zBoard; }
    public void SetXBoard(int x) { xBoard = x; }
    public void SetZBoard(int z) {  zBoard = z; }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() &&
            controller.GetComponent<Game>().GetCurrentPlayer() == player) 
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
       
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }

    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "Black_Queen":
            case "White_Queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            
            case "Black_Knight":
            case "White_Knight":
                LMovePlate();
                break;
            
            case "Black_Bishop":
            case "White_Bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            
            case "Black_King":
            case "White_King":
                SurroundMovePlate();
                break;
            
            case "White_Rook":
            case "Black_Rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            
            case "Black_Pawn":
                PawnMovePlate(xBoard, zBoard - 1);
                break;

            case "White_Pawn":
                PawnMovePlate(xBoard, zBoard + 1);

                break;

        }
    }

    public void LineMovePlate(int xIncrement, int zIncrement) 
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int z = zBoard + zIncrement;

        while(sc.PositionOnBoard(x, z) && sc.GetPosition(x,z) == null) // check if still on board and none other piece there
        {
            MovePlateSpawn(x, z);
            x += xIncrement;
            z += zIncrement;
        }

        if(sc.PositionOnBoard(x,z) && 
            sc.GetPosition(x,z).GetComponent<Chessman>().player != player) 
        {
            MovePlateSpawn(x, z, true);
        }
    }

    public void LMovePlate() 
    {
        PointMovePlate(xBoard + 1, zBoard + 2);
        PointMovePlate(xBoard - 1, zBoard + 2);
        PointMovePlate(xBoard + 2, zBoard + 1);
        PointMovePlate(xBoard + 2, zBoard - 1);
        PointMovePlate(xBoard + 1, zBoard - 2);
        PointMovePlate(xBoard - 1, zBoard - 2);
        PointMovePlate(xBoard - 2, zBoard + 1);
        PointMovePlate(xBoard - 2, zBoard - 1);
    }

    public void SurroundMovePlate() 
    {
        PointMovePlate(xBoard, zBoard + 1);
        PointMovePlate(xBoard, zBoard - 1);
        PointMovePlate(xBoard - 1, zBoard - 1);
        PointMovePlate(xBoard -1, zBoard - 0);
        PointMovePlate(xBoard - 1, zBoard + 1);
        PointMovePlate(xBoard + 1, zBoard - 1);
        PointMovePlate(xBoard + 1 , zBoard - 0);
        PointMovePlate(xBoard + 1, zBoard + 1);

    }

    public void PointMovePlate(int x, int z) 
    {
        Game sc =  controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, z)) 
        {
            GameObject chessPiece = sc.GetPosition(x, z);

            if( chessPiece == null) 
            {
                MovePlateSpawn(x, z);
            }

            else if(chessPiece.GetComponent<Chessman>().player != player)  // if chesspPiece is other player then spawn attackPlate
            {
                MovePlateSpawn(x, z, true);
            }
        }
    }

    public void PawnMovePlate(int x, int z) 
    {
        Game sc = controller.GetComponent<Game>();


        int direction = (sc.GetCurrentPlayer() == "white") ? 1 : -1;


        if (pawnFirstMove && sc.PositionOnBoard(x, z)) 
        {
            if (sc.GetPosition(x,z) == null && sc.GetPosition(x,z+direction) == null) 
            {
                MovePlateSpawn(x, z);
                MovePlateSpawn(x, z+direction);
            }

        }

        if (sc.PositionOnBoard(x, z) && !pawnFirstMove) 
        {
            if(sc.GetPosition(x,z) == null) 
            {
                MovePlateSpawn(x, z);

            }


            if(sc.PositionOnBoard(x+1,z) // if still on board and there is other player chessPiece
                && sc.GetPosition(x+1,z) != null 
                && sc.GetPosition(x+1,z).GetComponent<Chessman>().player != player) 
            {
                MovePlateSpawn(x+1,z, true);
            }


            if (sc.PositionOnBoard(x - 1, z) 
               && sc.GetPosition(x - 1, z) != null
               && sc.GetPosition(x - 1, z).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x - 1, z, true);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixZ, bool isAttack = false) 
    {
        float x = matrixX;
        float z = matrixZ;

        x *= 0.18f;
        z *= 0.18f;

        x += -0.63f;
        z += -0.63f;

        GameObject mp = Instantiate(movePlate,new Vector3(x,0.04f,z), Quaternion.identity);
        mp.transform.Rotate(90f, 0f, 0f, Space.World);
        MovePlate mpScript = mp.GetComponent<MovePlate>();

        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixZ);
        if (isAttack) 
        {
            mpScript.attack = true;
        }

    }

    public string GetPlayer() 
    {
        return player;
    }

    


}
