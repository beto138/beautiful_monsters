﻿using vhasselmann.Core.GenericStateMachine;

namespace BeautifulMonsters.Core
{
    public enum GameStates
    {
        Menu = 0,
        GamePlay,
        Count
    }

    public abstract class GameState : State
    {
        internal abstract void OnSceneLoaded();
    }
}