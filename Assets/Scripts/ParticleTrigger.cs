using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    // Use this for initialization
     void Start () {
         GetComponent<ParticleSystem>().Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
