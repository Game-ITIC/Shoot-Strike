using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAmmoCount : MonoBehaviour
{
    public Text ammoText;
    public Text magazineText;

    public static UiAmmoCount occurrence;

    private void Awake()
    {
        occurrence = this;
        
    }

    public void UpdateAmmoText(int presentAmmount)
    {
        ammoText.text = "Ammo: " + presentAmmount;
    }


    public void UpdateMagText(int mag)
    {
        magazineText.text = "Magazine: " + mag; 
    }

}
