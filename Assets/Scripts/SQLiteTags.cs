using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SQLiteTags : MonoBehaviour
{

    private string dbPath;

	private List<PlanetData> planetList;

    private void Start()
    {
        dbPath = "URI=file:" + "Assets/Database" + "/universe.db";          
        Debug.Log(dbPath);
        string[] category = {"Beginner Project", "Relaxation"};
        Select(category);
		planetList = new List<PlanetData> ();
    }

	public List<PlanetData> Select(string[] tags)
    {
        Debug.Log("start");
        using (var conn = new SqliteConnection(dbPath))
        {
            Debug.Log("Conn");
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT DISTINCT* from planets where id in (SELECT planet_id from map where tag_id in (SELECT tag_id from tags where tag = @ftg))";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ftg",
                    Value = tags[0]
                });

                    
                   
                for (int i = 1; i < tags.Length; i++)
                {
                    string index = "tags" + i.ToString();

                    cmd.CommandText += "INTERSECT SELECT DISTINCT* from planets where id in (select planet_id from map where tag_id in (select tag_id from tags where tag = @"+index+"))";
                    Debug.Log(cmd.CommandText);
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = index,
                        Value = tags[i]
                    });

	            }
                    
                Debug.Log("tag1");


            Debug.Log("display begin");
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
					/*
                    var id = reader.GetInt32(0);
                    var Name = reader.GetString(1);
                    var text = string.Format("{0}: {1}", id, Name);
                    Debug.Log(text);
                    */
					PlanetData planet = new PlanetData ();
					planet.title = reader.GetString (1);
					planet.creator = reader.GetString (2);
					planet.description = reader.GetString (3);
					planet.year = (reader.GetInt32 (4)).ToString();
					planet.executable = reader.GetString (6);

					byte[] bytes = File.ReadAllBytes("VRClubUniverse_Data/VR_Demos/" + planet.year + "/" + planet.executable + "/" + reader.GetString (5));
					Texture2D texture = new Texture2D(0, 0);
					texture.LoadImage(bytes);
					Rect rect = new Rect(0, 0, texture.width, texture.height);
					planet.image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

					planetList.Add (planet);
                }
                Debug.Log("display end");
            }
        }

		return planetList;
    }
}

