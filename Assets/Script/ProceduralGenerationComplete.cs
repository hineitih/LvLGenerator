using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerationComplete : MonoBehaviour
{
    private int MAX_HEIGHT = 55;
    private int MIN_HEIGHT = 5;

    [Header("General settings")]
    public int width;
    public int baseHeight;
    [Space(1)]

    [Header("Biomes")]
    public int plainProportion = 1;
    public int slopeProportion = 1;
    public int holeProportion = 3;
    public int hillProportion = 0;
    public int valleyProportion = 0;
    public bool selectAltPlain = true;
    [Space(1)]

    [Header("Assets")]
    public GameObject dirt;
    public GameObject grass;
    public GameObject altDirt;
    public GameObject altGrass;
    [Space(1)]

    [Header("Plain settings")]
    public int plainMinWidth = 10;  
    public int plainMaxWidth = 20;
    [Space(1)]

    [Header("Slope settings")]
    public int slopeMinWidth = 10;
    public int slopeMaxWidth = 30;
    public int slopeStepMinWidth = 1;
    public int slopeStepMaxWidth = 3;
    public int slopeStepMinHeight = 1;
    public int slopeStepMaxHeight = 3;
    [Space(1)]

    [Header("Hole settings")]
    public int holeMinWidth = 3;
    public int holeMaxWidth = 7;
    public int holePlatformAppearanceTreshold = 5;
    [Space(1)]

    [Header("Hill settings")]
    public int hillMinWidth = 20;
    public int hillMaxWidth = 40;
    public int hillStepMinWidth = 2;
    public int hillStepMaxWidth = 4;
    [Space(1)]

    [Header("Valely settings")]
    public int valleyMinWidth = 15;
    public int valleyMaxWidth = 30;
    public int valleyStepMinWidth = 1;
    public int valleyStepMaxWidth = 3;
    [Space(1)]

    [Header("Alt plains")]
    [Range(0, 100)]
    public int altPlainProbability = 33;
    public int altMinWidth = 3;
    public int altPlainMinHeightDiff = 5;
    public int altPlainMaxHeightDiff = 10;


    private enum Biome
    {
        NO_BIOME,
        PLAIN,
        SLOPE,
        HOLE,
        HILL,
        VALLEY
    }
    private List<Biome> allBiomes;


    // Start is called before the first frame update
    void Start()
    {
        allBiomes = new List<Biome>();

        for(int i=0;i<plainProportion;i++) {
            allBiomes.Add(Biome.PLAIN);
        }
        for(int i=0;i<slopeProportion;i++){ 
            allBiomes.Add(Biome.SLOPE);
        }
        for(int i=0;i<holeProportion;i++){ 
            allBiomes.Add(Biome.HOLE); 
        }
        for(int i=0;i<hillProportion;i++)
        {
            allBiomes.Add(Biome.HILL);
        }
        for(int i=0;i<valleyProportion;i++)
        {
            allBiomes.Add(Biome.VALLEY);
        }
        Generation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generation()
    {
        int totalBiomeWidth = 0;
        Biome lastBiome = Biome.NO_BIOME;
        for (int i = 0; i < width; i++)
        {
            Biome randomBiome = allBiomes[UnityEngine.Random.Range(0, allBiomes.Count)];


            int biomeWidth = 0;
            switch (randomBiome)
            {
                case Biome.SLOPE:
                    biomeWidth = UnityEngine.Random.Range(slopeMinWidth, slopeMaxWidth);
                    break;
                case Biome.HOLE:
                    biomeWidth = UnityEngine.Random.Range(holeMinWidth, holeMaxWidth);
                    break;
                case Biome.HILL:
                    biomeWidth = UnityEngine.Random.Range(hillMinWidth, hillMaxWidth);
                    break;
                case Biome.VALLEY:
                    biomeWidth = UnityEngine.Random.Range(valleyMinWidth, valleyMaxWidth);
                    break;
                default: //PLAIN
                    biomeWidth = UnityEngine.Random.Range(plainMinWidth, plainMaxWidth);
                    break;
            }

            if (lastBiome == randomBiome)
            {
                totalBiomeWidth += biomeWidth;
            }
            else
            {
                totalBiomeWidth = biomeWidth;
            }

            //alternative background biome
            if (selectAltPlain && (
                UnityEngine.Random.Range(0, 100) <= altPlainProbability
                || randomBiome == Biome.HOLE && totalBiomeWidth > holePlatformAppearanceTreshold))
            {
                Debug.Log(">>>>>>> Adding alt plain.");
                int plainBiomeWidth = UnityEngine.Random.Range(altMinWidth, biomeWidth);
                int initialPos = i + UnityEngine.Random.Range(0, biomeWidth - plainBiomeWidth);
                int initialHeight = baseHeight + UnityEngine.Random.Range(altPlainMinHeightDiff, altPlainMaxHeightDiff);
                AddAltPlatform(initialPos, plainBiomeWidth, initialHeight);
            }




            //primary biome
            switch (randomBiome) {
                case Biome.PLAIN:
                    Debug.Log(">>>>>>> Chosing biome : Plain.");
                    AddPlain(i, biomeWidth, baseHeight);
                    i += biomeWidth;
                    break;

                case Biome.SLOPE:
                    Debug.Log(">>>>>>> Chosing biome : Slope.");
                    int slopestepWidth = UnityEngine.Random.Range(slopeStepMinWidth, slopeStepMaxWidth);
                    int slopestepHeight = UnityEngine.Random.Range(slopeStepMinHeight, slopeStepMaxHeight);
                    int[] possibleDirection = { -1, 1};
                    int slopeDirection = possibleDirection[UnityEngine.Random.Range(0,2)];
                    Debug.Log("##### Slope direction : " + slopeDirection);


                    bool isSlopeOverflowing = false;
                    //controls TODO => wrong... why ?
                    if (slopeDirection > 0 && (baseHeight + slopestepHeight * (biomeWidth / slopestepWidth)) > MAX_HEIGHT)
                    {
                        slopeDirection = -1;
                        isSlopeOverflowing = true;
                    }
                    if (slopeDirection < 0 && (baseHeight - slopestepHeight * (biomeWidth / slopestepWidth)) < MIN_HEIGHT)
                    {
                        slopeDirection = 1;
                        if (isSlopeOverflowing)
                        {
                            break;
                        }
                    }
                    //

                    i += AddSlope(i, biomeWidth, baseHeight, slopestepWidth, slopestepHeight, slopeDirection);
                    break;

                case Biome.HOLE:
                    Debug.Log(">>>>>>> Chosing biome : Hole.");
                    AddHole(i, biomeWidth);
                    i += biomeWidth;
                    break;

                case Biome.HILL:
                    Debug.Log(">>>>>>> Chosing biome : Hill");
                    int hillstepWidth = UnityEngine.Random.Range(hillStepMinWidth, hillStepMaxWidth);

                    //controls
                    if(baseHeight + biomeWidth/(2*hillstepWidth) > MAX_HEIGHT) { break; }
                    //

                    i += AddHill(i, biomeWidth, baseHeight, hillstepWidth);
                    break;

                case Biome.VALLEY:
                    Debug.Log(">>>>>>> Chosing biome : Valley");
                    int valleystepWidth = UnityEngine.Random.Range(valleyStepMinWidth, valleyStepMaxWidth);

                    //controls
                    if (baseHeight - biomeWidth / (2 * valleystepWidth) < MIN_HEIGHT) { break; }
                    //

                    i += AddValley(i, biomeWidth, baseHeight, valleystepWidth);
                    break;
            }

            lastBiome = randomBiome;

            //transition between biomes
            if (randomBiome != Biome.HOLE)
            {
                AddBasicColumns(i, 1, baseHeight);
            }
        }

    }

    private void AddPlain(int initialPosX, int width, int height)
    {
        AddBasicColumns(initialPosX, width, height);
    }

    private void AddAltPlatform(int initialPosX, int width, int height)
    {
        AddAltColumns(initialPosX, width, height);
    }

    private int AddSlope(int initialPosX, int width, int height, int stepWidth, int stepHeight, int slopeDirection)
    {
        int actualWidth = 0;

        while (actualWidth < width)
        {
            height += slopeDirection * stepHeight;
            AddBasicColumns(initialPosX + actualWidth, stepWidth, height);
            actualWidth += stepWidth;
        }
        baseHeight = height;
        return actualWidth;
    }

    private void AddHole(int initialPosX, int width)
    {
        //skip biome generation to create a hole
    }


    private int AddHill(int initialPosX, int width, int height, int stepWidth)
    {
        int actualWidth = 0;
        while(actualWidth < width )
        {
            AddBasicColumns(initialPosX + actualWidth, stepWidth, height);
            if(actualWidth < width / 2)
            {
                height++;
            }
            else
            {
                height--;
            }
            actualWidth += stepWidth;
        }
        return actualWidth;
    }

    private int AddValley(int initialPosX, int width, int height, int stepWidth)
    {
        int actualWidth = 0;
        while (actualWidth < width)
        {
            AddBasicColumns(initialPosX + actualWidth, stepWidth, height);
            if (actualWidth < width / 2)
            {
                height--;
            }
            else
            {
                height++;
            }
            actualWidth += stepWidth;
        }
        return actualWidth;
    }

    private void AddBasicColumns(int initialPosX, int width, int height )
    {
        for( int i = initialPosX; i<initialPosX + width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                SpawnObject(dirt, new Vector2(i, j));
            }
            SpawnObject(grass, new Vector2(i, height));
        }
    }

    private void AddAltColumns(int initialPosX, int width, int height)
    {
        for (int i = initialPosX; i < initialPosX + width; i++)
        {
            for (int j = height-1; j < height; j++)
            {
                SpawnObject(altDirt, new Vector2(i, j));
            }
            SpawnObject(altGrass, new Vector2(i, height));
        }
    }

    //what we spawn will be a child of procedural generation game object
    private void SpawnObject(GameObject obj, Vector2 position)
    {
        obj = Instantiate(obj, position, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
