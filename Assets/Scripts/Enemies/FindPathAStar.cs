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
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(GridObject locationBlock, float g, float h, float f, GameObject marker, PathMarker parent)
    {
        this.locationBlock = locationBlock;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        this.parent = parent;
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
            return locationBlock.Equals(((PathMarker)obj).locationBlock);
    }
    public override int GetHashCode()
    {
        return 0;
    }
}

public class FindPathAStar : MonoBehaviour
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

    void BeginSearch()
    {
        done = false;
        RemoveAllMArkers();

        List<GridObject> locations = new List<GridObject>();
        /* dobi zaèetne lokcaije */
        foreach (GridObject gridObject in grid.GetGridObjects())
        {
            if (!gridObject.IsOccupied || gridObject.IsOccupiedBySnakeHead)
            {
                locations.Add(gridObject);
            }
        }

        /* Ker lokacije premešamo lahko vzamemo prvo kot zaèetek */
        /* pozicije moramo skalirat na podlagi skale labirinta */
        //ShuffleList(locations);
        //GridObject enemyPositionBlock = 
        Vector3 startLocation = enemy.transform.position;
        // rabim grid kocko na kateri stoji nasprotnik
        //startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);

        /* drugaa lokacija pa naj bo cilj */
        GridObject playerPositionBlock = player.SnakeHead.GetNextBlock();
        Vector3 goalLocation = playerPositionBlock.transform.position;
        
        goalNode = new PathMarker(playerPositionBlock, 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode == null) {
            BeginSearch();
            thisNode = lastPos;
        }
        if (thisNode.Equals(goalNode))
        {
            /* dosegli smo konec */
            done = true; return;
        }

        /* maze ima to znotraj directions vse možne smeri */
        /* foreach(MapLocation dir in maze.directions)
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
        */
    }
    /*
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
}
