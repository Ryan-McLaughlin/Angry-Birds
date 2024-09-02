using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// press f12 to bring up external, then shift+f12 for more

public class _CritterCrush : MonoBehaviour
{
    public static _CritterCrush Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // set critters
    }

    private void Update()
    {
        // show hide critters

        // check for collision: player click/tap on critter
        // log hit


    }

    // special - rain of mallets, all critters on screen are crushed by falling mallets
}
