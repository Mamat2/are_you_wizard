using UnityEngine;


public class InvisibleWall : MonoBehaviour
{
    private Renderer wallRenderer;
    private Collider wallCollider;

    private void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallCollider = GetComponent<Collider>();
        MakeWallInvisible();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttachableObj"))
        {
            // Блокируем передвижение по оси Z
            Rigidbody attachedRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (attachedRigidbody != null)
            {
                attachedRigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttachableObj"))
        {
            // Разблокируем передвижение по оси Z
            Rigidbody attachedRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (attachedRigidbody != null)
            {
                attachedRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    private void OnMouseDown()
    {
        // Позволяем кликать и взаимодействовать с объектами за стеной
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == wallCollider)
            {
                // Обработка клика на стене
                // Можно добавить дополнительную логику взаимодействия с объектами за стеной
                RaycastHit hitBehindWall;
                if (Physics.Raycast(hit.point + ray.direction * 0.1f, ray.direction, out hitBehindWall))
                {
                    // Обработка клика на объекте за стеной
                    // Можно добавить дополнительную логику взаимодействия с объектом
                }
            }
        }
    }

    private void MakeWallInvisible()
    {
        // Делаем стену невидимой
        wallRenderer.enabled = false;
    }
}
