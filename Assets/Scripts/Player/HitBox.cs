using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // Game Object
    private Renderer hitBox;
    private Color hitBoxColor = Color.magenta;
    private PlayerController playerComponent;
    private AreaEffector2D directForceAE;
    // Might need to make radial force PE and magnetic field PE the same to dry code
    private PointEffector2D radialForcePE;
    private PointEffector2D magneticFieldPE;
    // private float directAttackSpeed = 40;

    // Navigation
    private float attackDirection = -1;

    // Attack status
    private bool directForceActive = false;
    private bool radialForceActive = false;
    private int pullForceActive = 1;

    // Interacting with other game object variables
    // enum MagnitudeType {Blue, Purple, Green, Yellow, Red};
    private Dictionary<string, Dictionary<int, GameObject>> collectedObjects;
    private Dictionary<int, GameObject> collisionCollectedObject;
    // private MagneticAttraction collisionComponent;
    // Initiation
    private MagneticAttraction collisionMetalObject;
    private bool hitBoxInitiated = false;

    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Renderer>();
        playerComponent = transform.GetComponentInParent<PlayerController>();
        directForceAE = GetComponent<AreaEffector2D>();
        radialForcePE = GetComponent<PointEffector2D>();
        // magneticFieldPE = GetComponent<PointEffector2D>();
        collectedObjects = new Dictionary<string, Dictionary<int, GameObject>>();
        collisionCollectedObject = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Make a function that checks the state of the collected objects vs 2 functions here for different purposes
        if (this.name == "Radial Hit Box")
        {
            CollectObjects();
            ReleasedCollectedObjects();
            UpdateMagneticField();
        }
        TriggerAttacks();
    }

    void TriggerAttacks()
    {
        attackDirection = playerComponent.isFacingLeft;

        // Check whether player is pulling or pushing objects
        if (Input.GetKey(KeyCode.Q))
        {
            pullForceActive = -1;
        } else
        {
            pullForceActive = 1;
        }

        // Trigger attacks
        if (Input.GetKeyDown(KeyCode.E) && this.name == "Direct Hit Box" && !radialForceActive)
        {
            if (attackDirection < 0)
            {
                directForceAE.forceAngle = 180;
            } else
            {
                directForceAE.forceAngle = 0;
            }
            directForceAE.forceMagnitude = 200 * pullForceActive;
            directForceActive = true;
            hitBox.material.SetColor("_Color", Color.green);
        }

        if (Input.GetKeyDown(KeyCode.R) && this.name == "Radial Hit Box" && !directForceActive)
        {
            radialForcePE.forceMagnitude = 200 * pullForceActive;
            radialForceActive = true;
            hitBox.material.SetColor("_Color", Color.green);
        }

        // Put key up conditions to turn force magnitude back to 0
        if (Input.GetKeyUp(KeyCode.E) && this.name == "Direct Hit Box")
        {
            directForceAE.forceMagnitude = 0;
            directForceActive = false;
            hitBox.material.SetColor("_Color", Color.white);
        }

        if (Input.GetKeyUp(KeyCode.R) && this.name == "Radial Hit Box")
        {
            radialForcePE.forceMagnitude = 0;
            radialForceActive = false;
            hitBox.material.SetColor("_Color", Color.white);
        }
    }
// Move these next 2 functions into Trigger Enter and Exit functions
    void CollectObjects()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W KEY PRESSED");
            Debug.Log("Tag count: " + collectedObjects.Count);
            Debug.Log("In range: " + collectedObjects.Count);
            foreach (KeyValuePair<string, Dictionary<int, GameObject>> magnitudeTypes in collectedObjects )
            {
                Debug.Log("LOOP 1");
                foreach (KeyValuePair<int, GameObject> collectedObject in magnitudeTypes.Value )
                {
                    Debug.Log("LOOP 2");
                    collectedObject.Value.layer = 8;
                }
            }
        }
    }

    void ReleasedCollectedObjects()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (KeyValuePair<string, Dictionary<int, GameObject>> magnitudeTypes in collectedObjects )
            {
                foreach (KeyValuePair<int, GameObject> collectedObject in magnitudeTypes.Value )
                {
                    collectedObject.Value.layer = 9;
                }
            }
            collectedObjects.Clear();
            collisionCollectedObject.Clear();
        }
    }

    void UpdateMagneticField()
    {
        // Dry code by setting Vector values with variables and assigning them after all level checks
        if (collectedObjects.Count > 4 && collectedObjects.Count < 10)
        {
            Debug.Log("COLLECTED 5 ITEMS!!!!!!!!!!");
            radialForcePE.transform.localScale += new Vector3(50, 50, 1);
            // magneticFieldPE.transform.localScale += new Vector3(50, 50, 1);
            hitBox.material.SetColor("_Color", Color.magenta);
        } else
        {
            radialForcePE.transform.localScale += new Vector3(40, 40, 1);
            // magneticFieldPE.transform.localScale += new Vector3(40, 40, 1);
            hitBox.material.SetColor("_Color", Color.white);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Metal Object"))
        {
            if (collectedObjects.ContainsKey(collision.tag))
            {
                if (!collectedObjects[collision.tag].ContainsKey(collisionMetalObject.metalObjectId))
                {
                    collectedObjects[collision.tag].Add(collisionMetalObject.metalObjectId, collision.gameObject);
                    Debug.Log("adding... " + collectedObjects[collision.tag].Count);
                }
                if (collectedObjects[collision.tag].Count > 4 && !hitBoxInitiated)
                {
                    collectedObjects.Clear();
                    hitBoxInitiated = true;
                }
            } else
            {
                collisionCollectedObject.Add(collisionMetalObject.metalObjectId, collision.gameObject);
                collectedObjects.Add(collision.tag, collisionCollectedObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionMetalObject = collision.gameObject.GetComponent<MagneticAttraction>();
        // Called when objects leave the hitbox
        if (collision.name.Contains("Metal Object"))
        {
            if (collectedObjects.Count > 0)
            {
                collectedObjects[collision.tag].Remove(collisionMetalObject.metalObjectId);
                Debug.Log("removing... " + collectedObjects[collision.tag].Count);
            }
        }
    }
}