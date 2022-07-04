
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

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

        //while (remaining.Count() > 0)
        //{
        Debug.Log($"remaining: {remaining.Count()}");

        var slot = GetNextSlot(remaining);

        PropogateFrom(slot);

        remaining = GetRemaining();
        //   }


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

                var obj = Instantiate(Resources.Load("blank"), transform) as GameObject;
                obj.transform.position = clone.position;
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
            .OrderBy(g => g.Key).First();

        return lowestEntropy.ElementAt(Random.Range(0, lowestEntropy.Count()));
    }

    private void PropogateFrom(Slot slot)
    {
        var potential = slot.potentials.ElementAt(Random.Range(0, slot.potentials.Count));

        CreateObject(slot.position, potential.name);

        slot.potentials.Clear();

        // north
        Adjacent(new Vector2(slot.position.x - 1, slot.position.y), potential.north.ToList());
        // south
        Adjacent(new Vector2(slot.position.x + 1, slot.position.y), potential.south.ToList());
        // east
        Adjacent(new Vector2(slot.position.x , slot.position.y -1 ), potential.west.ToList());
        // west
        Adjacent(new Vector2(slot.position.x , slot.position.y + 1), potential.north.ToList());
    }

    private void Adjacent(Vector2 position, List<Module> allowed)
    {
        if (position.x < 0 || position.x > sizeX - 1)
            return;

        if (position.y < 0 || position.y > sizeY - 1)
            return;

        var slot = slots.Where(p => p.position == position).FirstOrDefault();

        if (slot == null)
            return;

        if (slot.entropy == 0)
            return;

        var possibles = from p in slot.potentials
                        join a in allowed
                        on p.name equals a.name
                        select p;

        var match = slot.potentials.ElementAt(Random.Range(0, possibles.Count()));

        CreateObject(slot.position, match.name);

        slot.potentials.Clear();
    }

    private void CreateObject(Vector2 position, string match)
    {
        Debug.Log($"creating adjacent ({position}): {match}");

        var obj = Instantiate(Resources.Load(match),transform) as GameObject;
        obj.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
