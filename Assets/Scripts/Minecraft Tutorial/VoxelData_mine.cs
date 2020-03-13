// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public static class VoxelData
// {
//     public static readonly int ChunkWidth = 5;
//     public static readonly int ChunkHeight = 10;
//     public static readonly int WorldSizeInChunks = 100;
//     public static int WorldSizeInVoxels 
//     {
//         get {return WorldSizeInChunks  * ChunkWidth;}
//     }

//     public static readonly int ViewDistanceInChunks = 5;
//     public static readonly int TextureAtLastSizeInBlocks = 4; //proportional textures, 64x64 (16 bit x 16 squares)
//     public static float NormalizedBlockTextureSize 
//     {
//         get { return 1f / (float)TextureAtLastSizeInBlocks;} //section in texture 4x4
//     }

//     public static readonly Vector3[] voxelVertices = new Vector3[8] 
//     {
//         new Vector3(0.0f, 0.0f, 0.0f),
//         new Vector3(1.0f, 0.0f, 0.0f),
//         new Vector3(1.0f, 1.0f, 0.0f),
//         new Vector3(0.0f, 1.0f, 0.0f),
//         new Vector3(0.0f, 0.0f, 1.0f),
//         new Vector3(1.0f, 0.0f, 1.0f),
//         new Vector3(1.0f, 1.0f, 1.0f),
//         new Vector3(0.0f, 1.0f, 1.0f)
//     };

//     public static readonly Vector3[] faceChecks = new Vector3[6]
//     {
//         new Vector3(0.0f, 0.0f, -1.0f), //back
//         new Vector3(0.0f, 0.0f, 1.0f), //front
//         new Vector3(0.0f, 1.0f, 0.0f), //top
//         new Vector3(0.0f, -1.0f, 0.0f), //bottom
//         new Vector3(-1.0f, 0.0f, 0.0f), //left
//         new Vector3(1.0f, 0.0f, 0.0f) //right
//     };

//     public static readonly int[,] voxelTriangles = new int[6,4]
//     {
//         {0, 3, 1, 2}, //back face
//         {5, 6, 4, 7}, //front face
//         {3, 7, 2, 6}, //top face
//         {1, 5, 0, 4}, //bottom face
//         {4, 7, 0, 3}, //left face
//         {1, 2, 5, 6} //right face
//     };

//     public static readonly Vector2[] voxelUvs = new Vector2[4] 
//     {
//         new Vector2(0.0f, 0.0f),
//         new Vector2(0.0f, 1.0f),  
//         new Vector2(1.0f, 0.0f),
//         new Vector2(1.0f, 1.0f)
//     };
// }
