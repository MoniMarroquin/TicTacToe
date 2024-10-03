using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

public enum PlayerOption
{
    NONE, //0
    X, // 1
    O // 2
}
public enum EmptySpace
{
    yes, //0
    no //1
}

public class TTT : MonoBehaviour
{
    public int Rows;
    public int Columns;
    EmptySpace emptySpace = EmptySpace.yes;
    [SerializeField] BoardView board;

    PlayerOption currentPlayer = PlayerOption.X;
    Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[Columns, Rows];

        board.InitializeBoard(Columns, Rows);

        for(int i = 0; i < Rows; i++)
        {
            for(int j = 0; j < Columns; j++)
            {
                cells[j, i] = new Cell();
                cells[j, i].current = PlayerOption.NONE;
            }
        }
    }
    

    public void MakeOptimalMove()
    {/*
        switch (currentPlayer)    
        {
            case PlayerOption.X:

                if(IsEmplty())
                    ChooseSpace(0, 0);

                else
                {
                    ChooseSpace(1, 0);
                    for (int i = 0;i < Rows;i++)
                    {
                        for (int j = 0;j < Columns;j++)
                        {
                            if (cells[j,i].current == PlayerOption.NONE)
                            {
                                cells[j,i].current = PlayerOption.X;
                            }
                        }
                    }
                }
                break;

            case PlayerOption.O:
                break;
        }

        if (click == null)
        {
            click = new UnityEvent();
        }
        click.Invoke();
        Debug.Log("clicked");
        */
    }

    bool IsEmplty()
    {
        bool isEmplty = true;
        for (int i = 0; i <= Rows;i++) 
        {
            for(int j=0; j < Columns;j++)
            {
                if (cells[j,i].current != PlayerOption.NONE)
                    isEmplty = false;
            }
        }
        return isEmplty;
    }

    public UnityEvent click;
    public void OnClick()
    {
        switch(currentPlayer)
        {

        }
        if (cells[0, 0].current == PlayerOption.NONE)
        {
            if (currentPlayer == PlayerOption.X)
            {
                ChooseSpace(0, 0);
                return;
            }
            else if (cells[0, 0].current == PlayerOption.NONE)
            {
                if (currentPlayer == PlayerOption.O)
                {
                    ChooseSpace(0, 0);
                    return;
                }
            }

        }
        if (cells[1, 1].current == PlayerOption.NONE)
        {
            if (currentPlayer == PlayerOption.X)
            {
                ChooseSpace(1, 1);
                return;
            }
            else if (cells[1, 1].current == PlayerOption.NONE)
            {
                if (currentPlayer == PlayerOption.O)
                {
                    ChooseSpace(1, 1);
                    return;
                }
            }
        }
        if (cells[0, 0].current == PlayerOption.O)// blocks O if its in 0,0 aka top left
        {
            if (cells[0, 2].current == PlayerOption.NONE)
            {
                ChooseSpace(0, 2);
                return;
            }
            else if (cells[2, 0].current == PlayerOption.NONE)
            {
                ChooseSpace(2, 0);
                return;
            }
        }
        /*if (cells[2, 0].current == PlayerOption.X)
        {
            if (cells[0, 1].current == PlayerOption.NONE)
            {
                ChooseSpace(0, 1);
                return;
            }
            else if (cells[1, 2].current == PlayerOption.NONE)
            {
                ChooseSpace(1, 2);
                return;
            }
        }
        if (cells[2, 2].current == PlayerOption.X)
        {
            if (cells[1, 2].current == PlayerOption.NONE)
            {
                ChooseSpace(1, 2);
                return;
            }
            else if (cells[2, 1].current == PlayerOption.NONE)
            {
                ChooseSpace(2, 1);
                return;
            }
        }
        if (cells[0, 2].current == PlayerOption.X)
        {
            if (cells[1, 0].current == PlayerOption.NONE)
            {
                ChooseSpace(1, 0);
                return;
            }
            else if (cells[2, 1].current == PlayerOption.NONE)
            {
                ChooseSpace(2, 1);
                return;
            }
        }*/

        if (!IsEmplty())
        {
            GetWinner();
        }
        switch (currentPlayer)
        {
            case PlayerOption.X:

                if (IsEmplty())
                {
                    ChooseSpace(0, 0);
                }
                else
                {
                    ChooseSpace(1, 0);
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Columns; j++)
                        {
                            if (cells[j, i].current == PlayerOption.NONE)
                            {
                                cells[j, i].current = PlayerOption.X;
                            }
                        }
                    }
                }
                break;

            case PlayerOption.O:
                break;
            }
           
        }

    public void ChooseSpace(int column, int row)
    {
        // can't choose space if game is over
        if (GetWinner() != PlayerOption.NONE)
            return;

        // can't choose a space that's already taken
        if (cells[column, row].current != PlayerOption.NONE)
            return;

        // set the cell to the player's mark
        cells[column, row].current = currentPlayer;

        // update the visual to display X or O
        board.UpdateCellVisual(column, row, currentPlayer);

        // if there's no winner, keep playing, otherwise end the game
        if(GetWinner() == PlayerOption.NONE)
            EndTurn();
        else
        {
            Debug.Log("GAME OVER!");
        }
    }

    public void EndTurn()
    {
        // increment player, if it goes over player 2, loop back to player 1
        currentPlayer += 1;
        if ((int)currentPlayer > 2)
            currentPlayer = PlayerOption.X;
    }

    public PlayerOption GetWinner()
    {
        // sum each row/column based on what's in each cell X = 1, O = -1, blank = 0
        // we have a winner if the sum = 3 (X) or -3 (O)
        int sum = 0;

        // check rows
        for (int i = 0; i < Rows; i++)
        {
            sum = 0;
            for (int j = 0; j < Columns; j++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check columns
        for (int j = 0; j < Columns; j++)
        {
            sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check diagonals
        // top left to bottom right
        sum = 0;
        for(int i = 0; i < Rows; i++)
        {
            int value = 0;
            if (cells[i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        // top right to bottom left
        sum = 0;
        for (int i = 0; i < Rows; i++)
        {
            int value = 0;

            if (cells[Columns - 1 - i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[Columns - 1 - i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        return PlayerOption.NONE;
    }
}
