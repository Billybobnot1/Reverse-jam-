using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    
    public static ItemManager instance;

    public List<Item> items;

    public Item currentItem;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        if (items.Count > 0) {
            currentItem = items[0];
        }
    }

    public void SetItem(Item item) {
        currentItem = item;
    }
}
