using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class Texture3DRenderer : MonoBehaviour
{
    public Transform RenderReflector;
    Texture3D tex;
    Material mat;
    public Shader shader;
    string FilePath = "Resources/head/head-pgm";
    string FileNamePrefix = "head";
    string FileTypeExtension = ".pgm";
    int Width = 256, Height = 256, Depth = 128;

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
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string format = sr.ReadLine();
                    string heightWidth = sr.ReadLine();
                    string maxBitS = sr.ReadLine();
                    int maxBit;
                    int.TryParse(maxBitS, out maxBit);
                    byte[] bytes = new byte[256];

                    // read bytes
                    for (int k = 0; k < Height; k++)
                    {
                        //Debug.Log("Reading line K = " + k);
                        sr.BaseStream.Read(bytes, 0, Width);

                        for (int j = 0; j < Width; j++)
                        {
                            float val = (float)bytes[j] / (float)maxBit;
                            newC[j + (k * Width) + (i * Width * Height)]
                                = new Color(1, 1, 1, val);
                        }
                    }
                }

            }
            else
                Debug.LogFormat("File '{0}' does not exist!", filePath);
        }

        tex.SetPixels(newC);
        tex.Apply();

        Renderer renderer = GetComponent<Renderer>();
        mat = renderer.material;
        mat.shader = shader;
        mat.SetTexture("_Volume", tex);



    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnPostRender()
    {

        //GL.LoadPixelMatrix();
        //GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
        GL.Color(new Color(1f, 0.0f, 0.0f, 1f));
        GL.Begin(GL.QUADS);
        mat.SetPass(0);
        mat.SetFloat("_Depth", Depth);
        mat.SetMatrix("_Transform", RenderReflector.transform.localToWorldMatrix);

        for (int i = 0; i < Depth; i++)
        {
            float d = (float)i / Depth;
            GL.Vertex3(0, 0, d);
            GL.Vertex3(0, 1, d);
            GL.Vertex3(1, 1, d);
            GL.Vertex3(1, 0, d);
        }
        GL.End();
    }
}
