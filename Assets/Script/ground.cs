using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour
{
    private const int gridlen = 300;
    private int[][] colorgrid;
    // Start is called before the first frame update
    void Start()
    {
        colorgrid = new int[gridlen][];
        for (int i = 0; i < gridlen; i++)
        {
            colorgrid[i] = new int[gridlen];
            for (int j = 0; j < gridlen; j++)
            {
                colorgrid[i][j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    int color(int centerx, int centery, int radius, int color)
    {
        int oppoColor = 0;
        for (int i = centery - radius; i <= centery + radius; i++)
        {
            if (i >= 0 && i < gridlen)
            {
                for (int j = centerx - radius; j <= centerx + radius; j++)
                {
                    if (j >= 0 && j < gridlen && (j - centerx) * (j - centerx) + (i - centery) * (i - centery) <= radius * radius)
                    {
                        if (colorgrid[i][j] != color && colorgrid[i][j] != 0)
                        {
                            oppoColor++;
                        }
                        colorgrid[i][j] = color;
                    }
                }
            }

        }
        return oppoColor;
    }
}
