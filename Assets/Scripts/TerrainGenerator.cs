using Assets.Scripts.Jobs;
using Assets.Scripts.Meshes;
using Assets.Scripts.Meshes.Generators;
using Assets.Scripts.Meshes.Streams;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class TerrainGenerator
    {
        private readonly TerrainData terrainData;

        private readonly Mesh terrainMesh;

        public TerrainGenerator(TerrainData terrainData)
        {
            this.terrainData = terrainData;
            terrainMesh = new Mesh
            {
                name = "Terrain Mesh"
            };
        }

        public Mesh GenerateMesh()
        {
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];

            DebugUI.TerrainVerticesCompleted(MeshIJobFor<TerrainVertices, SingleStream>.ScheduleParallel(terrainMesh, meshData, terrainData, default).Complete);
            DebugUI.TerrainTrianglesCompleted(MeshIJobFor<TerrainTriangles, SingleStream>.ScheduleParallel(terrainMesh, meshData, terrainData, default).Complete);
            DebugUI.TerrainNormalsCompleted(Normals.ScheduleParallel(meshData, terrainData, default).Complete);

            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, terrainMesh);

            return terrainMesh;
        }
    }
}