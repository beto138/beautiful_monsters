using System;
using System.Collections;

namespace BeautifulMonsters.Core
{
    public interface IGameManager
    {
        void LoadScene(string scene, Action callBack);
    }
}