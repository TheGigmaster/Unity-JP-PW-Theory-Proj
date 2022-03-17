using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all the player boxes. Can be used on its own or inhereted from.
public class Box : MonoBehaviour, IBox
{
    // Uses values from prefab. Intended to be set with SetAttributes when new, modified boxes are spawned. 
    public PhysicMaterial boxBounce;
    public Color boxColor;

    // Mainly to make sure the particle effect plays in time before the game object is removed.
    [SerializeField]
    private float despawnDelay = 2.0f;

    // I prefer to get components at runtime. Find it clutters up the inspector too much for me.
    [SerializeField]
    private Renderer boxRenderer;
    
    public Rigidbody boxRb;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private ParticleSystem fxPoof;

    void Awake()
    {
        boxRenderer = GetComponent<Renderer>();
        boxRb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        fxPoof = GetComponentInChildren<ParticleSystem>();

        boxColor = boxRenderer.material.color;
        boxBounce = boxCollider.material;
    }
    void Start()
    {
        // Grabbing my components

    }

    //POLYMORPHISM
    // OpenBox() is inherited by child animal box classes, causing appropriate sprites to explode out on opening.
    public virtual void OpenBox()
    {
        Debug.Log("OpenBox() called.");
    }

    //public void ChangeBoxColor(float r, float g, float b)
    //{
    //    boxColor = new Color(r, g, b);
    //    boxRenderer.material.color = boxColor;
    //}

    // INHERITANCE
    // (also accessed using an interface, IBox)
    // SetAttributes is mainly intended to be run by the instantiator with player-provided variables.
    // ENCAPSULATION (also kinda)
    public void SetAttributes(PhysicMaterial _boxBounce, Color _boxColor)
    {
        Debug.Log($"SetAttributes(physMat {_boxBounce}, color {_boxColor}) called.");
        boxCollider.material = _boxBounce;
        boxRenderer.material.color = _boxColor;
    }

    // Spawn method will be on the instantiator.
    public virtual void Despawn()
    {
        // Coroutine coroutine = StartCoroutine(DespawnAnim(despawnDelay));
        Debug.Log($"Despawn called with delay of {despawnDelay}");
        boxRenderer.enabled = false;
        fxPoof.Play();
        Destroy(gameObject, despawnDelay);
    }
    // On first coding I did this as a coroutine, but various replies online seemed to indicate that if you don't need a coroutine, use the delay overload for Destroy since that does the delay on the native c/c++ side of things.
    // It's prolly a tiny difference even at scale, but dem good habits.
    //IEnumerator DespawnAnim(float delay)
    //{
    //    isDespawning = true;
    //    boxRenderer.enabled = false;
    //    fxPoof.Play();

    //    yield return new WaitForSeconds(delay);

    //    isDespawning = false;
    //    Destroy(gameObject);
    //}
}

