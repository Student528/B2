using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    public Transform PlayerTransform;
    public GameObject Weapon;
    public Camera Camera;
    public float range = 2f;
    public float open = 100f;

    // Start is called before the first frame update
    void Start()
    {
        Weapon.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            if (Weapon)
            {
                UnequipObject();
              
            }
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
                if (target.tag == "Weapon")
                {

                    EquipObject(hit.transform.gameObject);
                    
            }
        }
    }

    void UnequipObject()
    {
        PlayerTransform.DetachChildren();
        Weapon.transform.eulerAngles = new Vector3(Weapon.transform.eulerAngles.x, Weapon.transform.eulerAngles.y, Weapon.transform.eulerAngles.z - 45);
        Weapon.GetComponent<Rigidbody>().isKinematic = false;
        Weapon.GetComponent<MeshCollider>().enabled = true;
        Weapon = null;
    }

    void EquipObject(GameObject To_Equip)
    {
        Weapon = To_Equip;
        print("Unequip");
        Weapon.GetComponent<Rigidbody>().isKinematic = true;
        Weapon.GetComponent<MeshCollider>().enabled = false;
        Weapon.transform.position = PlayerTransform.transform.position;
        Weapon.transform.rotation = PlayerTransform.transform.rotation;
        Weapon.transform.SetParent(PlayerTransform);
    }
}