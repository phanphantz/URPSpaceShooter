using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spaceship : MonoBehaviour
{
   public enum State
   {
      Normal, Shield, Cloak
   }

   public State state;
   public float stateDuration;

   public Bullet bulletPrefab;
   public Transform bulletFireTransform;
   public GameObject shieldObject;
   public Rigidbody rigidbody;
   public Collider collider;
   public MeshRenderer renderer;
   public Material normalMaterial;
   public Material cloakMaterial;
   public Transform gunTransform;

   [Header("Settings")]
   public bool isPlayer;
   public float speed = 1;
   public int maxHealth = 100;
   public int currentHealth;
   public float maxEnergy = 100;
   public float currentEnergy;
   public float parryEnergyUse = 30;
   public float shieldEnergyUse = 50;
   public float cloakEnergyUse = 50;

   public float rechargeSpeed;
   public float stateTimeLeft;
   public bool appeared;
   public bool isInvincible;
   public bool isInvisible;
   public bool isParrying;

   private Vector3 targetPosition;
  
   private bool isDead;

   private void Awake() 
   {
      targetPosition = rigidbody.position;
      currentHealth = maxHealth;
      currentEnergy = maxEnergy;
   }

   public void TakeDamage(int damage)
   {
      if (isDead || isInvincible)
         return;

      ModifyHealth(-damage);
   }

   public void ModifyHealth(int value)
   {
      currentHealth = Mathf.Clamp (currentHealth + value , 0 , maxHealth);
      if (currentHealth <= 0)
         Explode();
   }

   public void Shoot(Vector3 dir)
   {

      var angle = Mathf.Atan2(dir.y , dir.x) * Mathf.Rad2Deg;
      gunTransform.localEulerAngles = new Vector3(0 , 0 , angle);
      var bullet = Instantiate(bulletPrefab, bulletFireTransform.position, Quaternion.identity);
      bullet.fromPlayer = isPlayer;
      bullet.Fire(dir);
   }

   public void TrackShoot(Transform targetTransform)
   {
      var dir = targetTransform.transform.position - transform.position;
      var angle = Mathf.Atan2(dir.y , dir.x) * Mathf.Rad2Deg;
      gunTransform.localEulerAngles = new Vector3(0 , 0 , angle);
      var bullet = Instantiate(bulletPrefab, bulletFireTransform.position, Quaternion.identity);
      bullet.fromPlayer = isPlayer;
      bullet.Track(targetTransform);
   }

   private void Update() 
   {
      if (GameplayCore.Instance.paused || isDead)
         return;

      if (state != State.Normal)
      {
         if (stateTimeLeft > 0)
            stateTimeLeft -= Time.deltaTime;
         else
         {
            ResetState();
         }
      }

      if (currentEnergy < maxEnergy)
      {
         currentEnergy = Mathf.Clamp(currentEnergy + (Time.deltaTime * rechargeSpeed), 0 ,maxEnergy);
      }
      rigidbody.position = Vector3.MoveTowards(rigidbody.position , targetPosition , Time.deltaTime * speed);
   }

   public void SetTargetPosition(Vector3 position)
   {
      targetPosition = position;
   }

   public void Explode()
   {
      rigidbody.useGravity = true;
      collider.enabled = false;
      isDead = true;

      if (isPlayer)
      {
         GameplayCore.Instance.EndGame();
      }
      else
      {
         GameplayCore.Instance.AddScore(100);
         PlayerController.Instance.ResetTrackBullet();
      }
   }

   public void Parry()
   {
      if (!DrainEnergy(parryEnergyUse))
         return;

      isInvincible = true;
      isParrying = true;
      collider.enabled = false;
      LeanTween.value(gameObject , 0 , 360f , 1f).setOnUpdate
      (
         (float val)=>
         {
            transform.localEulerAngles = new Vector3 (val , 0 , 0);
         }
      );
      StartCoroutine(EndParry());

      IEnumerator EndParry()
      {
         yield return new WaitForSeconds(2f);
         isParrying = false;
         isInvincible = false;
      }
   }

   public void ActivateShield()
   {
      if (!DrainEnergy(shieldEnergyUse))
         return;

      shieldObject.SetActive(true);
      stateTimeLeft = stateDuration;
      isInvincible = true;
      state = State.Shield;
   }

   public void ActivateCloak()
   {
       if (!DrainEnergy(cloakEnergyUse))
         return;

      renderer.material = cloakMaterial;
      stateTimeLeft = stateDuration;
      isInvisible = true;
      state = State.Cloak;
   }

   public bool DrainEnergy(float value)
   {
      if (currentEnergy < value)
         return false;

      currentEnergy = Mathf.Clamp (currentEnergy - cloakEnergyUse , 0 , maxEnergy);
      return true;
   }

   public void ResetState()
   {
      renderer.material = normalMaterial;
      shieldObject.SetActive(false);
      isInvisible = false;
      isInvincible = false;
      state = State.Normal;
   }

   private void OnTriggerEnter(Collider other) 
   {
      var spaceShip = other.GetComponent<Spaceship>();
      if (spaceShip != null)
      {
         TakeDamage(30);
         return;
      }

      var debris = other.GetComponent<Debris>();
      if (debris)
         TakeDamage(30);
   }

   private void OnMouseDown() 
   {
      if (!isPlayer && !isInvisible)
      {
         PlayerController.Instance.SetTrackedTarget(this);
      }
   }

   private void OnBecameVisible() 
   {
      appeared = true;
   }

   private void OnBecameInvisible() 
   {
      appeared = false;
   }

}
