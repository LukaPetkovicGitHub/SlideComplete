using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public Manager Manager;
    public void DrawMap() {

        if ( PlayerPrefs.GetInt("Progressive_Level", 0) > 99 && PlayerPrefs.GetInt("LevelPack2Unlocked", 0) == 0 ) {
            PlayerPrefs.SetInt("Progressive_Level", Random.Range(0, 100));
        } else
        if ( PlayerPrefs.GetInt("Progressive_Level", 0) > 199 && PlayerPrefs.GetInt("LevelPack3Unlocked", 0) == 0 ) {
            PlayerPrefs.SetInt("Progressive_Level", Random.Range(0, 200));
        }
        
        if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetSize() == "SMALL" ) {
            Camera.main.orthographicSize = 19;
            Manager.MapWidth = 15;
            Manager.MapHeight = 21;
        } else 
        if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetSize() == "MEDIUM" ) {
            Camera.main.orthographicSize = 23;
            Manager.MapWidth = 19;
            Manager.MapHeight = 25;
        } else
        if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetSize() == "LARGE" ) {
            Camera.main.orthographicSize = 29;
            Manager.MapWidth = 25;
            Manager.MapHeight = 31;
        } 

        Manager.Map = new int[ Manager.MapHeight, Manager.MapWidth ];
        Manager.ObjectMap = new GameObject[ Manager.MapHeight, Manager.MapWidth ];

        for ( int i = 0; i < Manager.MapHeight; i++ ) {
            for ( int j = 0; j < Manager.MapWidth; j++ ) {

                Vector3 InstancePosition = new Vector3( j - Mathf.FloorToInt( Manager.MapWidth / 2), ( i - Mathf.FloorToInt( Manager.MapHeight / 2 )) * -1, 0 );

                if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetBits()[i, j] == 0 ) {

                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    } else {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    Manager.ObjectMap[i, j] = Instantiate<GameObject>( Manager.Tile, InstancePosition, Manager.Tile.transform.rotation );
                    Manager.Map[i, j] = 0;

                    Manager.TilesLeft++;

                } else 
                if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetBits()[i, j] == 1 ) {

                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = Color.white;
                    } else {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    }

                    Manager.ObjectMap[i, j] = Instantiate<GameObject>( Manager.Tile, InstancePosition, Manager.Tile.transform.rotation );
                    Manager.Map[i, j] = 1;

                } else 
                if ( Manager.Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetBits()[i, j] == 2 ) {

                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    } else {
                        Manager.Tile.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    Manager.ObjectMap[i, j] = Instantiate<GameObject>( Manager.Tile, InstancePosition, Manager.Tile.transform.rotation );
                    Manager.ObjectMap[i, j].GetComponent<SpriteRenderer>().color = Manager.Variables.GetMapColor();
                    Manager.Map[i, j] = 2;

                    Manager.Player.transform.position = Manager.ObjectMap[i, j].transform.position + new Vector3( 0, 0, -3 );
                    Manager.TargetPosition.transform.position = Manager.ObjectMap[i, j].transform.position + new Vector3( 0, 0, -3 );

                } 

            }
        }

        
    }
}
