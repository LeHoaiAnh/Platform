
using UnityEngine;

[CreateAssetMenu(fileName = "SpritesCollection", menuName = "Sprite Collection")]
public class SpriteCollection : ScriptableObject
{
    [Header("Sprites")]
    public Sprite[] sprites;

    [Header("Player_Apperance")] 
    public Material[] hairMT;
    public Material[] hatMT;
    public Material[] clothMT;
    public Material[] shoesMT;
    public Material[] backMT;
    public GameObject[] weapons;
    public Material[] sockMT;
    public Material[] eyesMT;

    public Sprite GetSprite(string spriteName)
    {
        if (sprites == null || sprites.Length == 0) return null;
        return System.Array.Find(sprites, e => e != null && e.name == spriteName);
    }
        
    public Material GetHairMT(string spriteName, string color = "Black")
    {
        if (hairMT == null || hairMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Hair_Default";
        }
        return System.Array.Find(hairMT, e => e != null && e.name == spriteName + "_" +color);
    }
        
    public Material GetHatMT(string spriteName)
    {
        if (hatMT == null || hatMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Hat_Default";
        }
        return System.Array.Find(hatMT, e => e != null && e.name == spriteName);
    }

    public Material GetClothMT(string spriteName)
    {
        if (clothMT == null || clothMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Cloth_Default";
        }
        return System.Array.Find(clothMT, e => e != null && e.name == spriteName);
    }
    
    public Material GetShoesMT(string spriteName)
    {
        if (shoesMT == null || shoesMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Shoes_Default";
        }
        return System.Array.Find(shoesMT, e => e != null && e.name == spriteName);
    }
    
    public Material GetBackMT(string spriteName)
    {
        if (backMT == null || backMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Back_Default";
        }
        return System.Array.Find(backMT, e => e != null && e.name == spriteName);
    }
    
    public GameObject GetWeaponObj(string spriteName)
    {
        if (weapons == null || weapons.Length == 0) return null;
        return System.Array.Find(weapons, e => e != null && e.name == spriteName);
    }
    
    public Material GetSockMT(string spriteName)
    {
        if (sockMT == null || sockMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Socks_Default";
        }
        return System.Array.Find(sockMT, e => e != null && e.name == spriteName);
    }
    
    public Material GetEyesMT(string spriteName)
    {
        if (eyesMT == null || eyesMT.Length == 0) return null;
        if (string.IsNullOrEmpty(spriteName))
        {
            spriteName = "Eyes_Default";
        }
        return System.Array.Find(eyesMT, e => e != null && e.name == spriteName);
    }
}