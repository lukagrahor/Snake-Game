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
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            /*Debug.Log("locationBlock.name");
            Debug.Log(locationBlock.name);
            Debug.Log("obj name");
            Debug.Log(((PathMarker)obj).locationBlock.name);*/
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
        Debug.Log("goal: " + goal.locationBlock.name);

        List<PathMarker> open = new List<PathMarker>();
        List<PathMarker> closed = new List<PathMarker>();
        open.Add(start);
        //Debug.Log("start pathfinding");
        //Debug.Log(start.locationBlock.name);

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

            // vsako tocko predela v enem frame-u
            await Awaitable.NextFrameAsync();
        }
        return new List<GridObject>();
    }


    public List<GridObject> FindPath(GridObject startBlock, GridObject endBlock)
    {
        PathMarker start = new PathMarker(startBlock, null, 0, 0, 0);
        PathMarker goal = new PathMarker(endBlock, null, 0, 0, 0);

        List<PathMarker> open = new List<PathMarker>();
        List<PathMarker> closed = new List<PathMarker>();
        open.Add(start);
        //Debug.Log("start pathfinding");
        //Debug.Log(start.locationBlock.name);

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
            foreach (GridObject neighbour in neighbours)
            {
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
        }
        return new List<GridObject>();
    }

    private List<GridObject> ReconstructPath(PathMarker end)
    {
        List<GridObject> path = new();
        PathMarker current = end;
        while (current != null)
        {
            //Debug.Log("Del poti: " + current.locationBlock.name);
            path.Insert(0, current.locationBlock);
            current = current.parent;
        }
        return path;
    }
}



/*
public class FindPathAStar
{
    [SerializeField] Snake player;
    [SerializeField] Enemy enemy;

    [SerializeField] ArenaGrid grid;
    [SerializeField] Material closedNodeMaterial;
    [SerializeField] Material openNodeMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    [SerializeField] GameObject start;
    [SerializeField] GameObject end;
    [SerializeField] GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;

    void RemoveAllMArkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject marker in markers)
        {
            if (marker != null)
            {
                Destroy(marker);
            }
        }
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode == null) {
            BeginSearch();
            thisNode = lastPos;
        }
        if (thisNode.Equals(goalNode))
        {
            //dosegli smo konec
            done = true; return;
        }

        //maze ima to znotraj directions vse možne smeri
        //foreach(MapLocation dir in maze.directions)
         {
             MapLocation neighbour = dir + thisNode.location;
             // èe je sosednje polje zid, ga ne upoštevamo
             if (maze.map[neighbour.x, neighbour.z] == 1) continue;
             // èe je sosednje polje izven labirinta
             if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
             if (IsClosed(neighbour)) continue;

             // rabiš tudi prejšnjo vrednost --> gre za kumulativo
             float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
             float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
             float F = G + H;

             GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);

             TMP_Text[] values = pathBlock.GetComponentsInChildren<TMP_Text>();
             values[0].text = "G: " + G.ToString("0.00");
             values[1].text = "H: " + H.ToString("0.00");
             values[2].text = "F: " + F.ToString("0.00");

             if(!UpdateMarker(neighbour, G, H, F, thisNode)) {
                 open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
             }
         }
         // najprej posortiramo narašèujoèe po F vrednostih i nnato še po H vrednostih
         // razlog za to je, da èe imamo veè markerjev z istimi F vrednsotmi, so boljši tisti z veèjimi H vrednostmi
         open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();

         PathMarker pm = (PathMarker)open.ElementAt(0);
         closed.Add(pm);
         open.RemoveAt(0);
         pm.marker.GetComponent<Renderer>().material = closedNodeMaterial;

         lastPos = pm;

    }
    //
    public List<GridObject> ShuffleList(List<GridObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
        return list;
    }
    bool UpdateMarker(Vector3 pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in open)
        {
            if(p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed)
        {
            if (p.location.Equals(marker)) return true;
        }
        return false;
    }

    void GetPath()
    {
        RemoveAllMArkers();
        PathMarker begin = lastPos;
        SpawnParentMarker(begin);
    }

    void SpawnParentMarker(PathMarker marker)
    {
        Vector3 markerPosition = new(marker.location.x * maze.scale, 0f, marker.location.z * maze.scale);
        Instantiate(marker.marker, markerPosition, Quaternion.identity);
        PathMarker parent = marker.parent;

        if (parent == null) return;

        SpawnParentMarker(parent);
    }
    // ne rekurzivna verzija
    void GetPathIterative()
    {
        RemoveAllMArkers();
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            Vector3 markerPosition = new(begin.location.x * maze.scale, 0f, begin.location.z * maze.scale);
            Instantiate(pathP, markerPosition, Quaternion.identity);
            begin = begin.parent;
        }

        Vector3 LastMarkerPosition = new(begin.location.x * maze.scale, 0f, begin.location.z * maze.scale);
        Instantiate(pathP, LastMarkerPosition, Quaternion.identity);
    }

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !done)
        {
            Search(lastPos);
        }
        if (Input.GetKeyDown(KeyCode.M) && done)
        {
            GetPath();
        }
    }
    */
