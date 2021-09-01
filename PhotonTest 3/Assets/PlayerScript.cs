using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    public Player player;

    #region Photon

    [Header("PhotonView")]
    public PhotonView pv;
    private float ping;
    #endregion

    #region Inputs

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    [Header("Skill Keys")]
    public List<KeyCode> InputKeys;
    private bool[] InputKeyState;
    #endregion

    #region Camera

    [Header("Camera")]
    public GameObject cameraandui;
    public Camera followcam;
    [SerializeField]
    private float cameraZoffset;
    #endregion

    #region Player

    [Header("Player Mechanics")]
    public Rigidbody2D rb;
    public bool UseAcceleration;
    private Vector2 lookDir;
    #endregion

    #region Game Mechanics

    //Health
    float Health;
    float MaxHealth;
    float HealthRegenRate;
    private float MoveSpeed;
    bool Alive;

    //Skills
    private List<Skill> Skills;
    public Transform[] firepoint;
    private float[] SkillTimer;
    private bool[] UseSkill;
    private bool[] SkillReady;


    private bool Isfrozen;
    private bool rotIsfrozen;
    private float freezetimer;
    private int[] remainingFire;
    private float[] remainingTimer;
    private bool allowUnfreeze;

    float SpermCount;
    float MaxSperm;
    float SpermRate;
    int Orgiballcount;

    //buffs
    [Header("BuffIDs")]
    //0- cumregen, 1-addsperm, 2-addhealth, 3-addhealthregen
    public int itembuffcount;
    public float[] CurrentBuffs;
    public int HealthRegenID;
    public int HealthPointID;
    public int SpermAddID;
    public int SpermRegenID;


    #endregion

    #region UI
    [Header("UI")]
    //mechanics
    public Slider cumbar;
    public Slider healthbar;
    public Text maxhealthtext;
    public Text healthtext;
    public Text maxcumtext;
    public Text cumtext;
    public Text Orgiballcountnum;
    public GameObject respawnMenu;

    //skills
    public Text[] SkillTimeDisplay;
    public Slider[] SkillSliders;

    //ping
    public Text pingtext;
    public GameObject pingfill;
    public Slider pingslider;
    public Color[] pingbarcolors;
    #endregion

    #region Audio & Animation
    [Header("Audio & Animation")]
    public AudioSource playerSounds;


    #endregion

    private void Start()
    {
        CopyFromPlayerScriptable();
        UseSkill = new bool[Skills.Count];
        SkillReady = new bool[Skills.Count];
        SkillTimer = new float[Skills.Count];
        InputKeyState = new bool[InputKeys.Count];
        remainingFire = new int[Skills.Count];
        remainingTimer = new float[Skills.Count];
        CurrentBuffs = new float[itembuffcount];
    }
    private void Awake()
    {
        if (pv.IsMine)
        {
            Alive = true;
            cameraandui.SetActive(true);
            Orgiballcount = 0;
        }
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            UpdateUI();
            UpdateInputs();
            UpdateView();
        }
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            UpdateMechanics();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            GameObject bullet = collision.gameObject;
            PlayRandomHitSound();
            TakeDamage(bullet.GetComponent<bulletscript>().Damage);
        }
        if (collision.gameObject.tag == "item")
        {
            GameObject item = collision.gameObject;
            float[] buffs = item.GetComponent<ItemScript>().buffs;
            for (int i = 0; i < CurrentBuffs.Length; i++)
            {
                CurrentBuffs[i] = CurrentBuffs[i] + buffs[i];
            }
            if (item.GetComponent<ItemScript>().PowerUpName == "orgiball")
            {
                Orgiballcount = Orgiballcount + 1;

            }
            ApplyCurrentBuffs();

        }
    }

    #region Functions

    #region Inputs
    public void UpdateInputs()
    {
        UpdateKeyInputs();
        UpdateMovementInputs();
        UpdateMousePositionOnCamera();
        UpdateSkillInputs();
    }
        #region Child Functions
    void UpdateKeyInputs()
    {
        for (int i = 0; i < InputKeys.Count; i++)
        {
            foreach (bool key in InputKeyState)
            {
                InputKeyState[i] = Input.GetKey(InputKeys[i]);
            }
        }
    }
    void UpdateMousePositionOnCamera()
    {
        
        mousePosition = followcam.ScreenToWorldPoint(Input.mousePosition);
       
        
    }
    void UpdateMovementInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    void UpdateSkillInputs()
    {
        for (int i = 0; i < UseSkill.Length; i++)
        {
            foreach (bool state in UseSkill)
            {
                UseSkill[i] = InputKeyState[i];
            }
        }
    }
    #endregion

    #endregion


    #region Mechanics
    void UpdateView()
    {
        CameraFollow();
        if (Alive)
        {
            Look();

        }
    }
    void UpdateMechanics()
    {
        Freeze();
        if (Alive)
        {
            if (!Isfrozen)
            {
                Move();
            }
        SkillMechanics();
        RegenSperm();
        RegenHealth();

        }
        CheckAlive();
    }
        #region Child Functions
        void CameraFollow()
    {
        Vector3 camMove;
        camMove.x = pv.transform.position.x;
        camMove.y = pv.transform.position.y;
        camMove.z = cameraZoffset;
        followcam.transform.position = camMove;
    }
        void Move()
    {
        if (!UseAcceleration)
        {
            rb.MovePosition(rb.position + moveDirection * MoveSpeed * Time.fixedDeltaTime);
        }
    }
        void Look()
    {
        lookDir = mousePosition - rb.position;
       
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - (player.spriteRotation);
        rb.rotation = angle;
    }
        void RegenSperm()
    {
        if (SpermCount <= MaxSperm)
        {
            SpermCount = SpermCount + (SpermRate);
        }
        if (SpermCount > MaxSperm)
        {
            SpermCount = MaxSperm;
        }
    }
        void RegenHealth()
    {
        if (Health <= MaxHealth)
        {
            Health = Health + (HealthRegenRate);
        }
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
        void ApplyCurrentBuffs()
    {
        SpermRate = player.startCumrate + CurrentBuffs[SpermRegenID];
        HealthRegenRate = player.startRegenrate + CurrentBuffs[HealthRegenID];
    }
        void TakeDamage(float i)
    {
        Health = Health - i;
    }
        void CheckAlive()
    {
        if (Health < 0)
        {
            Alive = false;
        }
        if (!Alive)
        {
            respawnMenu.SetActive(true);
        }
    }
        public void Respawn(bool keepUpgrade)
    {
        respawnMenu.SetActive(false);
        if (!keepUpgrade)
        {
            SpermCount = MaxSperm;
            Health = MaxHealth;
            Orgiballcount = 0;
            for (int i = 0; i < CurrentBuffs.Length; i++)
            {
                CurrentBuffs[i] = 0;
            }
        }
        Alive = true;
    }
    void SkillMechanics()
    {
        for (int i = 0; i < UseSkill.Length; i++)
        {
            CheckSkillReady(i);
            if (UseSkill[i])
            {
            FireRemaining(i);

            }
            else
            {
                remainingFire[i] = 0;
            }
          
            //Fire Skill i if Ready on Use
            if (SkillReady[i] && UseSkill[i] && SpermCount >= Skills[i].cost)
            {
                if (Skills[i].FireMultiple)
                {
                    remainingFire[i] = Skills[i].FireCount;
                }
                else
                {
                if (Skills[i].IsProjectile)
                {
                FireSkillProjectile(i);

                }

                }
                SkillReady[i] = false;
                SpermCount = SpermCount - Skills[i].cost;
                if (!Skills[i].UseTrailingFreeze)
                {
                    StartFreeze(Skills[i].freezetime);
                }
                PlaySkillSound(i);
                ResetSkillCoolDown(i);
            }
        }
        
    }
            void FireRemaining(int i)
    {
        
        if (remainingFire[i] > 0 && remainingTimer[i] < Time.time)
        {
            remainingTimer[i] = Time.time + Skills[i].TimeInBetween;
            remainingFire[i] = remainingFire[i] - 1;
            FireSkillProjectile(i);
            if (!Skills[i].UseTrailingFreeze)
            {
            StartFreeze(Skills[i].TimeInBetween + Skills[i].FreezeTrail);

            }
            
        }
        
        
        
    }
            void FireSkillProjectile(int i)
    {
        firepoint[i].localRotation = Quaternion.identity;
        firepoint[i].Rotate(Vector3.forward, Random.Range(-Skills[i].spread, Skills[i].spread));
        Skill currentskill = Skills[i];
        GameObject skill = PhotonNetwork.Instantiate(currentskill.skillpf.name, firepoint[i].position, firepoint[i].rotation);
        Rigidbody2D rskill = skill.GetComponent<Rigidbody2D>();

        

        rskill.AddForce(firepoint[i].up.normalized * currentskill.skillforce, ForceMode2D.Force);
    }
            void CheckSkillReady(int i)
    {
        if (Time.time > SkillTimer[i] && SkillReady[i] == false)
        {
            SkillReady[i] = true;
        }
    }
            void ResetSkillCoolDown(int i)
    {
        Skill currentskill = Skills[i];
        SkillTimer[i] = Time.time + currentskill.Cooldowntime;
    }
        void Freeze()
    {
        if(freezetimer > Time.time)
        {
            Isfrozen = true;
        }
        else
        {
            Isfrozen = false;
        }
    }
        void StartFreeze(float timeinseconds)
    {
        freezetimer = timeinseconds + Time.time;
    }
        void StopFreeze()
    {
        freezetimer = 0;
    }
        
    #endregion


    #endregion


    #region UI
    void UpdateUI()
    {
        UpdatePing();
        UpdateSliders();
        UpdateCountdown();
        UpdateHealthBar();
        UpdateCumBar();
        UpdateOtherUI();
    }
        void UpdatePing()
    {
        ping = PhotonNetwork.GetPing();
        pingtext.text = ping.ToString() + " ms";
        int pingint = Mathf.FloorToInt(ping / 50);
        pingslider.value = pingint;
        if (pingint >= 0 && pingint <= 4)
        {
            pingfill.GetComponent<Image>().color = pingbarcolors[pingint + 1];
        }
        else
        {
            pingfill.GetComponent<Image>().color = pingbarcolors[0];
        }
    }
        void UpdateSliders()
    {
        for (int i = 0; i < SkillSliders.Length; i++)
        {
            SkillSliders[i].maxValue = Skills[i].Cooldowntime;
            SkillSliders[i].value = SkillTimer[i] - Time.time;
        }
    }
        void UpdateCountdown()
    {
        for (int i = 0; i < SkillTimeDisplay.Length; i++)
        {
            int cdt = Mathf.FloorToInt((SkillTimer[i] - Time.time));
            if (SkillReady[i])
            {
                SkillTimeDisplay[i].text = " ";
            }
            else
            {
                SkillTimeDisplay[i].text = cdt.ToString();
            }
        }
    }
        void UpdateHealthBar()
    {
        healthbar.maxValue = MaxHealth;
        healthtext.text = Health.ToString();
        maxhealthtext.text = MaxHealth.ToString();
        healthbar.value = Health;
    }
        void UpdateCumBar()
    {
        cumbar.maxValue = MaxSperm;
        cumbar.value = SpermCount;
        cumtext.text = SpermCount.ToString();
        maxcumtext.text = MaxSperm.ToString();
    }
        void UpdateOtherUI()
    {
        Orgiballcountnum.text = Orgiballcount.ToString();
    }
    #endregion


    #region Audio & Animation
    void PlayRandomHitSound()
    {
        int i = Random.Range(0, player.hitsound.Length);
        if (player.hitsound[i] != null)
        {
        playerSounds.PlayOneShot(player.hitsound[i]);
        }

    }
    void PlaySkillSound(int i)
    {
        playerSounds.PlayOneShot(Skills[i].skillsound);
    }

    #endregion


    #region Single Use Stuff
    void CopyFromPlayerScriptable()
    {
        Skills = player.Skills;
        gameObject.GetComponent<SpriteRenderer>().sprite = player.idleSprite;
        MoveSpeed = player.MoveSpeed;

        MaxHealth = player.startHealth;
        HealthRegenRate = player.startRegenrate;
        Health = MaxHealth;
        HealthRegenRate = player.startRegenrate;

        MaxSperm = player.startCumMax;
        SpermCount = MaxSperm;
        SpermRate = player.startCumrate;
        
    }
   

    #endregion


    #endregion
}
