using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class TileSprite {
    public string Name;
    public Sprite tileImage;
    public Tiles tileType;

    public TileSprite()
    {
        Name = "Unset";
        tileImage = new Sprite();
        tileType = Tiles.Unset;
    }

    public TileSprite(string name, Sprite image, Tiles tile)
    {
        Name = name;
        tileImage = image;
        tileType = tile;
    }
}
