using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType
{
    VACIO,
    ARENA,
    ARENAMOJADA,
    PISO,
    AIRE,
    TRANSICION,
    TRANSICIONESQ
}

public class Cell 
{
    public cellType cT;
    public bool isBuildable;
    public int height = 0;

}

public class Grid {

    private float x_center;
    private float y_center;
    private int width;
    private int height;
    private float cellSize;
    private Cell[,] gridArray;

    private GameObject newLine;
    private LineRenderer drawLine;
    private GameObject lineParent;

    public Grid(int width, int height, float cellSize, float xcenter = 0, float ycenter = 0){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.x_center = xcenter;
        this.y_center = ycenter;


        gridArray = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridArray[i,j] = new Cell();
            }
        }

        //DrawGrid();
    }

    public Vector3 GetCellPosition(int x, int y)
    {
        return new Vector3(x,0,y) * cellSize;
    }

    public int GetCellHeight(int x, int y)
    {
        return gridArray[x,y].height;
    }

    public void SetCellHeight(int x, int y, int height)
    {
        gridArray[x,y].height = height;
    }


    public Vector3 GetCellCenter(int x, int y)
    {
        return new Vector3(x* this.cellSize + this.cellSize/2 - (this.width * this.cellSize/2) + this.x_center,0,y* this.cellSize + this.cellSize/2 - (this.height * this.cellSize/2) + this.y_center) ;
    }

    public cellType GetCellType(int x, int y)
    {
        return gridArray[x,y].cT;
    }

    public void setCellType(int x, int y, cellType cT, int height = 0)
    {
        gridArray[x,y].cT = cT;
        gridArray[x,y].height = height;
    }

    private void DrawGrid()
    {
        lineParent = new GameObject();

        for(int x = 0; x < this.width; x++)
        {
            for(int y = 0; y < this.height; y++)
            {
                DrawCell(x, y);
            }
        }
    }

    private void DrawCell(int x, int y)
    {
        newLine = new GameObject();
        newLine.transform.parent = lineParent.transform;
        drawLine = newLine.AddComponent<LineRenderer>();

        drawLine.positionCount = 5;

        float x_temp = x * this.cellSize;
        float y_temp = y * this.cellSize;

        drawLine.SetPosition(0, new Vector3(x_temp - (width * cellSize/2) + x_center, 10, y_temp - (height * cellSize/2) + y_center));
        drawLine.SetPosition(1, new Vector3(x_temp - (width * cellSize/2) + x_center, 10, y_temp - (height * cellSize/2)  + y_center+ this.cellSize));
        drawLine.SetPosition(2, new Vector3(x_temp - (width * cellSize/2) + x_center + this.cellSize, 10, y_temp - (height * cellSize/2) + y_center + this.cellSize));
        drawLine.SetPosition(3, new Vector3(x_temp - (width * cellSize/2) + x_center + this.cellSize, 10, y_temp - (height * cellSize/2) + y_center));
        drawLine.SetPosition(4, new Vector3(x_temp - (width * cellSize/2) + x_center, 10, y_temp - (height * cellSize/2) + y_center));
    }

}

