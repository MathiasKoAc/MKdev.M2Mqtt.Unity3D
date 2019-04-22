using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

    public GameObject objectToCreate;
    public Transform transformPosition;
    public GameObject newGameObject;

    public void CreateIt()
    {
        newGameObject = GameObject.Instantiate(objectToCreate, transformPosition);
    }
}
