/*
Copyright (c) 2018
Unity Technologies ApS (“Unity”, “our” or “we”) provides game-development and related software (the “Software”), development-related services (like Unity Teams (“Developer Services”)), and various Unity communities (like Unity Answers and Unity Connect (“Communities”)), provided through or in connection with our website, accessible at unity3d.com or unity.com (collectively, the “Site”). Except to the extent you and Unity have executed a separate agreement, these terms and conditions exclusively govern your access to and use of the Software, Developer Services, Communities and Site (collectively, the “Services”), and constitute a binding legal agreement between you and Unity (the “Terms”).
If you accept or agree to the Agreement on behalf of a company, organization or other legal entity (a “Legal Entity”), you represent and warrant that you have the authority to bind that Legal Entity to the Agreement and, in such event, “you” and “your” will refer and apply to that company or other legal entity.
You acknowledge and agree that, by accessing, purchasing or using the services, you are indicating that you have read, understand and agree to be bound by the agreement whether or not you have created a unity account, subscribed to the unity newsletter or otherwise registered with the site. If you do not agree to these terms and all applicable additional terms, then you have no right to access or use any of the services.
*/
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype.NetworkLobby
{
    public class EventSystemChecker : MonoBehaviour
    {
        //public GameObject eventSystem;

        // Use this for initialization
        void Awake()
        {
            if (!FindObjectOfType<EventSystem>())
            {
                //Instantiate(eventSystem);
                var obj = new GameObject("EventSystem");
                obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
            }
        }


    }
}