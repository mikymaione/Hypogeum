/*
Copyright (c) 2018
Unity Technologies ApS (“Unity”, “our” or “we”) provides game-development and related software (the “Software”), development-related services (like Unity Teams (“Developer Services”)), and various Unity communities (like Unity Answers and Unity Connect (“Communities”)), provided through or in connection with our website, accessible at unity3d.com or unity.com (collectively, the “Site”). Except to the extent you and Unity have executed a separate agreement, these terms and conditions exclusively govern your access to and use of the Software, Developer Services, Communities and Site (collectively, the “Services”), and constitute a binding legal agreement between you and Unity (the “Terms”).
If you accept or agree to the Agreement on behalf of a company, organization or other legal entity (a “Legal Entity”), you represent and warrant that you have the authority to bind that Legal Entity to the Agreement and, in such event, “you” and “your” will refer and apply to that company or other legal entity.
You acknowledge and agree that, by accessing, purchasing or using the services, you are indicating that you have read, understand and agree to be bound by the agreement whether or not you have created a unity account, subscribed to the unity newsletter or otherwise registered with the site. If you do not agree to these terms and all applicable additional terms, then you have no right to access or use any of the services.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace Prototype.NetworkLobby
{
    public class LobbyServerList : MonoBehaviour
    {
        public LobbyManager lobbyManager;

        public RectTransform serverListRect;
        public GameObject serverEntryPrefab;
        public GameObject noServerFound;

        protected int currentPage = 0;
        protected int previousPage = 0;

        static Color OddServerColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        static Color EvenServerColor = new Color(.94f, .94f, .94f, 1.0f);

        void OnEnable()
        {
            currentPage = 0;
            previousPage = 0;

            foreach (Transform t in serverListRect)
                Destroy(t.gameObject);

            noServerFound.SetActive(false);

            RequestPage(0);
        }

        public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (matches.Count == 0)
            {
                if (currentPage == 0)
                    noServerFound.SetActive(true);

                currentPage = previousPage;

                return;
            }

            noServerFound.SetActive(false);

            foreach (Transform t in serverListRect)
                Destroy(t.gameObject);

            for (var i = 0; i < matches.Count; ++i)
            {
                var o = Instantiate(serverEntryPrefab) as GameObject;

                o.GetComponent<LobbyServerEntry>().Populate(matches[i], lobbyManager, (i % 2 == 0) ? OddServerColor : EvenServerColor);
                o.transform.SetParent(serverListRect, false);
            }
        }

        public void ChangePage(int dir)
        {
            var newPage = Mathf.Max(0, currentPage + dir);

            //if we have no server currently displayed, need we need to refresh page0 first instead of trying to fetch any other page
            if (noServerFound.activeSelf)
                newPage = 0;

            RequestPage(newPage);
        }

        public void RequestPage(int page)
        {
            previousPage = currentPage;
            currentPage = page;
            lobbyManager.matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGUIMatchList);
        }


    }
}