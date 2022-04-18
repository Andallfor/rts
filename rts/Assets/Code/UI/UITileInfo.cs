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
    public TextMeshProUGUI title, healthCount, defenseCount, shieldCount;
    public Slider healthSlider, defenseSlider, shieldSlider;
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
    }

    public void closeMenu() {
        tilePanel.anchoredPosition = new Vector2(180, 45);
    }
}
