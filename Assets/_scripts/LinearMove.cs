using UnityEngine;
using DG.Tweening;
public class LinearMove : MonoBehaviour
{


    void Start()
    {
        transform.DOLocalMove(new Vector3(5, transform.localPosition.y, 0), 2f);


    }


    void Update()
    {
        
    }
}
