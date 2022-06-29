
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int sizeX = 10;

    public int sizeY = 10;

    public Slot slot;

    private List<Slot> slots;

    // Start is called before the first frame update
    void Start()
    {
        if (slot is null)
            Debug.LogError($"{nameof(WaveFunctionCollapse)} slot not set");

        PopulateSlots();

        // TODO get remaining by selecting those slots left with entropy > 0
    }

    private void PopulateSlots()
    {
        slots = new List<Slot>();

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                var clone = Instantiate(slot);
                clone.position = new Vector2(x, y);
                slots.Add(clone);
            }
        }
    }

    private Vector2 GetNextSlot(IEnumerable<Slot> remaining)
    {
        var grouped = from r in remaining
                      group r by r.entropy into entropy
                      orderby entropy.Key
                      select entropy;

        // TODO order and select random from group with lowest entropy


        return new Vector2(
            Random.Range(0, sizeX),
            Random.Range(0, sizeY));
    }

    private void Propogate(Vector2 start)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
