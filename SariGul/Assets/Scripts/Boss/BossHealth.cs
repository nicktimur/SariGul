using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

	public int health = 500;

	public GameObject deathEffect;
	private HealthBar healthBar;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private Behaviour[] components;
    [SerializeField] private AudioClip dieSound;
    private LevelLoader levelLoader;

    public bool isInvulnerable = false;

    private void Start()
    {

        healthBar = GameObject.Find("BossHealthBar").GetComponent<HealthBar>();
        GameObject.Find("BossHealthBar").GetComponent<CanvasGroup>().alpha = 1;


        healthBar.setMaxHealth(health);
    }

    public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

        SoundManager.instance.PlaySound(hurtSound);
        health -= damage;
		healthBar.setHealth(health);

		if (health <= 500)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
			this.GetComponent<BossWeapon>().attackRange = 2f;

        }

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
        GetComponent<Animator>().SetBool("Died", true);
        SoundManager.instance.PlaySound(dieSound);
        GameObject.FindGameObjectWithTag("BossHealthBar").gameObject.SetActive(false);

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        levelLoader.LoadNextLevel();

        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Main Camera"));

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
