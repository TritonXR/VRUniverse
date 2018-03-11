using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;



public class SQLiteTags : MonoBehaviour
{

    private string dbPath;
    private void Start()
    {
        dbPath = "URI=file:" + "Assets/Database" + "/universe.db";          
        Debug.Log(dbPath);
        string[] category = {"Beginner Project", "Relaxation"};
        Select(category);
    }

    public void Select(string[] tags)
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
                    var id = reader.GetInt32(0);
                    var Name = reader.GetString(1);
                    var text = string.Format("{0}: {1}", id, Name);
                    Debug.Log(text);
                }
                Debug.Log("display end");
            }
        }
    }
}

