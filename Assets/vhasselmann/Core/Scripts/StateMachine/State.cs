using UnityEngine;
using System.Collections;

namespace vhasselmann.Core.GenericStateMachine
{
    /// <summary>
    /// The State class runs all the flow of a State Machine.
    /// </summary>
    public class State
    {
        public State()
        {

        }

        /// <summary>
        /// Runs everytime a state inits.
        /// </summary>
        /// <param name="entity">The current GameEntity that possess the State Machine this state belongs.</param>
        public virtual void Enter(GameEntity entity)
        {

        }

        /// <summary>
        /// Runs everytime a Update is called on the GameEntity.
        /// </summary>
        /// <param name="entity">The current GameEntity that possess the State Machine this state belongs.</param>
        public virtual void Update(GameEntity entity)
        {

        }

        /// <summary>
        /// Runs everytime a LateUpdate is called on the GameEntity.
        /// </summary>
        /// <param name="entity">The current GameEntity that possess the State Machine this state belongs.</param>
        public virtual void LateUpdate(GameEntity entity)
        {

        }

        /// <summary>
        /// Runs everytime a FixedUpdate is called on the GameEntity.
        /// </summary>
        /// <param name="entity">The current GameEntity that possess the State Machine this state belongs.</param>
        public virtual void FixedUpdate(GameEntity entity)
        {

        }

        /// <summary>
        /// Runs when the state is killed, just before the Enter of the next state run.
        /// </summary>
        /// <param name="entity">The current GameEntity that possess the State Machine this state belongs.</param>
        public virtual void Exit(GameEntity entity)
        {

        }
    }
}
