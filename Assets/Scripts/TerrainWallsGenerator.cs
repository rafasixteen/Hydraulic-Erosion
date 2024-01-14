using Assets.Scripts.Jobs;
using Assets.Scripts.Meshes;
using Assets.Scripts.Meshes.Generators;
using Assets.Scripts.Meshes.Streams;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainWallsGenerator
    {
        private readonly TerrainData terrainData;

        private readonly Mesh terrainWallsMesh;
        private Vector3[] terrainMeshVertices;

        public TerrainWallsGenerator(TerrainData terrainData)
        {
            this.terrainData = terrainData;
            terrainWallsMesh = new Mesh
            {
                name = "Terrain Wall Mesh",
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };
        }

        /// <summary>
        /// Updates The Terrain Mesh Vertices Wich Are Required To Regenerate The Wall Vertices.
        /// </summary>
        public void UpdateTerrainMeshVertices(Vector3[] vertices)
        {
            terrainMeshVertices = vertices;
        }

        public Mesh GenerateMesh()
        {
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];

            DebugUI.TerrainWallVerticesCompleted(() => MeshIJob<TerrainWallVertices, SingleStream>.Schedule(terrainWallsMesh, meshData, terrainData, terrainMeshVertices, default));
            DebugUI.TerrainWallTrianglesCompleted(() => MeshIJob<TerrainWallTriangles, SingleStream>.Schedule(terrainWallsMesh, meshData, terrainData, terrainMeshVertices, default));

            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, terrainWallsMesh);

            DebugUI.TerrainWallNormalsCompleted(terrainWallsMesh.RecalculateNormals);

            return terrainWallsMesh;
        }
    }
}