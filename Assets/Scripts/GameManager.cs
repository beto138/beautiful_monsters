using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using vhasselmann.Core.GenericStateMachine;

namespace BeautifulMonsters.Core
{
    public class GameManager : GameEntity, IGameManager
    {
        private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadScene(string scene, Action callBack)
        {
            var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

            while (!async.isDone)
            {
                yield return null;
            }

            yield return waitForEndOfFrame;

            if(null != callBack)
            {
                callBack.Invoke();
            }
        }

        public void ChangeState(GameStates gameStates)
        {
            switch (gameStates)
            {
                case GameStates.Menu:
                    //SetState(new Menu());
                    break;
                case GameStates.GamePlay:
                    //SetState(new GamePlay());
                    break;
            }
        }
    }
}
