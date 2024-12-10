using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && collision.gameObject.name.StartsWith("Pocong"))
        {
            PocongScript s = collision.gameObject.GetComponent<PocongScript>();
            if (s != null) {
                s.OnLight(true);
            
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.name.StartsWith("Pocong"))
        {
            PocongScript s = collision.gameObject.GetComponent<PocongScript>();
            if (s != null)
            {
                s.OnLight(false);

            }

        }
    }

}
