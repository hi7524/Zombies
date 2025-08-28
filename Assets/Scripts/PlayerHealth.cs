using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private AudioSource audioSource;
    private Animator animator;

    private PlayerMovement movement;
    private PlayerShooter shooter;

    public UIManager uiManager;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = Health / MaxHealth;

        movement.enabled = true;
        shooter.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnDamage(10f, Vector3.zero, Vector3.zero);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead) return;

        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = Health / MaxHealth;
        audioSource.PlayOneShot(hitClip);
    }

    public void Heal(int amount)
    {
        Debug.Log("Heal()");
        Health = Mathf.Min(Health + amount, MaxHealth);
        healthSlider.value = Health / MaxHealth;
    }

    protected override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        animator.SetTrigger(AnimIds.DieHash);
        audioSource.PlayOneShot(deathClip);

        movement.enabled = false;
        shooter.enabled = false;
        uiManager.SetActiveGameOverUI(true);
    }
}