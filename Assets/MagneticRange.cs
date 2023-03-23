using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticRange : MonoBehaviour
{
    // Game Object
    private Renderer hitBox;
    private Transform magneticField;
    private Vector3 scaleChange;
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
    private Dictionary<string, Dictionary<int, GameObject>> inRangeObjects;
    private Dictionary<string, Dictionary<int, GameObject>> collectedObjects;
    private Dictionary<int, GameObject> collisionInRange;
    private Dictionary<string, bool> attractableObjectTypes;
    // private MagneticAttraction collisionComponent;
    private MagneticAttraction collisionMetalObject;
    private Dictionary<string, float> magneticRangeLevels;

    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Renderer>();
        playerComponent = transform.GetComponentInParent<PlayerController>();
        directForceAE = GetComponent<AreaEffector2D>();
        radialForcePE = GetComponent<PointEffector2D>();
        // magneticFieldPE = GetComponent<PointEffector2D>();
        inRangeObjects = new Dictionary<string, Dictionary<int, GameObject>>();
        collectedObjects = new Dictionary<string, Dictionary<int, GameObject>>();
        collisionInRange = new Dictionary<int, GameObject>();

        // The levels Prism's magnetic range can reach
        magneticField = GetComponent<Transform>();
        magneticRangeLevels = new Dictionary<string, float>();
        magneticRangeLevels.Add("White", 40f);
        magneticRangeLevels.Add("Yellow", 50f);
        magneticRangeLevels.Add("Orange", 60f);
        magneticRangeLevels.Add("Red", 70f);
        magneticRangeLevels.Add("Pink", 80f);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerActions();
    }

    void PlayerActions()
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
        // if (Input.GetKeyDown(KeyCode.E) && this.name == "Direct Hit Box" && !radialForceActive)
        // {
        //     if (attackDirection < 0)
        //     {
        //         directForceAE.forceAngle = 180;
        //     } else
        //     {
        //         directForceAE.forceAngle = 0;
        //     }
        //     directForceAE.forceMagnitude = 200 * pullForceActive;
        //     directForceActive = true;
        //     hitBox.material.SetColor("_Color", Color.green);
        // }

        if (Input.GetKeyDown(KeyCode.R) && this.name == "Magnetic Range")
        {
            if (pullForceActive > 0)
            {
                ReleaseCollectedObjects();
            }

            radialForcePE.forceMagnitude = 600 * pullForceActive;
            radialForceActive = true;
            hitBox.material.SetColor("_Color", Color.green);
            
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CollectInRangeObjects();
        }
        // Put key up conditions to turn force magnitude back to 0
        // if (Input.GetKeyUp(KeyCode.E) && this.name == "Direct Hit Box")
        // {
        //     directForceAE.forceMagnitude = 0;
        //     directForceActive = false;
        //     hitBox.material.SetColor("_Color", Color.white);
        // }

        if (Input.GetKeyUp(KeyCode.R) && this.name == "Magnetic Range")
        {
            radialForcePE.forceMagnitude = 0;
            radialForceActive = false;
            hitBox.material.SetColor("_Color", Color.white);
        }
    }
// Move these next 2 functions into Trigger Enter and Exit functions
    void CollectInRangeObjects()
    {
        foreach (KeyValuePair<string, Dictionary<int, GameObject>> magnitudeType in inRangeObjects )
        {
            Dictionary<int, GameObject> objectsOfMagnitudeType = new Dictionary<int, GameObject>();
            foreach (KeyValuePair<int, GameObject> inRangeObject in magnitudeType.Value )
            {
                // Debug.Log("Collecting objects... " + inRangeObject.Key);
                if (collectedObjects.ContainsKey(magnitudeType.Key))
                {
                    if (!collectedObjects[magnitudeType.Key].ContainsKey(inRangeObject.Key))
                    {
                        collectedObjects[magnitudeType.Key].Add(inRangeObject.Key, inRangeObject.Value);
                    }
                } else {
                    objectsOfMagnitudeType.Add(inRangeObject.Key, inRangeObject.Value);
                }
                inRangeObject.Value.layer = 8;
            }
            if (!collectedObjects.ContainsKey(magnitudeType.Key))
            {
                collectedObjects.Add(magnitudeType.Key, objectsOfMagnitudeType);
            }
        }
        inRangeObjects.Clear();
        UpdateMagneticField();
    }

    void ReleaseCollectedObjects()
    {
        // NEXT STEP - fix this function then UpdateMagneticField();
        foreach (KeyValuePair<string, Dictionary<int, GameObject>> magnitudeType in collectedObjects )
        {
            foreach (KeyValuePair<int, GameObject> collectedObject in magnitudeType.Value )
            {
                // Debug.Log("Releasing objects...");
                collectedObject.Value.layer = 9;
            }
        }
        collectedObjects.Clear();
        UpdateMagneticField();
    }

    void UpdateMagneticField()
    {
        float newMagneticFieldSize = 40f;
        foreach (MagneticAttraction thisMagneticObject in Resources.FindObjectsOfTypeAll(typeof(MagneticAttraction)) as MagneticAttraction[])
        {
            if (collectedObjects.Count < 1 && thisMagneticObject.metalObjectType != "White")
            {
                thisMagneticObject.isAttractable = false;
            }
            else if (collectedObjects.ContainsKey(thisMagneticObject.requiredObjectTypeForAttraction))
            {
                if (collectedObjects[thisMagneticObject.requiredObjectTypeForAttraction].Count >= thisMagneticObject.requiredObjectsForAttraction)
                {
                    thisMagneticObject.isAttractable = true;
                    if ( magneticRangeLevels[thisMagneticObject.metalObjectType] > newMagneticFieldSize)
                    {
                        newMagneticFieldSize = magneticRangeLevels[thisMagneticObject.metalObjectType];
                    }
                }
            }
        }
        magneticField.localScale = new Vector3(newMagneticFieldSize, newMagneticFieldSize, 0f);
        // scaleChange = new Vector3(newMagneticFieldSize, newMagneticFieldSize, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Metal Object"))
        {
            collisionMetalObject = collision.gameObject.GetComponent<MagneticAttraction>();
            if (collisionMetalObject.isAttractable)
            {
                if (!inRangeObjects.ContainsKey(collisionMetalObject.metalObjectType))
                {
                    collisionInRange.Add(collisionMetalObject.metalObjectId, collision.gameObject);
                    inRangeObjects.Add(collisionMetalObject.metalObjectType, new Dictionary<int, GameObject>(collisionInRange));
                } else
                {
                    if (!inRangeObjects[collisionMetalObject.metalObjectType].ContainsKey(collisionMetalObject.metalObjectId))
                    {
                        inRangeObjects[collisionMetalObject.metalObjectType].Add(collisionMetalObject.metalObjectId, collision.gameObject);
                    }
                }
                // Debug.Log("objects count... " + inRangeObjects[collisionMetalObject.metalObjectType].Count);
            }
        }
        collisionInRange.Clear();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Contains("Metal Object"))
        {
            collisionMetalObject = collision.gameObject.GetComponent<MagneticAttraction>();
            // Debug.Log("metal object... " + collisionMetalObject.metalObjectId);   
             if (inRangeObjects.ContainsKey(collisionMetalObject.metalObjectType))
            {
                if (inRangeObjects[collisionMetalObject.metalObjectType].ContainsKey(collisionMetalObject.metalObjectId))
                {
                    inRangeObjects[collisionMetalObject.metalObjectType].Remove(collisionMetalObject.metalObjectId);
                }
            }
        }
    }
}
