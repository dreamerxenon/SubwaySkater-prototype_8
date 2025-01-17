using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 10;
    private const int INITIAL_TRANSITIONS_SEGMENT = 2;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    public  bool  SHOW_COLLIDER = true;
    private Transform  cameraContainer;
    private int amountOfActiveSegments;
    private int continousSegments;
    private int currentSpawnZ;
    private int currentLevel;
     private int y1, y2 ,y3;

     public List<Segment> availableSegments =  new List<Segment>();
     public List<Segment> availableTransition =  new List<Segment>();
     [HideInInspector]
     public List<Segment> Segments =  new List<Segment>();

     private bool isMoving;
     


   public static LevelManager Instance { set; get;}
   public List<Piece> ramps = new List<Piece>();
   public List<Piece> longBlocks = new List<Piece>();
   public List<Piece> jump = new List<Piece>();
   public List<Piece> slides = new List<Piece>();
   [HideInInspector]
   public List<Piece> pieces = new List<Piece>();



   private void Awake()
   {
    Instance = this;
    cameraContainer = Camera.main.transform;

    currentSpawnZ = currentLevel = 0;
   }

   private void Start()
   {
      for(int i = 0; i < INITIAL_SEGMENTS; i++)
        if( i < INITIAL_TRANSITIONS_SEGMENT)
        SpawnTransition();
        else
      GenerateSegment();
   }

   private void Update()
   {
    if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
    {
        GenerateSegment();
    }
    if (amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN)
    {
        Segments[amountOfActiveSegments - 1].DeSpawn();
        amountOfActiveSegments--;
    }
   }

   private void GenerateSegment()
   {
    SpawnSegment();

    if(Random.Range(0f,1f) < (continousSegments * 0.25f))
    {
      SpawnTransition();
      continousSegments = 0;
    }else{
        continousSegments++;
    }
   }

   private void SpawnSegment()
   {
     List<Segment> possibleSegments = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2|| x.beginY3 == y3);
     int id =  Random.Range(0 , possibleSegments.Count);
     Segment s =  GetSegment(id ,false);

     y1  = s.endY1;
     y2 =  s.endY2;
     y3 = s.endY3;

     s.transform.SetParent(transform);
     s.transform.localPosition = Vector3.forward * currentSpawnZ;
     currentSpawnZ += s.length;
     amountOfActiveSegments++;
     s.Spawn();

   }
      private void SpawnTransition()
   {
     List<Segment> possibleTransition = availableTransition.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2|| x.beginY3 == y3);
     int id =  Random.Range(0 , possibleTransition.Count);
     Segment s =  GetSegment(id ,true);

     y1  = s.endY1;
     y2 =  s.endY2;
     y3 = s.endY3;

     s.transform.SetParent(transform);
     s.transform.localPosition = Vector3.forward * currentSpawnZ;
     currentSpawnZ += s.length;
     amountOfActiveSegments++;
     s.Spawn();

   }



   private Segment GetSegment(int id, bool transition)
   {
    Segment r =  null;
    r =  Segments.Find(x => x.segId == id && x.transition == transition && !x.gameObject.activeSelf);
    if (r ==  null)
    {
        GameObject go = Instantiate( (transition) ? availableTransition[id].gameObject: availableSegments[id].gameObject) as GameObject;
        r = go.GetComponent<Segment>();

        r.segId = id;
        r.transition =  transition;
        Segments.Insert(0,r);
    }else
    {
        Segments.Remove(r);
        Segments.Insert(0 ,r);
    }
    return r;
   }

   public Piece  GetPiece(PieceType pt, int visualIndex)
   {
    Piece p = pieces.Find(x => x.type ==  pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

     if (p  == null)
     {
        GameObject go = null;

        if (pt == PieceType.ramp)
        go = ramps[visualIndex].gameObject;
        else if (pt == PieceType.longBlock)
        go = longBlocks[visualIndex].gameObject;
        else if (pt == PieceType.jump)
        go = jump[visualIndex].gameObject;
        else if (pt == PieceType.slide)
        go = slides[visualIndex].gameObject;

        go =  Instantiate(go);
        p = go.GetComponent<Piece>();
        pieces.Add(p);

        
     }
     return p;
   }


}
