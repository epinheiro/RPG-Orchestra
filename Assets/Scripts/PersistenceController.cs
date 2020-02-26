using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class PersistenceController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    public void SaveFile() {
        // Set destination
        // System.DateTime.Now.Date
        string destination = Application.persistentDataPath + "/" + "mapSave.dat";
        
        FileStream file;      
        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        // MyData
        GameData data = new GameData();

        // Saving
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Saved file in "+ destination);
    }

    public void LoadFile() {
        string destination = Application.persistentDataPath + "/mapSave.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        ////// --> CLEAN the map space from screen and recreate the map from the loaded one
        recreateTileMap(data);

        Debug.Log("Loaded file from " + destination);
        Debug.Log(data.tileMap.ToString());
    }

    void recreateTileMap(GameData data) {
        // Clean screen
        TileController.DestroyAllMapTiles();

        // Transform the tile information to controller
        reloadTileList(data);

        // Instantiate the tiles on screen
        GameObject.Find("Map Editor Elements").GetComponent<MapEditorController>().RecreateTilesMapFromList();
    }

    void reloadTileList(GameData data) {
        TileController.tiles = data.TileMapList();
    }





    ///////////////////////////////////////

    /// <summary>
    /// Class to serialize all the important data of constructed map
    /// </summary>
    [System.Serializable]
    public class GameData {
        public TileMap tileMap;

        public GameData() {
            tileMap = new TileMap();
        }

        public List<TileController.TileControl> TileMapList() {
            List<TileController.TileControl> tiles = new List<TileController.TileControl>();
            int size = tileMap.size;
            for(int i =0; i < size; i++) {
                tiles.Add(tileMap.tilesArray[i]);
            }
            
            return tiles;
        }
    }

    //// https://docs.unity3d.com/Manual/script-Serialization.html
    // https://docs.unity3d.com/Manual/script-Serialization.html#FieldSerliaized2
    [System.Serializable]
    public class TileMap {
        public TileController.TileControl[] tilesArray;
        public int size;

        public TileMap() {
            size = TileController.tiles.Count;
            tilesArray = new TileController.TileControl[size];
            int index = 0;

            foreach (TileController.TileControl tile in TileController.tiles) {
                tilesArray[index] = tile;
                index++;
            }
        }

        override public string ToString() {
            size = TileController.tiles.Count;
            string str = string.Empty;
            for (int i =0; i< size; i++) {
                str += tilesArray[i].ToStringMetaData();
            }
            return "TileMap: "+str;
        }

    }

}
