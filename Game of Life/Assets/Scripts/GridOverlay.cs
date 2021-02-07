using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    private Material lineMaterial;

    public Color mainColor = new Color(0f, 1f, 0f, 1f);
    public Color subColor = new Color(0f, 0.5f, 0f, 1f);

    public bool showMain = true;
    public bool showSub = false;

    public int gridSizeX;
    public int gridSizeY;

    public float startX;
    public float startY;
    public float startZ;

    public float smallStep;
    public float largeStep;
   
    void CreateLineMaterial()
    {
        if(!lineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);

            // hides from garbace collector
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // disable depth writing
            lineMaterial.SetInt("_ZWrite", 0);

            // disable backface culling
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }

    private void OnDisable()
    {
        // destroy the instance of lineMaterial
        DestroyImmediate(lineMaterial);
    }

    private void OnPostRender()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (showSub)
        {
            GL.Color(subColor);
            for (float y = 0; y <= gridSizeY; y+=smallStep)
            {
                // line horizontal
                GL.Vertex3(startX, startY + y, startZ); // 0 0 0
                GL.Vertex3(startX + gridSizeX, startY + y, startZ); // 64 0 0 
            }

            for (float x = 0; x <= gridSizeX; x+=smallStep)
            {
                // vertical
                GL.Vertex3(startX + x, startY, startZ);
                GL.Vertex3(startX + x, startY + gridSizeY, startZ);
            }
        } 

        if (showMain)
        {
            GL.Color(mainColor);
            for (float y = 0; y <= gridSizeY; y += largeStep)
            {
                // line horizontal
                GL.Vertex3(startX, startY + y, startZ); // 0 0 0
                GL.Vertex3(startX + gridSizeX, startY + y, startZ); // 64 0 0 
            }

            for (float x = 0; x <= gridSizeX; x += largeStep)
            {
                // vertical
                GL.Vertex3(startX + x, startY, startZ);
                GL.Vertex3(startX + x, startY + gridSizeY, startZ);
            }
        }

        GL.End();
    }
}
