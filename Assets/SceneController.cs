using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabList;
    private Camera mainCamera;

    private readonly Dictionary<string, int> girlCount = new Dictionary<string, int>(4);

    private float launchTime;

    public static AsyncOperation StartGame()
    {
        return SceneManager.LoadSceneAsync("main");
    }

    void Start()
    {
        Random.InitState(DateTime.Now.Millisecond);
        mainCamera = FindObjectOfType<Camera>();


        foreach (var girl in FindObjectsOfType<GirlController>())
        {
            if (!girlCount.ContainsKey(girl.name)) girlCount.Add(girl.name, 0);
            girlCount[girl.name]++;
        }

        for (var i = 0; i < 3; i++) LaunchGirl();
    }

    void Update()
    {
        launchTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.transform != null)
            {
                CheckChange(hit.transform.name);
            }

        }
        else
        {
            if (launchTime > 3f) //со временем девушек прибывает
            {
                if (girlCount.Values.Sum() < 20)
                {
                    LaunchGirl();
                }
                else
                {
                    Debug.LogFormat("lose! :(");
                }

                launchTime = 0;
            }
        }
    }


    private void LaunchGirl()
    {
        if (prefabList.Length <= 0) return;

        var index = Random.Range(0, prefabList.Length);
        var prefab = prefabList[index];

        var newGirl = Instantiate(prefab, transform);
        newGirl.name = prefab.name;

        var pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-8f, 4f), 0);
        //pos.z = (pos.x + 3f) / 10f;
        newGirl.transform.localPosition = pos;

        var controller = newGirl.GetComponent<GirlController>();

        controller.velocity = Random.Range(0.8f, 1.4f);
        //идет вправо
        if (Random.value >= 0.5f) controller.TurnRound();

        if (!girlCount.ContainsKey(prefab.name)) girlCount.Add(prefab.name, 0);
        girlCount[prefab.name]++;
    }

    private void CheckChange(string name)
    {
        var minCount = girlCount.Values.Min();

        Debug.LogFormat("click {0} ({1}/{2})", name, girlCount[name], minCount);

        if (minCount < girlCount[name])
        {
            LaunchGirl();
        }
        else
        {
            Debug.LogFormat("win! :)");
        }
    }
}
