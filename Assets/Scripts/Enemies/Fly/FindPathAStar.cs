using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Timeline;

public class PathMarker
{
    public GridObject locationBlock;
    public float G;
    public float H;
    public float F;
    public PathMarker parent;

    public PathMarker(GridObject locationBlock, PathMarker parent, float g, float h, float f)
    {
        this.locationBlock = locationBlock;
        this.parent = parent;
        G = g;
        H = h;
        F = f;
    }

    /// <summary>
    /// Primerjava ali je en pathMarker enak kot drugi
    /// </summary>
    /// <param name="obj">PathMarker s katerim primerjamo</param>
    /// <returns>Vrne true, ko sta dva markerja na enaki lokaciji --> to je en in isti marker</returns>
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()) || locationBlock == null)
            return false;
        else
        {
            return locationBlock.name == ((PathMarker)obj).locationBlock.name;
        }
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
public class FindPathAStar
{
    private ArenaGrid grid;
    public FindPathAStar(ArenaGrid grid)
    {
        this.grid = grid;
    }
    public async Awaitable<List<GridObject>> FindPathAsync(GridObject startBlock, GridObject endBlock)
    {
        PathMarker start = new PathMarker(startBlock, null, 0, 0, 0);
        PathMarker goal = new PathMarker(endBlock, null, 0, 0, 0);

        List<PathMarker> open = new List<PathMarker>();
        List<PathMarker> closed = new List<PathMarker>();
        open.Add(start);

        int i = 0;
        while (open.Count > 0)
        {
            open.Sort((a, b) => a.F.CompareTo(b.F));
            PathMarker selectedMarker = open[0];
            if (selectedMarker.Equals(goal))
            {
                return ReconstructPath(selectedMarker);
            }

            open.Remove(selectedMarker);
            closed.Add(selectedMarker);

            List<GridObject> neighbours = grid.GetNeighbours(selectedMarker.locationBlock);
            // dobi vse sosede izbrane kocke, ustvari njihove path markerje, izraèunaj njihove vrednosti in jih dodaj v open
            foreach(GridObject neighbour in neighbours)
            {
                if (neighbour == null) continue;
                if (neighbour.IsOccupied && !neighbour.IsOccupiedBySnakeHead) continue;
                if (closed.Exists(x => x.locationBlock == neighbour)) continue;

                float g = selectedMarker.G + Vector3.Distance(selectedMarker.locationBlock.transform.position, neighbour.transform.position);
                float h = Vector3.Distance(neighbour.transform.position, endBlock.transform.position);
                float f = g + h;

                PathMarker existing = open.Find(x => x.locationBlock == neighbour);
                if (existing == null)
                {
                    open.Add(new PathMarker(neighbour, selectedMarker, g, h, f));
                }
                else if (g < existing.G)
                {
                    existing.G = g;
                    existing.F = f;
                }
            }

            // pet tock predela v enem frame-u
            if (i % 5 == 0) await Awaitable.NextFrameAsync();
            i++;
        }
        return new List<GridObject>();
    }

    // toliko toèk kot je v novi poti, tolikokrat gre v to metodo
    private List<GridObject> ReconstructPath(PathMarker end)
    {
        List<GridObject> path = new();
        PathMarker current = end;
        while (current != null)
        {
            path.Insert(0, current.locationBlock);
            current = current.parent;
        }
        return path;
    }
}
