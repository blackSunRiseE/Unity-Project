using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTarget : MonoBehaviour
{
    
    [SerializeField] public float maxHealth = 150f;
    [SerializeField] public float health;
    [SerializeField] GameObject keyPrefab; 
    [HideInInspector] public static int keys = 0;
    [HideInInspector] public static int maxKeys = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        GameObject.FindGameObjectWithTag("Health Bar").GetComponent<HealthBar>().SetHealth((int)health);
        if (health <= 0f)
        {
            health = 0;
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<Main>().gameOver = true;
        }
    }
    public void Heal(float healCount)
    {
        health += healCount;
        GameObject.FindGameObjectWithTag("Health Bar").GetComponent<HealthBar>().SetHealth((int)health);
    }
    public void AddKey()
    {
        var parent = GameObject.FindGameObjectWithTag("Key");
        Instantiate(keyPrefab, new Vector3(parent.transform.position.x + keys * 100, parent.transform.position.y,0),Quaternion.identity, parent.transform);
        keys++;
    }

}
