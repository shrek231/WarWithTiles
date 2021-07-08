using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEngine.Tilemaps;
public class WorldGen2 : MonoBehaviour {
    public int rand = 0;

    public int seed = 225;
    public int scale = 5;
    public int width = 60;
    public int hight = 1500;
    public double coalGradiant = .2;
    public double DirtGradiant = .4;
    public double stoneGradiant = .4;
    public double ironGradiant = .75; //backwards

    public int MountanSeed = 225;
    public int mountanScale = 2;
    public int mountanWidth = 60;
    public int mountanHight = 2000;

    public int CaveScale = 15;
    public int CaveSeed = 225;
    public int currentSufaceCaveCount = 0;
    public int currentSufaceCaveCountCaves = 0;

    public Tilemap Tilemap;
    public TileBase Irontile;
    public TileBase Stonetile;
    public TileBase Mountantile;
    public TileBase Dirttile;
    public TileBase Coaltile;
    public TileBase emptyTile;
    //
    void Start() {
        //if (!File.Exists("WorldGenConf.txt")) {
        //    File.Create("WorldGenConf.txt");
        //}
        GenGround();
        GenMountans();
        GenCaves();
    }
    float GenGround(){
        float noise = 0;
        float ironNoise = 0;
        for (int x = 0; x < width; x++){
            for (int y = 0; y < hight; y++){
                float xCoord = (float)x / scale;
                float yCoord = (float)y / scale;
                noise = Mathf.PerlinNoise(xCoord + seed, yCoord + seed);
                ironNoise = Mathf.PerlinNoise(xCoord + UnityEngine.Random.Range(0,100) + seed, yCoord + UnityEngine.Random.Range(0, 100) + seed);
                //noise
                Vector3Int position = new Vector3Int(x, y, 0);
                if (noise > stoneGradiant){
                    Tilemap.SetTile(position, Stonetile);
                } if (noise < DirtGradiant){
                    Tilemap.SetTile(position, Dirttile);
                } if (noise < coalGradiant){
                    Tilemap.SetTile(position, Coaltile);
                }
                //iron noise
                if (noise > ironGradiant){
                    Tilemap.SetTile(position, Irontile);
                }
            }
        }
        return noise;
    }
    float GenMountans(){
        float noise = 0;
        for (int x = 0; x < mountanWidth; x++){
            for (int y = hight; y < mountanHight; y++){
                float xCoord = (float)x / mountanWidth * mountanScale;
                float yCoord = (float)y / mountanHight * mountanScale;
                noise = yCoord * Mathf.PerlinNoise(xCoord + MountanSeed, 0f);
                //print(noise * hight / mountanHight);
                if (noise * hight / mountanHight < .5){
                    Vector3Int position = new Vector3Int(x, y, 0);
                    Tilemap.SetTile(position, Mountantile);
                } if (noise * hight / mountanHight > .5 && noise * hight / mountanHight < .51){
                    Vector3Int position = new Vector3Int(x, y, 0);
                    Tilemap.SetTile(position, Dirttile);
                }
            }
        }
        return noise;
    }
    float GenCaves(){
        float noise = 0;
        for (int x = 0; x < width; x++){
            for (int y = 0; y < hight; y++){
                float xCoord = (float)x / CaveScale;
                float yCoord = (float)y / CaveScale;
                if (y > 600 && y < hight - 500){ // 500 blocks under to 600
                    noise = Mathf.PerlinNoise(xCoord + 10 + CaveSeed, yCoord - 10);
                    //print("ground: "+noise);
                    Vector3Int position = new Vector3Int(x, y, 0);
                    if (noise < .4){
                        Tilemap.SetTile(position, emptyTile);
                    }
                } else {
                    //only spawn 10000 suface caves blocks
                    if (currentSufaceCaveCount == 10000 && currentSufaceCaveCountCaves < 3) {
                        currentSufaceCaveCount = 0;
                        currentSufaceCaveCountCaves++;
                    }
                    if (currentSufaceCaveCount < 1){
                        Rand();
                    }
                    if (currentSufaceCaveCount != 10000 && rand == 2 && y > 1000){
                        noise = Mathf.PerlinNoise(xCoord + 10 + CaveSeed, yCoord - 10);
                        //print("ground: "+noise);
                        Vector3Int position = new Vector3Int(x, y, 0);
                        if (noise < .4){
                            Tilemap.SetTile(position, emptyTile);
                        }
                        currentSufaceCaveCount++;
                        //print("1 "+currentSufaceCaveCount);
                        //print("2 "+currentSufaceCaveCountCaves);
                    }
                    //dont spawn caves
                }
            }
        }
        return noise;
    }
    void Rand(){
        rand = UnityEngine.Random.Range(0,width *15);
    }
    void SpawnWater(){
        //use the GenCaves noise to gen water in caves
    }
    void SpawnLava(){
        //
    }
}
