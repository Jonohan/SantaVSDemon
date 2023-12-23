using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    public float color(int centerx, int centery, float radius, int color)
    {
        float oppoColor = 0f;
        int thisColor = 0;
        float total = 0;
        for (int i = centery - (int)radius - 1; i <= centery + (int)radius + 1; i++)
        {
            if (i >= 0 && i < gridlen)
            {
                for (int j = centerx - (int)radius - 1; j <= centerx + (int)radius + 1; j++)
                {
                    if (j >= 0 && j < gridlen && (j - centerx) * (j - centerx) + (i - centery) * (i - centery) <= radius * radius)
                    {
                        if (colorgrid[i][j] == color)
                        {
                            thisColor++;
                        }else if ( colorgrid[i][j] != 0)
                        {
                            oppoColor++;
                        }
                        colorgrid[i][j] = color;
                        total++;
                    }
                }
            }

        }
        if (oppoColor == 0)
        {
            oppoColor = - thisColor/total;
        }
        return oppoColor;
    }

    public string toString()
    {
        string buffer = "";
        for (int i = 0; i < gridlen; i++)
        {
            for (int j = 0; j < gridlen - 1; j++)
            {
                buffer += colorgrid[i][j].ToString() + ",";
            }
            buffer += colorgrid[i][gridlen - 1].ToString() + "\n";
        }
        return buffer;
    }

    void OnDestroy()
    {
        //WriteString(toString());
        //Debug.Log(redWins());
    }

    static void WriteString(string str)
    {
        string path = "./trace.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(str);
        writer.Close();
    }

    public float redPerc()
    {
        int red = 0;
        for (int i = 0; i < gridlen; i++)
        {
            for (int j = 0; j < gridlen; j++)
            {
                if (colorgrid[i][j] == 2)
                {
                    red++;
                }
            }
        }
        return red;
    }

    public float bluePerc()
    {
        int red = 0;
        int blue = 0;
        for (int i = 0; i < gridlen; i++)
        {
            for (int j = 0; j < gridlen; j++)
            {
                if (colorgrid[i][j] == 1)
                {
                    blue++;
                }
                else if (colorgrid[i][j] == 2)
                {
                    red++;
                }
            }
        }
        return red;
    }
}
