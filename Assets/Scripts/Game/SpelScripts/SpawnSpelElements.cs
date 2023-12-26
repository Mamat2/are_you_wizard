using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class SpawnSpelElements : MonoBehaviour
{

    Dictionary<string, int> listOfElements;// = new Dictionary<string, int>()
    public float spawnSpeed = 0.001f;


    void Start()
    {
        SpellManager.CreateSpellElementEvt.AddListener(CreateElement); ;
    }



    public void CreateElement(string name)
    {
        int index = 1;
        while (GameObject.Find(name + index) != null)
        {
            index++;
        }

        GameObject obj = new GameObject(name + index);


        obj.AddComponent<SphereCollider>();
        obj.GetComponent<SphereCollider>().radius = 0.08f;


        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        string path = "SpellElementIMG/" + name;
        spriteRenderer.sprite = Resources.Load<Sprite>(path);
        Type scriptType = Type.GetType(name);
        //obj.AddComponent(scriptType.GetType());
        //obj.AddComponent(scriptType);
        // scriptType = Type.GetType("BackLigthScriprt");
        obj.GetComponent<SpriteRenderer>().sortingOrder = 20;
        GameObject backLightObj = new GameObject("BacklightObject");
        BackLigthScriprt backLightScript = backLightObj.AddComponent<BackLigthScriprt>();
        backLightObj.transform.SetParent(obj.transform);

        obj.AddComponent(scriptType);
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.tag = "AttachibleObj";
        Vector3 spawnPosition = CalculateSpawnPosition(obj);
        obj.transform.localPosition = spawnPosition;
        obj.transform.localScale = new Vector3(0.1f,0.1f, 1f);
        obj.transform.rotation = transform.rotation;
        obj.transform.SetParent(transform);
        // obj.GetComponent<CircleCollider2D>().transform.rotation = obj.transform.rotation;




        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector2 downForce = new Vector2(0f, -1f); // Направление вниз
        float forceMagnitude = 1f; // Желаемая сила движения вниз
        rb.AddForce(downForce * forceMagnitude, ForceMode.Impulse);

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        GameObject childObject = new GameObject("BacklightRing");
        childObject.transform.SetParent(obj.transform);  
        childObject.transform.localScale = new Vector3(1, 1f, 1f);
        childObject.transform.position = obj.transform.position;
        childObject.transform.rotation = transform.rotation;
        spriteRenderer = childObject.AddComponent<SpriteRenderer>();
        Sprite image = Resources.Load<Sprite>("Backlight/BacklightRing");
        if (image != null)
        {
            spriteRenderer.sprite = image;
        }



        // rb.velocity = ((Vector2)Vector3.zero - (Vector2)spawnPosition) * spawnSpeed;
        //rb.velocity = ((Vector2)Vector3.zero - (Vector2)spawnPosition).normalized * spawnSpeed;
        LineRenderer lineRenderer;
        lineRenderer = obj.AddComponent<LineRenderer>();

        // Устанавливаем настройки для LineRenderer
        //lineRenderer.positionCount = objectCollider.bounds.size.x * 10;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.material.color = Color.blue;
        lineRenderer.SetPosition(0, obj.transform.position);
        lineRenderer.SetPosition(1, obj.transform.position);
        lineRenderer.sortingOrder = 0;
        //rb.velocity = (Vector2.zero - spawnPosition) * spawnSpeed;


    }

    /*    void FillDictionary(Dictionary<string, int> tmp)
        {
            listOfElements = new Dictionary<string, int>(tmp);



        }*/
    private Vector3 CalculateSpawnPosition(GameObject obj)
    {
     /*   string debugMessage = "Calculating spawn position...\n";*/

        // Получаем размеры экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
/*        debugMessage += "Screen width: " + screenWidth + "\n";
        debugMessage += "Screen height: " + screenHeight + "\n";*/

        // Вычисляем случайное смещение в пределах экрана
        float offsetX = UnityEngine.Random.Range(-screenWidth / 2f, screenWidth / 2f);
      //  debugMessage += "Offset X: " + offsetX + "\n";
        float offsetY = UnityEngine.Random.Range(screenHeight / 2f, screenHeight);
      //  debugMessage += "Offset Y: " + offsetY + "\n";

        // Получаем позицию родительского объекта
        Vector3 parentPosition = transform.position;
      //  debugMessage += "Parent position: " + parentPosition + "\n";

        // Вычисляем позицию появления объекта с учетом смещения
        Vector3 spawnPosition = parentPosition + transform.TransformVector(new Vector3(offsetX, offsetY, 0f));
  /*      debugMessage += "Spawn position: " + spawnPosition;

        print(debugMessage);*/

        return spawnPosition;
    }

}












