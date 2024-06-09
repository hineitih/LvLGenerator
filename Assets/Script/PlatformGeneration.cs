using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public int width, height;
    public int minGapWidth, maxGapWidth;
    public int minHeight, maxHeight;
    public int repeatValue, repeatNum;
    public GameObject dirt;
    public GameObject grass;

    private int gapCounter;

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
        gapCounter = Random.Range(minGapWidth, maxGapWidth);
        
        repeatValue = 0;
        for(int i = 0; i < width; i++){

            // if (gapCounter == 0){
            //     CreatePlatformWithGap(i);
            //     gapCounter = Random.Range(minGapWidth, maxGapWidth);
            // }
            // else{
            //     CreatePlatformWithGap(i);
            //     gapCounter--;
            // }



            if (repeatValue == 0){
                height = Random.Range(minHeight, maxHeight);
                platform(i);
                repeatValue = repeatNum;
            }
            else{
                platform(i);
                repeatValue--;
            }
            
            
        }
        
    }

    private void platform(int i){
        for(int j = 0; j < height; j++){
                SpawnObject(dirt, new Vector2(i, j));
            }
        SpawnObject(grass, new Vector2(i, height));
    }

     private void CreatePlatformWithGap(int i)
    {
        int height = Random.Range(minHeight, maxHeight);
        for (int j = 0; j < height; j++)
        {
            SpawnObject(dirt, new Vector2(i, j));
        }
        SpawnObject(grass, new Vector2(i, height));
        // Increase the gap width if needed
        gapCounter = Random.Range(minGapWidth, maxGapWidth);
    }

    private void hill(){
          for(int i = 0; i < width; i++){
            height = Random.Range(minHeight, maxHeight);
            
            SpawnObject(grass, new Vector2(i, height));
        }
    }

    //what we spawn will be a child of procedural generation game object
    private void SpawnObject(GameObject obj, Vector2 position){ 
        obj =Instantiate(obj, position, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
