using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatSpawner : MonoBehaviour
{
    [SerializeField]private GameObject eat,point1,point2,slider;
    private float speed; 

    private void Start()
    {
        StartCoroutine(CheckAll());
    }
    public void OnSpeedChanged()
    {
        speed = slider.GetComponent<Slider>().value;
        Debug.Log(speed);
    }
    IEnumerator CheckAll()
    {
        while(true)
        {
            yield return new WaitForSeconds(speed);
            var position = new Vector2(Random.Range(point1.transform.position.x, point2.transform.position.x),Random.Range(point1.transform.position.y, point2.transform.position.y));
            Instantiate(eat, position, Quaternion.identity);
        }
    }
}
