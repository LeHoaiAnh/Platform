using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    //reference to objects inside the character prefab
    #region OBJECTS
    public Animator animator;

    public Renderer hat;
    public Renderer hair;
    public Renderer hairClipped;
    public Renderer eye;
    public Renderer eyeBase;
    public Renderer facewear;
    public Renderer cloth;
    public Renderer skirt;
    public Renderer pants;
    public Renderer socks;
    public Renderer shoes;
    public Renderer back;
    public Renderer expression;
    public Renderer body;

    public Transform weaponSlot;
    #endregion
    public void Init()
    {
        SpriteCollection spriteCollection = GameClient.instance.spriteCollection;
        for (int i = 0; i < GameClient.instance.UInfo.itemEquips.Count; i++)
        {
            ItemInventoryInfor item = GameClient.instance.UInfo.itemEquips[i];
            switch (item.typeItem)
            {
                case TypeItem.Hair:
                    Material hair = spriteCollection.GetHairMT(item.codename);
                    if (hair != null)
                    {
                        HairMaterial = hair;
                    }  
                    break;
                case TypeItem.Hat:
                    Material hat = spriteCollection.GetHatMT(item.codename);
                    if (hat != null)
                    {
                        HatMaterial = hat;
                    }  
                    break;
                case TypeItem.Shoes:
                    Material shoes = spriteCollection.GetShoesMT(item.codename);
                    if (shoes != null)
                    {
                        ShoesMaterial = shoes;
                    }  
                    break;
                case TypeItem.Back:
                    Material back = spriteCollection.GetBackMT(item.codename);
                    if (back != null)
                    {
                        BackMaterial = back;
                    }  
                    break;
                case TypeItem.Cloth: 
                    Material cloth = spriteCollection.GetClothMT(item.codename);
                    if (cloth != null)
                    {
                        ClothMaterial = cloth;
                    }  
                    break;
                case TypeItem.Eyes: 
                    Material eyes = spriteCollection.GetEyesMT(item.codename);
                    if (eyes != null)
                    {
                        EyeMaterial = eyes;
                    }  
                    break;
                case TypeItem.Socks: 
                    Material socks = spriteCollection.GetSockMT(item.codename);
                    if (socks != null)
                    {
                        SocksMaterial = socks;
                    }  
                    break;
                case TypeItem.Weapon: 
                    GameObject weaponPrefab = spriteCollection.GetWeaponObj(item.codename);
                    if (weaponPrefab != null)
                    {
                        ClearWeapon();
                        var weapon = Instantiate(weaponPrefab);
                        weapon.transform.parent = weaponSlot;
                        weapon.transform.localPosition = Vector3.zero;
                        weapon.transform.localRotation = Quaternion.identity;
                    }  
                    break;
                default: 
                    Debug.LogError("Can not find slot item equip");
                    break;
            }
        }
    }

    public void Init(TypeItem typeItem, string codeName)
    {
        SpriteCollection spriteCollection = GameClient.instance.spriteCollection;
        switch (typeItem)
        {
            case TypeItem.Hair:
                Material hair = spriteCollection.GetHairMT(codeName);
                if (hair != null)
                {
                    HairMaterial = hair;
                }
                break;
            case TypeItem.Hat:
                Material hat = spriteCollection.GetHatMT(codeName);
                if (hat != null)
                {
                    HatMaterial = hat;
                }
                break;
            case TypeItem.Shoes:
                Material shoes = spriteCollection.GetShoesMT(codeName);
                if (shoes != null)
                {
                    ShoesMaterial = shoes;
                }
                break;
            case TypeItem.Back:
                Material back = spriteCollection.GetBackMT(codeName);
                if (back != null)
                {
                    BackMaterial = back;
                }
                break;
            case TypeItem.Cloth:
                Material cloth = spriteCollection.GetClothMT(codeName);
                if (cloth != null)
                {
                    ClothMaterial = cloth;
                }
                break;
            case TypeItem.Eyes:
                Material eyes = spriteCollection.GetEyesMT(codeName);
                if (eyes != null)
                {
                    EyeMaterial = eyes;
                }
                break;
            case TypeItem.Socks:
                Material socks = spriteCollection.GetSockMT(codeName);
                if (socks != null)
                {
                    SocksMaterial = socks;
                }
                break;
            case TypeItem.Weapon:
                GameObject weaponPrefab = spriteCollection.GetWeaponObj(codeName);
                if (weaponPrefab != null)
                {
                    ClearWeapon();
                    var weapon = Instantiate(weaponPrefab);
                    weapon.transform.parent = weaponSlot;
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = Quaternion.identity;
                }
                break;
            default:
                Debug.LogError("Can not find slot item equip");
                break;
        }
    }
    //parameters for tweaking the character's appearance
    #region APPEARANCE
    public Material HatMaterial
    {
        get
        {
            if (!hat) return null;
            return hat.sharedMaterial;
        }
        set
        {
            if (!hat) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(hat, "Modify Hat Material");
            #endif

            hat.sharedMaterial = value;
        }
    }

    public Material HairMaterial
    {
        get
        {
            if (!hair) return null;
            return hair.sharedMaterial;
        }
        set
        {
            if (!hair) return;
            if (!hairClipped) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Modify Hair Material");
            #endif

            hair.sharedMaterial = value;
            hairClipped.sharedMaterial = value;
        }
    }

    public Material EyeMaterial
    {
        get
        {
            if (!eye) return null;
            return eye.sharedMaterial;
        }
        set
        {
            if (!eye) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(eye, "Modify Eye Material");
            #endif

            eye.sharedMaterial = value;
        }
    }

    public Material EyeBaseMaterial
    {
        get
        {
            if (!eyeBase) return null;
            return eyeBase.sharedMaterial;
        }
        set
        {
            if (!eyeBase) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(eyeBase, "Modify Eye Base Material");
            #endif

            eyeBase.sharedMaterial = value;
        }
    }

    public Material FacewearMaterial
    {
        get
        {
            if (!facewear) return null;
            return facewear.sharedMaterial;
        }
        set
        {
            if (!facewear) return;

            #if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(facewear, "Modify Facewear Material");
            #endif

            facewear.sharedMaterial = value;
        }
    }

    public Material ClothMaterial
    {
        get
        {
            if (!cloth) return null;
            return cloth.sharedMaterial;
        }
        set
        {
            if (!cloth) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(cloth, "Modify Cloth Material");
            #endif

            cloth.sharedMaterial = value;
        }
    }

    public Material PantsMaterial
    {
        get
        {
            if (!pants) return null;
            return pants.sharedMaterial;
        }
        set
        {
            if (!pants) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObjects(new Object[] { pants, skirt }, "Modify Pants Material");
            #endif

            pants.sharedMaterial = value;
            skirt.sharedMaterial = value;
        }
    }

    public Material SocksMaterial
    {
        get
        {
            if (!socks) return null;
            return socks.sharedMaterial;
        }
        set
        {
            if (!socks) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(socks, "Modify Socks Material");
            #endif

            socks.sharedMaterial = value;
        }
    }

    public Material ShoesMaterial
    {
        get
        {
            if (!shoes) return null;
            return shoes.sharedMaterial;
        }
        set
        {
            if (!shoes) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(shoes, "Modify Shoes Material");
            #endif

            shoes.sharedMaterial = value;
        }
    }

    public Material BackMaterial
    {
        get
        {
            if (!back) return null;
            return back.sharedMaterial;
        }
        set
        {
            if (!back) return;

            #if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(back, "Modify Back Material");
            #endif

            back.sharedMaterial = value;
        }
    }

    public Material BodyMaterial
    {
        get
        {
            if (!body) return null;
            return body.sharedMaterial;
        }
        set
        {
            if (!body) return;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(body, "Modify Body Material");
            #endif

            body.sharedMaterial = value;
        }
    }

    //to hide part of the hair when wearing curtain hat
    public bool ClipHair
    {
        get { return clipHair; }
        set
        {
            clipHair = value;

            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Toggle Clip Hair");
            #endif

            hair.enabled = !clipHair;
            hairClipped.enabled = clipHair;
        }
    }
    [SerializeField, HideInInspector]
    private bool clipHair;

    //the interval for the character to blink, random between x and y
    public Vector2 blinkInterval = new Vector2(0.5f, 5.0f);
    #endregion
    
    private float blinkTimer;

    private MaterialPropertyBlock MPBHair
    {
        get
        {
            if (mpbHair == null) mpbHair = new MaterialPropertyBlock();
            return mpbHair;
        }
    }
    private MaterialPropertyBlock mpbHair;

    private void Start()
    {
        if (Application.isPlaying == false) return;

        blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
        if (blinkTimer < 0.1f) blinkTimer = 0.1f;
    }
    
    private void Update()
    {
        if (Application.isPlaying == false) return;

        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0.0f)
        {
            blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
            if (blinkTimer < 0.1f) blinkTimer = 0.1f;

            animator.SetTrigger("Blink");
        }
    }

    //clear out everything in weapon slot
    public void ClearWeapon ()
    {
        for ( int i = 0; i < weaponSlot.childCount; i++)
        {
            var w = weaponSlot.GetChild(i);
            Destroy(w.gameObject);
        }
    }
    
}
