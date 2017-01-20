using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class TilingEngine : MonoBehaviour {

    public List<TileSprite> tileSprites;
    public Vector2 mapSize;
    public Sprite defaultImage;
    public GameObject tileContainerPrefab;
    public GameObject tilePrefab;
    public Vector2 currentPosition;
    public Vector2 viewPortSize;

    private TileSprite[,] _map;
    private GameObject controller;
    private GameObject _tileContainer;
    private List<GameObject> _tiles = new List<GameObject>();

	// Use this for initialization
	void Start () {
        controller = GameObject.Find("Controller");
        _map = new TileSprite[(int)mapSize.x, (int)mapSize.y];

        DefaultTiles();
        SetTiles();

	}

    public void DefaultTiles()
    {
        for (var y = 0; y < mapSize.y; y++){
            for (var x = 0; x < mapSize.x; x++) {
                _map[x, y] = new TileSprite("unset", defaultImage, Tiles.Unset);
            }
        }
    }

    public void SetTiles()
    {
        var index = 0;
        for (var y = 0; y < mapSize.y; y++)
        {
            for (var x = 0; x < mapSize.x; x++)
            {
                _map[x, y] = new TileSprite(tileSprites[index].Name, tileSprites[index].tileImage, tileSprites[index].tileType);
                if (index > tileSprites.Count - 1)
                {
                    index = 0;
                }
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        AddTilesToWorld();
	}

    public void AddTilesToWorld()
    {
        foreach (GameObject o in _tiles)
        {
            LeanPool.Despawn(o);
        }
        _tiles.Clear();
        LeanPool.Despawn(_tileContainer);
        _tileContainer = LeanPool.Spawn(tileContainerPrefab);
        var tileSize = .64f;
        var viewOffsetX = viewPortSize.x / 2f;
        var viewOffsetY = viewPortSize.y / 2f;
        for ( var y = -viewOffsetY; y < viewOffsetY; y++) {
            for (var x = -viewOffsetX; x < viewOffsetX; x++){
                var tX = x * tileSize;
                var tY = y * tileSize;

                var iX = x + currentPosition.x;
                var iY = y + currentPosition.y;

                if ( iX < 0) { continue; }
                if ( iY < 0) { continue; }
                if ( iX > mapSize.x - 2) { continue; }
                if (iY > mapSize.y - 2) { continue; }

                var t = LeanPool.Spawn(tilePrefab);
                t.transform.position = new Vector3(tX, tY, 0);
                t.transform.SetParent(_tileContainer.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x + (int)currentPosition.x, (int)y + (int)currentPosition.y].tileImage;
                _tiles.Add(t);
                    
            }
        }
    }

    private TileSprite FindTile(Tiles tile)
    {
        foreach (TileSprite tileSprite in tileSprites)
        {
            if (tileSprite.tileType == tile)
            {
                return tileSprite;
            }
        }
    }
}
