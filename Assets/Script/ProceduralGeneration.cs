using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public int width, height;
    public GameObject dirt;
    public GameObject grass;

    private int minHeight, maxHeight;



    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generation(){
        for(int i = 0; i < width; i++){
            minHeight = height - 1;
            maxHeight = height + 2;
            height = Random.Range(minHeight, maxHeight);
            for(int j = 0; j < height; j++){
                SpawnObject(dirt, new Vector2(i, j));
            }
            SpawnObject(grass, new Vector2(i, height));
        }
        
    }

    //what we spawn will be a child of procedural generation game object
    private void SpawnObject(GameObject obj, Vector2 position){ 
        obj =Instantiate(obj, position, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
