using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public int seed;
    public BiomeAttributes biome;

    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public BlockType[] blocktypes;

    Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    public ChunkCoord playerChunkCoord;
    ChunkCoord playerLastChunkCoord;

    List<ChunkCoord> chunksToCreate = new List<ChunkCoord>();
    private bool isCreatingChunks;

    public GameObject debugScreen;

    private void Start() {

        Random.InitState(seed);

        spawnPosition = new Vector3((VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f, VoxelData.ChunkHeight - 50f, (VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f);
        GenerateWorld();
        playerLastChunkCoord = GetChunkCoordFromVector3(player.position);
        

    }

    private void Update() {

        playerChunkCoord = GetChunkCoordFromVector3(player.position);

        // Only update the chunks if the player has moved from the chunk they were previously on.
        if (!playerChunkCoord.Equals(playerLastChunkCoord))
            CheckViewDistance();

        if (chunksToCreate.Count > 0 && !isCreatingChunks)
            StartCoroutine("CreateChunks");

        if (Input.GetKeyDown(KeyCode.F3))
            debugScreen.SetActive(!debugScreen.activeSelf);


    }

    void GenerateWorld () {

        for (int x = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; x < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; x++) {
            for (int z = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; z < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; z++) {

                chunks[x, z] = new Chunk(new ChunkCoord(x, z), this, true);
                activeChunks.Add(new ChunkCoord(x, z));

            }
        }

        player.position = spawnPosition;

    }

    IEnumerator CreateChunks () {

        isCreatingChunks = true;

        while (chunksToCreate.Count > 0) {

            chunks[chunksToCreate[0].x, chunksToCreate[0].z].Init();
            chunksToCreate.RemoveAt(0);
            yield return null;

        }

        isCreatingChunks = false;
        
    }

    ChunkCoord GetChunkCoordFromVector3 (Vector3 pos) {

        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
        return new ChunkCoord(x, z);

    }

    public Chunk GetChunkFromVector3 (Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
        return chunks[x,z];

    }

    void CheckViewDistance () {

        ChunkCoord coord = GetChunkCoordFromVector3(player.position);
        playerLastChunkCoord = playerChunkCoord;

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

        // Loop through all chunks currently within view distance of the player.
        for (int x = coord.x - VoxelData.ViewDistanceInChunks; x < coord.x + VoxelData.ViewDistanceInChunks; x++) {
            for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++) {

                // If the current chunk is in the world...
                if (IsChunkInWorld (new ChunkCoord (x, z))) {

                    // Check if it active, if not, activate it.
                    if (chunks[x, z] == null) {
                        chunks[x, z] = new Chunk(new ChunkCoord(x, z), this, false);
                        chunksToCreate.Add(new ChunkCoord(x, z));
                    }  else if (!chunks[x, z].isActive) {
                        chunks[x, z].isActive = true;
                    }
                    activeChunks.Add(new ChunkCoord(x, z));
                }

                // Check through previously active chunks to see if this chunk is there. If it is, remove it from the list.
                for (int i = 0; i < previouslyActiveChunks.Count; i++) {

                    if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
                        previouslyActiveChunks.RemoveAt(i);
                       
                }

            }
        }

        // Any chunks left in the previousActiveChunks list are no longer in the player's view distance, so loop through and disable them.
        foreach (ChunkCoord c in previouslyActiveChunks)
            chunks[c.x, c.z].isActive = false;

    }

    public bool CheckForVoxel (Vector3 pos) {

        ChunkCoord thisChunk = new ChunkCoord(pos);

        if (!IsChunkInWorld(thisChunk) || pos.y < 0 || pos.y > VoxelData.ChunkHeight)
            return false;

        if (chunks[thisChunk.x, thisChunk.z] != null && chunks[thisChunk.x, thisChunk.z].isVoxelMapPopulated)
            return blocktypes[chunks[thisChunk.x, thisChunk.z].GetVoxelFromGlobalVector3(pos)].isSolid;

        return blocktypes[GetVoxel(pos)].isSolid;

    }


    public byte GetVoxel (Vector3 pos) {

        int yPos = Mathf.FloorToInt(pos.y);

        /* IMMUTABLE PASS */

        // If outside world, return air.
        if (!IsVoxelInWorld(pos))
            return 0;

        // If bottom block of chunk, return bedrock.
        if (yPos == 0)
            return 1;

        /* BASIC TERRAIN PASS */

        int terrainHeight = Mathf.FloorToInt(biome.terrainHeight * Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.terrainScale)) + biome.solidGroundHeight;
        byte voxelValue = 0;

        if (yPos == terrainHeight)
            voxelValue = 3;
        else if (yPos < terrainHeight && yPos > terrainHeight - 4)
            voxelValue = 5;
        else if (yPos > terrainHeight)
            return 0;
        else
            voxelValue = 2;

        /* SECOND PASS */

        if (voxelValue == 2) {

            foreach (Lode lode in biome.lodes) {

                if (yPos > lode.minHeight && yPos < lode.maxHeight)
                    if (Noise.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
                        voxelValue = lode.blockID;

            }

        }

        return voxelValue;


    }

    bool IsChunkInWorld (ChunkCoord coord) {

        if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
            return true;
        else
            return
                false;

    }

    bool IsVoxelInWorld (Vector3 pos) {

        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
            return true;
        else
            return false;

    }

}

