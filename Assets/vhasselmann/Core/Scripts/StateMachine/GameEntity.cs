using UnityEngine;
using System.Collections;

namespace vhasselmann.Core.GenericStateMachine
{
    /// <summary>
    /// Entity class that automatically creates a State Machine.
    /// </summary>
    public class GameEntity : MonoBehaviour
    {
        private StateMachine m_StateMachine;

        [HideInInspector]
        public bool m_HasStarted = false;

        public GameEntity()
        {
        }

        public virtual void Awake()
        {
            m_HasStarted = true;
            m_StateMachine = gameObject.AddComponent<StateMachine>();
            m_StateMachine.Init(this);
        }

        /// <summary>
        /// Changes the current State to a new one.
        /// </summary>
        /// <param name="newState">A new State to run</param>
        public void SetState(State newState)
        {
            if (m_StateMachine == null)
            {
                Debug.LogError("For some reason the State Machine is null");
                return;
            }

            m_StateMachine.SetState(newState);
        }

        /// <summary>
        /// Gets the current State Machine
        /// </summary>
        /// <returns>Returns the a StateMachine object that can be typechecked with the "is" operator</returns>
        public StateMachine GetStateMachine()
        {
            return m_StateMachine;
        }

        /// <summary>
        /// Gets the current state running in the State Machine
        /// </summary>
        /// <returns>Returns the a State object that can be typechecked with the "is" operator</returns>
        public State GetCurrentState()
        {
            if (GetStateMachine() == null)
            {
                return null;
            }

            return m_StateMachine.GetState();
        }
    }
}
