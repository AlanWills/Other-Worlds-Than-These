using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;

namespace ToGalaxy.AI
{
    public enum BehaviourState
    {
        MovingToDestination,
        Following,
        Wandering,
        Fleeing,
        Idle
    }

    public class MovementBehaviourStateMachine
    {
        private BehaviourState CurrentBehaviour
        {
            get;
            set;
        }

        public Ship TargetShip
        {
            get;
            private set;
        }

        private Ship Ship
        {
            get;
            set;
        }

        public MovementBehaviourStateMachine(Ship ship)
        {
            Ship = ship;
        }

        public MovementBehaviourStateMachine(Ship ship, Ship targetShip)
        {
            Ship = ship;
            TargetShip = targetShip;
        }

        public void Update()
        {
            UpdateBehaviour();
            PerformBehaviour();
        }

        public void SetTarget(Ship targetShip)
        {
            TargetShip = targetShip;
            CurrentBehaviour = BehaviourState.Following;
        }

        // Continue to follow a possible move to destination input order
        public void ClearTarget()
        {
            TargetShip = null;
            CurrentBehaviour = BehaviourState.MovingToDestination;
        }

        // Stop in place
        public void Stop()
        {
            TargetShip = null;
            CurrentBehaviour = BehaviourState.Idle;
            Ship.SetDestination(Ship.Position);
        }

        #region Behaviour Management Functions

        private void UpdateBehaviour()
        {
            // Ship is healthy
            /*if (Ship.Hull > Ship.ShipData.Hull / 5)
            {
                // Ship has a target
                if (TargetShip != null)
                {
                    CurrentBehaviour = BehaviourState.Following;
                }
                // Else ship wanders
                else if (Ship as EnemyShip != null && Ship.DestinationReached)
                {
                    CurrentBehaviour = BehaviourState.Wandering;
                }
            }
            // Ship is close to death and so should flee
            else
            {
                CurrentBehaviour = BehaviourState.Fleeing;
            }*/

            // Ship has a target
            if (TargetShip != null)
            {
                CurrentBehaviour = BehaviourState.Following;
            }
            // Else ship wanders
            else if (Ship as EnemyShip != null && Ship.DestinationReached)
            {
                CurrentBehaviour = BehaviourState.Wandering;
            }
        }

        private void PerformBehaviour()
        {
            float distanceToDestination = (Ship.Destination - Ship.Position).Length();

            // No update if target state is BehaviourState.MovingToDestination as that is handled in the Ship update loop
            if (TargetShip != null)
            {
                if (CurrentBehaviour == BehaviourState.Fleeing)
                {
                    Flee();
                }
                else if (CurrentBehaviour == BehaviourState.Following)
                {
                    Follow();
                }
            }

            if (CurrentBehaviour == BehaviourState.Wandering)
            {
                Wander();
            }
        }

        #endregion

        #region Automatic Behavioural Movements

        protected void Wander()
        {
            if (Ship.DestinationReached)
            {
                Random rand = new Random();
                int direction = rand.Next(0, 355);

                Vector2 directionVector2 = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
                Ship.SetDestination(Ship.Position + directionVector2 * Ship.MinimumTurretRange);
            }
        }

        protected void Flee()
        {
            if (Ship.DestinationReached)
            {
                Vector2 targetShipToEnemy = Ship.Position - TargetShip.Position;

                if (targetShipToEnemy.Length() < Ship.MinimumTurretRange)
                {
                    targetShipToEnemy.Normalize();
                    Vector2 fleePosition = TargetShip.Position + targetShipToEnemy * 2 * Ship.MinimumTurretRange;
                    Ship.SetDestination(fleePosition);
                }
            }
        }

        protected void Follow()
        {
            Vector2 targetShipToEnemy = Ship.Position - TargetShip.Position;

            if (targetShipToEnemy.Length() < Ship.MinimumTurretRange / 2)
            {
                Ship.SetDestination(Ship.Position);
                Ship.Stop();
            }
            else
            {
                targetShipToEnemy.Normalize();
                Ship.SetDestination(TargetShip.Position - targetShipToEnemy * Ship.MinimumTurretRange);
            }
        }

        #endregion
    }
}
