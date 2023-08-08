using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenBattle : ScreenBase
{
    public static ScreenBattle Instance;

    [Header("Set Up Energy")]
    [SerializeField] private Slider slideEnergy;
    [SerializeField] private TextMeshProUGUI slideEnergyText;
    [SerializeField] private GameObject leftPanel;

    [Header("Set up Time")] [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("HP BAR")] 
    private List<HPBar> mListEnemiesHPBar = new List<HPBar>();
    [SerializeField] private HPBar enemyHPBarPrefab;
    [SerializeField] private Transform enemyHPBarContainer;
    
    [Header("HUD")] [SerializeField]
    private Transform HUDContainer;
    public HUD_GUI normalDMG;
    public HUD_GUI critDMG;

    [Header("Button")] [SerializeField]
    private Button pauseBtn;

    [Header("Set Up Energy PvP")]
    public GameObject ContainerPvP;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(Pause);
    }

    private void Pause()
    {
        PopupPause.Create();
    }

    private void OnEnable()
    {
        if (Photon.Pun.PhotonNetwork.InRoom)
        {
            leftPanel.SetActive(false);
            ContainerPvP.SetActive(true);
        }
        else
        {
            leftPanel.SetActive(true);
            ContainerPvP.SetActive(false);
        }
    }

    public override void OnActive()
    {
        base.OnActive();
        if (ObjectPoolManager.instance &&
            ObjectPoolManager.instance.poolList != null &&
            ObjectPoolManager.instance.poolList.Count > 0)
        {
            var poolNormalDmg = ObjectPoolManager.instance.poolList.Find(e => e.prefab == normalDMG.gameObject);
            if (poolNormalDmg != null)
            {
                poolNormalDmg.UnspawnAll();
            }
            
            var poolCritDmg = ObjectPoolManager.instance.poolList.Find(e => e.prefab == critDMG.gameObject);
            if (poolCritDmg != null)
            {
                poolCritDmg.UnspawnAll();
            }
        }

        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.instance.PreCachePool(normalDMG.gameObject, 3);
            ObjectPoolManager.instance.PreCachePool(critDMG.gameObject, 3);
        }
    }

    public void UpdateEnergyPlayer()
    {
        if (PlayerManagement.Instance != null)
        {
            var total = PlayerManagement.Instance.PlayerUnit.originStat.Energy;
            var current = PlayerManagement.Instance.PlayerUnit.curStat.Energy;
            slideEnergyText.text = String.Format("{0} /{1}", Mathf.FloorToInt(current), Mathf.FloorToInt(total));
            slideEnergy.value = Mathf.Clamp01(current / total);
        }
    }

    public void UpdateTime()
    {
        if (QuanLyManChoi.Instance != null)
        {
            var curTime = QuanLyManChoi.Instance.TimeBattle;
            timeText.text = String.Format("Time: {0:D2}: {1:D2}: {2:D2}", (int) (curTime / 3600), (int) (curTime / 60),
                (int)(curTime % 60));
        }
    }
    
    public HPBar CreateHPBar(BossController unit)
    {
        HPBar hpBar = null;
        if (mListEnemiesHPBar.Count == 0)
        {
            hpBar = Instantiate(enemyHPBarPrefab, enemyHPBarContainer);
        }
        else
        {
            hpBar = mListEnemiesHPBar[mListEnemiesHPBar.Count - 1];
            mListEnemiesHPBar.RemoveAt(mListEnemiesHPBar.Count - 1);
        }
        hpBar.gameObject.SetActive(true);

        if (hpBar != null)
        {
            hpBar.Init(unit);
            hpBar.UpdateHP();
        }
        return hpBar;
    }

    #region Display HUD
    public struct DisplayHUDCommand
    {
        public string Text;
        public PlayerUnit Target;
        public GameObject Prefab;
        public bool canMerge;
        public int frame;
        public float Scale;
        public Color Color;
    }

    public struct DisplayHUDEnemyCommand
    {
        public string Text;
        public BossController Target;
        public GameObject Prefab;
        
        public bool canMerge;
        public int frame;
        public float Scale;
        public Color Color;
    }
    
    static readonly Color ColorCrit = Color.yellow;
    static readonly Color ColorNorm = Color.red;
    
    public void DisplayDmgHud(long dmg, bool isCrit, PlayerUnit target)
    {
        if (PlayerManagement.Instance == null || PlayerManagement.Instance.PlayerUnit == null)
        {
            return;
        }

        if (normalDMG == null)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        var comand = new DisplayHUDCommand()
        {
            Text = dmg.ToString(),
            Target = target,
            frame = Time.frameCount,
            canMerge = true,
            Color = isCrit ? ColorCrit : ColorNorm,
            Scale = 1,
            Prefab = normalDMG.gameObject
        };

        if (isCrit)
        {
            comand.Prefab = critDMG.gameObject;
        }

        if (target.GetCurrentHUDQueue() < ConfigManager.GetMaxHUDQueue())
        {
            target.QueueHudDisplay(comand);
        }
    }

    public void DisplayDmgHud(long dmg, bool isCrit, BossController target)
    {
        if (PlayerManagement.Instance == null || PlayerManagement.Instance.PlayerUnit == null)
        {
            return;
        }

        if (normalDMG == null)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        var comand = new DisplayHUDEnemyCommand()
        {
            Text = dmg.ToString(),
            Target = target,
            frame = Time.frameCount,
            canMerge = true,
            Color = isCrit ? ColorCrit : ColorNorm,
            Scale = 1,
            Prefab = normalDMG.gameObject
        };

        if (isCrit)
        {
            comand.Prefab = critDMG.gameObject;
        }

        if (target.GetCurrentHUDQueue() < ConfigManager.GetMaxHUDQueue())
        {
            target.QueueHudDisplay(comand);
        }
    }
    
    public void DisplayHUD(DisplayHUDEnemyCommand hudCommand)
    {
        if (hudCommand.Prefab && hudCommand.Target != null)
        {
            var hudObj = ObjectPoolManager.Spawn(hudCommand.Prefab, Vector3.zero, Quaternion.identity, HUDContainer);
            hudObj.transform.localScale = Vector3.one * hudCommand.Scale;
            var hud = hudObj.GetComponent<HUD_GUI>();
            hud.Display(hudCommand.Text, 0.65f, hudCommand.Color);
            var follower = hud.GetComponent<UIFollowTarget>();

            var offsetRand = Random.insideUnitCircle * 0.2f;
            var offset = hudCommand.Target.OffsetHUD + new Vector3(offsetRand.x, offsetRand.y ,0);
            follower.SetTarget(hudCommand.Target.transform, 0.6f, 1f, offset);
        }
    }
    
    public void DisplayHUD(DisplayHUDCommand hudCommand)
    {
        if (hudCommand.Prefab && hudCommand.Target != null)
        {
            var hudObj = ObjectPoolManager.Spawn(hudCommand.Prefab, Vector3.zero, Quaternion.identity, HUDContainer);
            hudObj.transform.localScale = Vector3.one * hudCommand.Scale;
            var hud = hudObj.GetComponent<HUD_GUI>();
            hud.Display(hudCommand.Text, 0.65f, hudCommand.Color);
            var follower = hud.GetComponent<UIFollowTarget>();

            var offsetRand = Random.insideUnitCircle * 0.4f;
            var offset = hudCommand.Target.OffsetHUD + new Vector3(offsetRand.x, offsetRand.y, 0);
            follower.SetTarget(hudCommand.Target.transform, 0.6f, 1f, offset);
        }
    }
    #endregion
}
