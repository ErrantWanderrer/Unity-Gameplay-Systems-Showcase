
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlayerController : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    public int numOfVerts = 12;
    public Material[] material = new Material[12];
    public GameObject[] tails = new GameObject[24];
    public GameObject CollisionDust;
    //public GameObject ScoreAddingField;
    public GameObject Player;
    public Transform Tails;
    public Transform Point;
    public GameObject tapJoystick;
    public float radius = 1f;

    public float speed = 500f;
    public float accelerationChanger = 4;
    public float maxVelocity = 50f;
    public float verticalVelocity = 10;

    //public float slowDownSpeed = 5f;
    public float health;
    public float currentHealth;
    //public Transform LeftEdge, RightEdge;
    public Transform AsteroidPart;
   // bool OnRightEdge, OnLeftEdge = false;
    private Rigidbody2D rb;
    Vector2[] Points;
    public Text ScoreText;
    public Text MassText;
    public int CurrentScore = 0;
    public int coinsEarn = 0;
    public Text coinsText;
    public Text coinsDeath;
    public Text CountText;
    
    public bool gameStarted = false;
    public float moveX = 0;
    public float moveY = 0;
    public bool checkTouch = false;
    public bool casualControl = false;
    public int playerLevel;
    public GameObject[] Panels;
    public GameObject DeathPanel;
    public Image hpBar;
    public Image shieldBar;

    public Text WarningText;
    public GameObject gameObjectController;

    public bool clickedRight;
    public bool clickedLeft;
    public bool clickedCentr;
    public float checkMove;
    private Vector2 m_pos;
    private Vector2 m_position;

    Vector3 dir = Vector3.zero;

    public GameObject Shield;
    public GameObject ShieldEffect;
    public float Shield_Health = 100f;
    public float current_Shield_Health;
    public GameObject Anti_gravitator;
    public GameObject Indicator_Grav;

    float interval_left = 0.5f;
    float interval_right = 0.5f;
    int tap_left;
    int tap_right;

    public bool IsShieldBarLow;
    public bool IsGravitatorLow;
    public bool boost = false;
    public int coinDoublerCheck = 1;

    private float exp = 0f;
    private float acceleration = 0f;


    public CameraOptions cameraShake;
    public Camera mainCamera;
    //public Joystick joystick;
    public VariableJoystick VerticalJoystick;
    List<System.Action> actions = new List<System.Action>();

    public Vector3 velocity = Vector3.zero;


    void FixedUpdate()
    {
        Debug.Log("wefefwf");
        if (gameStarted)
        {
            CurrentScore++;
            CurrentScore++;
            CurrentScore++;
            ScoreText.text = CurrentScore.ToString();
        }

        if (gameStarted)
        {
            moveX = VerticalJoystick.Horizontal;

            if (clickedCentr == true)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                m_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 synhr = new Vector2(0f, -transform.position.y);
                m_pos = m_pos + synhr;
                m_pos = m_pos * Vector2.up;
                m_pos = m_pos.normalized;
            }
            rb.AddForce(m_pos * verticalVelocity, ForceMode2D.Impulse);

            if (Player.transform.position.y <= m_position.y && m_pos.y == -1f)
            {
                rb.velocity = rb.velocity * Vector2.right;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            else if (Player.transform.position.y >= m_position.y && m_pos.y == 1f)
            {
                rb.velocity = rb.velocity * Vector2.right;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }

            if (PlayerPrefs.GetInt("ControlMode") == 1)
            {
                if (moveX > 0)
                {
                    if (!boost)
                    {
                        if (rb.velocity.x < maxVelocity)
                        {
                            if (acceleration <= 1f)
                            {
                                exp = (exp + 0.01f);
                                acceleration = Mathf.Exp(exp) / accelerationChanger;
                            }
                            rb.AddForce(Vector2.right * moveX * speed * acceleration);
                        }
                    }
                    else if (boost)
                    {
                        if (rb.velocity.x < 400)
                        {
                            exp = (exp + 0.01f);
                            acceleration = Mathf.Exp(exp) / accelerationChanger;
                            rb.AddForce(Vector2.right * moveX * speed * acceleration);
                        }
                    }
                }
                else if (moveX < 0)
                {
                    if (!boost)
                    {
                        if (rb.velocity.x > -maxVelocity)
                        {
                            if (acceleration <= 1f)
                            {
                                exp = (exp + 0.01f);
                                acceleration = Mathf.Exp(exp) / accelerationChanger;
                            }
                            rb.AddForce(Vector2.right * moveX * speed * acceleration);
                        }
                    }
                    else if (boost)
                    {
                        if (rb.velocity.x > -400)
                        {
                            exp = (exp + 0.01f);
                            acceleration = Mathf.Exp(exp) / accelerationChanger;
                            rb.AddForce(Vector2.right * moveX * speed * acceleration);
                        }
                    }
                }
                if (rb.velocity.x < 0 && moveX >= 0)
                {
                    exp = 0f;
                    acceleration = 0f;
                }
                if (rb.velocity.x > 0 && moveX <= 0)
                {
                    exp = 0f;
                    acceleration = 0f;
                }
                if (boost)
                {
                    if (rb.velocity.magnitude >= maxVelocity)
                    {
                        if (rb.velocity.magnitude <= 400)
                        {
                            rb.velocity = rb.velocity.normalized * maxVelocity;
                        }
                    }
                }
            }
            else if (PlayerPrefs.GetInt("ControlMode") == 0)
            {
                if (clickedRight == true/* || Input.GetKey(KeyCode.RightArrow)*/)
                {
                    if (!boost)
                    {
                        if (rb.velocity.x < maxVelocity)
                        {
                            if (acceleration <= 1f)
                            {
                                exp = (exp + 0.01f);
                                acceleration = Mathf.Exp(exp) / accelerationChanger;
                            }
                            rb.AddForce(Vector2.right * speed * acceleration);
                        }
                    }
                    else if (boost)
                    {
                        if (rb.velocity.x < 400)
                        {
                            exp = (exp + 0.01f);
                            acceleration = Mathf.Exp(exp) / accelerationChanger;
                            rb.AddForce(Vector2.right * speed * acceleration);
                        }
                    }
                }
                {
                    /*else if ((clickedLeft == true || Input.GetKey(KeyCode.LeftArrow)) && (Player.transform.position.x - Point.transform.position.x > 0))
                    {
                        rb.AddForce(Vector2.right * slowDownSpeed * -speed);
                    }*/
                }
                if (clickedLeft == true/* || Input.GetKey(KeyCode.LeftArrow)*/)
                {
                    if (!boost)
                    {
                        if (rb.velocity.x > -maxVelocity)
                        {
                            if (acceleration <= 1f)
                            {
                                exp = (exp + 0.01f);
                                acceleration = Mathf.Exp(exp) / accelerationChanger;
                            }
                            rb.AddForce(Vector2.left * speed * acceleration);
                        }
                    }
                    else if (boost)
                    {
                        if (rb.velocity.x > -400)
                        {
                            exp = (exp + 0.01f);
                            acceleration = Mathf.Exp(exp) / accelerationChanger;
                            rb.AddForce(Vector2.left * speed * acceleration);
                        }
                    }
                }
                {
                    /*else if ((clickedRight == true || Input.GetKey(KeyCode.RightArrow)) && (Player.transform.position.x - Point.transform.position.x < 0))
                    {
                        rb.AddForce(Vector2.right * slowDownSpeed * speed);
                    }*/
                }
                if (boost)
                {
                    if (rb.velocity.magnitude >= maxVelocity)
                    {
                        if (rb.velocity.magnitude <= 400)
                        {
                            rb.velocity = rb.velocity.normalized * maxVelocity;
                        }
                    }
                }
            }
            else if (PlayerPrefs.GetInt("ControlMode") == 2)
            {
                dir.x = Input.acceleration.x;
                if (dir.sqrMagnitude > 1)
                    dir.Normalize();
                rb.AddForce(speed / accelerationChanger * dir);

                if (rb.velocity.magnitude >= maxVelocity)
                {
                    rb.velocity = rb.velocity.normalized * maxVelocity;
                }
            }
        }
        {/*if (Player.transform.position.y >= 18f || Player.transform.position.y <=-30f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }*/

            /*else if (moveY > 0 && (Player.transform.position.y - Point.transform.position.x > 0))
            {
                rb.AddForce(Vector2.up * slowDownSpeed * -speed * Time.deltaTime);
            }
            else if (moveY < 0 && (Player.transform.position.x - Point.transform.position.x < 0))
            {
                rb.AddForce(Vector2.up * slowDownSpeed * speed * Time.deltaTime);
            }*/

            /*if (OnLeftEdge && (rb.velocity.x < 0 || moveX < 0))
            {
                if(casualControl)
                {
                    if (moveX < 0)
                        rb.velocity = new Vector2(0f, 0f);

                }else
                {
                    rb.velocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(0f, 0f), Time.deltaTime * slowDownSpeed);
                    if (moveX > 0)
                    {
                        rb.AddForce(Vector2.right * moveX * speed * Time.deltaTime);
                    }
                }
            }
            else if (OnRightEdge && (rb.velocity.x > 0 || moveX > 0))
            {
                if(casualControl)
                {
                    if(moveX > 0)
                        rb.velocity = new Vector2(0f, 0f);
                }
                else
                {
                    rb.velocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(0f, 0f), Time.deltaTime * slowDownSpeed);
                    if (moveX < 0)
                    {
                        rb.AddForce(Vector2.right * moveX * speed * Time.deltaTime);
                    }
                }
            }*/
        }
    }

    public void MoveRight()
    {
        clickedRight = true;
        tap_right++;

        if (tap_right == 1)
        {
           StartCoroutine(DoubleTapInterval_Right());
        }
        else if (tap_right > 2)
        {
            if (PlayerPrefs.GetInt("Shields_Count") > 0)
            {
                if (Indicator_Grav.gameObject.activeSelf == true && shieldBar.gameObject.activeSelf == false)
                {
                    IsShieldBarLow = true;
                    var pos = shieldBar.GetComponent<RectTransform>().anchoredPosition;
                    pos.y = -190f;
                    shieldBar.GetComponent<RectTransform>().anchoredPosition = pos;
                    //shieldBar.gameObject.transform.Translate(new Vector3(0, -32f));
                }
                Shield_Health = PlayerPrefs.GetInt("Shield_Health");
                current_Shield_Health = Shield_Health;
                shieldBar.fillAmount = 1f;
                Shield.SetActive(true);
                ShieldEffect.GetComponent<ParticleSystem>().Play();
                shieldBar.gameObject.SetActive(true);
                PlayerPrefs.SetInt("Shields_Count", PlayerPrefs.GetInt("Shields_Count") - 1);
            }
            tap_right = 0;
        }
    }
    public void MoveLeft()
    {
        clickedLeft = true;
        tap_left++;

        if (tap_left == 1)
        {
            StartCoroutine(DoubleTapInterval_Left());
        }
        else if (tap_left > 2)
        {
            if (PlayerPrefs.GetInt("Gravi_Count") > 0)
            {
                if (shieldBar.gameObject.activeSelf == true && Indicator_Grav.gameObject.activeSelf == false)
                {
                    IsGravitatorLow = true;
                    var pos = Indicator_Grav.GetComponent<RectTransform>().anchoredPosition;
                    pos.y = -190f;
                    Indicator_Grav.GetComponent<RectTransform>().anchoredPosition = pos;
                    //Indicator_Grav.gameObject.transform.Translate(new Vector3(0, -32f));
                }
                Anti_gravitator.SetActive(false);
                Anti_gravitator.SetActive(true);
                Anti_gravitator.GetComponent<ParticleSystem>().Play();
                Indicator_Grav.SetActive(true);
                Anti_gravitator.GetComponent<Indicator_Gravitator>().Gravitator_Launch();
                PlayerPrefs.SetInt("Gravi_Count", PlayerPrefs.GetInt("Gravi_Count") - 1);
            }
            tap_left = 0;
        }
    }

    public void MoveCentr()
    {
        clickedCentr = true;
    }

    public void StopMovingRight()
    {
        exp = 0;
        acceleration = 0;
        clickedRight = false;
        checkMove = 1;
    }

    public void StopMovingLeft()
    {
        exp = 0;
        acceleration = 0;
        clickedLeft = false;
        checkMove = -1;
    }

    public void StopMoveCentr()
    {
        clickedCentr = false;
    }

    IEnumerator DoubleTapInterval_Right()
    {
        yield return new WaitForSeconds(interval_right);
        this.tap_right = 0;
    }

    IEnumerator DoubleTapInterval_Left()
    {
        yield return new WaitForSeconds(interval_left);
        this.tap_left = 0;
    }

    /*public override void OnPointerUp(PointerEventData eventData)
    {
        checkMove = moveX;
    }*/

    void SetLevel()
    {
        //if (PlayerPrefs.HasKey("PlayerLevel"))
            playerLevel = PlayerPrefs.GetInt("asteroidType");
        //else
            //playerLevel = 0;
    }

    /*void Loads()
    {

    }*/
    public void colorBack1()
    {
        mainCamera.backgroundColor = new Color(19f / 255f, 18f / 255f, 63f / 255f);
        PlayerPrefs.SetInt("background", 1);
        PlayerPrefs.Save();
    }
    public void colorBack2()
    {
        mainCamera.backgroundColor = new Color(31f / 255f, 12f / 255f, 55f / 255f);
        PlayerPrefs.SetInt("background", 2);
        PlayerPrefs.Save();
    }
    public void colorBack3()
    {
        mainCamera.backgroundColor = new Color(61f / 255f, 19f / 255f, 22f / 255f);
        PlayerPrefs.SetInt("background", 3);
        PlayerPrefs.Save();
    }
    public void colorBack4()
    {
        //mainCamera.backgroundColor = new Color(54f / 255f, 46f / 255f, 0f / 255f);
        mainCamera.backgroundColor = new Color(0f / 255f, 0f / 255f, 0f / 255f);
        PlayerPrefs.SetInt("background", 4);
        PlayerPrefs.Save();
    }
    public void colorBack5()
    {
        mainCamera.backgroundColor = new Color(59f / 255f, 8f / 255f, 15f / 255f);
        PlayerPrefs.SetInt("background", 5);
        PlayerPrefs.Save();
    }
    public void colorBack6()
    {
        //mainCamera.backgroundColor = new Color(19f / 255f, 18f / 255f, 63f / 255f);
        mainCamera.backgroundColor = new Color(15f / 255f, 43f / 255f, 46f / 255f);
        PlayerPrefs.SetInt("background", 6);
        PlayerPrefs.Save();
    }

    void Awake()
    {
        if (!PlayerPrefs.HasKey("background"))
        {
            PlayerPrefs.SetInt("background", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("InterAd"))
        {
            PlayerPrefs.SetInt("InterAd", 0);
        }
        if (PlayerPrefs.GetInt("BlackBack") == 0)
        {
            actions.Add(colorBack1);
            actions.Add(colorBack2);
            actions.Add(colorBack3);
            actions.Add(colorBack4);
            actions.Add(colorBack5);
            actions.Add(colorBack6);
            int r = Random.Range(0, 6);
            actions[r]();
        }

        if (!PlayerPrefs.HasKey("asteroidType"))
        {
            PlayerPrefs.SetInt("asteroidType", 0);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("CoinDoubler") == 1)
        {
            coinDoublerCheck = 2;
        }
        if (PlayerPrefs.GetInt("Speed_Boost") == 1)
        {
            boost = true;
        }


        //InvokeRepeating("Loads", 0.0f, 3.0f);
        SetLevel();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new Vector3[numOfVerts + 1];
        triangles = new int[3 * numOfVerts];
        uv = new Vector2[numOfVerts + 1];
        rb = GetComponent<Rigidbody2D>();

        speed = PlayerPrefs.GetInt("Speed");
        accelerationChanger = PlayerPrefs.GetFloat("Acceleration");
        maxVelocity = PlayerPrefs.GetInt("MaxVelocity");
        verticalVelocity = PlayerPrefs.GetInt("VerticalVelocity");

        for (int i = 0; i<=11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                gameObject.GetComponent<MeshRenderer>().material = material[i];
            }
        }
        //Instantiate(AsteroidPart, new Vector3(radius, 0f, 0f), gameObject.transform.rotation);
        for (int i = 0; i <= 11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                //tails[i * 2].SetActive(true);
                //tails[i * 2+1].SetActive(true);
                Instantiate((tails[i * 2]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
                Instantiate((tails[i * 2 + 1]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
            }
        }
        Points = new Vector2[numOfVerts];
    }

    public void changeAsteroid()
    {
        for (int i = 0; i <= 11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                gameObject.GetComponent<MeshRenderer>().material = material[i];
                SetLevel();
                //health = 100 * ((PlayerPrefs.GetInt("asteroidType") + 1) * 2);
            }
        }
        for (int i = 0; i <= 11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                Instantiate((tails[i * 2]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
                Instantiate((tails[i * 2 + 1]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
            }
        }
        if (PlayerPrefs.GetInt("asteroidType") == 0)
        {
            health = 100;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 1)
        {
            health = 200;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 2)
        {
            health = 350;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 3)
        {
            health = 500;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 4)
        {
            health = 600;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 5)
        {
            health = 700;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 6)
        {
            health = 850;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 7)
        {
            health = 1000;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 8)
        {
            health = 1200;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 9)
        {
            health = 1350;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 10)
        {
            health = 1500;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 11)
        {
            health = 1850;
        }
        currentHealth = health;
        MassText.text = health.ToString() + " t";
    }
    /*public void cleanTails()
    {
        for (int i = 0; i <= 11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                //tails[i * 2].SetActive(false);
                //tails[i * 2 + 1].SetActive(false);
                //Destroy(tails[i]);
            }
        }
    }*/

    void Start()
    {
        MakeMeshData();
        CreateMesh();
        //SetCollider();
        //LeftEdge.transform.position = new Vector3(-(Mathf.Abs(edgeCoord)), gameObject.transform.position.y, 0f);
        //RightEdge.transform.position = new Vector3(Mathf.Abs(edgeCoord), gameObject.transform.position.y, 0f);
        //rb.AddTorque(Random.Range(-200f, 200f));
        Player.GetComponent<Rigidbody2D>().simulated = false;
        rb = GetComponent<Rigidbody2D>();

        if (PlayerPrefs.GetInt("asteroidType") == 0)
        {
            health = 100;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 1)
        {
            health = 200;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 2)
        {
            health = 350;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 3)
        {
            health = 500;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 4)
        {
            health = 600;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 5)
        {
            health = 700;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 6)
        {
            health = 850;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 7)
        {
            health = 1000;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 8)
        {
            health = 1200;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 9)
        {
            health = 1350;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 10)
        {
            health = 1500;
        }
        else if (PlayerPrefs.GetInt("asteroidType") == 11)
        {
            health = 1850;
        }
        currentHealth = health;
        MassText.text = health.ToString() + " t";
    }

    void MakeMeshData()
    {
        vertices[0] = new Vector3(0f, 0f, 0f);
        float angle = 0;
        for (int i = 1; i <= numOfVerts; i++)
        {
            float randScale = Random.Range(0.9f, 1.1f);
            vertices[i] = new Vector3(radius * Mathf.Cos(angle) * randScale, radius * Mathf.Sin(angle) * randScale, 0f);
            angle -= ((2 * Mathf.PI) / numOfVerts);

        }
        int k = 0;
        for (int i = 0; i < triangles.Length - 3; i += 3)
        {
            triangles[i] = 0;
            triangles[i + 1] = k + 1;
            triangles[i + 2] = k + 2;
            k++;
        }
        triangles[triangles.Length - 3] = 0;
        triangles[triangles.Length - 2] = numOfVerts;
        triangles[triangles.Length - 1] = 1;
    }

    void CalculateUVs()
    {
        for (int i = 0; i <= numOfVerts; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        CalculateUVs();
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "LeftEdge")
        {
            OnLeftEdge = true;
        }
        else if (other.gameObject.tag == "RightEdge")
        {
            OnRightEdge = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //checkTouch = false;
        if (other.gameObject.tag == "LeftEdge")
        {
            OnLeftEdge = false;
        }
        else if (other.gameObject.tag == "RightEdge")
        {
            OnRightEdge = false;
        }
    }*/

    /*float TwoPointsDist(float x, float x0, float y, float y0)
    {
        return Mathf.Sqrt(Mathf.Pow((x - x0), 2) + Mathf.Pow((y - y0), 2));
    }*/

    void OnCollisionEnter2D(Collision2D other)
    {
        checkTouch = true;
        Instantiate(CollisionDust, new Vector3(other.contacts[0].point.x, other.contacts[0].point.y, 0f), Quaternion.identity);
        if(other.gameObject.tag == "InactivePart")
        {
            return;
        }else if(other.gameObject.tag == "EnemyAsteroid" && PlayerPrefs.GetInt("Tips") == 0)
        {
            int other_level = other.gameObject.GetComponent<NPCasteroidBehaviour>().level;
            
            if (other_level > playerLevel && Shield.activeSelf != true)
            {
                float damage = ((((other_level - playerLevel) * 5 + 50) * 100) / currentHealth);
                health -= (other_level - playerLevel) * 5 + 50;
                hpBar.fillAmount -= (damage * 0.01f);
                if (health <= 0)
                {
                    PlayerDeath();
                    health = 0;
                }
                MassText.text = health.ToString() + " t";
                Vibration.Vibrate(400);
                StartCoroutine(cameraShake.Shake(.25f, .5f));
            }
            else if (Shield.activeSelf == true && Player.gameObject != null)
            {
                Shield_Health -= 10;
                shieldBar.fillAmount -= ((10 * 100) / current_Shield_Health) * 0.01f;
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (5 * coinDoublerCheck));
                GameObject.Find("Player").GetComponent<PlayerController>().coinsEarn += (5 * coinDoublerCheck);
                GameObject.Find("Player").GetComponent<PlayerController>().coinsText.text = GameObject.Find("Player").GetComponent<PlayerController>().coinsEarn.ToString();
                GameObject.Find("Credits").GetComponent<Animation>().Play("CreditAnim");
                for (int i = 0; i <= 11; i++)
                {
                    if (other_level == i)
                    {
                        PlayerPrefs.SetInt("Ast_" + (i + 1), PlayerPrefs.GetInt("Ast_" + (i + 1)) - 1);
                    }
                }
                if (Shield_Health <= 0)
                {
                    ParticleSystem Shield_Par = ShieldEffect.GetComponent<ParticleSystem>();
                    Shield_Par.Stop();
                    Shield_Health = current_Shield_Health;
                    current_Shield_Health = 0;
                    if (IsShieldBarLow)
                    {
                        IsShieldBarLow = false;
                        var pos = shieldBar.GetComponent<RectTransform>().anchoredPosition;
                        pos.y = -159.5f;
                        shieldBar.GetComponent<RectTransform>().anchoredPosition = pos;
                        //shieldBar.gameObject.transform.Translate(new Vector3(0, 32f));
                    }   
                    if (Indicator_Grav.gameObject.activeSelf == true && IsGravitatorLow)
                    {
                        IsGravitatorLow = false;
                        var pos = Indicator_Grav.GetComponent<RectTransform>().anchoredPosition;
                        pos.y = -159.5f;
                        Indicator_Grav.GetComponent<RectTransform>().anchoredPosition = pos;
                        //Indicator_Grav.gameObject.transform.Translate(new Vector3(0, 32f));
                    }  
                    shieldBar.gameObject.SetActive(false);
                    Shield.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(cameraShake.Shake(.15f, .2f));
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (10 * coinDoublerCheck));
                GameObject.Find("Player").GetComponent<PlayerController>().coinsEarn += (10 * coinDoublerCheck);
                GameObject.Find("Player").GetComponent<PlayerController>().coinsText.text = GameObject.Find("Player").GetComponent<PlayerController>().coinsEarn.ToString();
                GameObject.Find("Credits").GetComponent<Animation>().Play("CreditAnim");
                Vibration.Vibrate(200);
                for (int i = 0; i <= 11; i++)
                {
                    if (other_level == i)
                    {
                        PlayerPrefs.SetInt("Ast_" + (i + 1), PlayerPrefs.GetInt("Ast_" + (i + 1)) - 1);
                    }
                }
            } 
        }
        else if (other.gameObject.tag == "planet")
        {
            if (Shield.activeSelf != true)
            {
                float damage = (10000 / currentHealth);
                health -= 100;
                hpBar.fillAmount -= (damage * 0.01f);
                if (health <= 0)
                {
                    PlayerDeath();
                    health = 0;
                }
                MassText.text = health.ToString() + " t";
                Vibration.Vibrate(400);
                StartCoroutine(cameraShake.Shake(0.5f, 1f));
            }
            else if (Shield.activeSelf == true)
            {
                Shield_Health -= 50;
                shieldBar.fillAmount -= (5000 / current_Shield_Health) * 0.01f;  /*50*100*/
                if (Shield_Health <= 0)
                {
                    ParticleSystem Shield_Par = ShieldEffect.GetComponent<ParticleSystem>();
                    Shield_Par.Stop();
                    Shield_Health = current_Shield_Health;
                    current_Shield_Health = 0;
                    if (IsShieldBarLow)
                    {
                        IsShieldBarLow = false;
                        var pos = shieldBar.GetComponent<RectTransform>().anchoredPosition;
                        pos.y = -159.5f;
                        shieldBar.GetComponent<RectTransform>().anchoredPosition = pos;
                        //shieldBar.gameObject.transform.Translate(new Vector3(0, 32f));
                    }
                    if (Indicator_Grav.gameObject.activeSelf == true && IsGravitatorLow)
                    {
                        IsGravitatorLow = false;
                        var pos = Indicator_Grav.GetComponent<RectTransform>().anchoredPosition;
                        pos.y = -159.5f;
                        Indicator_Grav.GetComponent<RectTransform>().anchoredPosition = pos;
                        //Indicator_Grav.gameObject.transform.Translate(new Vector3(0, 32f));
                    }
                    shieldBar.gameObject.SetActive(false);
                    Shield.SetActive(false);
                    Vibration.Vibrate(400);
                }
            }
        }
        else if (other.gameObject.tag == "Plasma")
        {
            float damage = (2500 / currentHealth);
            health -= 25;
            hpBar.fillAmount -= (damage * 0.01f);
            if (health <= 0)
            {
                PlayerDeath();
                health = 0;
            }
            MassText.text = health.ToString() + " t";
            Vibration.Vibrate(400);
        }
        else if (other.gameObject.tag == "SpaceCraft")
        {
            if (hpBar.fillAmount < 1)
            {
                health += 10;
                hpBar.fillAmount += (0.1f);
                MassText.text = health.ToString() + " t";
            }
            Vibration.Vibrate(100);
        }

        /*int numOfVert = 1;
        float minDist = TwoPointsDist(other.contacts[0].point.x, gameObject.transform.TransformPoint(Points[0]).x, other.contacts[0].point.y, gameObject.transform.TransformPoint(Points[0]).y);
        for(int i = 2; i <= numOfVerts; i++)
        {
            float currDist = TwoPointsDist(other.contacts[0].point.x, gameObject.transform.TransformPoint(Points[i - 1]).x, other.contacts[0].point.y, gameObject.transform.TransformPoint(Points[i - 1]).y);
            if (currDist < minDist)
            {
                minDist = currDist;
                numOfVert = i;
            }
        }
        vertices[numOfVert] = new Vector3(vertices[numOfVert].x * 0.96f, vertices[numOfVert].y * 0.97f, 0f);
        CreateMesh();*/
        //RebuildCollider(numOfVert);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        checkTouch = false;
    }

    public void CoinsRaise()
    {
        StartCoroutine(CoinsEarnRaise());
    }
    public void CoinsRaiseX4()
    {
        StartCoroutine(CoinsEarnRaiseX4());
    }

    public IEnumerator CoinsEarnRaise()
    {
        yield return new WaitForSeconds(Random.Range(120, 180));
        coinDoublerCheck *= 2;
        WarningText.text = "The number of coins received for destroying spaceships has been increased! x2".ToString();
        WarningText.gameObject.SetActive(true);
        StartCoroutine(gameObjectController.GetComponent<GameObjectsCreator>().WarningText());
    }
    public IEnumerator CoinsEarnRaiseX4()
    {
        yield return new WaitForSeconds(Random.Range(300, 600));
        coinDoublerCheck *= 2;
        WarningText.text = "The number of coins received for destroying spaceships has been increased! x4".ToString();
        WarningText.gameObject.SetActive(true);
        StartCoroutine(gameObjectController.GetComponent<GameObjectsCreator>().WarningText());
    }

    /*void SetCollider()
    {
        gameObject.AddComponent<PolygonCollider2D>();
        for(int i = 0; i < numOfVerts; i++)
        {
            Points[i] = vertices[i + 1];
        }
        gameObject.GetComponent<PolygonCollider2D>().points = Points;
    }

    void RebuildCollider(int numOfVert)
    {
        if (GetComponent<PolygonCollider2D>() != null)
        {
            Points[numOfVert - 1] = new Vector2(Points[numOfVert - 1].x * 0.9f, Points[numOfVert - 1].y * 0.9f);
            gameObject.GetComponent<PolygonCollider2D>().points = Points;
            return;
        }
    }*/

    public void PlayerDeath()
    {
        //DeathAnimation();
        gameStarted = false;
        if (CurrentScore > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", CurrentScore);
        }
        Indicator_Grav.SetActive(false);
        shieldBar.gameObject.SetActive(false);
        DeathPanel.gameObject.SetActive(true);
        coinsDeath.text = PlayerPrefs.GetInt("coins").ToString();
        Anti_gravitator.GetComponent<ParticleSystem>().Stop();
        Anti_gravitator.SetActive(false);
        ShieldEffect.GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        Destroy(Tails.GetChild(0).gameObject);
        Destroy(Tails.GetChild(1).gameObject);
        PlayerPrefs.SetInt("InterAd", CurrentScore);
        Vibration.Vibrate(400);
    }

    public void PlayerLife()
    {
        if (PlayerPrefs.GetInt("coins") >= 1500)
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 1500);
            GetComponent<CircleCollider2D>().isTrigger = true;
            DeathPanel.gameObject.SetActive(false);
            StartCoroutine(Count());
            StartCoroutine(BackHpToNorm());
            for (int i = 0; i <= 11; i++)
            {
                if (PlayerPrefs.GetInt("asteroidType") == i)
                {
                    Instantiate((tails[i * 2]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
                    Instantiate((tails[i * 2 + 1]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
                }
            }
            health = currentHealth;
            MassText.text = health.ToString() + " t";
            hpBar.fillAmount = 1f;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            IEnumerator Count()
            {
                CountText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                CountText.text = "2".ToString();
                yield return new WaitForSeconds(1f);
                CountText.text = "1".ToString();
                yield return new WaitForSeconds(1f);
                CountText.gameObject.SetActive(false);
                CountText.text = "3".ToString();
                if (PlayerPrefs.GetInt("SimplifiedMode") == 1)
                {
                    Time.timeScale = 0.7f;
                }
                else if (PlayerPrefs.GetInt("SimplifiedMode") == 0)
                {
                    Time.timeScale = 1f;
                }
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                gameStarted = true;
            }
        }
    }

    public void RewardedLife()
    {
        DeathPanel.gameObject.SetActive(false);
        GetComponent<CircleCollider2D>().isTrigger = true;
        StartCoroutine(Count());
        StartCoroutine(BackHpToNorm());
        for (int i = 0; i <= 11; i++)
        {
            if (PlayerPrefs.GetInt("asteroidType") == i)
            {
                Instantiate((tails[i * 2]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
                Instantiate((tails[i * 2 + 1]), gameObject.transform.position, Quaternion.identity).transform.parent = Tails.gameObject.transform;
            }
        }
        health = currentHealth;
        MassText.text = health.ToString() + " t";
        hpBar.fillAmount = 1f;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        IEnumerator Count()
        {
            CountText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            CountText.text = "2".ToString();
            yield return new WaitForSeconds(1f);
            CountText.text = "1".ToString();
            yield return new WaitForSeconds(1f);
            CountText.gameObject.SetActive(false);
            CountText.text = "3".ToString();
            if (PlayerPrefs.GetInt("SimplifiedMode") == 1)
            {
                Time.timeScale = 0.7f;
            }
            else if (PlayerPrefs.GetInt("SimplifiedMode") == 0)
            {
                Time.timeScale = 1f;
            }
            gameObject.GetComponent<Rigidbody2D>().simulated = true;
            gameStarted = true;
        }
    }

    public IEnumerator BackHpToNorm()
    {
        yield return new WaitForSeconds(8f);
        GetComponent<CircleCollider2D>().isTrigger = false;
    }

    /*void DeathAnimation()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        float radius = 0.5f;
        Instantiate(AsteroidPart, new Vector3(radius, 0f, 0f), gameObject.transform.rotation);
        Instantiate(AsteroidPart, new Vector3(-radius, 0f, 0f), gameObject.transform.rotation);
        Instantiate(AsteroidPart, new Vector3(0f, radius, 0f), gameObject.transform.rotation);
        Instantiate(AsteroidPart, new Vector3(0f, -radius, 0f), gameObject.transform.rotation);
    }*/
}
