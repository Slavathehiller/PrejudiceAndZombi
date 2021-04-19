using Assets.Resources.Scripts.Entity;
using UnityEngine;
using System;
using static Assets.Resources.Scripts.Direction;
using Random = UnityEngine.Random;

namespace Assets.Resources.Scripts
{
    public abstract class Command
    {
        public float APCast;
        protected BaseEntity Owner;
        protected bool lastResult;
        public static string Name = "";

        protected Vector2 GetPosition(Vector2 startPosition, int direction)
        {
            var position = startPosition;
            if (direction == Up)
            {
                position.y += 1;
            }

            if (direction == Down)
            {
                position.y -= 1;
            }

            if (direction == Left)
            {
                position.x -= 1;
            }

            if (direction == Right)
            {
                position.x += 1;
            }

            if (direction == LeftUp)
            {
                position.y += 1;
                position.x -= 1;
            }

            if (direction == RightUp)
            {
                position.y += 1;
                position.x += 1;
            }

            if (direction == RightDown)
            {
                position.y -= 1;
                position.x += 1;
            }

            if (direction == LeftDown)
            {
                position.y -= 1;
                position.x -= 1;
            }

            return position;
        }
        public virtual bool Proceed(params object[] args)
        {
            return false;
        }

        public virtual bool Proceed(params int[] args)
        {
            lastResult = Owner.currentActionPoint >= APCast;
            if (lastResult)
            {
                Owner.currentActionPoint -= APCast;
            }

            return lastResult;
        }

        protected Command(BaseEntity owner)
        {
            Owner = owner;
        }
    }

    public class MoveCommand : Command
    {
        public MoveCommand(BaseEntity owner) : base(owner)
        {
            APCast = 5;
            Name = "Move";
        }

        public override bool Proceed(params int[] args)
        {
            if (!base.Proceed(args))
                return false;

            int direction = args[0];

            var position = GetPosition(Owner.RigidBody.transform.position, direction);

            if (!(Owner.Animator is null))
            {
                if (direction == Left || direction == LeftUp || direction == LeftDown)
                {
                    Owner.Animator.SetTrigger("TurnLeft");
                }
                if (direction == Right || direction == RightUp || direction == RightDown)
                {
                    Owner.Animator.SetTrigger("TurnRight");
                }

            }


            RaycastHit2D hit = Physics2D.Raycast(position, Owner.RigidBody.position,  0.5f,
                0xFFFFFFF);

            if (hit.collider == null)
            {
                Owner.RigidBody.transform.position = position;

                Debug.Log(Owner + "идет в " + position);
            }

            return lastResult;
        }
    }

    public abstract class MeleeAttackCommand : Command
    {
        protected BaseEntity target;
        public MeleeAttackCommand(BaseEntity owner) : base(owner)
        {
            Name = "MeleeAttack";
        }

        public override bool Proceed(params int[] args)
        {
            int direction = args[0];
            var position = GetPosition(Owner.RigidBody.transform.position, direction);
            RaycastHit2D hit = Physics2D.Raycast(position, Owner.RigidBody.position, 0.5f,
                0x100);
            if (hit.collider is null)
            {
                Debug.Log("Цель отсутствует");
                return false;
            }
            else
            {
                target = hit.collider.GetComponent<BaseEntity>();
                if (!(target is null))
                {
                    Debug.Log("Цель: " + target.Name + " " + target);
                    return true;
                }

            }

            if (!base.Proceed(args))
                return false;
            return lastResult;
        }
    }

    public class PunchCommand : MeleeAttackCommand
    {
        public PunchCommand(BaseEntity owner) : base(owner)
        {
            APCast = 8;
            Name = "Punch";
        }

        public override bool Proceed(params int[] args)
        {
            var result = base.Proceed(args);

            if (result)
            {
                Debug.Log(Owner.Name + " бъет " + target.Name);
                float hitChance = 50 + Owner.MeleeAbility * 5 - target.MeleeAbility * 5;
                bool isHit = Random.Range(1, 101) <= hitChance;
                if (isHit)
                {
                    float damageAmount = Owner.PureMeleeDamage;
                    bool crit = Random.Range(1, 101) <= Owner.MeleeCritChance;
                    if (crit)
                    {
                        damageAmount = damageAmount * 2;
                        Debug.Log(Owner.Name + " наносит критический удар!");
                    }
                    target.TakeDamage(damageAmount);
                    Debug.Log(target.Name + " получает " + damageAmount + " урона.");
                }
                else
                {
                    Debug.Log(Owner.Name + " промахивается.");
                    return false;
                }
            }

            return result;
        }
    }

}
