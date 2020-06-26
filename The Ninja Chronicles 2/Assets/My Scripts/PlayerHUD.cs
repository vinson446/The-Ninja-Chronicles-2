using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider expSlider;
    public Text levelText;

    Upgrades upgrades;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        upgrades = FindObjectOfType<Upgrades>();

        healthSlider.maxValue = player.maxHP;
        healthSlider.value = player.currentHP;

        expSlider.maxValue = player.maxExperience;
        expSlider.value = player.experience;

        manaSlider.maxValue = player.maxMana;
        manaSlider.value = player.currentMana;

        levelText.text = "Level " + player.level.ToString();
    }

    public void UpdateExperience()
    {
        expSlider.value = player.experience;
    }

    public void UpdateLevel()
    {
        levelText.text = "Level " + player.level.ToString();
        expSlider.maxValue = player.maxExperience;
    }

    public void UpdateHP()
    {
        healthSlider.value = player.currentHP;
    }

    public void UpdateMana()
    {
        manaSlider.value = player.currentMana;
    }
}
