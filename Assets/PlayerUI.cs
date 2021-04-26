using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Spaceship playerShip;
    public Text hpText;
    public Text energyText;
    public Text stateText;
    public Text trackBulletCountText;
    public Image hpImageFill;
    public Image energyImageFill;
    public Image stateImageFill;
    public Sprite cloakSprite;
    public Sprite shieldSprite;
    public Sprite parrySprite;

    public CanvasGroup normalShootCanvas;
    public CanvasGroup trackShootCanvas;
    public CanvasGroup parryCanvas;
    public CanvasGroup shieldCanvas;
    public CanvasGroup cloakCanvas;

    void Update()
    {
        if (playerShip == null)
            return;
    
        if (stateImageFill.gameObject.activeSelf)
        {
            if (playerShip.state == Spaceship.State.Normal)
                stateImageFill.gameObject.SetActive(false);
        }
        else
        {
            if (playerShip.state != Spaceship.State.Normal)
            {
                stateImageFill.sprite = playerShip.state == Spaceship.State.Cloak? cloakSprite : shieldSprite;
                stateImageFill.gameObject.SetActive(true);
            }
        }

        parryCanvas.alpha = playerShip.currentEnergy >= playerShip.parryEnergyUse ? 1 : 0.3f;
        shieldCanvas.alpha = playerShip.currentEnergy >= playerShip.shieldEnergyUse ? 1 : 0.3f;
        cloakCanvas.alpha = playerShip.currentEnergy >= playerShip.cloakEnergyUse ? 1 : 0.3f;

        hpText.text = playerShip.currentHealth + " / " + playerShip.maxHealth;
        energyText.text = Mathf.RoundToInt(playerShip.currentEnergy) + " / " + Mathf.RoundToInt(playerShip.maxEnergy);
        stateText.text = Mathf.RoundToInt(playerShip.stateTimeLeft).ToString();

        hpImageFill.fillAmount = playerShip.currentHealth / (float) playerShip.maxHealth;
        energyImageFill.fillAmount = playerShip.currentEnergy / (float) playerShip.maxEnergy;
        stateImageFill.fillAmount = playerShip.stateTimeLeft / playerShip.stateDuration;
    }

    public void SetTrackFire(bool on)
    {
        trackShootCanvas.alpha = on? 0.3f: 1f;
        normalShootCanvas.alpha = on? 1 : 0.3f;
    }

}
