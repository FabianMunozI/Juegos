using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class borrarPlayerPrefsPlanta : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteKey(keyToDelete);
    }

    public override void Interact()
    {
        base.Interact();
        if (PlayerPrefs.HasKey("terminoJuegoElectricidad1PlantaTratamientoAgua")){
            PlayerPrefs.DeleteKey("terminoJuegoElectricidad1PlantaTratamientoAgua");
        }
        if(PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            PlayerPrefs.DeleteKey("PalancaPlantaTratamientoAgua");
        }

        Debug.Log("eliminaste playerprefs terminoJuegoElectricidad1PlantaTratamientoAgua y PalancaPlantaTratamientoAgua");
    }
}
