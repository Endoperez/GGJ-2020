using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public delegate void DroneAction();

public class Drone : MonoBehaviour
{
    private AudioSource engineAudio;
    private KeywordRecognizer recognizer;
    private Rigidbody rb;

    bool isRunning = false;
    bool isCarryingSomething = false;
    private DateTime? lastDropTime = null;
    private List<GameObject> carriedObjects = new List<GameObject>();
    float maxY = 10f;

    [SerializeField]
    float movementSpeedForce = 1f;

    [SerializeField]
    GameObject hookPositionObject;

    // Start is called before the first frame upda
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initEngineAudio();
        initKeywordRecognizer("start", "stop", "land", "drop", "up", "down", "left", "right");

        if(hookPositionObject == null)
        {
            Debug.LogError("Hook position object has not been set in Unity editor!");
        }
    }


    void initEngineAudio()
    {
        engineAudio = GetComponent<AudioSource>();
        engineAudio.Stop();
        engineAudio.volume = 0.6f;
        engineAudio.loop = true;
    }

    #region KeywordRecognizer
    private string[] keywords;
    void initKeywordRecognizer(params string[] voiceCommands)
    {
        keywords = voiceCommands;
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low); // Low for non native speakers
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;

        recognizer.Start();
    }
    
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        OnVoiceCommand(args.text);
    }

    void QuitKeywordRecognizer()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            Debug.Log("Stopping recognizer...");
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
            Debug.Log("Stopping completed.");
        }
    }
    #endregion

    DroneAction droneAction = null;

    void OnVoiceCommand(string command)
    {
        droneAction = ConvertVoiceCommandToAction(command);    
    }

    DroneAction ConvertVoiceCommandToAction(string command)
    {
        switch (command)
        {
            //case "start": return startEngine;
            //case "stop": return stopEngine;
            case "stop": return stopMovement;
            case "up": return moveUp;
            case "down": return moveDown;
            case "left": return moveLeft;
            case "right": return moveRight;
            case "land": return land;
            case "drop": return drop;
            default: return null;
        }
    }

    public void startEngine()
    {
        if (!engineAudio.isPlaying)
            engineAudio.Play();
        rb.useGravity = false;
    }

    public void stopEngine()
    {
        engineAudio.Stop();
        // engineAudio.Volume
        rb.useGravity = true;
        rb.velocity = new Vector3(0, 0, 0);
    }

    public void stopMovement()
    {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
    }

    public void moveUp()
    {
        engineAudio.volume = carriedObjects.Count > 0 ? 1f : 0.8f;

        if (transform.position.y < maxY)
        {
            // replace AddForce with direct setting of velocity
            //rb.AddForce(0, force, 0, ForceMode.Acceleration);
            rb.velocity = new Vector3(0, movementSpeedForce, 0);
        }
        if (rb.rotation.eulerAngles.z < 10.5 && rb.rotation.eulerAngles.z > 0.5)
            rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z - 2f);
        else if (rb.rotation.eulerAngles.z > 270 && rb.rotation.eulerAngles.z < 359.5)
            rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z + 2f);
        else
            rb.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void moveDown()
    {
        // replace AddForce with direct setting of velocity
        //rb.AddForce(0, -1 * force, 0, ForceMode.Force);
        rb.velocity = new Vector3(0, -movementSpeedForce, 0);
        engineAudio.volume = carriedObjects.Count > 0 ? 0.6f : 0.4f;
        //rb.rotation = Quaternion.Euler(0, 0, 0);
        if (rb.rotation.eulerAngles.z < 10.5 && rb.rotation.eulerAngles.z > 0.5)
            rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z - 2f);
        else if (rb.rotation.eulerAngles.z > 270 && rb.rotation.eulerAngles.z < 359.5)
            rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z + 2f);
        else
            rb.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void moveLeft()
    {
        if (transform.position.x > -8)// && rb.velocity.x > -2)
        {
            //forceVector.x -= thrust;
            //rb.AddForce(-1f, 2f, 0, ForceMode.Force);

            // replace AddForce with direct setting of velocity
            //rb.AddForce(-1 * force, 0, 0, ForceMode.Acceleration);
            rb.velocity = new Vector3(-movementSpeedForce, 0, 0);

            //if (rb.rotation.z > 120)
            //    rb.AddTorque(0, 0, 0.5f, ForceMode.Acceleration);
            //if (rb.transform.rotation.eulerAngles.z < 20f)
            //    rb.transform.Rotate(0, 0, 1f);
            if (rb.rotation.eulerAngles.z < 9.5 || rb.rotation.eulerAngles.z > 270)
                rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z + 2f);
            else
                rb.rotation = Quaternion.Euler(0, 0, 10);
        }

    }

    public void moveRight()
    { 
        if (transform.position.x < 8) // && rb.velocity.x < 2)
        {
            //forceVector.x += thrust;
            //rb.AddForce(1f, 2f, 0, ForceMode.Force);

            // replace AddForce with direct setting of velocity
            //rb.AddForce(force, 0, 0, ForceMode.Acceleration);
            rb.velocity = new Vector3(movementSpeedForce, 0, 0);

            //rb.rotation = Quaternion.Euler(0, 0, -10);
            if (rb.rotation.eulerAngles.z < 90 || rb.rotation.eulerAngles.z > 350.5)
                rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z - 2f);
            else
                rb.rotation = Quaternion.Euler(0, 0, 350);
        }
    }

    public void drop()
    {
        foreach (var carriedObject in carriedObjects)
        {
            carriedObject.transform.parent = transform.parent;
            //carriedObject.transform.position = new Vector3(carriedObject.transform.position.x, carriedObject.transform.position.y - 0.3f, carriedObject.transform.position.z);
            // Add ridig body and apply same velocity as the drone
            Rigidbody carriedObjectRigidbody = carriedObject.AddComponent<Rigidbody>();
            carriedObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            carriedObjectRigidbody.velocity = rb.velocity - Vector3.up;
            lastDropTime = DateTime.Now;
        }
        //rb.AddForce(0, 1f, 0, ForceMode.Impulse);
        carriedObjects.Clear();
        isCarryingSomething = false;

    }

    public void land()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up")) droneAction = moveUp;
        if (Input.GetKeyDown("down")) droneAction = moveDown;
        if (Input.GetKeyDown("left")) droneAction = moveLeft;
        if (Input.GetKeyDown("right")) droneAction = moveRight;
        if (Input.GetKeyDown("space")) droneAction = drop;
        if (Input.GetKeyDown( "return" )) droneAction = stopMovement;
        if (Input.GetKeyDown("s"))
        {
            if (isRunning)
                droneAction = startEngine;
            else
                droneAction = startEngine;
        }

        if (droneAction != null)
            droneAction();
    }

    void OnCollisionEnter(Collision collision)
    {
        // if drone is already carrying something, it can't carry any more stuff
        if (isCarryingSomething)
            return;

        // Avoid picking up objects if drop action has been recently done
        if (lastDropTime.HasValue && (DateTime.Now - lastDropTime.Value).Seconds < 1)
            return;

        switch (collision.gameObject.tag)
        {
            case "CarriableObject":
                RawMaterial rawMaterialComponent = collision.gameObject.GetComponent<RawMaterial>();

                // if carriable object doesn't have RawMaterial component, or if the RawMaterial fell and is already broken, ignore it
                if (rawMaterialComponent == null || rawMaterialComponent.broken)
                    return;
                StartCarryingObject(collision.gameObject);
                break;
            default:
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCarryingSomething)
            return;

        // Avoid picking up objects if drop action has been recently done
        if (lastDropTime.HasValue && (DateTime.Now - lastDropTime.Value).Seconds < 1)
            return;

        switch (other.tag)
        {
            case "CarriableObject":
                StartCarryingObject(other.gameObject);
                break;
            default:
                break;
        }

    }

    void StartCarryingObject(GameObject target) {
        isCarryingSomething = true;

        // set carried object to be drone's child object
        target.transform.parent = gameObject.transform;

        // move to hook object's position in X and Y axes
        Vector3 newPosition = hookPositionObject.transform.position;
        newPosition.z = transform.position.z;
        target.transform.position = newPosition;

        // remove rigidbody component
        Rigidbody carriedObjectRigidbody = target.GetComponent<Rigidbody>();
        Destroy(carriedObjectRigidbody);

        // add carried object to list of carried objects, used to select which objects will be dropped
        this.carriedObjects.Add(target);
        //Physics.
        //Destroy(materialRb
    }

    private void OnApplicationQuit()
    {
        QuitKeywordRecognizer();  
    }
    
}
