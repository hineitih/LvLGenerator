using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBPCG : MonoBehaviour
{
    [Header("General settings")]
    public int width; // level length
    public int height; // level height

    [Header("Assets")]
    public GameObject dirt;
    public GameObject grass;
    public GameObject water;
    public GameObject flower;
    public GameObject block;


    private Vector2 spawnPosition = new Vector2(0, 0);
    const float THRESHOLD = 0.95f;
    const float MAX_ATTEMPTS = 100;


    // Start is called before the first frame update
    void Start()
    {
        int[] currLevel = GetRandomLevel();
        float fitness = EvaluateLevel(currLevel); 

        
        int steps = 0;
        while (fitness < THRESHOLD && steps < MAX_ATTEMPTS)
        {
            steps++;
            List<int[]> neighbors = GetNeighbors(currLevel);
            
            foreach (int[] neighbor in neighbors)
            {
                float neighborFitness = EvaluateLevel(neighbor);
                if (neighborFitness > fitness)
                {
                    currLevel = neighbor;
                    fitness = neighborFitness;
                    break;
                }
            }
        }
        Debug.Log("Steps: " + steps);
        Debug.Log("Fitness: " + fitness);
        

        for (int i = 0; i < width; i++)
        {
            if (currLevel[i] == 0)
            {
                AddBasicColumns4(i);
            }
            else if (currLevel[i] == 1)
            {
                AddBlockColumns4(i);
            }
            else if (currLevel[i] == 2)
            {
                AddFlowerColumns4(i);
            }
            else if (currLevel[i] == 3)
            {
                AddBasicColumns6(i);
            }
            else if (currLevel[i] == 4)
            {
                AddBlockColumns6(i);
            }
            else if (currLevel[i] == 5)
            {
                AddFlowerColumns6(i);
            }
            else if (currLevel[i] == 6)
            {
                AddBasicGap(i);
            }
            else if (currLevel[i] == 7)
            {
                AddBlockGap(i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///////////////////////////// Evaluation /////////////////////////////
    public float EvaluateLevel(int[] level)
    {
        float gaps = 0;
        float blocks = 0;
        float platforms = 0;
        float flowers = 0;

        float scoreModifier =0;
        for (int i = 0; i < width; i++)
        {
            if (level[i] == 6)
            {
                gaps += 1f;
            }
            if (level[i] == 1 || level[i] == 4 || level[i] == 7)
            {
                blocks += 1f;
            }
            if (level[i] == 0 || level[i] == 3)
            {
                platforms += 1f;
            }
            if(level[i]== 2 || level[i] == 5)
            {
                flowers += 1f;
            }
            if(i<3 && level[i] == 6 )
            {
                scoreModifier -= 0.1f;
            }

        }

        float desiredGaps = (float)width / 5f;
        float desiredBlocks = (float)width / 10f;
        float desiredPlatforms = (float)width / 2f;
        float desiredFlowers = (float)width / 10f;


        float score = 0;
        score += 1.0f - Mathf.Abs(gaps - desiredGaps) / desiredGaps;
        score += 1.0f - Mathf.Abs(blocks - desiredBlocks) / desiredBlocks;
        score += 1.0f - Mathf.Abs(platforms - desiredPlatforms) / desiredPlatforms;
        score += 1.0f - Mathf.Abs(flowers - desiredFlowers) / desiredFlowers;

        score /= 4f;
        score += scoreModifier;

        // Penalties
        if (flowers > desiredFlowers)
         {
            score -= (flowers - desiredFlowers) * 0.1f;
        }

        return score;
    }

    ///////////////////////////// Neighbors Generation /////////////////////////////

    public List<int[]> GetNeighbors(int[] level)
    {
        List<int[]> neighbors = new List<int[]>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < 8 ;j++)
            {
                if (level[i] != j)
                {
                    int[] neighbor = (int[])level.Clone();
                    neighbor[i] = j;
                    neighbors.Add(neighbor);
                }
            }
        }

        for (int i = 0; i<neighbors.Count; i++){
            int[] temp = neighbors[i];
            int randomIndex = Random.Range(i, neighbors.Count);
            neighbors[i] = neighbors[randomIndex];
            neighbors[randomIndex] = temp;
        }


        return neighbors;
    }
    ///////////////////////////// LVL Generation /////////////////////////////
    public int[] GetRandomLevel()
    {
        int[] level = new int[width];
        for (int i = 0; i < width; i++)
        {
            level[i] = Random.Range(0, 8);
        }
        return level;
    }

    ///////////////////////////// Columns Generation /////////////////////////////
    //////////////// Spawn basic Columns ////////////////
    public void AddBasicColumns4(int initialPosX)
    {
        height = 4;
        for( int i = initialPosX-1; i<initialPosX ;i++)
        {
            for (int j = 0; j < height; j++)
            {
                SpawnObject(dirt, new Vector2(i, j));
            }
            SpawnObject(grass, new Vector2(i, height));
        }
    }

     public void AddBasicColumns6(int initialPosX)
    {
        height = 6;
        for( int i = initialPosX - 1; i<initialPosX; i++)
        {
            for (int j = 0; j < height; j++)
            {
                SpawnObject(dirt, new Vector2(i, j));
            }
            SpawnObject(grass, new Vector2(i, height));
        }
    }

     public void AddBasicGap(int initialPosX)
    {
        height = 0;
        for( int i = initialPosX - 1; i<initialPosX; i++)
        {
            SpawnObject(water, new Vector2(i, 0));
        }
    }

    //////////////// Spawn block Columns ////////////////

    public void AddBlockColumns4(int initialPosX)
    {
        AddBasicColumns4(initialPosX);
        SpawnObject(block, new Vector2(initialPosX-1, 8));
    }

    public void AddBlockColumns6(int initialPosX)
    {
        AddBasicColumns6(initialPosX);
        SpawnObject(block, new Vector2(initialPosX-1, 11));
    }

    public void AddBlockGap(int initialPosX)
    {
        AddBasicGap(initialPosX);
        SpawnObject(block, new Vector2(initialPosX-1, 8));
    }

    //////////////// Spawn flower Columns ////////////////

    public void AddFlowerColumns4(int initialPosX)
    {
        AddBasicColumns4(initialPosX);
        SpawnObject(flower, new Vector2(initialPosX-1, 5));
    }

    public void AddFlowerColumns6(int initialPosX)
    {
        AddBasicColumns6(initialPosX);
        SpawnObject(flower, new Vector2(initialPosX-1, 7));
    }


    
    
    //what we spawn will be a child of procedural generation game object
    private void SpawnObject(GameObject obj, Vector2 position)
    {
        obj = Instantiate(obj, position, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
