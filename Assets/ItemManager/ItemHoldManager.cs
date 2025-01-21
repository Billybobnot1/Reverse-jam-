using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemHoldManager : MonoBehaviour {
    
    public static ItemHoldManager instance;

    public static List<Item> items = new List<Item>();

    public static Item currentItem;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
    }

    public static void SetItem(Item item) {
        currentItem = item;
    }
    public static void AddItem(){
        items.Add(currentItem);
    }
}
