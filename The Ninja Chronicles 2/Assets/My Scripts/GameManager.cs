using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;

    public Transform[] enemy_locations;

    Player p;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            for (int i = 0; i < enemy_locations.Length; i++)
                Instantiate(enemies[Random.Range(0, enemies.Length)], enemy_locations[i]);
        }
        */

        if (Input.GetKey(KeyCode.Alpha8))
        {
            p.GainExperience(100);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Game1");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
