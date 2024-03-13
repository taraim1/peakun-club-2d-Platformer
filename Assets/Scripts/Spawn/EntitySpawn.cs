using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EntitySpawn : MonoBehaviour
{
    public static EntitySpawn instance;

    public int currentSavePoint; // 이거 SavePoint에서 쓰고 있음

    public GameObject greenSlime_prefab;
    public GameObject BlueSlimeWithBallon_prefab;
    public GameObject FallingPlatform_prefab;

    public List<bool> isEntitiesOfSpawnPointSpawned = new List<bool>(); //소환 확인 리스트, PlayerDeath에서도 참조중
    int EnemySpawnTriggerNum;
    public List<Vector3> linearTempDestinations = new List<Vector3>();
    GameObject clone;

    void spawnEntity(string type, Vector3 pos) //개체 스폰 함수, type의 개체를 pos 위치에 소환
    {
        GameObject entity = greenSlime_prefab;
        switch (type) // 소환할 개체 설정
        {
            case "greenSlime":
                entity = greenSlime_prefab; //기본 개체
                break;
            case "blueSlimeWithBallon":
                entity = BlueSlimeWithBallon_prefab;
                break;
            case "fallingPlatform":
                entity = FallingPlatform_prefab;
                break;

        }

        clone = Instantiate(entity, pos, Quaternion.identity);
        clone.tag = "CloneEntity";
    }


    void SetLinearMoveDestinations() 
    {
        for (int i = 0; i < linearTempDestinations.Count; i++)//얕은복사 (무식하게) 해결
        {
            clone.GetComponent<EnemyLinearMove>().destinations.Add(linearTempDestinations[i]);   
        }


    }

    void spawnEntityRegardingTriggerNum(int triggerNum) //소환 트리거 번호에 따라 맞는 개체 스폰
    {
        switch (triggerNum)
        {
            case 0:
                spawnEntity("greenSlime", new Vector3(45, 3f, 1.5f));
                spawnEntity("greenSlime", new Vector3(54, 3f, 1.5f));
                break;
            case 1:
                spawnEntity("blueSlimeWithBallon", new Vector3(97, 8.3f, 1.5f));
                linearTempDestinations.Clear();
                linearTempDestinations.Add(new Vector3(85, 8.3f, 1.5f));
                linearTempDestinations.Add(new Vector3(97, 8.3f, 1.5f));
                SetLinearMoveDestinations();
                clone.GetComponent<EnemyLinearMove>().DoThisObjectLinearMove = true;
                clone.GetComponent<EnemyLinearMove>().startTrembleY = true;

                spawnEntity("blueSlimeWithBallon", new Vector3(120, 6.5f, 1.5f));
                clone.GetComponent<EnemyLinearMove>().startTrembleY = true;

                spawnEntity("blueSlimeWithBallon", new Vector3(128, 6.5f, 1.5f));
                linearTempDestinations.Clear();
                linearTempDestinations.Add(new Vector3(128, 6.5f, 1.5f));
                linearTempDestinations.Add(new Vector3(135, 6.5f, 1.5f));
                SetLinearMoveDestinations();
                clone.GetComponent<EnemyLinearMove>().DoThisObjectLinearMove = true;
                clone.GetComponent<EnemyLinearMove>().startTrembleY = true;

                spawnEntity("blueSlimeWithBallon", new Vector3(150, 6.5f, 1.5f));
                spawnEntity("blueSlimeWithBallon", new Vector3(157, 6.5f, 1.5f));
                spawnEntity("blueSlimeWithBallon", new Vector3(164, 6.5f, 1.5f));
                spawnEntity("blueSlimeWithBallon", new Vector3(171, 6.5f, 1.5f));
                spawnEntity("blueSlimeWithBallon", new Vector3(178, 6.5f, 1.5f));
                spawnEntity("blueSlimeWithBallon", new Vector3(185, 6.5f, 1.5f));
                break;
            case 2:
                spawnEntity("fallingPlatform", new Vector3(225, 10.7f, 1f));
                spawnEntity("fallingPlatform", new Vector3(236.5f, 10.7f, 1f));
                break;
            case 3:
                spawnEntity("fallingPlatform", new Vector3(267.5f, 0f, 1f));
                clone.transform.localScale = new Vector3(2f, 6f, 1f);
                spawnEntity("greenSlime", new Vector3(280, -4.5f, 1.5f));
                spawnEntity("greenSlime", new Vector3(273, -11.5f, 1.5f));
                break;
        }
        isEntitiesOfSpawnPointSpawned[triggerNum] = true;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemySpawnTrigger")
        {

            //EnemySpawnTrigger들 이름에서 숫자 뺴내는 작업
            //EnemySpawnTrigger 태그 오브젝트 이름은 EnemySpawnTrigger (숫자) 형태여야함
            string TriggerName = other.gameObject.name;
            TriggerName = TriggerName.Substring(19, TriggerName.Length - 20);
            EnemySpawnTriggerNum = int.Parse(TriggerName);
            if (isEntitiesOfSpawnPointSpawned[EnemySpawnTriggerNum] == false)
            {
                spawnEntityRegardingTriggerNum(EnemySpawnTriggerNum);
            }
        }

    }
    private void Awake()
    {
        if (instance == null) //싱글톤 생성
        {
            instance = this;
        }
    }
    void Start()
    {

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemySpawnTrigger").Length; i++) //적 소환 확인 리스트 초기화
        {
            isEntitiesOfSpawnPointSpawned.Add(false);
        }
    }
}


