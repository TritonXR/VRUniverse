using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public delegate void PassPlanetData(List<PlanetData> planet_list, List<string> image_locations);

public class SQLiteTags : MonoBehaviour
{
    private static readonly char[] delimiters = new char[] { '\\', '/', '.' };
    private string dbPath;
    private Thread databaseThread;

    private List<PlanetData> searchResults;
    private List<string> image_paths;
    private bool waitingForResults;

    private Object lockObj = new Object();

    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/../VRClubUniverse_Data" + "/universe.db";

        databaseThread = null;
        searchResults = null;
        waitingForResults = false;
    }

    void Update()
    {
        if(waitingForResults && searchResults != null)
        {
            lock (lockObj)
            {
                for (int index = 0; index < searchResults.Count; index++)
                {
                    PlanetData planet = searchResults[index];
                    string[] exeParts = planet.executable.Split(delimiters);
                    string exeName = exeParts[exeParts.Length - 2];

                    byte[] bytes = File.ReadAllBytes("VRClubUniverse_Data/VR_Demos/" + planet.year + "/" + exeName + "/" + image_paths[index]);
                    Texture2D texture = new Texture2D(0, 0);
                    texture.LoadImage(bytes);
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    planet.image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

                    searchResults[index] = planet;
                }

                ResultDisplay.GetInstance().DisplaySearchResults(searchResults);
                waitingForResults = false;
                searchResults = null;
                image_paths = null;
            }
        }
    }

    public void Select(string[] tags)
    {
        lock (lockObj) {
            if (databaseThread != null)
            {
                databaseThread.Abort();
                databaseThread = null;
            }

            DatabaseWorker worker = new DatabaseWorker(dbPath, tags, ReceivePlanetData);

            databaseThread = new Thread(new ThreadStart(worker.LoadPlanets));
            databaseThread.Start();
            waitingForResults = true;
        }
    }

    private void ReceivePlanetData(List<PlanetData> planet_list, List<string> image_locations)
    {
        lock(lockObj)
        {
            databaseThread = null;
            searchResults = planet_list;
            image_paths = image_locations;
        }
    }


}

public class DatabaseWorker
{
    private string dbPath;
    private string[] target_tags;
    private PassPlanetData dataReturnCallback;

    public DatabaseWorker(string path, string[] tags, PassPlanetData callback)
    {
        dbPath = path;
        dataReturnCallback = callback;
        target_tags = tags;
    }

    public void LoadPlanets()
    {
        try
        {
            List<PlanetData> planetList = new List<PlanetData>();
            List<string> planetImages = new List<string>();
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
                        Value = target_tags[0]
                    });



                    for (int i = 1; i < target_tags.Length; i++)
                    {
                        string index = "tags" + i.ToString();

                        cmd.CommandText += "INTERSECT SELECT DISTINCT* from planets where id in (select planet_id from map where tag_id in (select tag_id from tags where tag = @" + index + "))";
                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = index,
                            Value = target_tags[i]
                        });

                    }

                    var reader = cmd.ExecuteReader();
                    planetList = new List<PlanetData>();
                    while (reader.Read())
                    {
                        PlanetData planet = new PlanetData();
                        planet.title = reader.GetString(1);
                        planet.creator = reader.GetString(2);
                        planet.description = reader.GetString(3);
                        planet.year = (reader.GetInt32(4)).ToString();
                        planet.executable = @"../VRClubUniverse_Data/VR_Demos/" + planet.year + @"/" + reader.GetString(6) + @"/" + reader.GetString(6) + @".exe";
                        string db_tags = reader.GetString(7);
                        db_tags = db_tags.Replace("u'", "");
                        db_tags = db_tags.Replace("'", "");
                        db_tags = db_tags.Replace("[", "");
                        db_tags = db_tags.Replace("]", "");

                        planet.des_tag = db_tags.Split(',');

                        planet.image = null;

                        planetList.Add(planet);
                        planetImages.Add(reader.GetString(5));
                    }
                }
            }

            dataReturnCallback(planetList, planetImages);
        }
        finally
        {
            //no cleanup really required
        }
    }
}

