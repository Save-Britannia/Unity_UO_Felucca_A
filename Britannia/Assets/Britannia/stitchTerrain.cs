#if (UNITY_EDITOR)

using UnityEngine;
using UnityEditor;

public class stitchTerrain : EditorWindow
{
    [MenuItem("Window/Stitch Britannia")]
    static void Init()
    {
        GetWindow<stitchTerrain>();
    }

    Transform transform;
    int xLen = 3; // Match with TerrainManager if using
    int zLen = 2;

    public void OnGUI()
    {
        transform = (Transform)EditorGUILayout.ObjectField("Terrain to start from", transform, typeof(Transform), true);

        if (GUILayout.Button("Start!"))
        {
            if (transform == null)
            {
                Debug.LogWarning("No terrain found");
                return;
            }

            Terrain origTerrain = transform.GetComponent<Terrain>();
            if (origTerrain == null)
            {
                Debug.LogWarning("No terrain found on transform");
            return;
            }

            for (int x = 0; x < xLen; x++)
            {
                for (int z = 0; z < zLen; z++)
                {
                    Debug.Log("x " + x + " z " + z);
                    GameObject center = GameObject.Find(string.Format("{0}{1}_{2}", "Britannia", x, z));
                    GameObject left = GameObject.Find(string.Format("{0}{1}_{2}", "Britannia", x - 1, z));
                    GameObject top = GameObject.Find(string.Format("{0}{1}_{2}", "Britannia", x, z - 1));
                    Debug.Log(string.Format("{0}{1}_{2}", "Britannia", x, z) + " - " + string.Format("{0}{1}_{2}", "Britannia", x - 1, z) + " - " + string.Format("{0}{1}_{2}", "Britannia", x, z - 1));
                    stitch_Terrain(center, left, top, x, z);
                }
            }
        }
    }

    void stitch_Terrain(GameObject center, GameObject left, GameObject top, int x, int z)
    {
        if (center == null)
        {
            return;
        }

        Terrain centerTerrain = center.GetComponent<Terrain>();
        float[,] centerHeights = centerTerrain.terrainData.GetHeights(0, 0, centerTerrain.terrainData.heightmapWidth, centerTerrain.terrainData.heightmapHeight);

        if (top != null)
        {
            Terrain topTerrain = top.GetComponent<Terrain>();
            float[,] topHeights = topTerrain.terrainData.GetHeights(0, 0, topTerrain.terrainData.heightmapWidth, topTerrain.terrainData.heightmapHeight);
            if (topHeights.GetLength(0) != centerHeights.GetLength(0))
            {
                Debug.Log("Terrain sizes must be equal");
                return;
            }

            for (int i = 0; i < centerHeights.GetLength(0); i++)
            {
                if (x == 0 && z == 1)
                    centerHeights[0, i] = topHeights[topHeights.GetLength(0) - 1, i] - 0.122f;
                if (x == 1 && z == 1)
                    centerHeights[0, i] = topHeights[topHeights.GetLength(0) - 1, i] + 0.0586095f;
                if (x == 2 && z == 1)
                    centerHeights[0, i] = topHeights[topHeights.GetLength(0) - 1, i] - 0.0388f;
            }
        }
        if (left != null)
        {
            Terrain leftTerrain = left.GetComponent<Terrain>();
            float[,] leftHeights = leftTerrain.terrainData.GetHeights(0, 0, leftTerrain.terrainData.heightmapWidth, leftTerrain.terrainData.heightmapHeight);
            if (leftHeights.GetLength(0) != centerHeights.GetLength(0))
            {
                Debug.Log("Terrain sizes must be equal");
                return;
            }
            for (int i = 0; i < centerHeights.GetLength(0); i++)
            {
                if (x == 1 && z == 0)
                    centerHeights[i, centerHeights.GetLength(0) - 1] = leftHeights[i, 0] - 0.2463f;
                if (x == 1 && z == 1)
                    centerHeights[i, centerHeights.GetLength(0) - 1] = leftHeights[i, 0] - 0.0665f;
                if (x == 2 && z == 0)
                    centerHeights[i, centerHeights.GetLength(0) - 1] = leftHeights[i, 0] + 0.19122f;
                if (x == 2 && z == 1)
                    centerHeights[i, centerHeights.GetLength(0) - 1] = leftHeights[i, 0] + 0.0939088f;
            }
        }
        centerTerrain.terrainData.SetHeights(0, 0, centerHeights);
    }
}
#endif