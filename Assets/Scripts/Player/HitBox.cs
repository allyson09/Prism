using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // Martial
    private Renderer hitBox;
    private Color hitBoxColor = Color.magenta;

    // Game Object
    private PlayerController playerComponent;
    private AreaEffector2D directForceAE;
    private PointEffector2D radialForcePE;
    private Rigidbody2D enemyRB;
    private float directAttackSpeed = 40;

    // Navigation
    private float attackDirection = -1;

    // Attack status
    private bool directForceActive = false;
    private bool radialForceActive = false;
    private int pullForceActive = 1;

    // Interacting with other game object variables
    // private Dictionary<int, Dictionary> attackRangeObjects;
    // private Dictionary<string, object> objInRange;
    private MetalObject collisionComponent;

    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Renderer>();
        playerComponent = transform.GetComponentInParent<PlayerController>();
        directForceAE = GetComponent<AreaEffector2D>();
        radialForcePE = GetComponent<PointEffector2D>();
        // hitBoxColor.a = 0;
        // attackRangeObjects = new Dictionary<int, Dictionary>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.E) && this.name == "Direct Force" && !radialForceActive)
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

        if (Input.GetKeyDown(KeyCode.R) && this.name == "Radial Force" && !directForceActive)
        {
            radialForcePE.forceMagnitude = 200 * pullForceActive;
            radialForceActive = true;
            hitBox.material.SetColor("_Color", Color.green);
        }

        // Put key up conditions to turn force magnitude back to 0
        if (Input.GetKeyUp(KeyCode.E) && this.name == "Direct Force")
        {
            directForceAE.forceMagnitude = 0;
            directForceActive = false;
            hitBox.material.SetColor("_Color", Color.white);
        }

        if (Input.GetKeyUp(KeyCode.R) && this.name == "Radial Force")
        {
            radialForcePE.forceMagnitude = 0;
            radialForceActive = false;
            hitBox.material.SetColor("_Color", Color.white);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Called when objects enter the hitbox
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Called when objects leave the hitbox
    }
}


// *** TRIGGERED MAGNETIC ATTACKS ***

// Targeted magnetic push
// Target = Selected object
// Action = Push target object in the opposite direction as the direct path towards Prism
// Functionality = Apply direction and force to object

// Targeted magnetic pull
// Target = Selected object
// Action = Pull target object towards the player
// Functionality = Apply direction and force to object



// *** PASSIVE MAGNETIC EFFECTS ***

// Objects repelled from Player
// Target = All magnetically interactable objects in range
// Action = Push target objects in the opposite as the direct path towards Prism (The continuous passive version of Radial magnetic push)
// Functionality = Updating the path functionality for passive magnetic pull's target destination to be the linear direction away from the player

// Player drawn to object
// Target = The magnetically interactable object that has both more mass than the player and the most mast out of the other magnetically interactable objects
// Action = Pull the player towards the target object
// Functionality = Apply the object as the player's direction and force to the player

// Player repelled from object
// Target = The magnetically interactable object that has both more mass than the player and the most mast out of the other magnetically interactable objects
// Action = Push the player in the opposite direction of the direct path to the target object
// Functionality = Apply the direct path away from the target object as the player's direction and force to the player