/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class SpawnSpelElements : MonoBehaviour
{

    Dictionary<string, int> listOfElements;// = new Dictionary<string, int>()
    public float spawnSpeed = 0f;
*//*
    public float width = 0.3f;
    public float height = 0.3f;*//*


    void Start()
    {
        SpellManager.CreateSpellElementEvt.AddListener(CreateElement); ;
        // BatleManager.spavnElementEvt.AddListener(Create);
    }


*//*    public void Create(Dictionary<string, int> tmp)
    {
        FillDictionary(tmp);
        CallCreate();
    }*/




/*
    public void CallCreate()
    {
        var rand = new System.Random();
        List<string> keys = new List<string>(listOfElements.Keys);

        while (keys.Count > 0)
        {
            int index = rand.Next(keys.Count);
            string key = keys[index];

            if (listOfElements[key] > 0)
            {
                CreateElement(key); // Передаем ссылку на BaseSpelElement в метод CreateElement
                listOfElements[key]--;
            }

            if (listOfElements[key] == 0)
            {
                keys.RemoveAt(index);
            }
        }
    }

*//*

    public void CreateElement(string name)
    {
        int index = 1;
        while (GameObject.Find(name + index) != null)
        {
            index++;
        }

        GameObject obj = new GameObject(name + index);
        obj.AddComponent<CircleCollider2D>();
        obj.GetComponent<CircleCollider2D>().radius = 0.75f;
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        string path = "SpellElementIMG/"+name;
        spriteRenderer.sprite = Resources.Load<Sprite>(path);

        GameObject backLightObj = new GameObject("BacklightObject");
        BackLigthScriprt backLightScript = backLightObj.AddComponent<BackLigthScriprt>();
        backLightObj.transform.SetParent(obj.transform);

      
        obj.AddComponent<Rigidbody2D>();
        obj.GetComponent<Rigidbody2D>().gravityScale = 0f;
        obj.transform.localScale = new Vector3(0.1f,0.1f, 1f);
        obj.tag = "AttachibleObj";

        Type scriptType = Type.GetType(name);
        obj .AddComponent (scriptType);

        //Vector3 spawnPosition = CalculateSpawnPosition();
        Vector3 spawnPosition = CalculateSpawnPosition(transform,obj );
        obj.transform.position = spawnPosition;
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
        obj.transform.rotation = transform.rotation;
        Vector3 direction = (transform.position - spawnPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // obj.transform.LookAt(transform.position);
        Quaternion parentRotation = transform.rotation;
        obj.transform.rotation = parentRotation;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        GameObject childObject = new GameObject("BacklightRing");
        childObject.transform.SetParent(obj.transform);
        childObject.transform.position = obj.transform.position;
        spriteRenderer = childObject.AddComponent<SpriteRenderer>();
        Sprite image = Resources.Load<Sprite>("Backlight/BacklightRing");
        childObject.transform.localScale = new Vector3(1, 1f, 1f);

        if (image != null)
        {
            spriteRenderer.sprite = image;
        }



        // rb.velocity = ((Vector2)Vector3.zero - (Vector2)spawnPosition) * spawnSpeed;
        //rb.velocity = ((Vector2)Vector3.zero - (Vector2)spawnPosition).normalized * spawnSpeed;
        LineRenderer lineRenderer;
        lineRenderer = obj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = Color.white;

        // Устанавливаем настройки для LineRenderer
        //lineRenderer.positionCount = objectCollider.bounds.size.x * 10;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.SetPosition(0, obj.transform.position);
        lineRenderer.SetPosition(1, obj.transform.position);
        lineRenderer.sortingOrder = 1;
        //rb.velocity = (Vector2.zero - spawnPosition) * spawnSpeed;


    }

*//*    void FillDictionary(Dictionary<string, int> tmp)
    {
        listOfElements = new Dictionary<string, int>(tmp);



    }*/


/*    private Vector3 CalculateSpawnPosition(Transform parentTransform)
    {
        // Получаем размеры экрана в мировых координатах
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Расчитываем случайное смещение от позиции родительского элемента
        float offsetX = UnityEngine.Random.Range(-screenWidth / 15f, screenWidth / 15f);
        float offsetY = UnityEngine.Random.Range(-screenHeight / 20f, screenHeight / 16f);

        // Получаем позицию родительского элемента
        Vector3 parentPosition = parentTransform.position;

        // Расчитываем позицию объекта с учетом смещения от родительского элемента
        Vector3 spawnPosition = parentPosition + new Vector3(offsetX, offsetY, 0f);

        // Возвращаем позицию объекта
        return spawnPosition;
    }
*/

/*    private Vector3 CalculateSpawnPosition(Transform parentTransform)
    {
        // Получаем размеры экрана в мировых координатах
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Расчитываем случайное смещение только по оси X
        float offsetX = UnityEngine.Random.Range(-screenWidth / 15f, screenWidth / 15f);

        // Смещение по оси Y (ближе к середине экрана)
        float offsetY = screenHeight / 3f;

        // Получаем позицию родительского элемента
        Vector3 parentPosition = parentTransform.position;

        // Расчитываем позицию объекта с учетом смещения от родительского элемента
        Vector3 spawnPosition = parentPosition + new Vector3(offsetX, offsetY, 0f);

        // Задаем начальную скорость объекта в направлении от верхней части экрана к нижней
        Vector2 spawnDirection = Vector2.down;
        float spawnSpeed = 5f; // Здесь можно задать желаемую скорость падения
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.AddForce(spawnDirection * spawnSpeed, ForceMode2D.Impulse);

        // Возвращаем позицию объекта
        return spawnPosition;
    }
*/
/*      private Vector3 CalculateSpawnPosition(Transform parentTransform, GameObject obj)
      {
          float parentHeight = parentTransform.localScale.y;
          float parentWidth = parentTransform.localScale.x;

          float offsetX = UnityEngine.Random.Range(-parentWidth / 2f, parentWidth / 2f);
          float offsetY = UnityEngine.Random.Range(-parentHeight / 2f, parentHeight / 2f);

          Vector3 parentPosition = parentTransform.localPosition;

          // Изменение здесь: используем TransformPoint вместо TransformVector
          Vector3 spawnPosition = parentTransform.TransformPoint(new Vector3(offsetX, offsetY, 0f));

          Vector2 spawnDirection = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-15f, 15f)) * -parentTransform.up;

          float spawnSpeed = 5f;
          Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
          rb.gravityScale = 0f;

          // Изменение здесь: используем TransformDirection вместо TransformVector
          rb.velocity = parentTransform.TransformDirection(spawnDirection) * spawnSpeed;

          return spawnPosition;
      }*/
/* private Vector3 CalculateSpawnPosition(Transform parentTransform, GameObject obj)
 {
     // Получаем размеры родительского элемента в мировых координатах
     float parentHeight = parentTransform.localScale.y;
     float parentWidth = parentTransform.localScale.x;

     // Расчитываем случайное смещение по осям x и y в пределах половины ширины и высоты родительского элемента
     float offsetX = UnityEngine.Random.Range(-parentWidth / 2f, parentWidth / 2f);
     float offsetY = UnityEngine.Random.Range(-parentHeight / 2f, parentHeight / 2f);

     // Получаем позицию родительского элемента
     Vector3 parentPosition = parentTransform.localPosition;

     // Расчитываем позицию объекта с учетом смещения от родительского элемента
     Vector3 spawnPosition = parentPosition + parentTransform.TransformVector(new Vector3(offsetX, offsetY, 0f));

     // Расчитываем случайное направление спавна в пределах определенного диапазона
     Vector2 spawnDirection = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-15f, 15f)) * -parentTransform.up;

     // Задаем начальную скорость объекта для движения в плоскости интерфейса игрока
     float spawnSpeed = 5f; // Здесь можно настроить желаемую скорость
     Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
     rb.gravityScale = 0f;
     rb.velocity = spawnDirection * spawnSpeed;

     // Возвращаем позицию объекта
     return spawnPosition;
 }*//*
private Vector3 CalculateSpawnPosition(Transform parentTransform, GameObject obj)
{
    // Получаем размеры родительского элемента в мировых координатах
    float parentHeight = parentTransform.localScale.y;
    float parentWidth = parentTransform.localScale.x;

    // Расчитываем случайное смещение по оси x в пределах половины ширины родительского элемента
    float offsetX = UnityEngine.Random.Range(-parentWidth / 2f, parentWidth / 2f);

    // Смещение по оси y (верхняя часть экрана)
    float offsetY = parentHeight / 2f;

    // Получаем позицию родительского элемента
    Vector3 parentPosition = parentTransform.position;

    // Расчитываем позицию объекта с учетом смещения от родительского элемента
    Vector3 spawnPosition = parentPosition + new Vector3(offsetX, offsetY, 0f);

    // Задаем начальную скорость объекта в направлении от верхней части экрана к нижней
    Vector2 spawnDirection = Vector2.down;
    float spawnSpeed = 5f; // Здесь можно задать желаемую скорость падения
    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    rb.gravityScale = 0f;
    rb.AddForce(spawnDirection * spawnSpeed, ForceMode2D.Impulse);

    // Возвращаем позицию объекта
    return spawnPosition;
}


}





*/