using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace AS
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0;  // (In the future will be split into "Standart", "Strike", "Slahs", and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;


        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        protected virtual void Awake()
        {

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
           
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            //  NOT USING THIS ONE
            //if (damageTarget = null)
            //{
            //    damageTarget = other.GetComponent<CharacterManager>();
            //}

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            }

            //  CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE

            //  CHECK IF TARGET IS BLOCKING

            

            //  DAMAGE

            DamageTarget(damageTarget);
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            //  WE DONT WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            //  SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE

            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear(); // WE RESET THE CHARACTERS THAT HAVE BEEN HIT, WHEN WE RESET THE COLLIDER, SO THEY MAY BE HIT AGAIN
        }


    }
}

