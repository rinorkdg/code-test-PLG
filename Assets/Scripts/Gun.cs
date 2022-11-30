using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
using Platformer.UI;

public class Gun : MonoBehaviour
{
    public float fireRange = 100f;

    private Vector3 mousePositionInWorld;
    private Vector3 deltaMouseToObject;
    private float rotationZ;

    private LineRenderer lineRenderer;
    private MetaGameController metaGameController;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        metaGameController = FindObjectOfType<MetaGameController>();
    }
    
    private void Update()
    {
        if (!metaGameController.GameIsPaused)
        {
            //get the position of the mouse in worldspace, we're working with Vector3 since we want to rotate our gun in the XY plane, around the Z axis (depth)
            mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //the difference between the mouse position and our position will give as a result that is analogues to an imaginary right-angle triangle
            deltaMouseToObject = mousePositionInWorld - transform.position;
            //using our X and Y values, corresponing to the horizontal and verticle sides of our imaginary triangle, we can calculate the angle of our imaginer hypotenuse; the line going from the gun to the mouse
            rotationZ = Mathf.Atan2(deltaMouseToObject.y, deltaMouseToObject.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, fireRange);

        if (hit.collider != null)
        {
            StartCoroutine(DrawFireLine(hit.point));

            if(hit.transform.CompareTag("Enemy"))
            {
                Schedule<EnemyDeath>().enemy = hit.transform.GetComponent<EnemyController>();
                Destroy(hit.transform.gameObject, 5f);
            }
        }
        else
        {
            Vector3 lineEnd = transform.position + transform.right * fireRange;
            StartCoroutine(DrawFireLine(lineEnd));
        }
    }

    private IEnumerator DrawFireLine(Vector3 endPoint)
    {
        lineRenderer.enabled = true;
        Vector3[] linePoints = new Vector3[2];
        linePoints[0] = transform.position;
        linePoints[1] = endPoint;
        lineRenderer.SetPositions(linePoints);
        yield return new WaitForSeconds(0.16f);
        lineRenderer.enabled = false;
    }
}
