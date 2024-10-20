using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwapper : MonoBehaviour
{
    public GameObject player; //the player object

    public Dropdown characterDropdown, kartDropdown, hairDropdown, skinDropdown; //the dropdowns for the character, kart, hair, and skin

    public GameObject catCharacter, boyCharacter, girlCharacter, catKart, boyKart, girlKart;

    private int charInUse = 0; //the index of the character in use

    private GameObject hairDropdownParent, skinDropdownParent; //the parents of the hair and skin dropdowns

    public Material boyFace, boyHair, girlFace, girlHair; //the materials for the boy and girl faces and hairs

    public Texture boySkin1, boySkin2, boySkin3, girlSkin1, girlSkin2, girlSkin3, 
                   boyHair1, boyHair2, boyHair3, girlHair1, girlHair2, girlHair3; //the textures for the boy and girl skins and hairs
    
    // Start is called before the first frame update
    void Start()
    {
        AnimController.characterAnim = player.transform.Find("Char").gameObject.GetComponent<Animator>(); //set the character to the char object
        AnimController.kartAnim = player.transform.Find("Kart").gameObject.GetComponent<Animator>(); //set the kart to the kart object
        AnimController.ready = true;

        characterDropdown.onValueChanged.AddListener(delegate { CharacterChanged();}); //add the CharacterChanged function to the character dropdown
        kartDropdown.onValueChanged.AddListener(delegate { KartChanged();}); //add the KartChanged function to the kart dropdown

        hairDropdown.onValueChanged.AddListener(delegate { HairChanged();}); //add the CharacterChanged function to the character dropdown
        skinDropdown.onValueChanged.AddListener(delegate { SkinChanged();}); //add the KartChanged function to the kart dropdown

        hairDropdownParent = hairDropdown.transform.parent.gameObject; //? add after finish changed character
        skinDropdownParent = skinDropdown.transform.parent.gameObject; //? add after finish changed character

        hairDropdownParent.SetActive(false); 
        skinDropdownParent.SetActive(false);
        
        CharacterRotator.objectToRotate = player.transform; //set the object to rotate to the player object
    }

    void CreateNewCharacter(GameObject character, Vector3 position, Quaternion rotation) //create a new character
    {
        GameObject newChar = Instantiate(character, position, rotation); //create a new character

        newChar.SetActive(true); //set the new character to active
        newChar.name = "Char"; //set the name of the new character to "Char"
        newChar.transform.parent = player.transform; //set the parent of the new character to the player object

        if(hairDropdownParent.activeInHierarchy == false)
        {
            hairDropdownParent.SetActive(true);
        }
        if(skinDropdownParent.activeInHierarchy == false)
        {
            skinDropdownParent.SetActive(true);
        }

        hairDropdown.value = 0;
        skinDropdown.value = 0;

        AnimController.characterAnim = newChar.GetComponent<Animator>(); //get the animator component of the new character

    }

    void CharacterChanged() //called when the character is changed
    {
        GameObject previousChar = player.transform.Find("Char").gameObject; //get the previous character
        Vector3 previousPosition = previousChar.transform.position; //get the previous character's position
        Quaternion previousRotation = previousChar.transform.rotation; //get the previous character's rotation

        Destroy(previousChar); //destroy the previous character

        switch(characterDropdown.value) //switch statement to create a new character based on the index of the character in the dropdown
        {
            case 0:
                CreateNewCharacter(catCharacter, previousPosition, previousRotation); //create a new cat character
                charInUse = 0; //set the index of the character in use to 0
                break; 
            case 1:
                CreateNewCharacter(boyCharacter, previousPosition, previousRotation); //create a new boy character
                charInUse = 1; //set the index of the character in use to 1
                break;
            case 2:
                CreateNewCharacter(girlCharacter, previousPosition, previousRotation); //create a new girl character
                charInUse = 2; //set the index of the character in use to 2
                break;
        }
        CharacterRotator.objectToRotate = player.transform; //set the object to rotate to the player object
    }
    void CreateNewKart(GameObject kart, Vector3 position, Quaternion rotation) //create a new character
    {
        GameObject newKart = Instantiate(kart, position, rotation); //create a new character

        newKart.SetActive(true); //set the new character to active
        newKart.name = "Kart"; //set the name of the new character to "Kart"
        newKart.transform.parent = player.transform; //set the parent of the new character to the player object

        AnimController.kartAnim = newKart.GetComponent<Animator>(); //get the animator component of the new character

    }

    void KartChanged() //called when the character is changed
    {
        GameObject previousKart = player.transform.Find("Kart").gameObject; //get the previous character
        Vector3 previousPosition = previousKart.transform.position; //get the previous character's position
        Quaternion previousRotation = previousKart.transform.rotation; //get the previous character's rotation

        Destroy(previousKart); //destroy the previous character

        switch(kartDropdown.value) //switch statement to create a new character based on the index of the character in the dropdown
        {
            case 0:
                CreateNewKart(catKart, previousPosition, previousRotation); //create a new cat character
                break; 
            case 1:
                CreateNewKart(boyKart, previousPosition, previousRotation); //create a new boy character
                break;
            case 2:
                CreateNewKart(girlKart, previousPosition, previousRotation); //create a new girl character
                break;
        }
        CharacterRotator.objectToRotate = player.transform; //set the object to rotate to the player object
    }

    void HairChanged()
    {
        GameObject currentAvatar = player.transform.Find("Char").Find("Avatar").gameObject; //? make a available for the current avatar

        Material currentMaterial;
        Material currentMaterial2;

        bool isGirl = false;

        if (charInUse == 2)
        {
            isGirl = true;
        }

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
        switch(hairDropdown.value)
        {
            case 0:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlHair1);
                }
                else
                {
                    currentMaterial.SetTexture("_MainTex", boyHair1);
                    currentMaterial2.SetTexture("_MainTex", boyHair1);
                }
                break;
            case 1:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlHair2);
                }
                else
                {
                    currentMaterial.SetTexture("_MainTex", boyHair2);
                    currentMaterial2.SetTexture("_MainTex", boyHair2);
                }
                break;
            case 2:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlHair3);
                }
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
        GameObject currentAvatar = player.transform.Find("Char").Find("Avatar").gameObject; //? make a available for the current avatar

        Material currentMaterial;

        bool isGirl = false;

        if (charInUse == 2)
        {
            isGirl = true;
        }

        if (isGirl)
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[0];

        }
        else
        {
            currentMaterial = currentAvatar.GetComponent<Renderer>().materials[2];

        }
        switch(skinDropdown.value)
        {
            case 0:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlSkin1);
                }
                else
                {
                    currentMaterial.SetTexture("_MainTex", boySkin1);
                }
                break;
            case 1:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlSkin2);
                }
                else
                {
                    currentMaterial.SetTexture("_MainTex", boySkin2);

                }
                break;
            case 2:
                if (isGirl)
                {
                    currentMaterial.SetTexture("_MainTex", girlSkin3);
                }
                else
                {
                    currentMaterial.SetTexture("_MainTex", boySkin3);
                }
                break;
        }
    }
}
