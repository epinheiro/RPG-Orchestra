using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    public static int number;
    
    TileControl data;
    
    private int _tileId;

	void Awake() {
        this._tileId = number;
        this.name = "Tile" + number;
        number++;
    }
    
    private void OnMouseOver() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // forward
            Debug.Log("Going up with Tile" + this._tileId);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // backwards
            Debug.Log("Going down with Tile" + this._tileId);
        }
    }



    /////// STATIC LIST REFERENCES
    public void SetMetadata(Vector3 position, int type, int terrain) {
        data = new TileControl(position, type, terrain, this._tileId, this.gameObject);
        tiles.Add(data);
    }

    public static List<TileControl> tiles = new List<TileControl>();
    public static void DestroyAllMapTiles() {
        Debug.Log("Destroying all map tiles in screen");
        foreach (TileControl tile in tiles) {
            Destroy(tile.Reference);
        }
        tiles.Clear();
    }

    [System.Serializable]
    public class TileControl {
        public int _id;
        public int Id {
            get { return _id; }
        }

        //public Vector3 _position;
        public Vector3 Position {
            get { return new Vector3(_positionX, _positionY, _positionZ); }
        }

        public float _positionX;
        public float _positionY;
        public float _positionZ;

        public int _type;
        public int Type {
            get { return _type; }
        }

        public int _terrain;
        public int Terrain {
            get { return _terrain; }
        }

        [System.NonSerialized]
        private GameObject _reference;
        public GameObject Reference {
            get { return _reference; }
            set { _reference = value;  }
        }

        public TileControl(Vector3 position, int type, int terrain, int id, GameObject tileGo) {
            //this._position = position;
            this._positionX = position.x;
            this._positionY = position.y;
            this._positionZ = position.z;

            if (CheckIfValidType(type)) this._type = type;
            else this._type = 0;

            if (CheckIfValidTerrain(type)) this._terrain = terrain;
            else this._terrain = 0;
            
            this._id = id;

            _reference = tileGo;
        }

        bool CheckIfValidType(int i) {
            if (i > (System.Enum.GetValues(typeof(MapEditorController.TilesTypes)).Length) || i < 0) return false;
            return true;
        }

        bool CheckIfValidTerrain(int i) {
            if (i > (System.Enum.GetValues(typeof(MapEditorController.TilesTerrains)).Length) || i < 0) return false;
            return true;
        }

        public void DestroyData() {
            tiles.Remove(this);
            Destroy(_reference);
        }

        public string ToStringMetaData() {
            return "{id: " + this._id + ", position: (" + _positionX + ", " + _positionY + ", " + _positionZ + "), " + "type: " + _type + ", " + "terrain: " + _terrain + "} ";
        }
    }
}
