using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Text loveCountUI;
    [SerializeField] GameObject[] inventory;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] Tilemap groundTiles;
    [SerializeField] Tilemap highlightTiles;
    [SerializeField] Tile highlightTile;
    [SerializeField] AudioClip[] sfx;

    private int loveCount;
    private int inventoryMaxCapacity;
    private int activeItem = 0;
    private Vector3Int facingTile;
    private Vector3Int standingTile;
    private Vector3Int prevTile;
    private Animator ar;
    private AudioSource asrc;
    private enum groundType {dirt}
    private Dictionary<Vector3Int, groundType> workedOn = new Dictionary<Vector3Int, groundType>();
    private Dictionary<Vector3Int, GameObject> planted = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        inventoryMaxCapacity = inventory.Length;
        updateInventoryUI();
        toggleHighlight(true);
        updateFacingTile(0,0);
        ar = GetComponent<Animator>();
        asrc = GetComponent<AudioSource>();
    }

    void addItem(GameObject newItem)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] = null)
            {
                inventory[i] = newItem;
                return;
            }
        }
    }

    void updateInventoryUI()
    {
        GameObject inventoryItem;
        Image icon;
        for (int i = 0; i < inventoryMaxCapacity; i++)
        {
            inventoryItem = inventoryUI.transform.GetChild(i).gameObject;
            icon = inventoryItem.GetComponentsInChildren<Image>()[2];
            if (inventory[i] != null)
            {
                icon.sprite = inventory[i].GetComponent<Image>().sprite;
                icon.color = new Color(255,255,255,255);
            } else
            {
                icon.color = new Color(255, 255, 255, 0);
            }
        }
    }

    public void chooseInventoryItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            toggleHighlight(false);
            if(context.ReadValue<Vector2>().y == -120)
            {
                activeItem = (activeItem + 1) % inventoryMaxCapacity;
            }
            else if(context.ReadValue<Vector2>().y == 120)
            {
                activeItem = (activeItem - 1);
                if(activeItem < 0)
                    activeItem = inventoryMaxCapacity-1;
            }
            toggleHighlight(true);
            onSelect();
        }
    }

    private void toggleHighlight(bool isEnabled)
    {
        GameObject inventoryItem = inventoryUI.transform.GetChild(activeItem).gameObject;
        Image highlight = inventoryItem.GetComponentsInChildren<Image>()[0];
        if (isEnabled)
        {
            highlight.color = new Color(255, 255, 255, 255);
        } else
        {
            highlight.color = new Color(255, 255, 255, 0);
        }
    }

    private void onSelect()
    {
        //On select
    }

    public void onUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject inventoryItem = inventory[activeItem];
            if (inventoryItem != null)
            {
                Tool toolData = inventoryItem.GetComponent<Tool>();
                if (toolData != null)
                {
                    Debug.Log("Tool: " + toolData.tool);
                    toolData.init();
                    if (toolData.tool == Tool.toolType.Hoe)
                    {
                        if (!workedOn.ContainsKey(facingTile))
                        {
                            ar.SetTrigger("Hoe");
                            asrc.PlayOneShot(sfx[0]);
                            toolData.onUse(groundTiles, facingTile);
                            workedOn.Add(facingTile, groundType.dirt);
                        }
                    } else if (toolData.tool == Tool.toolType.Watering)
                    {
                        if (planted.ContainsKey(facingTile))
                        {
                            if (planted[facingTile].GetComponent<PlantInteraction>().phase != 5)
                            {
                                ar.SetTrigger("Water");
                                asrc.PlayOneShot(sfx[1]);
                                planted[facingTile].GetComponent<PlantInteraction>().updatePhase();
                            }
                        }
                    }
                    return;
                }

                Seed seedData = inventoryItem.GetComponent<Seed>();
                if(seedData != null)
                {
                    if (workedOn.ContainsKey(facingTile))
                    {
                        if (workedOn[facingTile] == groundType.dirt)
                        {
                            if (!planted.ContainsKey(facingTile))
                            {
                                asrc.PlayOneShot(sfx[2]);
                                Vector3 pos = groundTiles.CellToWorld(facingTile);
                                planted.Add(facingTile, seedData.useSeed(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z)));
                                planted[facingTile].GetComponent<PlantInteraction>().pos = facingTile;
                            }
                        }
                    }
                }
            }
        }
    }

    public void updateFacingTile(float hDir, float vDir)
    {
        Vector3Int currentPos = groundTiles.WorldToCell(transform.position);
        highlightTiles.SetTile(prevTile, null);
        standingTile = currentPos;
        facingTile = currentPos;
        
        if(vDir != 0)
        {
            facingTile.y = vDir > 0 ? (currentPos.y + 1) : (currentPos.y - 1);
        } else if(hDir != 0)
        {
            facingTile.x = hDir > 0 ? (currentPos.x + 1) : (currentPos.x - 1);
        } else
        {
            return;
        }

        prevTile = facingTile;
        highlightTiles.SetTile(facingTile, highlightTile);
    }

    public void removePlant(Vector3 pos)
    {
        asrc.PlayOneShot(sfx[3]);
        loveCount++;
        loveCountUI.text = loveCount.ToString();
        planted.Remove(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z));
    }

    public void onClick(int selectedIndex)
    {
        Application.Quit();
    }
}
