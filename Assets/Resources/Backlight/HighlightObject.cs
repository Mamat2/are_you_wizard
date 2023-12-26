using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Color lightColor = Color.red; // Цвет свечения

    private Light objectLight; // Ссылка на компонент света

    private void Start()
    {
        objectLight = GetComponent<Light>(); // Получаем компонент света объекта

        if (objectLight != null)
        {
            objectLight.color = lightColor; // Устанавливаем цвет свечения
        }
    }
}
