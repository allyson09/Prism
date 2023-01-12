using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // Martial Variables
    private Renderer hitBox;
    private Color hitBoxColor = Color.magenta;

    // Game Object Variables
    private PlayerController playerComponent;
    private AreaEffector2D areaEffector;
    private Rigidbody2D enemyRB;
    private float directAttackSpeed = 40;

    // Navigation Variables
    private float attackDirection = -1;

    // Interacting with other game object variables
    // private Dictionary<int, Dictionary> attackRangeObjects;
    // private Dictionary<string, object> objInRange;
    private MetalObject collisionComponent;

    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Renderer>();
        playerComponent = transform.GetComponentInParent<PlayerController>();
        areaEffector = GetComponent<AreaEffector2D>();
        // attackRangeObjects = new Dictionary<int, Dictionary>();
    }

    // Update is called once per frame
    void Update()
    {
        attackDirection = playerComponent.isFacingLeft;

        // Update direction of direct force attacks
        if (attackDirection < 0)
        {
            areaEffector.forceAngle = 180;
        } else
        {
            areaEffector.forceAngle = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eventually this method will call various attack functions based on conditions

        // Visual feedback
        // hitBoxColor.a = 1;
        // hitBox.material.color = hitBoxColor;
        // Debug.Log("Object entered collision");

        // Get entering object's component
        // Later write conditionals so the component I'm grabbing is defined by a variable
        // Example collision.gameObject.GetComponent<objectTypeVariable>();
        // if (collision.tag == "MetalObject")
        // {
            // MetalObject collisionComponent = collision.gameObject.GetComponent<MetalObject>();
            // Debug.Log("Metal object speed: " + collisionComponent.speed);
        // }

        // Add new collision object to dictionary of current objects in range

        // if (collision.gameObject.tag == "MetalObject")
        // {
        //     // This functionality happens while F is being held down when enemies come into range
        //     if (Input.GetKey(KeyCode.F))
        //     {
        //         // Update enemy's movement velocity
        //         enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
        //         enemyRB.velocity = new Vector2(attackDirection * directAttackSpeed, enemyRB.velocity.y);
        //     }
        // }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove collision object from dictionary of current objects in range
        if (collision.gameObject.tag == "Enemy")
        {
            hitBoxColor.a = 0;
            hitBox.material.color = hitBoxColor;
        }
    }
}

// *** Potential objects *** [ WIP ]
// Wait to make classes after finalizing core mechanics


// Potential code structure

// *** TRIGGERED MAGNETIC ATTACKS ***

// Directional magnetic push
// Target = All magnetically interactable objects in range
// Action = Push target objects in direction player is facing
// Functionality = Apply the same direction and amount of force (depending on mass) to all objects in range

// Directional magnetic pull
// Target = All magnetically interactable objects in range
// Action = Pull target objects towards player
// Functionality = Apply the same direction and amount of force (depending on mass) to all objects in range

// Radial magnetic push
// Target = All magnetically interactable objects in range
// Action = Push target objects in direction opposite of the direct path towards the player
// Functionality = Apply the opposite direction as the path towards the player as the target destination and the same amount of force (depending on mass) to all objects in range

// Radial magnetic pull
// Target = All magnetically interactable objects in range
// Action = Pull target objects towards the player
// Functionality = Apply the player as the target destination and the same amount of force (depending on mass) to all objects in range

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