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

    public int color(int centerx, int centery, float radius, int color)
    {
        int oppoColor = 0;
        for (int i = centery - (int)radius - 1; i <= centery + (int)radius + 1; i++)
        {
            if (i >= 0 && i < gridlen)
            {
                for (int j = centerx - (int)radius - 1; j <= centerx + (int)radius + 1; j++)
                {
                    if (j >= 0 && j < gridlen && (j - centerx) * (j - centerx) + (i - centery) * (i - centery) <= radius * radius)
                    {
                        if (colorgrid[i][j] != color)
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

    public string toString()
    {
        string buffer = "";
        for (int i = 0; i < gridlen; i++)
        {
            for (int j = 0; j < gridlen; j++)
            {
                buffer += colorgrid[i][j].ToString();
            }
            buffer += "\n";
        }
        return buffer;
    }

    void OnDestroy()
    {
        Debug.Log(toString());
    }
}
