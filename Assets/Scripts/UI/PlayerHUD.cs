/// <summary>
/// This script handles the behavior of the player HUD.
/// </summary>
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public static PlayerHUD Instance;

    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Image staminaFill;
    [SerializeField] private Color sprintEnabledColor;
    [SerializeField] private Color sprintDisabledColor;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject hotbar;
    private HorizontalLayoutGroup hotbarLayout;
    [SerializeField] private GameObject repairToolHotbarPanel;
    [SerializeField] private GameObject grenadeHotbarPanel;
    [SerializeField] private GameObject healthPotionHotbarPanel;

    [SerializeField] private Color equippedColor;
    [SerializeField] private Color unequippedColor;

    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<Image> panelImages;
    [SerializeField] private List<TextMeshProUGUI> indices;
    [SerializeField] private List<TextMeshProUGUI> counts;

    [SerializeField] private int[] spacings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("Hotbar: " + hotbar);
        hotbarLayout = hotbar.GetComponent<HorizontalLayoutGroup>();
        Debug.Log("Hotbar layout: " + hotbarLayout);
        hotbarLayout.spacing = spacings[1];
    }

    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = Player.Instance.Health.MaxHealth;
        healthbar.value = Player.Instance.Health.CurrentHealth;
        staminaBar.minValue = 0;
        staminaBar.maxValue = Player.Instance.Stamina.MaxStamina;
        staminaBar.value = Player.Instance.Stamina.CurrentStamina;
        staminaFill.color = sprintEnabledColor;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = Player.Instance.Health.CurrentHealth;
        healthText.text = Player.Instance.Health.CurrentHealth.ToString() + "/" + Player.Instance.Health.MaxHealth.ToString();
        staminaBar.value = Player.Instance.Stamina.CurrentStamina;
        staminaFill.color = (Player.Instance.Stamina.SprintDisabled ? sprintDisabledColor : sprintEnabledColor);

        for (int i = 0; i < panels.Count; i++)
        {
            panelImages[i].color = (i == Player.Instance.Inventory.EquippedIndex ? equippedColor : unequippedColor);
            indices[i].fontStyle = (i == Player.Instance.Inventory.EquippedIndex ? FontStyles.Bold : FontStyles.Normal);
        }

        moneyText.text = "$" + Player.Instance.Bank.Amount.ToString();
    }

    public void IncrementItem(int index)
    {
        counts[index].text = (int.Parse(counts[index].text) + 1).ToString();
    }

    public void AddItem(PlayerInventory.Item item)
    {
        GameObject newPanel = null;
        switch (item)
        {
            case PlayerInventory.Item.REPAIR_TOOL:
                newPanel = Instantiate(repairToolHotbarPanel, hotbar.transform);
                break;
            case PlayerInventory.Item.GRENADE:
                newPanel = Instantiate(grenadeHotbarPanel, hotbar.transform);
                break;
            case PlayerInventory.Item.HEALTH_POTION:
                newPanel = Instantiate(healthPotionHotbarPanel, hotbar.transform);
                break;
        }
        panels.Add(newPanel);
        panelImages.Add(newPanel.GetComponent<Image>());
        TextMeshProUGUI index = newPanel.transform.Find("HotbarIndex").GetComponent<TextMeshProUGUI>();
        index.text = panels.Count.ToString();
        indices.Add(index);
        TextMeshProUGUI count = newPanel.transform.Find("Count").GetComponent<TextMeshProUGUI>();
        count.text = "1";
        counts.Add(count);
        Debug.Log(hotbarLayout);
        hotbarLayout.spacing = spacings[panels.Count - 1];
    }

    public void DecrementItem(int index)
    {
        counts[index].text = (int.Parse(counts[index].text) - 1).ToString();
    }

    public void RemoveItem(int index)
    {
        GameObject removedPanel = panels[index];
        panels.RemoveAt(index);
        panelImages.RemoveAt(index);
        indices.RemoveAt(index);
        counts.RemoveAt(index);
        Destroy(removedPanel);
        for (int i = index; i < panels.Count; i++)
        {
            indices[i].text = (i + 1).ToString();
        }
        hotbarLayout.spacing = spacings[panels.Count - 1];
    }
}
