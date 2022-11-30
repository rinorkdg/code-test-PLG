using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
using Platformer.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private float fireRange = 100f;
    [SerializeField] private float throwForce = 100;
    public int maxAmmo = 5;
    public int ammo;

    [SerializeField] AudioClip fireSound;

    private bool hasBeenThrown = false;
    private Vector3 mousePositionInWorld;
    private Vector3 deltaMouseToObject;
    private float rotationZ;

    private LineRenderer lineRenderer;
    private MetaGameController metaGameController;
    public AudioSource audioSource;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        metaGameController = FindObjectOfType<MetaGameController>();
        audioSource = GetComponent<AudioSource>();
        ammo = maxAmmo;
    }
    
    private void Update()
    {
        //make sure you can't operate the gun while the menu is up or while it's on the ground
        if (!metaGameController.GameIsPaused && !hasBeenThrown)
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
                //if they have ammo, fire the gun
                if (ammo > 0)
                {
                    Fire();
                }
                //if they don't have ammo, THROW the gun
                else
                {
                    Throw();
                }
            }
        }
    }

    private void Fire()
    {
        ammo--;
        audioSource.PlayOneShot(fireSound);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, fireRange);

        //if the shot hits something
        if (hit.collider != null)
        {
            //draw line
            StartCoroutine(DrawFireLine(hit.point));

            //if it hits an enemy
            if(hit.transform.CompareTag("Enemy"))
            {
                //kill enemy
                Schedule<EnemyDeath>().enemy = hit.transform.GetComponent<EnemyController>();
                Destroy(hit.transform.gameObject, 5f);
            }
        }
        //if the shot hits nothing
        else
        {
            //calculate a point ourselves to draw a line
            Vector3 lineEnd = transform.position + transform.right * fireRange;
            StartCoroutine(DrawFireLine(lineEnd));
        }
    }

    private void Throw()
    {
        //unparent from the player and get physics working
        transform.parent = null;
        hasBeenThrown = true;
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = true;
        GetComponent<BoxCollider2D>().enabled = true;

        //figuring out what direction to throw in based on mouse position
        var mousePositionFromCenter = Input.mousePosition;
        mousePositionFromCenter.x -= Screen.width / 2;
        mousePositionFromCenter.y -= Screen.height / 2;

        //throw the gun and despawn after a bit to declutter
        rigidbody.AddForce(mousePositionFromCenter.normalized * throwForce);
        Destroy(gameObject, 2f);
    }

    //this function draws a line for 160ms for visual clarity
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
