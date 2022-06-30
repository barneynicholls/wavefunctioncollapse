using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "ScriptableObjects/Slot", order = 1)]
public class Slot : ScriptableObject
{
    public Module[] modules;

    [HideInInspector]
    public List<Module> potentials;

    [HideInInspector]
    public int entropy => potentials?.Count ?? 0;

    [HideInInspector]
    public Module selected;

    [HideInInspector]
    public Vector2 position;

    private void OnEnable()
    {
        potentials = new List<Module>();
        potentials.AddRange(modules);
    }
}
