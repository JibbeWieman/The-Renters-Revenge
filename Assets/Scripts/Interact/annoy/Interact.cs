/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    [SerializeField] private bool inReach;

    [SerializeField] GameObject text;
    [SerializeField] GameObject canvas;

    void Start()
    {
        inReach = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
        }
    }


    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {

                /*Ray ray = new Ray(transform.position, transform.forward);
                 if(Physics.Raycast(ray, out RaycastHit hitInfo, number))
                 {*/
                /* Instantiate(text, canvas.transform);
                 gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 300, 1);
                }
                }
                }
                }*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private bool inReach;
    [SerializeField] GameObject text;
    [SerializeField] GameObject canvas;

    [SerializeField] float targetTimePauseStart = 10.0f;
    float targetTimePause;
    [SerializeField] bool PauseTimerOn = false;
    [SerializeField] int AmountCanInteract = 1;
    [SerializeField] int AgroDistance;
    [SerializeField] bool CameraObject = false;

    // Event delegate for interaction event
    public delegate void InteractionEventHandler(Transform target);
    public static event InteractionEventHandler OnInteract;

    public delegate void InteractEventHandler(int Count);
    public static event InteractEventHandler OnInteractCount;

    void Start()
    {
        inReach = false;
        targetTimePause += targetTimePauseStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
        }
    }

    void OnGUI()
    {
        if (targetTimePause < targetTimePauseStart)
        {
            int targetTimePauseInt = (int)targetTimePause;
            var targetTimePauseString = targetTimePauseInt.ToString();
            GUI.Label(new Rect(50, 50, 400, 200), "Time Frozen " + targetTimePauseString);
        }

        if (inReach == true)
        {
            GUI.Box(new Rect(Screen.width / 3, (Screen.height /3)*2, Screen.width/3, Screen.height/3), "Press | E | to interact");
        }
    }

    void Update()
    {
        // Find the player object and obtain its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform playerTransform = player.transform;

        if (inReach && Input.GetButtonDown("Interact") && AmountCanInteract > 0 && PauseTimerOn == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 300, 1);

                // Notify AI that interaction occurred with the player's transform
                OnInteract?.Invoke(playerTransform);
                OnInteractCount?.Invoke(1);
                UIAnnoyCounter.CountSwitch = true;

                //Sets timer amount and starts it(currently 10 seconds)
                PauseTimerOn = true;

                //Can interact with it one less time
                --AmountCanInteract;
            }

        }

        if (PauseTimerOn == true)
        {
            if(CameraObject == false)
            {
                targetTimePause -= Time.deltaTime;

                GameObject Landlord = GameObject.FindGameObjectWithTag("Landlord");
                Transform LandlordTransform = Landlord.transform;

                //if there is less than 10 distance between landlord and tennant, they follow(for as long as timer is active)
                if (Mathf.Sqrt(Mathf.Pow((Landlord.transform.position.x - player.transform.position.x), 2) +
                              Mathf.Pow((Landlord.transform.position.y - player.transform.position.y), 2) +
                              Mathf.Pow((Landlord.transform.position.z - player.transform.position.z), 2)) < AgroDistance)
                {
                    OnInteract?.Invoke(playerTransform);
                }
                //else the landlord stays in place(for as long as the timer is active(up to 10s))
                else
                {
                    OnInteract?.Invoke(LandlordTransform);
                }

                if (targetTimePause <= 0.0f)
                {
                    GameObject block = GameObject.FindGameObjectWithTag("block");
                    Transform blockTransform = block.transform;
                    OnInteract?.Invoke(blockTransform);
                    targetTimePause = targetTimePauseStart;
                    PauseTimerOn = false;
                }
            }
            else
            {
                targetTimePause -= Time.deltaTime;

                OnInteract?.Invoke(playerTransform);

                if (targetTimePause <= 0.0f)
                {
                    GameObject block = GameObject.FindGameObjectWithTag("block");
                    Transform blockTransform = block.transform;
                    OnInteract?.Invoke(blockTransform);
                    targetTimePause = targetTimePauseStart;
                    PauseTimerOn = false;
                }
            }
            
        }
    }
}