[System.Serializable]
public class BlockType {

    public string blockName;
    public bool isSolid;
    public Sprite icon;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    // Back, Front, Top, Bottom, Left, Right

    public int GetTextureID (int faceIndex) {

        switch (faceIndex) {

            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;


        }

    }

}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class World : MonoBehaviour {

//     public int seed;
//     public BiomeAttributes biome;

//     public Transform player;
//     public Vector3 spawnPosition;

//     public Material material;
//     public BlockType[] blocktypes;

//     Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

//     List<ChunkCoord> activeChunks = new List<ChunkCoord>();
//     ChunkCoord playerChunkCoord;
//     ChunkCoord playerLastChunkCoord;

//     private void Start() {

//         Random.InitState(seed);
//         // spawnPosition = new Vector3(2f, VoxelData.ChunkHeight + 2f,2f);

//         spawnPosition = new Vector3((VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f, VoxelData.ChunkHeight + 2f, (VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f);
//         GenerateWorld();
//         playerLastChunkCoord = GetChunkCoordFromVector3(player.position);
        

//     }

//     private void Update() {

//         playerChunkCoord = GetChunkCoordFromVector3(player.position);

//         // Only update the chunks if the player has moved from the chunk they were previously on.

//         // if (!playerChunkCoord.Equals(playerLastChunkCoord))
//             // CheckViewDistance();


//     }

//     void GenerateWorld () {

//         for (int x = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; x < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; x++) {
//             for (int z = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; z < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; z++) {

//                 CreateNewChunk(x, z);

//             }
//         }

//         player.position = spawnPosition;

//     }

//     ChunkCoord GetChunkCoordFromVector3 (Vector3 pos) {

//         int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
//         int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
//         return new ChunkCoord(x, z);

//     }

//     void CheckViewDistance () {

//         ChunkCoord coord = GetChunkCoordFromVector3(player.position);

//         List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

//         // Loop through all chunks currently within view distance of the player.
//         for (int x = coord.x - VoxelData.ViewDistanceInChunks; x < coord.x + VoxelData.ViewDistanceInChunks; x++) {
//             for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++) {

//                 // If the current chunk is in the world...
//                 if (IsChunkInWorld (new ChunkCoord (x, z))) {

//                     // Check if it active, if not, activate it.
//                     if (chunks[x, z] == null)
//                         CreateNewChunk(x, z);
//                     else if (!chunks[x, z].isActive) {
//                         chunks[x, z].isActive = true;
//                         activeChunks.Add(new ChunkCoord(x, z));
//                     }

//                 }

//                 // Check through previously active chunks to see if this chunk is there. If it is, remove it from the list.
//                 for (int i = 0; i < previouslyActiveChunks.Count; i++) {

//                     if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
//                         previouslyActiveChunks.RemoveAt(i);
                       
//                 }

//             }
//         }

//         // Any chunks left in the previousActiveChunks list are no longer in the player's view distance, so loop through and disable them.
//         foreach (ChunkCoord c in previouslyActiveChunks)
//             chunks[c.x, c.z].isActive = false;

//     }

//     public bool CheckForVoxel(float _x, float _y, float _z)
//     {
//         int xCheck = Mathf.FloorToInt(_x);
//         int yCheck = Mathf.FloorToInt(_y);
//         int zCheck = Mathf.FloorToInt(_z);

//         int xChunk = xCheck / VoxelData.ChunkWidth;
//         int zChunk = zCheck / VoxelData.ChunkWidth;

//         xCheck -= (xChunk * VoxelData.ChunkWidth); //no idea what's going on -- chunk movement limits?
//         zCheck -= (zChunk * VoxelData.ChunkWidth);

//         return blocktypes[chunks[xChunk, zChunk].voxelMap[xCheck, yCheck, zChunk]].isSolid;

       
//     }

//     public byte GetVoxel (Vector3 pos) {

//         int yPos = Mathf.FloorToInt(pos.y);

//         /* IMMUTABLE PASS */

//         // If outside world, return air.
//         if (!IsVoxelInWorld(pos))
//             return 0;

//         // If bottom block of chunk, return bedrock.
//         if (yPos == 0)
//             return 1;

//         /* BASIC TERRAIN PASS */

//         int terrainHeight = Mathf.FloorToInt(biome.terrainHeight * Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.terrainScale)) + biome.solidGroundHeight;
//         byte voxelValue = 0;

//         if (yPos == terrainHeight)
//             voxelValue = 3;
//         else if (yPos < terrainHeight && yPos > terrainHeight - 4)
//             voxelValue = 5;
//         else if (yPos > terrainHeight)
//             return 0;
//         else
//             voxelValue = 2;

//         /* SECOND PASS */

//         if (voxelValue == 2) {

//             foreach (Lode lode in biome.lodes) {

//                 if (yPos > lode.minHeight && yPos < lode.maxHeight)
//                     if (Noise.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
//                         voxelValue = lode.blockID;

//             }

//         }

//         return voxelValue;


//     }

//     void CreateNewChunk (int x, int z) {

//         chunks[x, z] = new Chunk(new ChunkCoord(x, z), this);
//         activeChunks.Add(new ChunkCoord(x, z));

//     }

//     bool IsChunkInWorld (ChunkCoord coord) {

//         if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
//         // if (coord.x >= 0 && coord.x < VoxelData.WorldSizeInChunks && coord.z >= 0 && coord.z < VoxelData.WorldSizeInChunks) //youtube fix that didn't work
//             return true;
//         else
//             return
//                 false;

//     }

//     bool IsVoxelInWorld (Vector3 pos) {

//         if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
//             return true;
//         else
//             return false;

//     }

// }

// [System.Serializable]
// public class BlockType {

//     public string blockName;
//     public bool isSolid;

//     [Header("Texture Values")]
//     public int backFaceTexture;
//     public int frontFaceTexture;
//     public int topFaceTexture;
//     public int bottomFaceTexture;
//     public int leftFaceTexture;
//     public int rightFaceTexture;

//     // Back, Front, Top, Bottom, Left, Right

//     public int GetTextureID (int faceIndex) {

//         switch (faceIndex) {

//             case 0:
//                 return backFaceTexture;
//             case 1:
//                 return frontFaceTexture;
//             case 2:
//                 return topFaceTexture;
//             case 3:
//                 return bottomFaceTexture;
//             case 4:
//                 return leftFaceTexture;
//             case 5:
//                 return rightFaceTexture;
//             default:
//                 Debug.Log("Error in GetTextureID; invalid face index");
//                 return 0;


//         }

//     }

// }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class World : MonoBehaviour {

//     public int seed;
//     public BiomeAttributes biome;
//     public Transform player;
//     public Vector3 spawnLocation;

//     public Material material;
//     public BlockType[] blocktypes;

//     Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];
//     List<ChunkCoord> activeChunks = new List<ChunkCoord>();
//     ChunkCoord playerLastChunkCoord;

//     private void Start() {

//         Random.InitState(seed);
//         GenerateWorld();
//         playerLastChunkCoord = GetChunkCoordFromVector3(player.transform.position);

//     }

//     private void Update() {

//         if (!GetChunkCoordFromVector3(player.transform.position).Equals(playerLastChunkCoord))
//             CheckViewDistance();

//     }

//     ChunkCoord GetChunkCoordFromVector3 (Vector3 pos) {

//         int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
//         int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
//         return new ChunkCoord(x, z);

//     }

//     private void GenerateWorld () {

//         for (int x = VoxelData.WorldSizeInChunks / 2 - VoxelData.ViewDistanceInChunks / 2; x < VoxelData.WorldSizeInChunks / 2 + VoxelData.ViewDistanceInChunks / 2; x++) {
//             for (int z = VoxelData.WorldSizeInChunks / 2 - VoxelData.ViewDistanceInChunks / 2; z < VoxelData.WorldSizeInChunks / 2 + VoxelData.ViewDistanceInChunks / 2; z++) {

//                 CreateChunk(new ChunkCoord(x, z));

//             }
//         }

//         spawnLocation = new Vector3(VoxelData.WorldSizeInBlocks / 2, VoxelData.ChunkHeight + 2, VoxelData.WorldSizeInBlocks / 2);
//         player.position = spawnLocation;

//     }

//     private void CheckViewDistance () {

//         // int chunkX = Mathf.FloorToInt(player.position.x / VoxelData.ChunkWidth);
//         // int chunkZ = Mathf.FloorToInt(player.position.z / VoxelData.ChunkWidth);
//         ChunkCoord coord = GetChunkCoordFromVector3(player.position);

//         List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);
//         // activeChunks.Clear(); //from youtube comment

//         // Loop through all chunks currently within view distance of the player.
//         for (int x = coord.x - VoxelData.ViewDistanceInChunks; x < coord.x + VoxelData.ViewDistanceInChunks; x++) {
//             for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++) {

//                 // If the current chunk is in the world...
//                 if (IsChunkInWorld (new ChunkCoord (x, z))) {

//                     // Check if it active, if not, activate it.
//                     if (chunks[x, z] == null)
//                         // CreateNewChunk(x, z);
//                         CreateChunk(new ChunkCoord(x, z));

//                     else if (!chunks[x, z].isActive) {
//                         chunks[x, z].isActive = true;
//                         activeChunks.Add(new ChunkCoord(x, z));
//                     }

//                 }

//                 // Check through previously active chunks to see if this chunk is there. If it is, remove it from the list.
//                 for (int i = 0; i < previouslyActiveChunks.Count; i++) {

//                     if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
//                         previouslyActiveChunks.RemoveAt(i);
                       
//                 }

//             }
//         }

//         // Any chunks left in the previousActiveChunks list are no longer in the player's view distance, so loop through and disable them.
//         foreach (ChunkCoord c in previouslyActiveChunks)
//             chunks[c.x, c.z].isActive = false;

//     }

//     // void CreateNewChunk (int x, int z) {

//     //     chunks[x, z] = new Chunk(new ChunkCoord(x, z), this);
//     //     activeChunks.Add(new ChunkCoord(x, z));

//     // }

//     bool IsChunkInWorld (ChunkCoord coord) {

//         if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
//             return true;
//         else
//             return
//                 false;

//     }
// /*
//     bool IsChunkInWorld(int x, int z) {

//         if (x > 0 && x < VoxelData.WorldSizeInChunks - 1 && z > 0 && z < VoxelData.WorldSizeInChunks - 1)
//             return true;
//         else
//             return false;

//     }
//     */

//     private void CreateChunk (ChunkCoord coord) {

//         chunks[coord.x, coord.z] = new Chunk(new ChunkCoord(coord.x, coord.z), this);
//         activeChunks.Add(new ChunkCoord(coord.x, coord.z));


//     }
//     public byte GetVoxel (Vector3 pos) {
//         int yPos = Mathf.FloorToInt(pos.y);

//         /* IMMUTABLE PASS */

//         //if outside world, air
//         if (!isVoxelInWorld(pos))
//             return 0;
//         //if bottom block, bedrock
//         if (yPos == 0)
//             return 1;

//         /* BASIC TERRAIN PASS */

//         //translates height based on perlin noise
//         int terrainHeight = Mathf.FloorToInt(VoxelData.ChunkHeight * Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 500, biome.terrainScale)); //offset is for diff sections of the noise map

//         if (yPos == terrainHeight)
//             return 3; //grass
//         else if (yPos < terrainHeight && yPos > terrainHeight - 4)
//             return 5; //dirt
//         else if (yPos > terrainHeight)
//             return 0; //air
//         else
//             return 2; //stone

//         // if (pos.x < 0 || pos.x > VoxelData.WorldSizeInBlocks - 1 || pos.y < 0 || pos.y > VoxelData.ChunkHeight - 1 || pos.z < 0 || pos.z > VoxelData.WorldSizeInBlocks - 1)
//         //     return 0;
//         // if (pos.y < 1)
//         //     return 1;
//         // // else if (pos.y == VoxelData.ChunkHeight - 1)
//         // //     return 3;
//         // else if (pos.y == VoxelData.ChunkHeight - 1){
//         //     float tempNoise = Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, 0.1f);
//         //     if (tempNoise < 0.5f)
//         //         return 3; //grass
//         //     else  
//         //         return 4; //sand
//         // }
//         // else
//         //     return 2;

//     }

//     bool isVoxelInWorld(Vector3 pos)
//     {
//         if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
//             return true;
//         else 
//             return false;
//     }

// }

// public class ChunkCoord {

//     public int x;
//     public int z;

//     public ChunkCoord (int _x, int _z) {

//         x = _x;
//         z = _z;

//     }

//     public bool Equals(ChunkCoord other) {

//         if (other == null)
//             return false;
//         else if (other.x == x && other.z == z)
//             return true;
//         else
//             return false;

//     }

    

// }

// [System.Serializable]
// public class BlockType {

//     public string blockName;
//     public bool isSolid;

//     [Header("Texture Values")]
//     public int backFaceTexture;
//     public int frontFaceTexture;
//     public int topFaceTexture;
//     public int bottomFaceTexture;
//     public int leftFaceTexture;
//     public int rightFaceTexture;

//     // Back, Front, Top, Bottom, Left, Right

//     public int GetTextureID (int faceIndex) {

//         switch (faceIndex) {

//             case 0:
//                 return backFaceTexture;
//             case 1:
//                 return frontFaceTexture;
//             case 2:
//                 return topFaceTexture;
//             case 3:
//                 return bottomFaceTexture;
//             case 4:
//                 return leftFaceTexture;
//             case 5:
//                 return rightFaceTexture;
//             default:
//                 Debug.Log("Error in GetTextureID; invalid face index");
//                 return 0;


//         }

//     }

// }