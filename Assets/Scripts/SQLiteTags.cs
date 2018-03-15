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

#if UNITY_EDITOR
        dbPath = "URI=file:" + Application.dataPath + "/../Website/data/VRClubUniverseData" + "/universe.db";
#elif UNITY_STANDALONE
        dbPath = "URI=file:" + Application.dataPath + "/../VRClubUniverseData" + "/universe.db";
#endif
    }

    public List<PlanetData> Select(string[] tags)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
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
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = index,
                        Value = tags[i]
                    });

	            }
                    
                var reader = cmd.ExecuteReader();
                planetList = new List<PlanetData>();
                while (reader.Read())
                {
					PlanetData planet = new PlanetData ();
					planet.title = reader.GetString(1);
					planet.creator = reader.GetString(2);
					planet.description = reader.GetString(3);
					planet.year = (reader.GetInt32(4)).ToString();

                    #if UNITY_EDITOR
                        planet.executable = @"../Website/data/VRClubUniverseData/VR_Demos/" + planet.year + @"/" + reader.GetString(6) + @"/" + reader.GetString(6) + @".exe";
                    #elif UNITY_STANDALONE
                        planet.executable = @"../VRClubUniverseData/VR_Demos/" + planet.year + @"/" + reader.GetString(6) + @"/" + reader.GetString(6) + @".exe";
                    #endif

                    //planet.executable = @"../VRClubUniverseData/VR_Demos/" + planet.year + @"/" + reader.GetString(6) + @"/" + reader.GetString(6) + @".exe";
                    string db_tags = reader.GetString(7);
                    db_tags = db_tags.Replace("u'", "");
                    db_tags = db_tags.Replace("'", "");
                    db_tags = db_tags.Replace("[", "");
                    db_tags = db_tags.Replace("]", "");

                    planet.des_tag = db_tags.Split(',');

                    //byte[] bytes = File.ReadAllBytes("VRClubUniverseData/VR_Demos/" + planet.year + "/" + reader.GetString(6) + "/" + reader.GetString (5));

                    byte[] bytes;

                    #if UNITY_EDITOR
                        bytes = File.ReadAllBytes("Website/data/VRClubUniverseData/VR_Demos/" + planet.year + "/" + reader.GetString(6) + "/" + reader.GetString(5));
                    #elif UNITY_STANDALONE
                        bytes = File.ReadAllBytes("VRClubUniverseData/VR_Demos/" + planet.year + "/" + reader.GetString(6) + "/" + reader.GetString (5));
                    #endif

                    Texture2D texture = new Texture2D(0, 0);
					texture.LoadImage(bytes);
					Rect rect = new Rect(0, 0, texture.width, texture.height);
					planet.image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

					planetList.Add (planet);
                }
            }
        }

		return planetList;
    }
}

