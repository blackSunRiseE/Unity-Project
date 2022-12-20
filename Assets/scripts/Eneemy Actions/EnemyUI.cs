using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] BaseEnemyAI enemy;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = enemy.maxHealth;
        healthBar.value = enemy.maxHealth;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
