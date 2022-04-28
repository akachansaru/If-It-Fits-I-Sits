using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public int levelNum;

    public int numSmallCats;
    public int numMedCats;
    public int numLargeCats;

    public float catSpeed;

    public List<Sizes> remainingCats;

    public void GenerateLevel()
    {
        remainingCats = new List<Sizes>();

        for (int i = 0; i < numSmallCats; i++)
        {
            remainingCats.Add(Sizes.Small);
        }

        for (int i = 0; i < numMedCats; i++)
        {
            remainingCats.Add(Sizes.Medium);
        }

        for (int i = 0; i < numLargeCats; i++)
        {
            remainingCats.Add(Sizes.Large);
        }
    }
}
