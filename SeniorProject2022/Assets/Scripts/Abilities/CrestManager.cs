using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Crafting;

public class CrestManager : MonoBehaviour
{
    [Header("Shroom Boots")]
    public GameObject shroomBootsLeft;
    public GameObject shroomBootsRight;
    [Header("Sword")]
    public GameObject sword;
    [Header("Stick")]
    public GameObject stick;
    
    [Header("Sheep's Crook")]
    public GameObject sheepsCrook;

    [Header("Fire Cloak")] 
    public GameObject cape;
    public GameObject fireCape;
    public GameObject cloak;
    public GameObject fireCloak;
    
    [Header("Super Dash")] 
    public GameObject cap;
    
    private PlayerController playerController;
    private AbilityState abilityState;
    private Dictionary<Crest.Abilities, bool> isAbilityEnabled = new Dictionary<Crest.Abilities, bool>();
    [SerializeField] private WeaponObject swordWeapon; 
    [SerializeField] private WeaponObject sheepsCrookWeapon;
    //private Dictionary<Crest, bool> isCrestEquipped = new Dictionary<Crest, bool>();

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>(true);
        abilityState = playerController.abilityState;
    }

    private void Start()
    {
        if (abilityState == null)
            abilityState = playerController.abilityState;
    }
    public void EnableShroomBoots(bool enabled)
    {
        shroomBootsLeft.SetActive(enabled);
        shroomBootsRight.SetActive(enabled);
        
    }

    public void EnableSheepsCrook(bool enabled)
    {

        if(enabled)
        {
            EquipCrook();
        }
        else
        {
            UnEquipCrook();
        }
    }

    public void EquipCrook()
    {
        if(stick)
        {
            Debug.Log("disabling stick");
            stick.SetActive(!enabled);
        }
        if(sword){
            Debug.Log("disabling sword");
            sword.SetActive(!enabled);
        }
        
        sheepsCrook.SetActive(enabled);
        playerController.SetPlayerWeapon(sheepsCrookWeapon);
    }
    public void UnEquipCrook()
    {
        if(sheepsCrook)
        {
            Debug.Log("Disabling SheepsCrook");
            sheepsCrook.SetActive(!enabled);
            playerController.SetPlayerWeapon(null);
        }
    }

    public void EnableFireCloak(bool enabled)
    {
        cape.SetActive(!enabled);
        cloak.SetActive(!enabled);
        fireCape.SetActive(enabled);
        fireCloak.SetActive(enabled);
    }
    
    public void EnableSuperDash(bool enabled)
    {
        cap.SetActive(enabled);
        //var action = playerController._playerInput.FindAction("UltraRun");
    }

    // sword
    public void EnableSword(bool enabled)
    {

        if(enabled)
        {
            EquipSword();
        }
        else
        {
            UnEquipSword();
        }
    }

    public void EquipSword()
    {
        if(stick)
        {
            Debug.Log("disabling stick");
            stick.SetActive(!enabled);
        }
        if(sheepsCrook){
            Debug.Log("disabling sheepCrook");
            sheepsCrook.SetActive(!enabled);
        }
        
        sword.SetActive(enabled);
        sheepsCrook.SetActive(!enabled);
        var manager = FindObjectOfType<DialogueManager>();
        if (manager != null)
        {
            Debug.Log("increase index");
            manager.IncreaseLineIndex();
        }
        playerController.SetPlayerWeapon(swordWeapon);
    }
    public void UnEquipSword()
    {
        if(sword)
        {
            Debug.Log("Disabling Sword");
            sword.SetActive(!enabled);
            playerController.SetPlayerWeapon(null);
        }
    }

    // stick
    public void EnableStick(bool enabled)
    {

        if(enabled)
        {
            EquipStick();
        }
        else
        {
            UnEquipStick();
        }
    }

    public void EquipStick()
    {
        if(sword)
        {
            Debug.Log("disabling sword");
            sword.SetActive(!enabled);
        }
        if(sheepsCrook)
        {
            Debug.Log("disabling Crook");
            sheepsCrook.SetActive(!enabled);
        }
        stick.SetActive(enabled);
    }
    public void UnEquipStick()
    {
        if(stick)
        {
            Debug.Log("Disabling Stick");
            stick.SetActive(!enabled);
        }
    }

    public bool IsAbilityEnabled(Crest.Abilities ability)
    {
        if (!isAbilityEnabled.ContainsKey(ability))
            isAbilityEnabled.Add(ability, false);
        return isAbilityEnabled[ability];
    }

    public void EnableAbility(Crest.Abilities ability, bool enabled)
    {
        if (!isAbilityEnabled.ContainsKey(ability))
            isAbilityEnabled.Add(ability, false);
        isAbilityEnabled[ability] = enabled;

        switch (ability)
        {
            case Crest.Abilities.Stomp:
                EnableShroomBoots(enabled);
                break;
            case Crest.Abilities.Summon:
                EnableSheepsCrook(enabled);
                playerController.CheckIfWeaponEquipped();
                break;
            case Crest.Abilities.FireTrail:
                EnableFireCloak(enabled);
                break;
            case Crest.Abilities.SuperDash:
                EnableSuperDash(enabled);
                break;
            case Crest.Abilities.Sword:
                EnableSword(enabled);
                playerController.CheckIfWeaponEquipped();
                break;
            case Crest.Abilities.Stick:
                EnableStick(enabled);
                playerController.CheckIfWeaponEquipped();
                break;
            default:
                break;
        }
    }

    

    // public void EquipCrest(Crest crest, bool enabled)
    // {
    //     if (!isCrestEquipped.ContainsKey(crest))
    //         isCrestEquipped.Add(crest,false);
    //     isCrestEquipped[crest] = enabled;
    // }

    // public Dictionary<Crest, bool> GetEquipped()
    // {
    //     foreach(KeyValuePair<Crest, bool> entry in isCrestEquipped)
    //     {
    //         Debug.Log(entry.Key);
    //         Debug.Log(entry.Value);
    //     }
    //     return isCrestEquipped;
    // }

}
