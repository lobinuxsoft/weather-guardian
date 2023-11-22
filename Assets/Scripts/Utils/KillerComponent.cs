using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using WeatherGuardian.Behaviours;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(ColliderDetector))]
    public class KillerComponent : MonoBehaviour
    {
        private ColliderDetector colliderDetector;

        bool killProcess = false;

        private void Awake()
        {
            colliderDetector = GetComponent<ColliderDetector>();

            colliderDetector.onEnter += Kill;
            colliderDetector.onStay += Kill;
            colliderDetector.onExit += Kill;
        }

        private void OnDestroy()
        {
            if (colliderDetector != null)
            {
                colliderDetector.onEnter -= Kill;
                colliderDetector.onStay -= Kill;
                colliderDetector.onExit -= Kill;
            }
        }

        private async void Kill(GameObject player)
        {
            if (!killProcess)
            {
                killProcess = true;

                await KillTask(player);

                killProcess = false;
            }
        }

        private async Task KillTask(GameObject player, int delayToRevive = 1)
        {
            Animator animator = player.GetComponent<Animator>();
            DeathBehaviour deathBehaviour = animator.GetBehaviour<DeathBehaviour>();

            deathBehaviour.Death(animator);
            
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Death");

            // TODO hacer el cross fade.
            await Task.Delay(delayToRevive * 1000);            

            await ScreenFader.Instance.FadeIn(() =>
            {
                player.SetActive(false);

                CheckPoint.JumpToLastCheckPoint(player);

                player.SetActive(true);

                deathBehaviour.Revive(animator);
            });

            await ScreenFader.Instance.FadeOut();
        }
    }
}