using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviourMobile : MonoBehaviour
{
    public float fallTime=1f;
    private float previousTime;
    private SoundManager soundManager;
    private static int width = 10;
    private static int height = 20;
    public Vector3 rotationPoint;
    private static Transform[,] grid = new Transform[width, height];
    public Button[] buttons;
    private UIManager uIManager;
    private bool isSpeedIncrease = false;

    private void Start()
    {
        buttons = new Button[4];
        uIManager = GameObject.FindObjectOfType<UIManager>();
        if (uIManager == null)
        {
            Debug.Log("Ui manager is not found");
        }
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.Log("sound manager not found");
        }

        buttons[0] = GameObject.FindGameObjectWithTag("upbutton").GetComponent<Button>();
        if (buttons[0] == null)
        {
            Debug.Log("Up Button not found");
        }
        else
        {

            buttons[0].onClick.AddListener(UpMovement);
        }
        
        
        buttons[1] = GameObject.FindGameObjectWithTag("downbutton").GetComponent<Button>();
        if (buttons[1] == null)
        {
            Debug.Log("Down Button not found");
        }
        else
        {
            uIManager.GetComponent<UIManager>().IncreaseScore();
            isSpeedIncrease = true;
            buttons[1].onClick.AddListener(DownMovement);
        }
        
        buttons[2] = GameObject.FindGameObjectWithTag("leftbutton").GetComponent<Button>();
        if (buttons[2] == null)
        {
            Debug.Log("Left Button not found");
        }
        else
        {

            buttons[2].onClick.AddListener(LeftMovement);
        }
        
        buttons[3] = GameObject.FindGameObjectWithTag("rightbutton").GetComponent<Button>();
        if (buttons[3] == null)
        {
            Debug.Log("Right Button not found");
        }
        else
        {

            buttons[3].onClick.AddListener(RightMovement);
        }
  
    }

   

    public void UpMovement()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

        if (!isValid())
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
    }

    public void DownMovement()
    {
        if (Input.GetKey(KeyCode.DownArrow)){
            isSpeedIncrease = true;
            uIManager.GetComponent<UIManager>().IncreaseScore();
        }

        if (Time.time - previousTime > (isSpeedIncrease? fallTime/10 : fallTime))
        {
            previousTime = Time.time;
            transform.position += Vector3.down;

            soundManager.SelectAudio(1, 0.5f);

            if (!isValid())
            {
                transform.position -= Vector3.down;
                SpawnManager spawner = FindObjectOfType<SpawnManager>();
                if (spawner == null)
                {
                    Debug.Log("Spawn Manager Component not found");
                }
                else
                {
                    AddToGrid();
                    // it is disabling the script ahead functions will still works the same only update function will not be called as the script is being disabled
                    this.enabled = false;
                    // it is checking for the lines and reducing it
                    CheckForLines();
                    //// spawning a new block
                    if (!uIManager.isGameOver)
                    {
                        spawner.SpawnBlock();
                    }

                }

            }
            isSpeedIncrease = false;
        }

    }

    public void RightMovement()
    {
        transform.position += Vector3.right;
        if (!isValid())
        {
            transform.position -= Vector3.right;
        }
    }

    public void LeftMovement()
    {
        transform.position += Vector3.left;
        if (!isValid())
        {
            transform.position -= Vector3.left;
        }
    }

    void Update()
    {
        // for Movement of block
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftMovement();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightMovement();
        }

        // rotation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            UpMovement();
            
        }


        // speed 
        DownMovement();    
    }

    

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            var roundedX = Mathf.RoundToInt(children.position.x);
            var roundedY = Mathf.RoundToInt(children.position.y);
            grid[roundedX, roundedY] = children;
        }
    }

    bool isValid()
    {
        foreach(Transform children in transform)
        {
            var roundedX = Mathf.RoundToInt(children.position.x);
            var roundedY = Mathf.RoundToInt(children.position.y);

            
            if (roundedY > height - 3)
            {
                Debug.Log("Exceed the height");
                uIManager.isGameOver = true;   
            }

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                
               return false;
            }

            
            // this function is acting like a collider as if the grid already have a value that is a block then other block cannot go at its place
            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }

        }
        return true;
    }

    void CheckForLines()
    {
        for (int i= height-1; i>=0; i--)
        {
            if (HasLine(i))
            {
                uIManager.GetComponent<UIManager>().IncreaseScore(50);
                soundManager.SelectAudio(2);
                DeleteRow(i);
                ReduceRow(i);
                soundManager.SelectAudio(3);
            }
        }
    }

    bool HasLine(int line)
    {
        for (int i=0; i<width; i++)
        {
            if (grid[i, line] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteRow(int line)
    {
        for (int i=0; i<width; i++)
        {
            if (grid[i, line] == null)
            {
                
                continue;
            }
            
            
            Destroy(grid[i, line].gameObject);
            grid[i, line] = null;
        }
    }

    void ReduceRow(int line)
    {
        for (int r=line; r<height-2; r++)
        {
            for (int c = 0; c < width; c++)
            {
                
                
                if (grid[c, r+1] != null)
                {
                    grid[c, r] = grid[c, r + 1];
                    grid[c, r].gameObject.transform.position -= new Vector3(0, 1, 0);
                }
                else
                {
                    grid[c, r] = grid[c, r + 1];
                }
                                
            }
        }
        
    }
}

