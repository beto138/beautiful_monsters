using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace vhasselmann.Core.GenericStateMachine
{
    /// <summary>
    /// The StateMachine manager class.
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        private const int MAX_STATES = 3;

        private GameEntity m_Entity;
        private State m_CurrentState;

        private List<State> oldStates;

        public StateMachine()
        {

        }

        internal void Init(GameEntity entity)
        {
            this.m_Entity = entity;
            m_CurrentState = new State();
            oldStates = new List<State>();
        }

        void Update()
        {
            if (m_CurrentState != null) m_CurrentState.Update(m_Entity);
        }

        void LateUpdate()
        {
            if (m_CurrentState != null) m_CurrentState.LateUpdate(m_Entity);
        }

        void FixedUpdate()
        {
            if (m_CurrentState != null) m_CurrentState.FixedUpdate(m_Entity);
        }

        /// <summary>
        /// Changes the current State to a new one.
        /// </summary>
        /// <param name="newState">A new State to run</param>
        public void SetState(State newState)
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.Exit(m_Entity);
            }

            if (m_CurrentState != null)
            {
                oldStates.Add(m_CurrentState);
            }

            m_CurrentState = newState;

            m_CurrentState.Enter(m_Entity);

            CheckMaxStates();
        }

        /// <summary>
        /// Gets the current state running in the State Machine
        /// </summary>
        /// <returns>Returns the a State object that can be typechecked with the "is" operator</returns>
        public State GetState()
        {
            return m_CurrentState;
        }

        private void CheckMaxStates()
        {
            while (oldStates.Count > MAX_STATES)
            {
                oldStates.RemoveAt(oldStates.Count - 1);
            }
        }
    }
}
