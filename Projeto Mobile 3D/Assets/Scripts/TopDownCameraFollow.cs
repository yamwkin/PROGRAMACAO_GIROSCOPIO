using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 20f; //distância vertical
    public float smooth = 5f; //quanto maior valor, mais suave

    private Vector3 offset; //será usada para salvar o Y fixo
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - target.position;
        offset.y = height;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!target) return;

        Vector3 desirePos = target.position + offset;
        desirePos.y = height;

        transform.position = Vector3.Lerp(transform.position, desirePos, Time.deltaTime * smooth);
    }
}
