using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITileInfo : MonoBehaviour
{
    public hex currentlySelected;
    public bool isOpen = false;

    public RectTransform tilePanel;
    public TextMeshProUGUI title, healthCount, defenseCount, shieldCount, gold, iron, wood, food;
    public Slider healthSlider, defenseSlider, shieldSlider;
    public RawImage teamAffiliationImage;
    public GameObject actionParent, actionPrefab;
    private List<GameObject> displayedActions = new List<GameObject>();
    public RawImage display;

    // TODO: figure out a good way to implement multiple types of actions
    // progress bar of in progress actions
    // actions list
    // desc, etc
    // dont worry about getting every single one in just yet, work instead of making a modular system
    // so you can add more actions as needed
    // probably each action will be a class that we just call?

    public void openMenu(hex h) {
        tilePanel.anchoredPosition = new Vector2(-245, 45);

        title.text = h.name;
        healthCount.text = h.health.ToString();
        defenseCount.text = h.defense.ToString();
        shieldCount.text = h.shield.ToString();

        healthSlider.value = h.health / h.maxHealth;
        defenseSlider.value = h.defense / h.maxDefense;
        shieldSlider.value = h.shield / h.maxShield;

        if (h.team == teamId.none) teamAffiliationImage.texture = Resources.Load("UI/images/uipack/grey_circle") as Texture2D;
        else if (h.team == teamId.red) teamAffiliationImage.texture = Resources.Load("UI/images/uipack/red_circle") as Texture2D;
        else if (h.team == teamId.blue) teamAffiliationImage.texture = Resources.Load("UI/images/uipack/blue_circle") as Texture2D;

        gold.text = h.inventory.gold.ToString();
        wood.text = h.inventory.wood.ToString();
        iron.text = h.inventory.iron.ToString();
        food.text = h.inventory.food.ToString();

        currentlySelected = h;

        // load in possible actions
        int index = 0;
        foreach (ITileAction ta in h.possibleActions) {
            addNewAction(ta, new Vector2(-88, -25 + index * 40));
            index++;
        }
    }

    private GameObject addNewAction(ITileAction ta, Vector2 position) {
        GameObject go = GameObject.Instantiate(actionPrefab, actionParent.transform);
        go.GetComponent<RectTransform>().anchoredPosition = position;
        UITileAction uta = go.GetComponent<UITileAction>();
        uta.button.interactable = ta.canRunAction(currentlySelected);
        uta.gold.text = ta.cost.gold.ToString();
        uta.iron.text = ta.cost.iron.ToString();
        uta.wood.text = ta.cost.wood.ToString();
        uta.food.text = ta.cost.food.ToString();

        uta.buttonName.text = ta.displayName;

        uta.button.onClick.RemoveAllListeners();
        uta.button.onClick.AddListener(() => {
            ta.action(currentlySelected, new object[0]);
            mouseController.closeTileInfoMenu();
        });

        displayedActions.Add(go);

        return go;
    }

    public void closeMenu() {
        tilePanel.anchoredPosition = new Vector2(180, 45);
        foreach (GameObject go in displayedActions) {
            Destroy(go);
        }
    }
}
