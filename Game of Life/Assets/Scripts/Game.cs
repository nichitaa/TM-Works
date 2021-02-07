using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    private static int SCREEN_WIDTH = 64; // 1024 --> 64 rows
    private static int SCREEN_HEIGHT = 48; // 768 --> 48 cols

    public float speed = 0.1f;
    private float timer = 0;

    public bool play = false;


    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            // overwrite default behaviour of 60 fps
            if (timer >= speed)
            {
                timer = 0f;
                CountNeigobors();
                PopulationControl();
            }
            else
            {
                // increment by amount of time past since the last frame
                timer += Time.deltaTime;
            }
        }

        // if is pause
        UserInput(); // get user input
    }

    void UserInput()
    {
        // if clicked
        if( Input.GetMouseButtonDown(0) )
        {
            // get mouse position coords
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            // for valid coords
            if ( x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT )
            {
                bool alive = grid[x, y].isAlive;
                grid[x, y].SetAlive(!alive); // toggle alive
            }
        } 

        // toggle play state
        if (Input.GetKeyUp(KeyCode.Space))
        { 
            play = !play;
        }
    }

    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        { 
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                // set all cells to dead
                grid[x, y].SetAlive(false);
                // either set some random alive cell
                // grid[x, y].SetAlive(RandomAliveCell());
            }
        }
    }


    void CountNeigobors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int numNeighbors = 0;

                // up
                if (y+1 < SCREEN_HEIGHT)
                {
                    if (grid[x, y+1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // right
                if (x + 1 < SCREEN_WIDTH)
                {
                    if (grid[x +1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // down
                if (y - 1 >= 0)
                {
                    if (grid[x, y-1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // left 
                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // up right
                if ( x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x+1, y+1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // up left
                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x - 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // down right
                if ( x + 1 < SCREEN_WIDTH && y - 1 >= 0 )
                {
                    if (grid[x + 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // down left
                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // update cell with neighbors num
                if (numNeighbors > 0)
                {
                    grid[x, y].numNeighbors = numNeighbors;
                }
            }
        }
    }

    void PopulationControl()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                // live cell rule: 
                // if 2 || 3 live cels neighbors -> cell survive

                // dead cell rule: 
                // if 3 live neighbors -> cell become alive

                // al other cells dies || remain dead

                if (grid[x, y].isAlive)
                {
                    // alive
                    if (grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false); // set cell to dead
                    }
                }
                else
                {
                    // dead
                    if (grid[x, y].numNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true); // set cell to alive
                    }
                }
            }
        }
    }

    bool RandomAliveCell()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > 50)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
