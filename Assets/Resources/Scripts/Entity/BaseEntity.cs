using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Resources.Scripts.Entity
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField]
        private float inStrength = 0;
        [SerializeField]
        private float inAgility = 0;
        [SerializeField]
        private float inDexterity = 0;
        [SerializeField]
        private float inConstitution = 0;
        [SerializeField]
        private float inIntellect = 0;
        [SerializeField]
        private float inConcentration = 0;


        [NonSerialized]
        public Rigidbody2D RigidBody;
        [NonSerialized]
        public Animator Animator;
        public BoxCollider2D BoxCollider;
        [NonSerialized]
        public List<Command> Commands = new List<Command>();

        [NonSerialized] 
        public bool isActive = true;
        [NonSerialized]
        public float currentActionPoint;
        [NonSerialized]
        public float currentHitpoint;
        [NonSerialized]
        public string Name = "";
        [NonSerialized]
        public string Description = "";


        protected MoveCommand Move = null;
        public float Strength
        {
            get
            {
                var result = inStrength;

                return result;
            }
        }

        public float Agility
        {
            get
            {
                var result = inAgility;

                return result;
            }
        }

        public float Dexterity
        {
            get
            {
                var result = inDexterity;

                return result;
            }
        }

        public float Constitution
        {
            get
            {
                var result = inConstitution;

                return result;
            }
        }

        public float Intellect
        {
            get
            {
                var result = inIntellect;

                return result;
            }
        }
        public float Concentration
        {
            get
            {
                var result = inConcentration;

                return result;
            }
        }
        public float MaxHealth
        {
            get
            {
                var result = Constitution * 10;

                return result;
            }
        }

        public float MeleeAbility
        {
            get
            {
                var result = Dexterity / 2 + Agility / 2;

                return result;
            }
        }

        public float PureMeleeDamage
        {
            get
            {
                double result = Strength / 2;
                result = Math.Round(result * Random.Range(0.75f, 1.26f) * 100) / 100;
                return (float)result;
            }
        }

        public float MeleeCritChance
        {
            get
            {
                var result = (Dexterity / 5) + (Intellect / 10);

                return result;
            }
        }

        public float Initiative
        {
            get
            {
                var result = Agility;

                return result;
            }
        }

        public float MaxActionPoint
        {
            get
            {
                var result = Concentration + Agility / 2;

                return result;
            }
        }

        public float IncomeActionPoint
        {
            get
            {
                var result = Agility / 3 + Dexterity / 3;

                return result;
            }
        }

        protected virtual bool ActPrimary()
        {
            return false;
        }

        protected virtual bool ActSecondary()
        {
            return false;
        }

        protected virtual void ActOther() { }
        public void Act()
        {
            if (ActPrimary())
            {
                isActive = false;
                return;
            }

            if (ActSecondary())
            {
                isActive = false;
                return;
            }

            ActOther();
            isActive = false;
        }

        protected Command GetFastestCommand()
        {
           // Debug.Log("GetFastestCommand():" + ToString());
            var result = Commands[0];
            foreach (var command in Commands)
            {
                if (result.APCast > command.APCast)
                    result = command;
            }

            //Debug.Log("GetFastestCommand().currentActionPoint:" + currentActionPoint);
            return result;
        }
        public void ResetState()
        {
            if (currentHitpoint <= 0)
                return;
            if (currentActionPoint < MaxActionPoint)
                currentActionPoint = Math.Min(currentActionPoint + IncomeActionPoint, MaxActionPoint);
            if (GetFastestCommand().APCast <= currentActionPoint)
                isActive = true;
            //Debug.Log("currentAP = " + currentActionPoint);
            //Debug.Log("MaxActionPoint = " + MaxActionPoint);
            //Debug.Log("isActive = " + isActive);
        }

        public void TakeDamage(float damageAmount)
        {
            currentHitpoint -= damageAmount;
            if (currentHitpoint <= 0)
            {
                Animator.SetTrigger("Die");
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            Move = new MoveCommand(this);
            Commands.Add(Move);
            currentActionPoint = MaxActionPoint;
            ResetState();
            currentHitpoint = MaxHealth;
            Animator = GetComponent<Animator>();
            BoxCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
