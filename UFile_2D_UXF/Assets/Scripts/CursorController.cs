using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorController : MonoBehaviour
{

    //camera to get world position from (main camera)
    public Camera cam;

    private void Start()
    {
        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }

    //Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 9f));

        transform.localPosition = worldPos - transform.parent.transform.position;
        //print(worldPos);
    }
}
