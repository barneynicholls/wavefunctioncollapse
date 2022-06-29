using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "ScriptableObjects/Slot", order = 1)]
public class Slot : ScriptableObject
{
    public Module[] modules;

    [HideInInspector]
    public Module[] potentials;

    [HideInInspector]
    public int entropy => potentials.Length;

    [HideInInspector]
    public Module selected;

    [HideInInspector]
    public Vector2 position;

    private void OnEnable()
    {
        modules.CopyTo(potentials, 0);
    }
}
