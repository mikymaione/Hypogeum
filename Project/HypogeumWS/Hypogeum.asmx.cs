/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using HypogeumDBW.DB;
using HypogeumDBW.DB.Tabelle;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace HypogeumWS
{
    [WebService(Namespace = "http://www.maionemiky.it/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Hypogeum : WebService
    {

        public Hypogeum() => SettaConnessione();


        [WebMethod]
        public cRisultatoSQL<List<Utente>> LoginUtenteFB(string facebook_key)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        public cRisultatoSQL<Utente> LoginUtenteEmail(string email)
        {
            var classeUtente = new cUtente();

            return classeUtente.RicercaByEmail(email);
        }

        [WebMethod]
        public cRisultatoSQL<Tuple<int, int>> RegistraUtente(string email, string descrizione)
        {
            var classeUtente = new cUtente();

            return classeUtente.Inserisci(new Utente()
            {
                descrizione = descrizione,
                email = email
            }, "id_utente", true);
        }

        [WebMethod]
        public cRisultatoSQL<Tuple<int, int>> Partecipa(string codice_unet, int id_utente)
        {
            var classePartecipanti = new cPartecipanti();

            return classePartecipanti.Inserisci(new Partecipanti()
            {
                codice_unet = codice_unet,
                id_utente = id_utente
            }, null, false);
        }

        [WebMethod]
        public cRisultatoSQL<int> Muori(string codice_unet, int id_utente, int punti, int posizione)
        {
            var classePartecipanti = new cPartecipanti();

            return classePartecipanti.Modifica(new Partecipanti()
            {
                codice_unet = codice_unet,
                id_utente = id_utente,
                posizione = posizione,
                punti = punti
            }, null);
        }


        [WebMethod]
        public cRisultatoSQL<Tuple<int, int>> IniziaPartita(string codice_unet)
        {
            var classePartita = new cPartita();

            return classePartita.Inserisci(new Partita()
            {
                codice_unet = codice_unet,
                inizio = DateTime.Now,
                fine = DateTime.MaxValue,
            }, "codice_unet", false);
        }

        [WebMethod]
        public cRisultatoSQL<int> FinePartita(string codice_unet)
        {
            var classePartita = new cPartita();

            return classePartita.Modifica(new Partita()
            {
                codice_unet = codice_unet,
                fine = DateTime.Now,
            }, "codice_unet");
        }

        [WebMethod]
        public cRisultatoSQL<Partita> StatoPartita(string codice_unet)
        {
            var classePartita = new cPartita();

            return classePartita.Carica(codice_unet);
        }

        [WebMethod]
        public cRisultatoSQL<List<Partita>> ListaPartite(DateTime dal, DateTime al)
        {
            var classePartita = new cPartita();

            return classePartita.Ricerca(new Partita()
            {
                inizio = dal,
                fine = al
            });
        }

        private void SettaConnessione()
        {
            var App_Data = Server.MapPath("App_Data");
            var path_db = System.IO.Path.Combine(App_Data, "Hypogeum.sqlite3");

            cDB.Application_StartupPath = App_Data;
            cDB.ApriConnessione(true, $"Version=3;Data Source={path_db};", cDB.DataBase.SQLite);
        }


    }
}