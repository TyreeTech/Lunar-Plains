using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
      //Grabs the tile prefab
      [SerializeField] GameObject tilePrefab;

      Color selectedTileColor = new Color(0, 1, 1, 1);
      Color defaultTileColor = new Color(1,1,1,1);

      Point[] dirs = new Point[4]
      {
          new Point(0,1),
          new Point(0,-1),
          new Point(1,0),
          new Point(-1,0)
      };

      //creates empty dictionary based on point location called tiles
      public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

      public void Load (LevelData data)
      {
          for (int i = 0; i < data.tiles.Count; ++i)
          {
            // instantiate all of the board tiles.
              GameObject instance = Instantiate(tilePrefab) as GameObject;

              //save the board tiles into a dictionary based on point
              Tile t = instance.GetComponent<Tile>();
              t.Load(data.tiles[i]);
              tiles.Add(t.pos, t);
          }

      }

      //algorithm for finding path from tiles
      public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile)
      {
          List<Tile> retValue = new List<Tile>();
          retValue.Add(start);

          ClearSearch();
          //queue's are used for checking algorithm
          //one tile queue for checking now, one for checking later
          Queue<Tile> checkNext = new Queue<Tile>();
          Queue<Tile> checkNow = new Queue<Tile>();

          //set correct values on the start tile
          //add it to the list of tiles which need to be checked
          start.distance = 0;
          checkNow.Enqueue(start);

          //main loop that dequeues a tile and does logic
          //stops when "CheckNoW" is empty
          while (checkNow.Count > 0)
          {
              Tile t = checkNow.Dequeue();

              for (int i = 0; i < 4; ++i)
              {
                  Tile next = GetTile(t.pos + dirs[i]);

                  if (next == null || next.distance <= t.distance + 1)
                      continue;

                  if(addTile(t, next))
                  {
                      next.distance = t.distance + 1;
                      next.prev = t;
                      checkNext.Enqueue(next);
                      retValue.Add(next);
                  }
              }

              if(checkNow.Count == 0)
                  SwapReference(ref checkNow, ref checkNext);


          }



          return retValue;
      }

      void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
      {
          Queue<Tile> temp = a;
          a = b;
          b = temp;
      }

      //a function for getting our tile
      public Tile GetTile(Point p)
      {
          return tiles.ContainsKey(p) ? tiles[p] : null;
      }

      void ClearSearch ()
      {
          foreach (Tile t in tiles.Values)
          {
              t.prev = null;
              t.distance = int.MaxValue;
          }
      }

      public void SelectTiles (List<Tile> tiles)
      {
        	for (int i = tiles.Count - 1; i >= 0; --i)
        		tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
      }

      public void DeSelectTiles (List<Tile> tiles)
      {
        	for (int i = tiles.Count - 1; i >= 0; --i)
        		tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
      }


}
