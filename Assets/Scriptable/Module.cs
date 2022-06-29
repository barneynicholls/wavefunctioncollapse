using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Module", menuName = "ScriptableObjects/Module", order = 1)]
public class Module : ScriptableObject
{
    public Module[] north;
    public Module[] south;
    public Module[] east;
    public Module[] west;
    public new string name;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(name))
            Debug.Log($"{nameof(Module)} {nameof(name)} not set");

        List<string> errors = new List<string>();
        if ((north?.Length ?? 0) == 0)
            errors.Add("north");
        if ((south?.Length ?? 0) == 0)
            errors.Add("south");
        if ((east?.Length ?? 0) == 0)
            errors.Add("east");
        if ((west?.Length ?? 0) == 0)
            errors.Add("west");

        if (errors.Count > 0)
        {
            Debug.Log($"{nameof(Module)} '{name}' allowed modules not set: {string.Join(',', errors)}");
        }
    }
}
