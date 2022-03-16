using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all the player boxes. Can be used on its own or inhereted from.
public class Box : MonoBehaviour
{
    // Uses values from prefab. Intended to be set with SetAttributes when new, modified boxes are spawned. 
    private Vector3 boxSize;
    private PhysicMaterial boxBounce;
    private Color boxColor;

    // Mainly to make sure the particle effect plays in time before the game object is removed.
    [SerializeField]
    private float despawnDelay = 2.0f;

    // I prefer to get components at runtime. Find it clutters up the inspector too much for me.
    private Renderer boxRenderer;
    private Rigidbody boxRb;
    private BoxCollider boxCollider;
    private ParticleSystem fxPoof;

    // Our prefab.
    [SerializeField]
    private GameObject boxPrefab;

    // Getters in case I need em
    public Vector3 BoxSize { get { return boxSize; } }
    public PhysicMaterial BoxBounce { get { return boxBounce; } }
    public Color BoxColor { get { return boxColor; } }

    void Start()
    {
        // Grabbing my components
        boxRenderer = GetComponent<Renderer>();
        boxRb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        fxPoof = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        
    }

    // Duplicate-, Move- and OpenBox are intended to be modified by classes inheriting from Box.
    public virtual void DuplicateBox()
    {
        Debug.Log("DuplicateBox() called.");
    }

    public virtual void MoveBox()
    {
        Debug.Log("MoveBox() called.");
    }

    public virtual void OpenBox()
    {
        Debug.Log("OpenBox() called.");
    }

    // SetAttributes is mainly intended to be run by the instantiator with player-provided variables.
    public void SetAttributes(Vector3 _boxSize, PhysicMaterial _boxBounce, Color _boxColor)
    {
        Debug.Log($"SetAttributes(size {_boxSize}, physMat {_boxBounce}, color {_boxColor}) called.");
        boxSize = _boxSize;
        boxBounce = _boxBounce;
        boxColor = _boxColor;
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

