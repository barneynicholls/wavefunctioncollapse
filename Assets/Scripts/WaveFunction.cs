using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFunction : MonoBehaviour
{
    public int dimensions;

    public TMPro.TMP_Text text;
    public TMPro.TMP_Text seedUsed;

    public Cell cellObj;
    public Tile[] tileObjects;
    public int cellSize = 1;
    public int seed = 0;

    int iterations = 0;

    List<Cell> gridComponents;
    
    DateTime start = DateTime.Now;

    void Awake()
    {
        gridComponents = new List<Cell>();
        InitializeGrid();
    }

    void UpdateText(string message)
    {
        if (text != null)
            text.text = message;
    }

    void UpdateSeed(int seedValue)
    {
        if (seedUsed != null)
            seedUsed.text = seedValue.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = gridComponents.Count - 1; i >= 0; i--)
            {
                Destroy(obj: gridComponents.ElementAt(i).transform.gameObject);
                gridComponents.RemoveAt(i);
            }

            gridComponents = new List<Cell>();
            iterations = 0;
            InitializeGrid();
        }
    }

    void InitializeGrid()
    {
        UpdateText("Initializing grid.");
        start = DateTime.Now;

        for (int y = 0; y < dimensions; y++)
        {
            for (int x = 0; x < dimensions; x++)
            {
                Cell newCell = Instantiate(cellObj, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
                newCell.CreateCell(false, tileObjects);
                gridComponents.Add(newCell);
            }
        }

        var wfcSeed = seed > 0 ? seed : UnityEngine.Random.Range(0,Int32.MaxValue);

        UnityEngine.Random.InitState(wfcSeed);

        UpdateSeed(wfcSeed);

        UpdateText("Initializing grid...");

        StartCoroutine(CheckEntropy());
    }


    IEnumerator CheckEntropy()
    {
        var uncollapsed = gridComponents.Where(c => !c.collapsed);

        var lowestEntropy = uncollapsed.Min(c => c.tileOptions.Length);

        var tempGrid = uncollapsed
                        .Where(c => c.tileOptions.Length == lowestEntropy);

        yield return null;

        CollapseCell(tempGrid);
    }

    void CollapseCell(IEnumerable<Cell> tempGrid)
    {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count());

        Cell cellToCollapse = tempGrid.ElementAt(randIndex);

        cellToCollapse.collapsed = true;
        Tile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
        cellToCollapse.tileOptions = new Tile[] { selectedTile };

        Tile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, Quaternion.identity, cellToCollapse.transform);

        UpdateGeneration();
    }

    void UpdateGeneration()
    {
        for (int y = 0; y < dimensions; y++)
        {
            for (int x = 0; x < dimensions; x++)
            {
                var index = x + y * dimensions;

                if (gridComponents[index].collapsed)
                    continue;

                IEnumerable<Tile> options = new List<Tile>(tileObjects);

                //update above
                if (y > 0)
                {
                    Cell up = gridComponents[x + (y - 1) * dimensions];

                    var validOptions = from tileOption in up.tileOptions from tile in tileOption.upNeighbours select tile;

                    options = options.Intersect(validOptions);
                }

                //update right
                if (x < dimensions - 1)
                {
                    Cell right = gridComponents[x + 1 + y * dimensions];

                    var validOptions = from tileOption in right.tileOptions from tile in tileOption.leftNeighbours select tile;

                    options = options.Intersect(validOptions);
                }

                //look down
                if (y < dimensions - 1)
                {
                    Cell down = gridComponents[x + (y + 1) * dimensions];

                    var validOptions = from tileOption in down.tileOptions from tile in tileOption.downNeighbours select tile;

                    options = options.Intersect(validOptions);
                }

                //look left
                if (x > 0)
                {
                    Cell left = gridComponents[x - 1 + y * dimensions];

                    var validOptions = from tileOption in left.tileOptions from tile in tileOption.rightNeighbours select tile;

                    options = options.Intersect(validOptions);
                }

                gridComponents[index].RecreateCell(options.ToArray());
            }
        }

        iterations++;

        if (iterations < dimensions * dimensions)
        {
            StartCoroutine(CheckEntropy());
        }
        else
        {
            var time = (DateTime.Now - start).TotalSeconds;
            UpdateText($"Done in {time:F2} seconds");
        }

    }
}