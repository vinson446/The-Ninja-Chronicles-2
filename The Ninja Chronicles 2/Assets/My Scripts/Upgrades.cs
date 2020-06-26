using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public GameObject upgradePower;
    public GameObject upgradeAttackSpeed;
    public GameObject upgradeRange;

    public Text powerPointsText;
    public Text attackSpeedPointsText;
    public Text rangePointsText;

    // TODO- change points to visual indicator (polish)
    int pPoints;
    int asPoints;
    int rPoints;

    int numUpgrades = 0;

    // Start is called before the first frame update
    void Start()
    {
        pPoints = 0;
        asPoints = 0;
        rPoints = 0;

        upgradePower.SetActive(false);
        upgradeAttackSpeed.SetActive(false);
        upgradeRange.SetActive(false);
    }

    public void DisplayUpgrades()
    {
        Time.timeScale = 0;

        numUpgrades += 1;

        upgradePower.SetActive(true);
        upgradeAttackSpeed.SetActive(true);
        upgradeRange.SetActive(true);
    }

    public void PowerUpgrade()
    {
        pPoints += 1;
        numUpgrades -= 1;
        powerPointsText.text = pPoints.ToString();
    }

    public void AttackSpeedUpgrade()
    {
        asPoints += 1;
        numUpgrades -= 1;
        attackSpeedPointsText.text = asPoints.ToString();
    }

    public void RangeUpgrade()
    {
        rPoints += 1;
        numUpgrades -= 1;
        rangePointsText.text = rPoints.ToString();
    }

    public void TurnOffUpgrades()
    {
        if (numUpgrades <= 0)
        {
            Time.timeScale = 1;

            upgradePower.SetActive(false);
            upgradeAttackSpeed.SetActive(false);
            upgradeRange.SetActive(false);
        }
    }

}
