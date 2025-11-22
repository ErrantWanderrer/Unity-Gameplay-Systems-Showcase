
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectsCreator : MonoBehaviour {

    public float CameraOrthoSize;
    public int numberOfObjects;
    public int numberOfObjectsAst;
    public int numberOfObjectsPlanets;
    public int numberOfObjectsBlackHole;
    public int numberOfObjectsSpaceStations;
    public Transform[] GameplayObjects;
    public GameObject[] AsteroidObjects;
    public GameObject[] PlanetObjects;
    public GameObject[] BlackHolesObjects;
    public GameObject[] SpaceStationsObjects;
    public GameObject Player;
    public List<GameObject> list = new List<GameObject>();
    public List<Transform> spaceCraft1 = new List<Transform>();
    public List<GameObject> planets = new List<GameObject>();
    public List<GameObject> blackHoles = new List<GameObject>();
    public List<GameObject> SpaceStations = new List<GameObject>();
    /*public List<GameObject> tails = new List<GameObject>();
    public List<GameObject> tails2 = new List<GameObject>();*/
    public Transform Point;
    public int PresentAtOnce;
    public int PresentAtOnceAst;
    public int PresentAtOncePlanet;
    public int PresentAtOnceBlackHoles;
    public int PresentAtOnceSpaceStations;
    public int objectsCounter;
    public int objectsCounterAst;
    public int objectsCounterPlanet;
    public int objectsCounterBlackHoles;
    public int objectsCounterSpaceStations;
    /*public int objectsCounterTail;
    public int objectsCounterTail2;*/
    private bool formation = false;
    private bool formationAst = false;
    private bool formationPlanet = false;
    private bool formationBlackHoles = false;
    private bool formationStation = false;
    public float DelayBetweenSpawn;
    public float OptimizeDistanation;

    public GameObject Anti_gravitator;
    public Text Warning_BH;
    private int indexStation = 0;

    public float Gravity;
    private float G = 0.2f; // 0.25

    List<System.Action> actions = new List<System.Action>();

    void Start()
    {
        objectsCounter = 0;
        objectsCounterAst = 0;
        objectsCounterBlackHoles = 0;
        /*objectsCounterTail = 0;
        objectsCounterTail2 = 0;;*/
        actions.Add(action);
        actions.Add(action_1);
        actions.Add(action_2);

        Gravity = PlayerPrefs.GetFloat("GravityForce");
        CameraOrthoSize = Player.GetComponent<PlayerController>().mainCamera.GetComponent<Camera>().orthographicSize;
    }

    void FixedUpdate()
    {
        if (objectsCounter < PresentAtOnce && !formation)
        {
            StartCoroutine(SpawnGameplayObjects());
        }
        if (objectsCounterAst < PresentAtOnceAst && !formationAst)
        {
            StartCoroutine(SpawnAsteroidObjects());
        }
        if (objectsCounterPlanet < PresentAtOncePlanet && !formationPlanet)
        {
            int r = Random.Range(0, 3);
            actions[r]();
        }
        if (objectsCounterBlackHoles < PresentAtOnceBlackHoles && !formationBlackHoles)
        {
            StartCoroutine(SpawnBlackHoleObjects());
        }
        if (objectsCounterSpaceStations < PresentAtOnceSpaceStations && !formationStation)
        {
            StartCoroutine(SpawnSpaceStations());
        }

        /*for (int i = 0; i < tails.Count; i++)
        {
            if (tails[i] == null)
            {
                objectsCounterTail--;
                tails.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < tails2.Count; i++)
        {
            if (tails2[i] == null)
            {
                objectsCounterTail2--;
                tails2.RemoveAt(i);
                break;
            }
        }*/

        for (int i = 0; i < spaceCraft1.Count; i++)
        {
            if (Player == null)
            {
                break;
            }
            if (spaceCraft1[i] == null)
            {
                spaceCraft1.RemoveAt(i);
                break;
            }
            /*if (Mathf.Abs(spaceCraft1[i].transform.position.x - Player.transform.position.x) >= OptimizeDistanation)
            {
                //spaceCraft1[i].GetComponent<SpriteRenderer>().enabled = false;
                spaceCraft1[i].GetComponent<PolygonCollider2D>().isTrigger = true;
            }
            else if (Mathf.Abs(spaceCraft1[i].transform.position.x - Player.transform.position.x) <= OptimizeDistanation)
            {
                //spaceCraft1[i].GetComponent<SpriteRenderer>().enabled = true;
                //if (spaceCraft1[i].GetComponent<PolygonCollider2D>() != null)
                //{
                spaceCraft1[i].GetComponent<PolygonCollider2D>().isTrigger = false;
                //}
            }*/
        }

        for (int i = 0; i < planets.Count; i++)
        {
            if (Player == null)
            {
                break;
            }
            if (planets[i] == null)
            {
                planets.RemoveAt(i);
                break;
            }
            if (Mathf.Abs(planets[i].transform.position.x - Player.transform.position.x) >= 450)
            {
                Destroy(planets[i].gameObject);
            }
            /*if (Mathf.Abs(planets[i].transform.position.x - Player.transform.position.x) >= (OptimizeDistanation + 10))
            {
                planets[i].GetComponent<MeshRenderer>().enabled = false;
                //planets[i].GetComponent<CircleCollider2D>().isTrigger = true;
            }
            else if (Mathf.Abs(planets[i].transform.position.x - Player.transform.position.x) <= (OptimizeDistanation + 10))
            {
                planets[i].GetComponent<MeshRenderer>().enabled = true;
                //planets[i].GetComponent<CircleCollider2D>().isTrigger = false;
            }*/
            Vector2 player = Player.transform.localPosition;
            Vector2 obj = planets[i].transform.localPosition;
            if (Vector2.Distance(player, obj) <= 70 && Anti_gravitator.activeSelf != true)
            {
                Vector2 gVect = (player - obj);
                float fGravity = (G * 0.6f * Gravity * planets[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(player, obj));
                Player.GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity * Vector2.right);
            }
            for (int j = 0; j < spaceCraft1.Count; j++)
            {
                if (spaceCraft1[j] == null)
                {
                    break;
                }
                Vector2 spaceCraft = spaceCraft1[j].transform.localPosition;
                if (Vector2.Distance(spaceCraft, obj) <= 70)
                {
                    Vector2 gVect = (spaceCraft - obj);
                    float fGravity = (G * 0.01f * Gravity * planets[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(spaceCraft, obj));
                    spaceCraft1[j].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
            for (int k = 0; k < list.Count; k++)
            {
                if (list[k] == null)
                {
                    break;
                }
                Vector2 asteroidNPC = list[k].transform.localPosition;
                if (Vector2.Distance(asteroidNPC, obj) <= 50)
                {
                    Vector2 gVect = (asteroidNPC - obj);
                    float fGravity = (G * 0.01f * Gravity * planets[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(asteroidNPC, obj));
                    list[k].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
            for (int p = 1; p < planets.Count; p++)
            {
                if (planets[p] == null)
                {
                    break;
                }
                Vector2 planetNPC = planets[p].transform.localPosition;
                if (Vector2.Distance(planetNPC, obj) <= 70 && planets[p] != planets[i])
                {
                    Vector2 gVect = (planetNPC - obj);
                    float fGravity = (G * 0.5f * planets[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(planetNPC, obj));
                    planets[p].GetComponent<Rigidbody2D>().AddForce(gVect * fGravity);
                }
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (Player == null)
            {
                break;
            }
            if (list[i] == null)
            {
                list.RemoveAt(i);
                break;
            }
            //Vector3 player = Player.transform.localPosition;
            //Vector3 obj = list[i].transform.localPosition;
            //if (Mathf.Abs(list[i].transform.position.x - Player.transform.position.x) >= OptimizeDistanation)
            //{
                //list[i].GetComponent<MeshRenderer>().enabled = false;
                /*if (list[i].GetComponent<NPCasteroidBehaviour>().level == 0)
                {
                    tails[i].SetActive(false);
                    tails2[i].SetActive(false);
                }
                else if (list[i].GetComponent<NPCasteroidBehaviour>().level == 1)
                {
                    tails3[i].SetActive(false);
                    tails4[i].SetActive(false);
                }
                else if (list[i].GetComponent<NPCasteroidBehaviour>().level == 2)
                {
                    tails5[i].SetActive(false);
                    tails6[i].SetActive(false);
                }*/
            //}
            //else if (/*Vector3.Distance(player, obj)*/(Mathf.Abs(list[i].transform.position.x - Player.transform.position.x) <= OptimizeDistanation) && list[i].GetComponent<PolygonCollider2D>().isActiveAndEnabled)
            //{
                //list[i].GetComponent<MeshRenderer>().enabled = true;
                /*if (list[i].GetComponent<NPCasteroidBehaviour>().level == 0)
                {
                    tails[i].SetActive(true);
                    tails2[i].SetActive(true);
                }
                else if (list[i].GetComponent<NPCasteroidBehaviour>().level == 1)
                {
                    tails3[i].SetActive(true);
                    tails4[i].SetActive(true);
                }
                else if (list[i].GetComponent<NPCasteroidBehaviour>().level == 2)
                {
                    tails5[i].SetActive(true);
                    tails6[i].SetActive(true);
                }*/
            //}
            if (Mathf.Abs(list[i].transform.position.x - Player.transform.position.x) >= 250)
            {
                Destroy(list[i].gameObject);
            }
        }

        for (int i = 0; i < blackHoles.Count; i++)
        {
            if (Player == null)
            {
                break;
            }
            if (blackHoles[i] == null)
            {
                blackHoles.RemoveAt(i);
                break;
            }
            Vector2 player = Player.transform.localPosition;
            Vector2 obj = blackHoles[i].transform.localPosition;
            /*Vector2 part;
            if (GameObject.FindGameObjectWithTag("InactivePart") != null)
            {
                part = GameObject.FindGameObjectWithTag("InactivePart").transform.localPosition;
                if (Vector2.Distance(part, obj) <= 200)
                {
                    Vector2 gVect = (part - obj);
                    float fGravity = (G * Gravity * planets[i].GetComponent<Rigidbody2D>().mass * 0.5f) / (Vector2.Distance(part, obj));
                    GameObject.FindGameObjectWithTag("InactivePart").GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }*/
            blackHoles[i].GetComponent<AudioSource>().volume = ((-Vector2.Distance(player, obj) + 500) / 500) * PlayerPrefs.GetFloat("Sounds");
            if (Vector2.Distance(player, obj) <= 300 && Anti_gravitator.activeSelf != true)
            {
                Vector2 gVect = (player - obj);
                float fGravity = (0.04f * Gravity * blackHoles[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(player, obj));
                Player.GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity * Vector2.right);
            }
            for (int j = 0; j < spaceCraft1.Count; j++)
            {
                if (spaceCraft1[j] == null)
                {
                    break;
                }
                Vector2 spaceCraft = spaceCraft1[j].transform.localPosition;
                if (Vector2.Distance(spaceCraft, obj) <= 250)
                {
                    Vector2 gVect = (spaceCraft - obj);
                    float fGravity = (0.01f * Gravity * blackHoles[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(spaceCraft, obj));
                    spaceCraft1[j].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
            for (int k = 0; k < list.Count; k++)
            {
                if (list[k] == null)
                {
                    break;
                }
                Vector2 asteroidNPC = list[k].transform.localPosition;
                if (Vector2.Distance(asteroidNPC, obj) <= 250)
                {
                    Vector2 gVect = (asteroidNPC - obj);
                    float fGravity = (0.01f * Gravity * blackHoles[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(asteroidNPC, obj));
                    list[k].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
            for (int p = 0; p < planets.Count; p++)
            {
                if (planets[p] == null)
                {
                    break;
                }
                Vector2 planetNPC = planets[p].transform.localPosition;
                if (Vector2.Distance(planetNPC, obj) <= 250)
                {
                    Vector2 gVect = (planetNPC - obj);
                    float fGravity = (G * Gravity * blackHoles[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(planetNPC, obj));
                    planets[p].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
            for (int s = 0; s < SpaceStations.Count; s++)
            {
                if (SpaceStations[s] == null)
                {
                    break;
                }
                Vector2 stationNPC = SpaceStations[s].transform.localPosition;
                if (Vector2.Distance(stationNPC, obj) <= 100)
                {
                    Vector2 gVect = (stationNPC - obj);
                    float fGravity = (blackHoles[i].GetComponent<Rigidbody2D>().mass) / (Vector2.Distance(stationNPC, obj));
                    SpaceStations[s].GetComponent<Rigidbody2D>().AddForce(-gVect * fGravity);
                }
            }
        }

        for (int i = 0; i < SpaceStations.Count; i++)
        {
            if (Player == null)
            {
                break;
            }
            if (SpaceStations[i] == null)
            {
                SpaceStations.RemoveAt(i);
                break;
            }
            Vector2 obj = SpaceStations[i].transform.localPosition;
            for (int p = 1; p < SpaceStations.Count; p++)
            {
                if (SpaceStations[p] == null)
                {
                    break;
                }
                Vector2 stationNPC = SpaceStations[p].transform.localPosition;
                if (Vector2.Distance(stationNPC, obj) <= 30 && SpaceStations[p] != SpaceStations[i])
                {
                    Vector2 gVect = (stationNPC - obj);
                    float fGravity = (10000) / (Vector2.Distance(stationNPC, obj));
                    SpaceStations[p].GetComponent<Rigidbody2D>().AddForce(gVect * fGravity);
                }
            }
        }
    }

    private IEnumerator SpawnGameplayObjects()
    {
        formation = true;
        for (int i = objectsCounter; i < PresentAtOnce; i++)
        {
            objectsCounter++;
            yield return new WaitForSeconds(DelayBetweenSpawn /*+ Random.Range(0.5f, 1f)*/);
            int index = Random.Range(0, numberOfObjects);
            spaceCraft1.Add(Instantiate(GameplayObjects[index], new Vector3(Player.transform.position.x + Random.Range(-150f, 150f), -(CameraOrthoSize + 10), -1f), Quaternion.identity));
        }
        formation = false;
    }
    public IEnumerator SpawnAsteroidObjects()
    {
        formationAst = true;
        for (int i = objectsCounterAst; i < PresentAtOnceAst; i++)
        {
            objectsCounterAst++;
            yield return new WaitForSeconds(DelayBetweenSpawn /*+ Random.Range(0.5f, 1f)*/);
            list.Add(Instantiate(AsteroidObjects[0], new Vector3(Player.transform.position.x + Random.Range(-150f, 150f), -(CameraOrthoSize + 10), 2f), Quaternion.identity));

            /*tails.Add(list[objectsCounterTail].GetComponent<NPCasteroidBehaviour>().Tails[0]);
            tails2.Add(list[objectsCounterTail2].GetComponent<NPCasteroidBehaviour>().Tails[1]);
            tails3.Add(list[objectsCounterTail3].GetComponent<NPCasteroidBehaviour>().Tails[2]);
            
            objectsCounterTail++;
            objectsCounterTail2++;
            objectsCounterTail3++;;*/
        }
        formationAst = false;
    }

    public void action()
    {
        StartCoroutine(SpawnPlanetObjects());
        IEnumerator SpawnPlanetObjects()
        {
            formationPlanet = true;
            for (int i = objectsCounterPlanet; i < PresentAtOncePlanet; i++)
            {
                objectsCounterPlanet++;
                int index = Random.Range(0, numberOfObjectsPlanets);
                planets.Add(Instantiate(PlanetObjects[index], new Vector3(Player.transform.position.x + Random.Range(-150f, 150f), -(CameraOrthoSize + 13), -1f), Quaternion.identity));
                yield return new WaitForSeconds(DelayBetweenSpawn + Random.Range(2f, 4f));
            }
            formationPlanet = false;
        }
    }

    public void action_1()
    {
        StartCoroutine(SpawnPlanetObjects_1());
        IEnumerator SpawnPlanetObjects_1()
        {
            formationPlanet = true;
            for (int i = objectsCounterPlanet; i < PresentAtOncePlanet; i++)
            {
                objectsCounterPlanet++;
                int index = Random.Range(0, numberOfObjectsPlanets);
                planets.Add(Instantiate(PlanetObjects[index], new Vector3(Player.transform.position.x + Random.Range(190f, 270f), Random.Range(-50, 40), -1f), Quaternion.identity));
                yield return new WaitForSeconds(DelayBetweenSpawn + Random.Range(2f, 4f));
            }
            formationPlanet = false;
        }
    }

    public void action_2()
    {
        StartCoroutine(SpawnPlanetObjects_2());
        IEnumerator SpawnPlanetObjects_2()
        {
            formationPlanet = true;
            for (int i = objectsCounterPlanet; i < PresentAtOncePlanet; i++)
            {
                objectsCounterPlanet++;
                int index = Random.Range(0, numberOfObjectsPlanets);
                planets.Add(Instantiate(PlanetObjects[index], new Vector3(Player.transform.position.x - Random.Range(190f, 270f), Random.Range(-50, 40), -1f), Quaternion.identity));
                yield return new WaitForSeconds(DelayBetweenSpawn + Random.Range(2f, 4f));
            }
            formationPlanet = false;
        }
    }

    public IEnumerator SpawnBlackHoleObjects()
    {
        formationBlackHoles = true;
        if (Player.GetComponent<PlayerController>().gameStarted == true)
        {
            for (int i = objectsCounterBlackHoles; i < PresentAtOnceBlackHoles; i++)
            {
                yield return new WaitForSeconds(Random.Range(150f, 220f/*60 80*/));
                objectsCounterBlackHoles++;
                int index = Random.Range(0, numberOfObjectsBlackHole);
                float right = Random.Range(450f, 600f);
                float left = Random.Range(-600f, -450f);
                float rightSM = Random.Range(600f, 700f);
                float leftSM = Random.Range(-700f, -600f);
                float res;
                if (index == 3)
                {
                    Warning_BH.text = "Supermassive black hole is close!".ToString();
                    res = Random.value < 0.5f ? rightSM : leftSM;
                }
                else
                {
                    Warning_BH.text = "The black hole is close...".ToString();
                    res = Random.value < 0.5f ? right : left;
                }
                blackHoles.Add(Instantiate(BlackHolesObjects[index], new Vector3(Player.transform.position.x + res, -50, -1f), Quaternion.identity));
                Warning_BH.gameObject.SetActive(true);
                StartCoroutine(WarningText());
            }
        }
        formationBlackHoles = false;
    }

    public IEnumerator SpawnSpaceStations()
    {
        formationStation = true;
        if (Player.GetComponent<PlayerController>().gameStarted == true)
        {
            for (int i = objectsCounterSpaceStations; i < PresentAtOnceSpaceStations; i++)
            {

                yield return new WaitForSeconds(Random.Range(120f, 170f/*50 100*/));
                objectsCounterSpaceStations++;
                float right = Random.Range(450f, 600f);
                float left = Random.Range(-600f, -450f);
                float res = Random.value < 0.5f ? right : left;
                //float close = 46f;
                float far = -30f;
                //float res_2 = Random.value < 0.5f ? close : far;
                if (indexStation == 0)
                {
                    Warning_BH.text = "The first-level space station is approaching!".ToString();
                    far = -30f;
                }
                else if (indexStation == 1)
                {
                    Warning_BH.text = "The second-level space station is approaching!".ToString();
                    far = -60f;
                }
                else if (indexStation == 2)
                {
                    Warning_BH.text = "The third-level space station is approaching!".ToString();
                    far = -90f;
                }
                SpaceStations.Add(Instantiate(SpaceStationsObjects[indexStation], new Vector3(Player.transform.position.x + res, 0f, far), Quaternion.Euler(Random.Range(50f, 100f), Random.Range(-70f, 70f), -60f)));
                if (indexStation == 2)
                {
                    indexStation = 0;
                }
                else
                    indexStation++;
                Warning_BH.gameObject.SetActive(true);
                StartCoroutine(WarningText());

                yield return new WaitForSeconds(Random.Range(6f, 15f));
                int index = Random.Range(0, numberOfObjectsBlackHole-1);
                float rightBH = Random.Range(450f, 600f);
                float leftBH = Random.Range(-600f, -450f);
                float rightSM = Random.Range(600f, 700f);
                float leftSM = Random.Range(-700f, -600f);
                float resBH;
                if (index == 3)
                {
                    Warning_BH.text = "Supermassive black hole is close!".ToString();
                    resBH = Random.value < 0.5f ? rightSM : leftSM;
                }
                else
                {
                    Warning_BH.text = "The black hole is close...".ToString();
                    resBH = Random.value < 0.5f ? rightBH : leftBH;
                }
                blackHoles.Add(Instantiate(BlackHolesObjects[index], new Vector3(Player.transform.position.x + resBH, -50, -1f), Quaternion.identity));
                Warning_BH.gameObject.SetActive(true);
                StartCoroutine(WarningText());
            }
        }
        formationStation = false;
    }
    public IEnumerator WarningText()
    {
        yield return new WaitForSeconds(6f);
        Warning_BH.gameObject.SetActive(false);
    }
}
