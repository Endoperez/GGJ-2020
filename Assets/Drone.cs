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
    private DateTime? lastDropTime = null;
    private List<GameObject> carriedObjects = new List<GameObject>();
    float maxY = 10f;
    float force = 1f;

    // Start is called before the first frame upda
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initEngineAudio();
        initKeywordRecognizer("start", "stop", "land", "drop", "up", "down", "left", "right");
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
    void quitKeywordRecognizer()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            Debug.Log("Stopping recognizer...");
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
            Debug.Log("Done.");
        }
    }
    #endregion

    DroneAction droneAction = null; // DroneAction.None;

    void OnVoiceCommand(string command)
    {
        droneAction = ConvertVoiceCommandToAction(command);    
    }

    DroneAction ConvertVoiceCommandToAction(string command)
    {
        switch (command)
        {
            case "start": return startEngine;
            case "stop": return stopEngine;
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

    public void moveUp()
    {
        engineAudio.volume = carriedObjects.Count > 0 ? 1f : 0.8f;

        if (transform.position.y < maxY)
            rb.AddForce(0, force, 0, ForceMode.Acceleration);

        rb.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void moveDown()
    {
        rb.AddForce(0, -1 * force, 0, ForceMode.Force);
        engineAudio.volume = carriedObjects.Count > 0 ? 0.6f : 0.4f;
        rb.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void moveLeft()
    {
        if (transform.position.x > -8 && rb.velocity.x > -2)
        {
            //forceVector.x -= thrust;
            //rb.AddForce(-1f, 2f, 0, ForceMode.Force);
            rb.AddForce(-1 * force, 0, 0, ForceMode.Acceleration);
            //if (rb.rotation.z > 120)
            //    rb.AddTorque(0, 0, 0.5f, ForceMode.Acceleration);
            //if (rb.transform.rotation.eulerAngles.z < 20f)
            //    rb.transform.Rotate(0, 0, 1f);
            rb.rotation = Quaternion.Euler(0, 0, 10);
        }
    }

    public void moveRight()
    {
        if (transform.position.x < 8 && rb.velocity.x < 2)
        {
            //forceVector.x += thrust;
            //rb.AddForce(1f, 2f, 0, ForceMode.Force);
            rb.AddForce(force, 0, 0, ForceMode.Acceleration);

            rb.rotation = Quaternion.Euler(0, 0, -10);
        }
    }

    public void drop()
    {
        foreach (var carriedObject in carriedObjects)
        {
            carriedObject.transform.parent = transform.parent;
            //carriedObject.transform.position = new Vector3(carriedObject.transform.position.x, carriedObject.transform.position.y - 0.3f, carriedObject.transform.position.z);
            // Add ridig body and apply same velocity as the drone
            var ridigBody = carriedObject.AddComponent<Rigidbody>();
            ridigBody.velocity = rb.velocity;
            lastDropTime = DateTime.Now;
        }
        //rb.AddForce(0, 1f, 0, ForceMode.Impulse);
        carriedObjects.Clear();

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
        //ContactPoint contact = collision.contacts[0];

        //if (collision.gameObject.tag == "Material" && collision.collider.tag == "Material")
        //    return; // two materials hit each other...

        // Avoid picking up objects if drop action has been recently done
        if (lastDropTime.HasValue && (DateTime.Now - lastDropTime.Value).Seconds < 1)
            return;

        var didDroneHit = false;
        foreach (var contact in collision.contacts)
        {
            if (contact.thisCollider.gameObject == this.gameObject)
            {
                didDroneHit = true;
                break;
            }
        }
        if (!didDroneHit)
            return;

        switch (collision.gameObject.tag)
        {
            case "Material":
                collision.gameObject.transform.parent = gameObject.transform;
                var materialRb = collision.gameObject.GetComponent<Rigidbody>();
                Destroy(materialRb);
                //this.carriedObjects.
                this.carriedObjects.Add(collision.gameObject);
                //Physics.
                //Destroy(materialRb
                break;
            default:
                break;
        }

        //collision.gameObject
        //if (collision.gameObject == 
        //Destroy(gameObject);
    }


    private void OnApplicationQuit()
    {
        quitKeywordRecognizer();  
    }
    
}
