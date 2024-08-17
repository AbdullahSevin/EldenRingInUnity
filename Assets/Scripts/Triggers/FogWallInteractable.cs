using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace AS
{
    public class FogWallInteractable : Interactable
    {


        [Header("Fog")]
        [SerializeField] GameObject[] fogGameObjects;

        [Header("Collision")]
        [SerializeField] Collider fogWallCollider;

        [Header("I.D.")]
        public int fogWallID;

        [Header("Sound")]
        private AudioSource fogWallAudioSource;
        [SerializeField] AudioClip fogWallSFX;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        protected override void Awake()
        {
            base.Awake();

            fogWallAudioSource = gameObject.GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.right);
            player.transform.rotation = targetRotation;

            AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
            player.playerAnimatorManager.PlayTargetActionAnimation("Pass_through_Fog_01", true);

            StartCoroutine(StopAnimationAfterTime(player, 2));
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.instance.AddFogWallToList(this);

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }

        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (isActive.Value)
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(false);
                }
            }
        }

        // WHEN A SERVER RPC DOES NOT  REQUIRE OWNERSHIP, A NON OWNER CAN ACTIVATE THE FUNCTION (CLIENT PLAYER DOES NOT OWN FOG WALL, AS THEY ARE NOT THE HOST)
        [ServerRpc(RequireOwnership = false)]

        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
        {
            if (IsServer)
            {
                AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
            }

        }

        [ClientRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX);

            if (player != null)
            {
                StartCoroutine(DisableCollisionForTime(player));
            }

        }

        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            //  MAKE THIS FUNCTION THE SAME TIME AS THE WALKING THROUGH FOG WALL ANIMATION LENGTH
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);
            yield return new WaitForSeconds(3);
            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);


        }


        private IEnumerator StopAnimationAfterTime(PlayerManager player, float duration)
        {
            yield return new WaitForSeconds(duration);
            // Stop the animation or transition to another state
            player.playerAnimatorManager.PlayTargetActionAnimation("Empty", false, true, true, true);
        }


    }
}

