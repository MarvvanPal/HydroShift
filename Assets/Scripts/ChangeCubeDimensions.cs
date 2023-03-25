using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;


public class ChangeCubeDimensions : MonoBehaviour
{
    Tuple<float, float, float> dimensionsOfCube;
    float volumeInCubicMeters;

    // Database lookup is here:
    string itemName = "Avocado";
    float volume;


    // Start is called before the first frame update
    void Start()
    {
        string connectionString = "Data Source=Groceries.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Create a SQL command to select the "WaterConsumedPerPiece" value for the given item name
            string sql = "SELECT WaterConsumedPerPiece FROM Items WHERE Name = @itemName";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@itemName", itemName);

                // Execute the command and retrieve the value
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    volume = Convert.ToSingle(result);
                }
                else
                {
                    // Handle the case where the item name was not found in the database
                    Debug.LogError("Item not found in the database.");
                }
            }
        }
        volumeInCubicMeters = volInM3(volume);
        dimensionsOfCube = GetCubeDimensions(volumeInCubicMeters);
        Transform cubeTransform = GetComponent<Transform>();
        cubeTransform.localScale = new Vector3(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
    }

    public float volInM3(float volume)
    {
        float volInM3 = volume / 1000;
        return volInM3;
    }

    public Tuple<float, float, float> GetCubeDimensions(float m3)
    {
        float width, height, length;

        if (m3 <= 2.5)
        {
            width = 1;
            length = 1;
            height = m3;
        }

        else if (m3 <= 8 && m3 > 2.5)
        {
            width = 2;
            length = 2;
            height = m3 / 4;
        }

        else
        {
            width = 2.5f;
            length = 2.5f;
            height = m3 / 6.25f;
        }
        return Tuple.Create(width, height, length);
    }
}

