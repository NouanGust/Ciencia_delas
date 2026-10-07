using DG.Tweening;
using UnityEngine;

public class InOutMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        transform.DOLocalMove(new Vector3(5, transform.localPosition.y, 0), 2f).SetEase(Ease.InOutSine);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
