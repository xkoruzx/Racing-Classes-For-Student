using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwapperMP : MonoBehaviour
{
    public Dropdown KartDropdown, CharacterDropdown, HairDropdown, SkinDropdown;
    public GameObject player;
    public GameObject catCharacter, boyCharacter, girlCharacter, catKart, boyKart, girlKart;
    public Material boyFace, boyHair, girlFace, girlHair;
    private GameObject hairDropdownParent, skinDropdownParent;
    public Texture boySkin1, boySkin2, boySkin3, girlSkin1, girlSkin2, girlSkin3, boyHair1, boyHair2, boyHair3, girlHair1, girlHair2, girlHair3;
    private int charInUse = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameLogicMP.charModel = 0;
        GameLogicMP.charInUse = 0;
        GameLogicMP.kartModel = 0;
        GameLogicMP.skin = 0;
        GameLogicMP.hair = 0;

        AnimController.kartAnim = player.transform.Find("Kart").gameObject.GetComponent<Animator>();
        AnimController.characterAnim = player.transform.Find("Char").gameObject.GetComponent<Animator>();
        AnimController.ready = true;
        KartDropdown.onValueChanged.AddListener(delegate { KartChanged(); });
        CharacterDropdown.onValueChanged.AddListener(delegate { CharacterChanged(); });
        SkinDropdown.onValueChanged.AddListener(delegate { SkinChanged(); });
        HairDropdown.onValueChanged.AddListener(delegate { HairChanged(); });
        CharacterRotator.objectToRotate = player.transform;

        hairDropdownParent = HairDropdown.transform.parent.gameObject;
        skinDropdownParent = SkinDropdown.transform.parent.gameObject;
        hairDropdownParent.SetActive(false);
        skinDropdownParent.SetActive(false);
    }

    // KartChanged swaps out the kart model based on the player's selection.
    void KartChanged()
    {
        GameObject previousKart = player.transform.Find("Kart").gameObject;
        Vector3 previousPosition = previousKart.transform.position;
        Quaternion previousRotation = previousKart.transform.rotation;
        Destroy(previousKart);
        GameLogicMP.kartModel = (byte)KartDropdown.value;

        switch (KartDropdown.value)
        {
            // Cat racer
            case (0):
                CreateNewKart(catKart, previousPosition, previousRotation);
                break;
            // Boy racer
            case (1):
                CreateNewKart(boyKart, previousPosition, previousRotation);
                break;
            // Girl racer
            case (2):
                CreateNewKart(girlKart, previousPosition, previousRotation);
                break;
        }
        
        CharacterRotator.objectToRotate = player.transform;
    }

    void CreateNewKart(GameObject kart, Vector3 position, Quaternion rotation)
    {
        GameObject newKart = Instantiate(kart, position, rotation);
        newKart.SetActive(true);
        newKart.name = "Kart";
        newKart.transform.parent = player.transform;
        AnimController.kartAnim = newKart.GetComponent<Animator>();
    }

    // CharacterChanged swaps out the character model based on the player's selection.
    void CharacterChanged()
    {
        GameObject previousChar = player.transform.Find("Char").gameObject;
        Vector3 previousPosition = previousChar.transform.position;
        Quaternion previousRotation = previousChar.transform.rotation;
        Destroy(previousChar);
        GameLogicMP.charModel = (byte)CharacterDropdown.value;

        switch (CharacterDropdown.value)
        {
            // Cat
            case (0):
                charInUse = 0;
                CreateNewCharacter(catCharacter, previousPosition, previousRotation);
                break;
            // Boy
            case (1):
                charInUse = 1;
                CreateNewCharacter(boyCharacter, previousPosition, previousRotation);
                break;
            // Girl
            case (2):
                charInUse = 2;
                CreateNewCharacter(girlCharacter, previousPosition, previousRotation);
                break;
        }
        CharacterRotator.objectToRotate = player.transform;
    }

    void CreateNewCharacter(GameObject character, Vector3 position, Quaternion rotation)
    {
        GameObject newChar = Instantiate(character, position, rotation);
        newChar.SetActive(true);
        newChar.name = "Char";
        newChar.transform.parent = player.transform;
        if (charInUse > 0)
        {
            if (hairDropdownParent.activeInHierarchy == false)
                hairDropdownParent.SetActive(true);
            if (skinDropdownParent.activeInHierarchy == false)
                skinDropdownParent.SetActive(true);
        }
        else
        {
            if (hairDropdownParent.activeInHierarchy == true)
                hairDropdownParent.SetActive(false);
            if (skinDropdownParent.activeInHierarchy == true)
                skinDropdownParent.SetActive(false);
        }
        
        HairDropdown.value = 0;
        SkinDropdown.value = 0;
        AnimController.characterAnim = newChar.GetComponent<Animator>();
    }

    void HairChanged()
    {
        GameObject currentAvatar = player.transform.Find("Char").Find("Avatar").gameObject;
        Material currentMaterial;
        Material currentMaterial2;
        bool isGirl = false;
        if (charInUse == 2)
            isGirl = true;
        if (isGirl)
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[2];
            currentMaterial2 = currentAvatar.GetComponent<Renderer>().materials[2];
        }
        else
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[0];
            currentMaterial2 = currentAvatar.GetComponent<Renderer>().materials[1];
        }
        GameLogicMP.hair = (byte)HairDropdown.value;

        switch (HairDropdown.value)
        {
            // Skin 1
            case (0):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlHair1);
                else
                {
                    currentMaterial.SetTexture("_MainTex", boyHair1);
                    currentMaterial2.SetTexture("_MainTex", boyHair1);
                }
                break;
            // Skin 2
            case (1):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlHair2);
                else
                {
                    currentMaterial.SetTexture("_MainTex", boyHair2);
                    currentMaterial2.SetTexture("_MainTex", boyHair2);
                }
                break;
            // Skin 3
            case (2):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlHair3);
                else
                {
                    currentMaterial.SetTexture("_MainTex", boyHair3);
                    currentMaterial2.SetTexture("_MainTex", boyHair3);
                }
                break;
        }
    }

    void SkinChanged()
    {
        GameObject currentAvatar = player.transform.Find("Char").Find("Avatar").gameObject;
        bool isGirl = false;
        if (charInUse == 2)
            isGirl = true;
        Material currentMaterial;
        if (isGirl)
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[0];
        }
        else
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[2];
        }
        GameLogicMP.skin = (byte)SkinDropdown.value;

        switch (SkinDropdown.value)
        {
            // Skin 1
            case (0):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlSkin1);
                else
                    currentMaterial.SetTexture("_MainTex", boySkin1);
                break;
            // Skin 2
            case (1):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlSkin2);
                else
                    currentMaterial.SetTexture("_MainTex", boySkin2);
                break;
            // Skin 3
            case (2):
                if (isGirl)
                    currentMaterial.SetTexture("_MainTex", girlSkin3);
                else
                    currentMaterial.SetTexture("_MainTex", boySkin3);
                break;
        }
    }
}
