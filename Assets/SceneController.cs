using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    private const int maxGirl = 20;
    private const float launchDelay = 2.0011f * 2;

    [SerializeField] private GameObject[] prefabList;
    [SerializeField] private Text countText;
    private Camera mainCamera;

    private readonly Dictionary<string, int> girlCount = new Dictionary<string, int>(4);

    private float launchTime;
    private bool isWin;

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

        for (var i = 0; i < 4; i++) LaunchGirl();
        UpdateCounter();
    }

    void Update()
    {
        if (isWin) //перезапускаем игру после победы
        {
            if (Input.GetMouseButtonDown(0))
                WinController.WinGame();
            return;
        }

        launchTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.transform != null)
            {
                CheckChange(hit.transform.gameObject);
            }

        }
        else
        {
            if (launchTime > launchDelay) //со временем девушек прибывает
            {
                if (girlCount.Values.Sum() < maxGirl)
                {
                    LaunchGirl();
                    SoundManager.Instance.PlayHello();
                    UpdateCounter();
                }
                else
                {
                    Debug.LogFormat("lose! :(");
                    launchTime = float.MinValue;
                }
            }
        }
    }


    private void LaunchGirl(string prefabName = "")
    {
        launchTime = 0;
        if (prefabList.Length <= 0) return;

        var index = Random.Range(0, prefabList.Length);

        if (!string.IsNullOrEmpty(prefabName))
            for (var i = 0; i < prefabList.Length; i++)
                if (prefabList[i].name == prefabName)
                    index = i;

        var prefab = prefabList[index];

        var newGirl = Instantiate(prefab, transform);
        newGirl.name = prefab.name;

        //разбрасываем по экрану
        var pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-8f, 4f), 0);
        newGirl.transform.localPosition = pos;

        var controller = newGirl.GetComponent<GirlController>();
        controller.velocity = Random.Range(0.8f, 1.4f);
        //идет вправо
        if (Random.value >= 0.5f) controller.TurnRound();

        var animator = newGirl.GetComponent<Animator>();
        animator.speed = Mathf.Abs(controller.velocity);

        if (!girlCount.ContainsKey(prefab.name)) girlCount.Add(prefab.name, 0);
        girlCount[prefab.name]++;
    }

    private void CheckChange(GameObject selectGirl)
    {
        var minCount = girlCount.Values.Min();
        var uniqueCount = girlCount.Values.Count(g => g == minCount);

        Debug.LogFormat("click {0} ({1}/{2}x{3})", selectGirl.name, girlCount[selectGirl.name], minCount, uniqueCount);

        if (uniqueCount > 1) //две и более девушки в меньшинстве
        {
            LaunchGirl(selectGirl.name);
            LaunchGirl();
            SoundManager.Instance.PlayHello();
            selectGirl.GetComponent<GirlController>().TurnRound();
            UpdateCounter();
        }
        else if (minCount < girlCount[selectGirl.name]) //это не меньшинство
        {
            LaunchGirl();
            SoundManager.Instance.PlayHello();
            UpdateCounter();
        }
        else //самая неповторимая!
        {
            Debug.LogFormat("win! :)");
            SoundManager.Instance.PlayYeah();

            foreach (var girl in FindObjectsOfType<GirlController>())
                girl.gameObject.SetActive(false);

            selectGirl.SetActive(true);
            isWin = true;
            return;
        }

        if (girlCount.Values.Sum() >= maxGirl) //девушек стало слишком ного - начинаем заново!
        {
            StartGame();
        }
    }

    private void UpdateCounter()
    {
        var sum = girlCount.Values.Sum();
        var min = girlCount.Values.Min();

        var minCnt = girlCount.Values.Count(n=>n==min);

        countText.text = string.Format("{0}/{1}", minCnt, sum);

    }
}
