using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Texture3DRenderer : MonoBehaviour
{

    Texture3D tex;
    public Shader shader;
    string FilePath = "Resources/head/head-pgm";
    string FileNamePrefix = "head";
    string FileTypeExtension = ".pgm";
    int Width = 256, Height = 256, Depth = 64;

    // Use this for initialization
    void Start()
    {
        tex = new Texture3D(Width, Height, Depth, TextureFormat.Alpha8, false);

        Color[] newC = new Color[Width * Height * Depth];
        float oneOverWidth = 1.0f / (1.0f * Width - 1.0f);
        float oneOverHeight = 1.0f / (1.0f * Width - 1.0f);
        float oneOverDepth = 1.0f / (1.0f * Width - 1.0f);

        string path = Path.Combine(Application.dataPath, FilePath);

        for (int i = 0; i < Depth; i++)
        { 
            // load ppm/pgm file
            string filePath = string.Format("{0}/{1}-{2:d3}{3}", path, FileNamePrefix, i, FileTypeExtension);
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);

                for (int j = 0; j < Width; j++)
                {
                    for (int k = 0; k < Height; k++)
                    {
                        byte data = fileData[j + (k * Height)];
                        newC[j + (k * Width) + (i * Width * Height)] = new Color(1, 0, 0, data);
                    }
                }
            }
            else
                Debug.LogFormat("File '{0}' does not exist!", filePath);
        }

        tex.SetPixels(newC);
        tex.Apply();

        Renderer r = GetComponent<Renderer>();
        r.material.shader = shader;
        r.material.SetTexture("_Volume", tex);
        

        //GetComponent<Renderer>().material.SetTexture("_tex", tex);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
