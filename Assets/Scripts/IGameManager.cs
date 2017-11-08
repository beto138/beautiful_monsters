using System;
using System.Collections;

namespace BeautifulMonsters.Core
{
    public interface IGameManager
    {
        IEnumerator LoadScene(string scene, Action callBack);
    }
}