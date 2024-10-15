using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
     // Posizione del cubo (di default al centro della scena)
    public Transform target; // Riferimento al cubo
    public float speed = 5f; // Velocità del drone
    public float stoppingDistance = 50.5f; // Distanza di fermata

    private Rigidbody rb; 

    private Vector3 startPosition;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;
        if (target == null)
        {
            // Se non è stato assegnato nessun target, trova un oggetto chiamato "Cubo"
            target = GameObject.Find("house").transform;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Calcola la direzione verso il target
            Vector3 direction = (target.position - transform.position).normalized;

            // Calcola la distanza attuale dal target
            float distance = Vector3.Distance(transform.position, target.position);

            // Se il drone è abbastanza lontano, continua a muoversi
            if (distance > stoppingDistance)
            {
                // Muovi il drone verso il target
                transform.position += direction * speed * Time.deltaTime;

                // Orienta il drone verso il cubo
                transform.LookAt(target.position);
            }
        }
    } 

     private void OnCollisionEnter(Collision collision2)
    {
        //Debug.Log("Nemico ha raggiunto il centro! Terminando episodio.");
        // Se il nemico viene toccato da uno degli agenti
        //Debug.Log("Collision detected with: " + collision2.gameObject.name);
       
        if (collision2.gameObject.CompareTag("house"))
        {
           
            // Notifica all'EnvironmentController che il nemico è stato fermato
           FindObjectOfType<EnvironmentController>().EnemyReachedCenter();
            
        }
    }

     public void ResetPosition()
    {
        transform.position = startPosition;
    }
    
}
