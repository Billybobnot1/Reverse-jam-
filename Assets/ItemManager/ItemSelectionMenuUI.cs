using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemSelectionMenuUI : MonoBehaviour
{
    public GameObject optionPrefab;

    public Transform prevItem;
    public Transform selectedItem;

    private List<Item> selectedItems = new List<Item>(); // To store the selected items.

    private void Start()
    {
        List<Item> items = ItemHoldManager.items;

        // print(selectedItems);
        // print(items);
        // Randomly select 3 items without duplicates.
        System.Random random = new System.Random();
        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < 3 && selectedIndices.Count< items.Count)
        {
            int randomIndex = random.Next(items.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                selectedItems.Add(items[randomIndex]);
            }
        }

        // Populate UI with selected items.
        foreach (Item c in selectedItems)
        {
            GameObject option = Instantiate(optionPrefab, transform);
            Button button = option.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                ItemManager.instance.SetItem(c);
                ItemHoldManager.SetItem(c);
                if (selectedItem != null)
                {
                    prevItem = selectedItem;
                }

                selectedItem = option.transform;
            });

            Text text = option.GetComponentInChildren<Text>();
            text.text = c.name;

            Image image = option.GetComponentInChildren<Image>();
            image.sprite = c.image;
        }
    }

    private void Update()
    {
        if (selectedItem != null)
        {
            selectedItem.localScale = Vector3.Lerp(selectedItem.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10);
        }

        if (prevItem != null)
        {
            prevItem.localScale = Vector3.Lerp(prevItem.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10);
        }
    }
}
