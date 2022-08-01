using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public Vector3 pos;
    public int phase = 1;

    [SerializeField] Sprite[] phaseSprites;

    private SpriteRenderer sr;
    private int order;
    private GameObject player;
    

    void Start()
    {
        init();
        InvokeRepeating("Grow", 8, 8);
    }

    public void init()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(player != null)
        {
            order = player.transform.position.y >= transform.position.y + 0.15 ? 2 : 0;
            if(order != sr.sortingOrder)
            {
                sr.sortingOrder = order;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Entered Plant Range");
            player = collision.gameObject;
            if(phase == phaseSprites.Length)
            {
                player.GetComponent<PlayerInventory>().removePlant(pos);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (phase == phaseSprites.Length)
        {
            player.GetComponent<PlayerInventory>().removePlant(pos);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Exited Plant Range");
            player = null;
        }
    }

    public void updatePhase()
    {
        Debug.Log("Update Phase");
        init();
        if(phase == phaseSprites.Length)
        {
            Debug.Log("Destroy Phase");
            return;
        }
        phase++;
        sr.sprite = phaseSprites[phase - 1];
    }

    void Grow()
    {
        updatePhase();
    }
}
