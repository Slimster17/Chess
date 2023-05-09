using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    public GameObject moveSounds;

    GameObject reference = null;

    //Board positions
    public int matrixX;
    public int matrixZ;

    //false: movement, true: attacking
    public bool attack = false;


    public void Start()
    {
        if (attack)
        {
            //Change color to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0f, 0f, 1f);

        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        moveSounds = GameObject.FindGameObjectWithTag("MoveSounds");

        if (attack) // destroying an old  chess piece
        {
           
            GameObject chessPiece = controller.GetComponent<Game>().GetPosition(matrixX, matrixZ);
            Destroy(chessPiece);

            if(chessPiece.name == "White_King") 
            {
                controller.GetComponent<Game>().Winner("black");
            }

            if (chessPiece.name == "Black_King")
            {
                controller.GetComponent<Game>().Winner("white");
            }
        }
        


        controller.GetComponent<Game>().SetPositionEmpty // set previous position to empty
            (reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetZBoard());

        reference.GetComponent<Chessman>().SetXBoard(matrixX); // set new position
        reference.GetComponent<Chessman>().SetZBoard(matrixZ);
        reference.GetComponent<Chessman>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        moveSounds.GetComponent<MoveSounds>().Sounds();
        controller.GetComponent<Game>().NextTurn();


        reference.GetComponent<Chessman>().DestroyMovePlates();
        reference.GetComponent<Chessman>().pawnFirstMove = false;
    }

    public void SetCoords(int x, int z)
    {
        matrixX = x;
        matrixZ = z;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }


}
