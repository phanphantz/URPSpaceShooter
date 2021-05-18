using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public Spaceship spaceship;
    public PlayerUI playerUI;
    public bl_Joystick joystick;
    public Camera camera;
    public Text  trackBulletCountText;
    public Spaceship trackedEnemy;
    public Image lockOnImage;

    public int trackBulletCount;
    public int trackBulletCountMax = 100;

    bool isTrackFire = false;

    float fireDelayMax =0.2f;
    float fireDelayLeft = 0;

    public Vector3 GetRandomPositionInCameraBound()
    {
        var margin = 1;
        var horzExtent = (camera.orthographicSize * Screen.width / Screen.height) - margin;
        var vertExtent = (camera.orthographicSize) - margin ;
        return new Vector3( Random.Range(-horzExtent , horzExtent) , Random.Range(-vertExtent , vertExtent) , 0);
    }

    (bool isHold , int id) onHoldTap()
    {
        if (fireDelayLeft > 0)
        {
            fireDelayLeft -= Time.deltaTime;
            return (false , -1);
        }

        var fingerID = -1;
        if (Input.touchCount == 1)
            fingerID = 0;

        var notOverUI = !EventSystem.current.IsPointerOverGameObject(fingerID);
        if (Application.isEditor)
        {
            return ( Input.GetMouseButton(0) && notOverUI , 0);
        }

        for (var i = 0 ; i < Input.touchCount ; i++)
        {
            if (!EventSystem.current.IsPointerOverGameObject(i))
                return (true , i);
        }

        return (false , -1);
    }

    Vector3 tapPosition(int id)
    {
        if (Application.isEditor)
        {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        return camera.ScreenToWorldPoint(Input.GetTouch(id).position);
    }

    private void Awake() 
    {
        Instance = this;
        ResetTrackBullet();    
    }

    private void Update() 
    {
        if (GameplayCore.Instance.paused || GameplayCore.Instance.ended)
            return;
        
        if (spaceship == null)
            return;

        var moveVector = new Vector3(joystick.Horizontal, joystick.Vertical , 0 );
        MoveShip(moveVector);

        if (trackedEnemy && isTrackFire)
        {
            if (lockOnImage.enabled == false)
                lockOnImage.enabled = true;

            lockOnImage.rectTransform.anchoredPosition = camera.WorldToScreenPoint(trackedEnemy.transform.position) - new Vector3(Screen.width/2f , Screen.height/2f, 0);
        }
        else
        {
             if (lockOnImage.enabled)
                lockOnImage.enabled = false;
        }

        var hold = onHoldTap();
        if (hold.isHold)
        {
            if (isTrackFire)
            {
                if (trackedEnemy)
                {
                    if (trackedEnemy.currentHealth <= 0)
                        trackedEnemy = null;

                    if (trackedEnemy)
                    TrackFire(trackedEnemy.transform);
                }
            }
            else if (moveVector == Vector3.zero || Input.touchCount > 1 || Application.isEditor)
            {
                var shootDir = (tapPosition(hold.id) - spaceship.transform.position).normalized;
                Fire(shootDir);
            }
            fireDelayLeft = fireDelayMax;
        }
        

        
    }

    public void ResetTrackBullet()
    {
        trackBulletCount = trackBulletCountMax;
        trackBulletCountText.text = trackBulletCount.ToString();
    }

    public void SetTrackedTarget(Spaceship target)
    {
        trackedEnemy = target;
    }

    public void MoveShip(Vector3 dir)
    {
        var clampedPos = spaceship.transform.position + dir;
        var margin = 1;
        var horzExtent = (camera.orthographicSize * Screen.width / Screen.height) - margin;
        var vertExtent = (camera.orthographicSize) - margin ;
        clampedPos.x = Mathf.Clamp(clampedPos.x , -horzExtent , horzExtent);
        clampedPos.y = Mathf.Clamp(clampedPos.y , -vertExtent , vertExtent);
        spaceship.SetTargetPosition(clampedPos);
    }

    public void InvokeParry()
    {
        spaceship.Parry();
    }

    public void InvokeShield()
    {
        spaceship.ActivateShield();
    }

    public void InvokeCloak()
    {
        spaceship.ActivateCloak();
    }

    public void Fire(Vector3 dir)
    {
        spaceship.Shoot(dir);
    }

    public void TrackFire(Transform target)
    {
        if (trackBulletCount <= 0)
            return;

        trackBulletCount--;
        trackBulletCountText.text = trackBulletCount.ToString();
        spaceship.TrackShoot(target);
    }

    public void SetNormalShoot()
    {
        isTrackFire = false;
        playerUI.SetTrackFire(false);
    }

    public void SetTrackShoot()
    {
        isTrackFire = true;
        playerUI.SetTrackFire(true);
    }

}
