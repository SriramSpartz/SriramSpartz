using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] platforms;

    public GameObject[] powerups;

    public GameObject[] enemies;

    public Transform objectpool;

    public Transform camTrans;

    public float levelWidth = 2.5f;
    public float minY = 0.3f;
    public float maxY = 1.2f;

    public float recreate = 0f;

    public PlayerMovement playermove;

    public float startposford = 0f;

    void Start()
    {

    }

    void LateUpdate()
    {
        transform.position = camTrans.position;
        if (transform.position.y >= recreate - 10f)
        {
            CreateLevel();
        }
    }

    public void CreateLevel()
    {
        playermove.gameStarted = true;
        Vector3 spawnPosition = new Vector3(0f, recreate, 0f);
        for (int i = 0; i < 20; i++)
        {
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            spawnPosition.y += Random.Range(minY, maxY);


            //Platforms
            int pl = Random.Range(-1, 13);
            if (pl > 3 || pl==-1)
            {
                Instantiate(platforms[4], spawnPosition, Quaternion.identity,objectpool);
            }
            else
            {
                Instantiate(platforms[pl], spawnPosition, Quaternion.identity,objectpool);
            }

            if (pl == 4 || pl == 5)
            {
                //Powerups
                spawnPosition.y += 0.25f;
                int pu = Random.Range(-1, 5);
                if (pu == -1 || pu == 5)
                {
                    Instantiate(powerups[0], spawnPosition, Quaternion.identity,objectpool);
                }
                else
                {

                    Instantiate(powerups[pu], spawnPosition, Quaternion.identity,objectpool);
                }
            }

            if(pl==6 && camTrans.position.y-startposford >= 60f)
            {
                //Enemies
                spawnPosition.y += 0.5f;
                int pu = Random.Range(-1, 5);
                if (pu == -1 || pu == 5)
                {
                    Instantiate(enemies[0], spawnPosition, Quaternion.identity,objectpool);
                }
                else
                {

                    Instantiate(enemies[pu], spawnPosition, Quaternion.identity,objectpool);
                }
            }

        }

        recreate = spawnPosition.y;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            playermove.GameOver();
        }
        else
        {
            Destroy(col.gameObject);
        }
 
    }



    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void Quitgame()
    {
        Application.Quit();
    }

}
