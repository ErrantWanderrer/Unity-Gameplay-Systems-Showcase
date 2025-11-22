using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class NPCasteroidBehaviour : MonoBehaviour {

    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    int numOfVerts;
    public Material[] materials = new Material[12];
    public GameObject[] Tails;
    public GameObject[] ChildTails = new GameObject[2];
    public float radius;
    public int level;
    public GameObject DeathAnim1, DeathAnim2;
    public GameObject Asteroid_Part;
    public AudioClip[] Sound = new AudioClip[2];

    // Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/texture.jpg", typeof(Texture2D));
    //Material material;
    Vector2[] Points;
    private Rigidbody2D rb;

    void Awake()
    {
        if (PlayerPrefs.GetInt("Tips") == 0)
        {
            SetLevel();
        }
        else
        {
            level = 0;
        }
        SetTexure();
        radius = Random.Range(0.05f, 0.3f);
        numOfVerts = Random.Range(20, 28);
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new Vector3[numOfVerts + 1];
        triangles = new int[3 * numOfVerts];
        uv = new Vector2[numOfVerts + 1];
        //gameObject.GetComponent<MeshRenderer>().material = material;
        Points = new Vector2[numOfVerts];
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        MakeMeshData();
        CreateMesh();
        SetCollider();
        SetTail();
        float positive = Random.Range(0.6f, 2f);
        float negative = Random.Range(-2f, 0.6f);
        float res = Random.value < 0.5f ? positive : negative;
        rb.AddTorque(100f*res);
        rb.AddForce(Vector2.up * 800f);
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sounds");
    }

    void SetLevel()
    {
        //int bottomEdge;
        //int topEdge;
        int playerLevel;

        if (PlayerPrefs.HasKey("PlayerLevel"))
            playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        else
            playerLevel = 0;

        /*if (playerLevel > 2)
            bottomEdge = playerLevel - 2;
        else
            bottomEdge = 0;

        if (playerLevel < 8)
            topEdge = playerLevel + 2;
        else
            topEdge = 10;*/

        // Добавить нормальное распределение

        level = Random.Range(0, 12);
    }

    void MakeMeshData()
    {
        vertices[0] = new Vector3(0f, 0f, 0f);

        float angle = 0;
        for (int i = 1; i <= numOfVerts; i++)
        {
            float randScale = Random.Range(0.8f, 1.2f);
            vertices[i] = new Vector3(Mathf.Cos(angle) * (radius + randScale), Mathf.Sin(angle) * (radius + randScale), 0f);
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

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        CalculateUVs();
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    void CalculateUVs()
    {
        for (int i = 0; i <= numOfVerts; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
    }

    void SetCollider()
    {
        gameObject.AddComponent<PolygonCollider2D>();
        for (int i = 0; i < numOfVerts; i++)
        {
            Points[i] = vertices[i + 1];
        }
        gameObject.GetComponent<PolygonCollider2D>().points = Points;
    }

    void SetTexure()
    {
        for (int i=0; i<=11; i++)
        {
            if (level == i)
            {
                gameObject.GetComponent<MeshRenderer>().material = materials[i];
            }
        }
        //material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/ast_texture" + level + ".mat", typeof(Material));
    }

    void SetTail()
    {
        for (int i = 0; i <= 11; i++)
        {
            if (level == i)
            {
                Vector3 vec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -8f);
                Instantiate((Tails[i * 2]), vec, Quaternion.identity).transform.parent = gameObject.transform;
                Instantiate((Tails[i * 2 + 1]), vec, Quaternion.identity).transform.parent = gameObject.transform;
                ChildTails[0] = gameObject.transform.GetChild(0).gameObject;
                ChildTails[1] = gameObject.transform.GetChild(1).gameObject;
            }
        }
    }

    /*Object[] GetSprites(string fileName)
    {
        Object[] sprites = Resources.LoadAll(fileName);
        return sprites;
    }*/

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerController>().playerLevel >= level || other.gameObject.GetComponent<PlayerController>().current_Shield_Health > 0)
            {
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().simulated = false;
                var main_1 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().emission;
                var main_2 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().emission;
                var main_3 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().main;
                var main_4 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().main;
                var main_5 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().main;
                var main_6 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().main;
                main_3.simulationSpeed = 0.3f;
                main_4.simulationSpeed = 0.3f;
                main_5.gravityModifier = -5f;
                main_6.gravityModifier = -5f;
                main_1.enabled = false;
                //main_1.simulationSpeed = 5f;
                main_2.enabled = false;
                //main_2.simulationSpeed = 5f;
                SpawnDestroyedPart();
                GetComponent<AudioSource>().clip = Sound[0];
                GetComponent<AudioSource>().Play();
                Destroy(gameObject, 2f);
            }
            else
            {
                GetComponent<AudioSource>().clip = Sound[1];
                GetComponent<AudioSource>().Play();
            }
        }
        if (other.gameObject.tag == "BlackHole")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plasma")
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            var main_1 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().emission;
            var main_2 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().emission;
            var main_3 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().main;
            var main_4 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().main;
            var main_5 = ChildTails[0].gameObject.GetComponent<ParticleSystem>().main;
            var main_6 = ChildTails[1].gameObject.GetComponent<ParticleSystem>().main;
            main_3.simulationSpeed = 0.3f;
            main_4.simulationSpeed = 0.3f;
            main_5.gravityModifier = -5f;
            main_6.gravityModifier = -5f;
            main_1.enabled = false;
            main_2.enabled = false;
            Instantiate(DeathAnim1, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 2f);
        }
    }

    void OnDestroy()
    {
        if (GameObject.Find("GameObjects_Spawner") != null)
        {
            GameObject.Find("GameObjects_Spawner").GetComponent<GameObjectsCreator>().objectsCounterAst--;
        }
    }

    void SpawnDust()
    {
        /*Vector3 i_hat = new Vector3(0f, 1, 0f);
        Vector2 vel = rb.velocity;

        float angle = vel[1] / Mathf.Sqrt(Mathf.Pow(vel[0], 2) + Mathf.Pow(vel[1], 2));
        angle = Mathf.Acos(angle);
        // leadingValue = vec_prod([vx[1] - vx[2], vy[1] - vy[2]], [vx[3] - vx[2], vy[3] - vy[2]])

        bool rightTree;
        float vec_prod = i_hat[0] * vel[1] - i_hat[1] * vel[0];
        if (vec_prod >= 0)
            rightTree = true;
        else
            rightTree = false;

        if (rightTree)
            Instantiate(DeathAnim);*/
        Instantiate(DeathAnim1, gameObject.transform);
        Instantiate(DeathAnim2, gameObject.transform);
        ParticleSystem ps1 = DeathAnim1.GetComponent<ParticleSystem>();
        ParticleSystem ps2 = DeathAnim1.GetComponent<ParticleSystem>();
        var vel1 = ps1.velocityOverLifetime;
        var vel2 = ps1.velocityOverLifetime;
        vel1.x = gameObject.GetComponent<Rigidbody2D>().velocity.x * 10;
        vel1.y = gameObject.GetComponent<Rigidbody2D>().velocity.y * 10;
        vel2.x = gameObject.GetComponent<Rigidbody2D>().velocity.x * 10;
        vel2.y = gameObject.GetComponent<Rigidbody2D>().velocity.y * 10;

        //Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.x + " " + gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    void SpawnDestroyedPart()
    {
        GameObject AstPart1, AstPart2, AstPart3, AstRand;
        /*GameObject[] AstRand = new GameObject[3];
        for (int i = 0; i<=2; i++)
        {
            AstRand[i] = Asteroid_Part;
        }*/

        AstPart1 = Instantiate(Asteroid_Part, gameObject.transform.position + new Vector3(-1f, 1f, 0f), gameObject.transform.rotation);
        Rigidbody2D rb1 = AstPart1.gameObject.GetComponent<Rigidbody2D>();
        rb1.AddForce(Vector2.left * 50f * Random.Range(2f, 3f));
        AstPart1.GetComponent<NPC_Part_Behaviour>().level = level;
        AstPart1.GetComponent<NPC_Part_Behaviour>().SetTexure();

        AstPart2 = Instantiate(Asteroid_Part, gameObject.transform.position + new Vector3(1f, 1f, 0f), gameObject.transform.rotation);
        Rigidbody2D rb2 = AstPart2.gameObject.GetComponent<Rigidbody2D>();
        rb2.AddForce(Vector2.right * 50f * Random.Range(2f, 3f));
        AstPart2.GetComponent<NPC_Part_Behaviour>().level = level;
        AstPart2.GetComponent<NPC_Part_Behaviour>().SetTexure();

        AstPart3 = Instantiate(Asteroid_Part, gameObject.transform.position + new Vector3(0f, 0f, 0f), gameObject.transform.rotation);
        AstPart3.GetComponent<NPC_Part_Behaviour>().level = level;
        AstPart3.GetComponent<NPC_Part_Behaviour>().SetTexure();

        int Rand = Random.Range(0, 4);
        if (Rand > 0)
        {
            for (int j = 0; j <= Rand; j++)
            {
                AstRand = Instantiate(Asteroid_Part, gameObject.transform.position + new Vector3(Random.Range(-0.9f, 0.9f), Random.Range(-0.9f, 0.9f), 0f), gameObject.transform.rotation);
                AstRand.GetComponent<NPC_Part_Behaviour>().level = level;
                AstRand.GetComponent<NPC_Part_Behaviour>().SetTexure();
            }
        }
    }
}
