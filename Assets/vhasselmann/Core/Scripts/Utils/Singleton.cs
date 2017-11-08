using UnityEngine;
using System.Collections;

namespace vhasselmann.Core
{
    /// <summary>
    /// Singleton Base class. Creates a lazy unique instance. 
	/// This class guarantees that there's only a single instance of this object in the scene.
	/// 	* Upon Awake checks if there's an instance created different from this one. In case positive, destroy gameobject
	/// 	  containing this instance so it is guaranteed to only have one instnace in the scene.
	///     * Upon acquiring an instance it searches for one in the scene, in case none is found create a gameobject and add
	/// 	  the singleton component to it.
	///     * Makes sure the instance is persistent across scenes
	///  	* Prevents ghost instance when the objects has been marked for destruction and a client code creates another instance
	/// 	  through Instance property.
    /// 
    /// How to use: Derive your Singleton class from this class in the following manner:
    /// public MyClass : Singleton<MyClass>
	/// All Singleton handling will be transparent to you.
    /// </summary>
    /// <typeparam name="T">Derived Class</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        /// <summary>
        /// Unique instance of this object.
        /// </summary>
        protected static T _Instance = null;

        /// <summary>
        /// Flag to know when the application is quitting so we don't recreate instances of this singleton.
        /// </summary>
        static bool m_IsApplicationQuitting = false;

        /// <summary>
        /// Property to return an instance of this Singleton. If none is create first look for an instance in the
        /// scene, and then if none available create and attach a singleton component to the scene.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<T>();

                    // If application is quitting we can't create a new instance or else we'll end up with a ghost object
                    if (m_IsApplicationQuitting)
                    {
                        return null;
                    }

                    // There's no isntance in the scene. We need to create one.
                    if (_Instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _Instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString();

                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(singleton);
                        }
                    }
                    else
                    {
                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(_Instance.gameObject);
                        }
                    }
                }

                return _Instance;
            }
        }

		/// <summary>
		/// Awake checks if there's an instance created which is different from this one. 
		/// In case positive, we delete this gameobject and leave an unique reference to singleton
		/// derived class in scene.
		/// </summary>
		void Awake()
		{
			// Another instance in the scene. Destroy it.
			if (_Instance != null && _Instance != this) 
			{
				Destroy(gameObject);
			}
		}

        /// <summary>
        /// Upon application quitting we flag this object as being destroyed. This client code
        /// to issues a Instance after the singleton has been destroy recreating the object and 
        /// giving memory leak.
        /// </summary>
        void OnApplicationQuit()
        {
            m_IsApplicationQuitting = true;
        }

        void OnDestroy()
        {
            m_IsApplicationQuitting = true;
        }
    }
}
