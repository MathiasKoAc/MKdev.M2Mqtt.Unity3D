using UnityEngine;

namespace MKdev.MqttForUnity.MqttExample
{
    public class CreateObject : MonoBehaviour
    {

        public GameObject objectToCreate;
        public Transform transformPosition;
        public GameObject newGameObject;

        public void CreateIt()
        {
            newGameObject = GameObject.Instantiate(objectToCreate, transformPosition);
        }
    }
}