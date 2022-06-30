
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int sizeX = 10;

    public int sizeY = 10;

    public Slot slotDefinition;

    private List<Slot> slots;

    // Start is called before the first frame update
    void Start()
    {
        if (slotDefinition is null)
            Debug.LogError($"{nameof(WaveFunctionCollapse)} slotDefinition not set");

        PopulateSlots();

        Debug.Log($"slots created: {slots.Count}");

        var remaining = GetRemaining();

        while(remaining.Count() > 0)
        {
            Debug.Log($"remaining: {remaining.Count()}");

            var slot = GetNextSlot(remaining);

            PropogateFrom(slot);

            remaining = GetRemaining();
        }
    }

    private void PopulateSlots()
    {
        slots = new List<Slot>();

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                var clone = Instantiate(slotDefinition);
                clone.position = new Vector2(x, y);
                slots.Add(clone);
            }
        }
    }

    private IEnumerable<Slot> GetRemaining()
    {
        return slots.Where(s => s.entropy > 0);
    }

    private Slot GetNextSlot(IEnumerable<Slot> remaining)
    {
        var lowestEntropy = remaining
            .GroupBy(r => r.entropy)
            .OrderBy(g=>g.Key).First();

        return lowestEntropy.ElementAt(Random.Range(0, lowestEntropy.Count()));
    }

    private void PropogateFrom(Slot slot)
    {
        var potential = slot.potentials.ElementAt(Random.Range(0, slot.potentials.Count));

        Debug.Log($"create: {potential.name}");

        slot.potentials.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
