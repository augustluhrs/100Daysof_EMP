// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Chunk 
// {
//     public ChunkCoord coord;
//     GameObject chunkObject;
//     MeshRenderer meshRenderer;
//     MeshFilter meshFilter;

//     int vertexIndex = 0;
//     List<Vector3> vertices = new List<Vector3>();
//     List<int> triangles = new List<int>();
//     List<Vector2> uvs = new List<Vector2>();
   
//     byte [,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
//     // bool [,,] voxelMap = new bool[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
//     World world;

//     public Chunk(ChunkCoord _coord, World _world) 
//     {
//         coord = _coord;
//         world = _world;
//         chunkObject = new GameObject();
//         meshFilter = chunkObject.AddComponent<MeshFilter>();
//         meshRenderer = chunkObject.AddComponent<MeshRenderer>();

//         meshRenderer.material = world.material;
//         chunkObject.transform.SetParent(world.transform);
//         chunkObject.transform.position = new Vector3(coord.x * VoxelData.ChunkWidth, 0f,  coord.z * VoxelData.ChunkWidth);
//         chunkObject.name = "Chunk " + coord.x + ", " + coord.z;
//         PopulateVoxelMap();
//         CreateMeshData();
//         CreateMesh();
        
//     }
//     // void Start()
//     // {
//     //     world = GameObject.Find("World").GetComponent<World>();
//     //     PopulateVoxelMap();
//     //     CreateMeshData();
//     //     CreateMesh();
        
//     // }
//     void PopulateVoxelMap()
//     {
//         for (int y = 0; y < VoxelData.ChunkHeight; y++)
//         {
//             for (int x = 0; x < VoxelData.ChunkWidth; x++)
//             {
//                 for (int z = 0; z < VoxelData.ChunkWidth; z++)
//                 {
//                     voxelMap[x, y, z] = world.GetVoxel(new Vector3(x, y, z) + position);
//                     // if (y<1)
//                     // {
//                     //     voxelMap[x,y,z] = 1;
//                     // }
//                     // else if (y == VoxelData.ChunkHeight - 1)
//                     // {
//                     //     voxelMap[x,y,z] = 3;
//                     // }
//                     // else 
//                     // {
//                     //     voxelMap[x,y,z] = 2;
//                     // }
//                     // voxelMap[x, y, z] = 2;
//                 }
//             }
//         }
//     }
//     void CreateMeshData()
//     {
//         for (int y = 0; y < VoxelData.ChunkHeight; y++)
//         {
//             for (int x = 0; x < VoxelData.ChunkWidth; x++)
//             {
//                 for (int z = 0; z < VoxelData.ChunkWidth; z++)
//                 {
//                     AddVoxelDataToChunk(new Vector3(x, y, z));
//                 }
//             }
//         }
//     }

//     bool isVoxelInChunk(int x, int y, int z)
//     {
//         if(x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth -1)
//         {
//             return false;
//         }
//         else
//         {
//             return true;
//         }
//     }
//     public bool isActive
//     {
//         get { return chunkObject.activeSelf;}
//         set { chunkObject.SetActive(value);}
//     }
//     public Vector3 position
//     {
//         get { return chunkObject.transform.position; }
//     }
//     bool CheckVoxel(Vector3 pos)
//     {
//         int x = Mathf.FloorToInt(pos.x);
//         int y = Mathf.FloorToInt(pos.y);
//         int z = Mathf.FloorToInt(pos.z);

//         if(!isVoxelInChunk(x, y, z)) //only if outside the chunk
//             return world.blocktypes[world.GetVoxel(pos + position)].isSolid;
        
//         return world.blocktypes[voxelMap[x, y, z]].isSolid;
//     }
//     void AddVoxelDataToChunk(Vector3 pos)
//     {
//         for (int p = 0; p < 6; p++)
//         {
//             if(!CheckVoxel(pos + VoxelData.faceChecks[p]))
//             {
//                 byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];
//                 vertices.Add(pos + VoxelData.voxelVertices[VoxelData.voxelTriangles[p, 0]]);
//                 vertices.Add(pos + VoxelData.voxelVertices[VoxelData.voxelTriangles[p, 1]]);
//                 vertices.Add(pos + VoxelData.voxelVertices[VoxelData.voxelTriangles[p, 2]]);
//                 vertices.Add(pos + VoxelData.voxelVertices[VoxelData.voxelTriangles[p, 3]]);
//                 // uvs.Add(VoxelData.voxelUvs[0]);
//                 // uvs.Add(VoxelData.voxelUvs[1]);
//                 // uvs.Add(VoxelData.voxelUvs[2]);
//                 // uvs.Add(VoxelData.voxelUvs[3]);
//                 //for stone
//                 AddTexture(world.blocktypes[blockID].GetTextureID(p));

//                 triangles.Add(vertexIndex);
//                 triangles.Add(vertexIndex + 1);
//                 triangles.Add(vertexIndex + 2);
//                 triangles.Add(vertexIndex + 2);
//                 triangles.Add(vertexIndex + 1);
//                 triangles.Add(vertexIndex + 3);
//                 vertexIndex += 4;


                
//                 // for (int i = 0; i < 6; i++) 
//                 // {
//                 // int triangleIndex = VoxelData.voxelTriangles[p, i];
//                 // vertices.Add(VoxelData.voxelVertices[triangleIndex] + pos);
//                 // triangles.Add(vertexIndex);
//                 // uvs.Add(VoxelData.voxelUvs[i]);
//                 // vertexIndex++;
//                 // }
//             }
//         }
//     }

//     void CreateMesh()
//     {
//         Mesh mesh = new Mesh();
//         mesh.vertices = vertices.ToArray();
//         mesh.triangles = triangles.ToArray();
//         mesh.uv = uvs.ToArray();
//         mesh.RecalculateNormals();
//         meshFilter.mesh = mesh;
//     }

//     void AddTexture (int textureID)
//     {
//         float y = textureID / VoxelData.TextureAtLastSizeInBlocks;
//         float x = textureID - (y * VoxelData.TextureAtLastSizeInBlocks);

//         x *= VoxelData.NormalizedBlockTextureSize;
//         y *= VoxelData.NormalizedBlockTextureSize;
        
//         //because texture map is 0-15 from top left, and voxel map starts at bottom left
//         y = 1f - y - VoxelData.NormalizedBlockTextureSize;

//         //offset
//         uvs.Add(new Vector2(x, y));
//         uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
//         uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
//         uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));

//     }

// }

// public class ChunkCoord
// {
//     public int x,z;
//     public ChunkCoord(int _x, int _z)
//     {
//         x = _x;
//         z = _z;
//     }

//     public bool Equals (ChunkCoord other)
//     {
//         if (other == null)
//             return false;
//         else if (other.x == x && other.z == z)
//             return true;
//         else
//             return false;
//     }
// }