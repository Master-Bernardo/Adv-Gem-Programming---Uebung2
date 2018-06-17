using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public int size = 20;// for manna or health potions
    public Type type;

    public enum Type
    {
        PHYSICS,
        HEALTH,
        MANNA,
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            switch(type){
                case Type.PHYSICS:
                    collision.gameObject.GetComponent<PlayerMovement>().ApplyPhysicsBoost(); 
                    break;
                case Type.HEALTH:
                    collision.gameObject.GetComponent<PlayerMovement>().RegenerateHealth(size);
                    break;
                case Type.MANNA:
                    collision.gameObject.GetComponent<PlayerMovement>().RegenerateManna(size);
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
