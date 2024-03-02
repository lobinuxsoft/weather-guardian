using UnityEngine;

using FMODUnity;
using System;

namespace WeatherGuardian.PickUps 
{
    [RequireComponent(typeof(Collider))]
    public class MachineParts : MonoBehaviour, ICollectable
    {
        [SerializeField] private EventReference pickUpSFX;

        [SerializeField] private EventReference MachinePartSFX;

        [SerializeField] ParticleSystem particleSystem;

        [SerializeField] [Range(0.01f, 1.0f)] private float vanishSpeed;

        private Collider myCollider;

        private void Awake()
        {
            myCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            myCollider.enabled = true;
        }

        private void Start()
        {
            if (particleSystem != null) 
            {
                particleSystem.Stop();
            }
        }

        void ICollectable.PickedUp()
        {
            particleSystem.Play();

            if (!pickUpSFX.IsNull)
                RuntimeManager.PlayOneShot(pickUpSFX);

            if (!MachinePartSFX.IsNull)
                RuntimeManager.PlayOneShot(MachinePartSFX);

            myCollider.enabled = false;

            Destroy(gameObject, vanishSpeed);
        }        
    }
}