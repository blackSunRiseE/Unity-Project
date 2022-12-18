using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    PlayerTarget playerTarget;
    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTarget>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerTarget.maxHealth;
        healthBar.value = playerTarget.health;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
