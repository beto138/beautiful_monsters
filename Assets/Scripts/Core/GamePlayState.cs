using vhasselmann.Core.GenericStateMachine;

namespace BeautifulMonsters.Core
{
    public class GamePlayState : GameState
    {
        public override void Enter(GameEntity entity)
        {
            base.Enter(entity);

            IGameManager gameManager = entity.gameObject.GetComponent<IGameManager>();

            if(null != gameManager)
            {
                gameManager.LoadScene("SCN_GamePlay", OnSceneLoaded);
            }
        }

        public override void OnSceneLoaded()
        {

        }
    }
}
