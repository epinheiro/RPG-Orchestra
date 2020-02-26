using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapEditorController : MonoBehaviour {

    public GameObject blankTile1Prefab;

    /// MAP editor
    bool editableMap = false;
    GameObject mapReference;
    GameObject buttonConstruct;
    GameObject optionPanel;
    GameObject persistencePanel;
    GameObject bluePrintTile;
    Vector3 bluePrintTileRestPosition = new Vector3(0, 100, 0);
    bool bluePrintOn = false;
    bool tileCanBePlaced = true;

    public enum TilesTypes { height1 = 0 }
    int tileType = 0;
    int hoverTileType = 0;
    int numberOfTileTypes = System.Enum.GetValues(typeof(TilesTypes)).Length;

    public enum TilesTerrains { blank = 0, forest = 1 }
    int tileTerrain = 0;
    int hoverTileTerrain = 0;
    int numberOfTileTerrains = System.Enum.GetValues(typeof(TilesTerrains)).Length;


    Camera cam;

    void Start() {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mapReference = GameObject.Find("Map");
        bluePrintTile = GameObject.Find("BluePrintTile");
        GUIStart();
    }

    void GUIStart() {
        //buttonConstruct = this.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        buttonConstruct = this.transform.Find("GUI_MapEditorCanvas").Find("Construct Panel").Find("Button Construct").gameObject;
        optionPanel = this.transform.Find("GUI_MapEditorCanvas").Find("Construct Panel").Find("Panel Options").gameObject;
        optionPanel.SetActive(false);
        persistencePanel = this.transform.Find("GUI_MapEditorCanvas").Find("Persistence Panel").gameObject;
        persistencePanel.SetActive(false);
    }

    void Update() {
        /* // DEBUG RAY CASTER AND SNAP  
            Debug.Log(Input.mousePosition);
            Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(CheckSnappedPointInY(MouseToPointInPlaneY()));
        */

        if (editableMap && tileCanBePlaced) {
            // TILE blueprint
            CreateBluePrintImage(CheckSnappedPointInY(MouseToPointInPlaneY()), tileType);

            // PUT tile
            if (Input.GetButtonDown("Fire1")) {
                CreateTile(CheckSnappedPointInY(MouseToPointInPlaneY()), tileType);
            }
            // REMOVE tile
            if (Input.GetButtonDown("Fire2")) {
                DestroyTile(CheckSnappedPointInY(MouseToPointInPlaneY()));
            }
        } else {
            ToggleOffBluePrintTile();
        }

            

        
    }

    /// <summary>
    /// UI Designed method to activate the MAP EDITOR on and its children GUI
    /// </summary>
    public void ToggleMapEditor() {
        if (editableMap) {
            editableMap = false;
            changeButtonColor(buttonConstruct, Color.white);
            changeButtonText(buttonConstruct, "Construct");
            optionPanel.SetActive(false);
            persistencePanel.SetActive(false);
        } else {
            editableMap = true;
            changeButtonColor(buttonConstruct, Color.grey);
            changeButtonText(buttonConstruct, "BACK");
            optionPanel.SetActive(true);
            persistencePanel.SetActive(true);
        }
    }
    
    void changeButtonColor(GameObject button, Color newColor) {
        Color colors;

        try {
            colors = button.GetComponent<Image>().color;
            colors = newColor;
            button.GetComponent<Image>().color = colors;
        } catch (System.Exception e) {
            colors = button.GetComponent<RawImage>().color;
            colors = newColor;
            button.GetComponent<RawImage>().color = colors;
        }
    }

    void changeButtonText(GameObject button, string newTxt) {
        button.GetComponentInChildren<Text>().text = newTxt;
    }

    ///// Tile TYPE
    /// <summary>
    /// Set the tile Type on mouse click during CONSTRUCT time
    /// </summary>
    public void SetTileType() {
        // TODO hover
        tileType = hoverTileType;
    }
    /// <summary>
    /// Scroll Tile Type in mouse hovering in CONSTRUCT mode
    /// </summary>
    public void ScrollTileType() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // forward
            //Debug.Log("Going up with button TYPE");
            hoverTileType = Mathf.Abs((hoverTileType + 1) % numberOfTileTypes);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // backwards
            //Debug.Log("Going down with button TYPE");
            hoverTileType = Mathf.Abs((hoverTileType - 1) % numberOfTileTypes);
        }
        changeButtonColor(optionPanel.transform.Find("Button TYPE").gameObject, colorOfCertainTileType(hoverTileType));
    }
    Color colorOfCertainTileType(int terrainNumber) {
        Color color = Color.white;
        switch (terrainNumber) {
            case ((int)TilesTypes.height1):
                color = Color.white;
                break;
        }
        return color;
    }

    ///// Tile TERRAIN
    /// <summary>
    /// Set the tile Terrain on mouse click during CONSTRUCT time
    /// </summary>
    public void SetTileTerrain() {
        // TODO hover
        tileTerrain = hoverTileTerrain;
    }
    /// <summary>
    /// Scroll Tile Terrain in mouse hovering in CONSTRUCT mode
    /// </summary>
    public void ScrollTileTerrain() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // forward
            //Debug.Log("Going up with button TERRAIN");
            hoverTileTerrain = Mathf.Abs((hoverTileTerrain + 1) % numberOfTileTerrains);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // backwards
            //Debug.Log("Going down with button TERRAIN");
            hoverTileTerrain = Mathf.Abs((hoverTileTerrain - 1) % numberOfTileTerrains);
        }
        changeButtonColor(optionPanel.transform.Find("Button TERRAIN").gameObject, colorOfCertainTileTerrain(hoverTileTerrain));
    }
    Color colorOfCertainTileTerrain(int terrainNumber) {
        Color color = Color.white;
        switch (terrainNumber){
            case ((int)TilesTerrains.blank):
                color = Color.white;
                break;
            case ((int)TilesTerrains.forest):
                color = Color.green;
                break;
        }
        return color;
    }


    ///// MAP Editor AVAILABLE
    void CreateBluePrintImage(Vector3 position, int tileType) {
        bluePrintOn = true;

        if (whatTileIsInThatPosition(position) != null) return;

        // BLUEPRINT code
        bluePrintTile.transform.position = position;
    }
    void ToggleOffBluePrintTile() {
        bluePrintOn = false;
        bluePrintTile.transform.position = bluePrintTileRestPosition;
    }

    void CreateTile(Vector3 position, int tileType) {
        //if (thereIsATileByTag(position)) return;
        if (whatTileIsInThatPosition(position) != null) return;

        GameObject go = (GameObject)Instantiate(returnTileToCreate(tileType), position, new Quaternion(0, 0, 0, 0));
        go.transform.parent = mapReference.transform;

        // SET terrain
        MeshRenderer gameObjectRenderer = go.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = colorOfCertainTileTerrain(hoverTileTerrain);
        gameObjectRenderer.material = newMaterial;

        go.GetComponent<TileController>().SetMetadata(position, tileType, hoverTileTerrain);
    }

    /// <summary>
    /// During the persistance recriation, this method gets the list of tiles and recreate the objects in the screen
    /// </summary>
    public void RecreateTilesMapFromList() {
        foreach (TileController.TileControl tile in TileController.tiles) {
            GameObject go = (GameObject)Instantiate(returnTileToCreate(tile.Type), tile.Position, new Quaternion(0, 0, 0, 0));
            go.transform.parent = mapReference.transform;

            tile.Reference = go;

            // SET terrain
            MeshRenderer gameObjectRenderer = go.GetComponent<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Standard"));
            newMaterial.color = colorOfCertainTileTerrain(tile.Terrain);
            gameObjectRenderer.material = newMaterial;
        }
    }

    void DestroyTile(Vector3 position) {
        TileController.TileControl tile = whatTileIsInThatPosition(position);
        if ( tile != null) {
            tile.DestroyData();
        }
    }
    
    /// <summary>
    /// [DEPRECATED] UNKNOWN bug made the function behave weirdly
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    bool thereIsATileByTag(Vector3 position) {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles) {
            if( (tile.transform.position.x == position.x)  &&  (tile.transform.position.y == position.y)) {
                
                return true;
            }
        }
        return false;
    }

    TileController.TileControl whatTileIsInThatPosition(Vector3 position) {
        List<TileController.TileControl> tileList = TileController.tiles;

        foreach (TileController.TileControl tile in tileList) {
            if ( tile.Position == position) {
                Debug.Log("There is a tile in - " + tile.Position + " with id " + tile.Id);
                return tile;
            }
        }
        return null;
    }

    GameObject returnTileToCreate(int type) {
        GameObject tilePrefab = null;
        switch (type) {
            case ((int)TilesTypes.height1):
                tilePrefab = blankTile1Prefab;
                break;
        }

        return tilePrefab;
    }
    
    Vector3 CheckSnappedPointInY(Vector3 crudePoint) {
        return new Vector3(
                RoundUpNumber(crudePoint.x),
                0,
                RoundUpNumber(crudePoint.z)
            );
    }

    float RoundUpNumber(float number) {
        if (number - Mathf.Floor(number) < (Mathf.Ceil(number) - (number))) {
            return Mathf.Floor(number);
        } else {
            return Mathf.Ceil(number);
        }
    }

    /// <summary>
    /// UI Design funcion called to disable TILE PLACEMENT clicks when the mouse is on some UI element
    /// (Pointer ENTER and Pointer EXIT with event trigger) 
    /// </summary>
    public void ToggleTilePlacement() {
        if (tileCanBePlaced) tileCanBePlaced = false;
        else tileCanBePlaced = true;
    }

    /// <summary>
    /// [DEPRECATED] Checks if a screen position is valid through proportional (percentage) pixel checking
    /// </summary>
    /// <returns></returns>
    bool CheckIfValidMousePosition() {
        Vector3 mousePosition = Input.mousePosition;
        //Debug.Log(mousePosition);
        //Debug.Log(Screen.width+ ", " + Screen.height);

        // w 70 de 528 --> 13,4%
        // h 230 de 309 --> 74%

        double widthPorcentage = 0.20F;  //x // increase to increase menu area 
        double heightPorcentage = 0.65F; //y // decrease to increase menu area

        if ( (mousePosition.x < Screen.width* widthPorcentage) && (mousePosition.y > Screen.height * heightPorcentage) ) {
            return false;
        } else {
            return true;
        }
        
        
    }

    Vector3 MouseToPointInPlaneY() {
        /* //Finding the Point Where a Line Intersects a Plane - https://www.youtube.com/watch?v=qVvvy5hsQwk
         * //14. Equações do Plano. | Geometria Analítica. - https://www.youtube.com/watch?v=mKmdY5qKQiA
         * 
          
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
        //Debug.DrawRay(ray.origin, ray.GetPoint(point), Color.black);
        double x0 = ray.origin.x;
        double y0 = ray.origin.y;
        double z0 = ray.origin.z;
        Vector3 origin = new Vector3(ray.origin.x, ray.origin.y, ray.origin.z);

        int point = 100;
        double x1 = ray.GetPoint(point).x;
        double y1 = ray.GetPoint(point).y;
        double z1 = ray.GetPoint(point).z;
        Vector3 final = new Vector3(ray.GetPoint(point).x, ray.GetPoint(point).y, ray.GetPoint(point).z);

        //Vector3 diff = final - origin;
        // // Plane XZ or Y have the equation --> Y=0
        // Parametric representation of line (with the two defining points) - (x,y,z)
        //    (t * x1 + (1 - t) * x0), --> t*x1 + 1*x0 -t*x0
        //    (t * y1 + (1 - t) * y0), --> t*y1 + 1*y0 -t*y0 --> (y1-y0)t + y0 ~~> (y1-y0)t + y0 = 0 --> t = -y0/(y1-y0)
        //    (t * z1 + (1 - t) * z0)  --> t*z1 + 1*z0 -t*z0
        
        double t = -y0 / (y1 - y0);
        Debug.Log("x: " + (t * x1 + (1 - t) * x0) +", "+
                  "z: " + (t * z1 + (1 - t) * z0)
                );
        */

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float t = (-ray.origin.y / (ray.GetPoint(100).y - ray.origin.y));
        Vector3 point = new Vector3((t * ray.GetPoint(100).x + (1 - t) * ray.origin.x), 0, (t * ray.GetPoint(100).z + (1 - t) * ray.origin.z));
        //Debug.Log(point);
        return point;
    }

    // To another try on the POINT screen to PLANE Y https://answers.unity.com/questions/269760/ray-finding-out-x-and-z-coordinates-where-it-inter.html

    void Debug_MousePointerOnScreen() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.Log(ray);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
    }

}
