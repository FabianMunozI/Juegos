using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] public InventorySlot[] inventorySlots;
    [SerializeField] public InventorySlot[] hotbarSlots;

    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    public static GameObject Jugador;
    private InventoryItem lastItem;

    void Start(){
        Jugador = GameObject.Find("Player");
        //Invoke("Usar",2f);
    }

    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener( delegate { SpawnInventoryItem(); } );

        LoadInventory();

    }

    void Update()
    {
        SpawnHBIteminHand();

        lastItem = hotbarSlots[0].myItem;


        if(carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void SpawnHBIteminHand()
    {
        if(hotbarSlots[0].myItem != null)
        {
            if(hotbarSlots[0].myItem != lastItem && Jugador.GetComponent<PickUpObjects>().PickedObject == null)
            {
                GameObject objeto = Instantiate(hotbarSlots[0].myItem.myItem.equipmentPrefab, new Vector3(0,1f,0), Quaternion.identity);
                Jugador.GetComponent<PickUpObjects>().PickedObject = objeto;
                objeto.transform.SetParent(Jugador.transform.GetChild(1));
                objeto.transform.position = Jugador.transform.GetChild(1).position;

                objeto.GetComponent<Rigidbody>().useGravity = false;
                objeto.GetComponent<Rigidbody>().isKinematic = true;
                objeto.GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if(carriedItem != null)
        {
            //if( item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        //if(item.activeSlot.myTag != SlotTag.None)
        // { EquipEquipment(item.activeSlot.myTag, null); }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }


    public void SaveInventory()
    {
        string path = Application.persistentDataPath + "/Inventario.sav";
        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
            using (var fw = new StreamWriter(fs))
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if(inventorySlots[i].myItem != null)
                    {
                        for(int j = 0; j < items.Length; j++)
                        {
                            if (inventorySlots[i].myItem.myItem == items[j])
                            {
                                fw.WriteLine(j);
                                break;
                            }
                        }
                    } else {
                        fw.WriteLine("-1");
                    }

                }
                fw.WriteLine("");

                if(hotbarSlots[0].myItem != null)
                {
                    for(int j = 0; j < items.Length; j++)
                    {
                        if (hotbarSlots[0].myItem.myItem == items[j])
                        {
                            fw.WriteLine(j);
                            break;
                        }
                    }
                } else {
                    fw.WriteLine("-1");
                }

                fw.Flush();
            }
        }

    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/Inventario.sav";

        Debug.Log("Cargando Inventario:");

        if(File.Exists(path))
        {
            using (StreamReader reader = File.OpenText(path))
            {
                string line;
                int actual_slot = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        int item_n = int.Parse(line);
                        if (item_n != -1)
                        {
                            if (actual_slot < 18)
                                Instantiate(itemPrefab, inventorySlots[actual_slot].transform).Initialize(items[item_n], inventorySlots[actual_slot]);
                            else if (actual_slot == 19)
                                Instantiate(itemPrefab, hotbarSlots[0].transform).Initialize(items[item_n], hotbarSlots[0]);
                        }
                    }



                    actual_slot ++;
                }
            }
        }

    }



    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item;
        if(_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Check if the slot is empty
            if(inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }

    public void SpawnHotBarItem(Item item = null)
    {
        if(item == null)
        {
            item = items[0];
        }

        if(hotbarSlots[0].myItem == null)
        {
            Instantiate(itemPrefab, hotbarSlots[0].transform).Initialize(item, hotbarSlots[0]);
        }

    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }

    public void SoltarObjeto()
    {
        //hotbarSlots[0].SetItem(new InventoryItem());
        DestroyImmediate(hotbarSlots[0].transform.GetChild(0).gameObject);
    }

    void onDestroy()
    {
        Debug.Log("Inventario Destruido.");
    }
}