using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D theRB;
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float mouseSensitivity = 5f;
    public Camera viewCam;

    public GameObject bulletImpact;
    public int currentAmmo;

    public Animator gunAnim;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        theRB.velocity = (moveHorizontal + moveVertical) * moveSpeed;

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);
        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

        if(Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Instantiate(bulletImpact, hit.point, transform.rotation);

                    if(hit.transform.tag == "Enemy")
                    {
                        hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                    }
                }
                else
                {
                    Debug.Log("This is nothing");
                }
                currentAmmo--;
                gunAnim.SetTrigger("Shoot");
            }
        }
    }
}
