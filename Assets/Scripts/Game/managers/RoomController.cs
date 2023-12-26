using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class RoomController : MonoBehaviour
{
    public GameObject mapPrefab;
    public Transform spawnPoint;
    public Quaternion rotationOffset; 
    public static UnityEvent<int,int> RoomNumberEvt = new UnityEvent<int, int>();


    Dictionary<int, int> roomNumber = new Dictionary<int, int>()
    {
        { 1,0 },
        { 2,0 },
    };
    int lastTrigger = 0;

    public GameObject trigger;
    public List<GameObject> QuestItems;
 float distanceFromTrigger = 100;
  

    void Start()
    {
        GameManager.NextRoomEvt.AddListener(changeRoomNumber);
        //GameManager.NextRoomEvt.AddListner();
    }





    void changeRoomNumber(NextRoom trigger, int questNumber)
    {
        // RemoveObjectsAndChildren(trigger);
        RemoveObjects(trigger);
        GameObject[] objectsToKeep = trigger.objectsToKeep;



        if (lastTrigger != trigger.number)
        {
            roomNumber[trigger.number] += 1;
        }
        else
        {
            roomNumber[trigger.number] = 0;
        }
        //print("Change room number work\n" + number + " " + roomNumber[number]);
        lastTrigger = trigger.number;
        SpawnRoom(trigger,questNumber);
        RoomNumberEvt.Invoke(roomNumber[1], roomNumber[2]);
         
    }
    void SpawnRoom(NextRoom trigger, int questNumber )
    {


       // RemoveObjects(trigger);


            Vector3 triggerPosition = trigger.transform.position;
            Vector3 newPosition = new Vector3(triggerPosition.x, triggerPosition.y, triggerPosition.z) + trigger.transform.forward * distanceFromTrigger;
            GameObject room = Instantiate(mapPrefab, newPosition, trigger.transform.rotation);
        if (questNumber == 0)
        {


        }

    }

    void RemoveObjects(NextRoom trigger)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        GameObject[] objectsToKeep = trigger.objectsToKeep;
        List<GameObject> objectsToRemove = new List<GameObject>(); // Создаем список для объектов, которые нужно удалить
        SetIndestructibleTag(objectsToKeep);



        foreach (GameObject obj in allObjects)
        {
            if (!objectsToKeep.Contains(obj) && obj.tag != "indestructible" && obj.tag != "MainCamera")
            {
                objectsToRemove.Add(obj); // Добавляем объект в список для удаления
            }
        }
        foreach (GameObject objToRemove in objectsToRemove)
        {
            if (HasChildObject(objToRemove, objectsToKeep))
            {
                // Если у объекта есть дочерний объект, который должен быть сохранен, пропускаем удаление
                continue;
            }
            Destroy(objToRemove);
        }

        RemoveIndestructibleTag(objectsToKeep);
    }

    bool HasChildObject(GameObject parent, GameObject[] objectsToKeep)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject childObj = child.gameObject;
            if (objectsToKeep.Contains(childObj) || childObj.tag == "indestructible" || childObj.tag == "MainCamera")
            {
                return true; // Если есть дочерний объект, который должен быть сохранен, возвращаем true
            }
        }
        return false;
    }

    void SetIndestructibleTag(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.tag = "indestructible"; // Присваиваем тег "indestructible" текущему объекту
            SetIndestructibleTagRecursive(obj.transform); // Вызываем рекурсивную функцию для дочерних объектов
        }
    }

    void SetIndestructibleTagRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject childObj = child.gameObject;
            childObj.tag = "indestructible"; // Присваиваем тег "indestructible" дочернему объекту
            SetIndestructibleTagRecursive(child); // Рекурсивно вызываем функцию для дочерних объектов
        }
    }

    void RemoveIndestructibleTag(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.tag = "Untagged"; // Снимаем тег со всех объектов
            RemoveIndestructibleTagRecursive(obj.transform); // Вызываем рекурсивную функцию для дочерних объектов
        }
    }

    void RemoveIndestructibleTagRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject childObj = child.gameObject;
            childObj.tag = "Untagged"; // Снимаем тег с дочернего объекта
            RemoveIndestructibleTagRecursive(child); // Рекурсивно вызываем функцию для дочерних объектов
        }
    }




    void Update()
    {
    }
}





