using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestText : MonoBehaviour
{
    // Start is called before the first frame update

    public Text questText;
    // Update is called once per frame
    float distanceToMove = 700.0f; // Расстояние, на которое нужно сдвинуть объект
    float moveSpeed = 250.0f; // Скорость движения объекта

    private Vector3 originalPosition; // Исходная позиция объекта
    private Vector3 targetPosition; // Конечная позиция объекта  // Конечная позиция объекта
    private bool isMoving = false; // Флаг, указывающий на то, двигается ли объект в данный момент

    void Start()
    {
        // Запоминаем исходную позицию объекта
        originalPosition = transform.localPosition;
        GameManager.UpadteQuestEvt.AddListener(UpdateText);

    }

    void UpdateText(Dictionary<string, int> quests)
    {
        //print("Aasfsdfsdf");
        var text = questText.GetComponent<Text>().text ="";
/*        text = "";*/
        foreach (var quest in quests)
        {
            if (quest.Key == "killingSpree")
            {
             questText.GetComponent<Text>().text += "убейте 3 монстров -  " + quest.Value.ToString()+ "\n";
            }
            if (quest.Key == "RedRooms")
            {
                questText.GetComponent<Text>().text += "пройди во 2 красную комнату ты в комнате -  " + quest.Value.ToString() + "\n";

            }

            //print(quest.Key + " " + quest.Value);
        }
      //  questText.GetComponent<Text>().text = "sdfsdfsd";
    }

    void Update()
    {
        if (isMoving)
        {
            // Вычисляем новую позицию объекта на каждом кадре
            Vector3 newPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Обновляем позицию объекта
            transform.localPosition = newPosition;

            // Проверяем, достиг ли объект конечной позиции
            if (transform.localPosition == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    void OnMouseDown()
    {
        // Проверяем текущую позицию объекта
        if (transform.localPosition == originalPosition)
        {
            // Вычисляем новую конечную позицию объекта относительно локальных координат родителя
            targetPosition = transform.localPosition - new Vector3(distanceToMove, 0, 0);
        }
        else
        {
            // Возвращаем объект на исходную позицию
            targetPosition = originalPosition;
        }

        // Устанавливаем флаг, указывающий на то, что объект нужно двигать
        isMoving = true;
    }
}
