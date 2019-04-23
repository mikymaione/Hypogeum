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
        public Utente LoginUtenteFB(string facebook_key)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        public Utente LoginUtenteEmail(string email)
        {
            var classeUtente = new cUtente();

            var R = classeUtente.RicercaByEmail(email);

            return ReturnResult(R);
        }

        [WebMethod]
        public int RegistraUtente(string email, string descrizione)
        {
            var classeUtente = new cUtente();

            var R = classeUtente.Inserisci(new Utente()
            {
                descrizione = descrizione,
                email = email
            }, "id_utente", true);

            return ReturnResult(R);
        }

        [WebMethod]
        public int Partecipa(string codice_unet, int id_utente)
        {
            var classePartecipanti = new cPartecipanti();

            var R = classePartecipanti.Inserisci(new Partecipanti()
            {
                codice_unet = codice_unet,
                id_utente = id_utente
            }, null, false);

            return ReturnResult(R);
        }

        [WebMethod]
        public int Muori(string codice_unet, int id_utente, int punti, int posizione)
        {
            var classePartecipanti = new cPartecipanti();

            var R = classePartecipanti.Modifica(new Partecipanti()
            {
                codice_unet = codice_unet,
                id_utente = id_utente,
                posizione = posizione,
                punti = punti
            }, null);

            return ReturnResult(R);
        }


        [WebMethod]
        public int IniziaPartita(string codice_unet)
        {
            var classePartita = new cPartita();

            var R = classePartita.Inserisci(new Partita()
            {
                codice_unet = codice_unet,
                inizio = DateTime.Now,
                fine = DateTime.MaxValue,
            }, "codice_unet", false);

            return ReturnResult(R);
        }

        [WebMethod]
        public int FinePartita(string codice_unet)
        {
            var classePartita = new cPartita();

            var R = classePartita.Modifica(new Partita()
            {
                codice_unet = codice_unet,
                fine = DateTime.Now,
            }, "codice_unet");

            return ReturnResult(R);
        }

        [WebMethod]
        public Partita StatoPartita(string codice_unet)
        {
            var classePartita = new cPartita();

            var R = classePartita.Carica(codice_unet);

            return ReturnResult(R);
        }

        [WebMethod]
        public Partita[] ListaPartite(DateTime dal, DateTime al)
        {
            var classePartita = new cPartita();

            var R = classePartita.Ricerca(new Partita()
            {
                inizio = dal,
                fine = al
            });

            return ReturnResult(R).ToArray();
        }

        private void SettaConnessione()
        {
            var App_Data = Server.MapPath("App_Data");
            var path_db = System.IO.Path.Combine(App_Data, "Hypogeum.sqlite3");

            cDB.Application_StartupPath = App_Data;
            cDB.ApriConnessione(true, $"Version=3;Data Source={path_db};", cDB.DataBase.SQLite);
        }

        private int ReturnResult(cRisultatoSQL<Tuple<int, int>> R)
        {
            if (R.Errore)
                throw R.Eccezione;

            return R.Risultato.Item1;
        }

        private G ReturnResult<G>(cRisultatoSQL<G> R)
        {
            if (R.Errore)
                throw R.Eccezione;

            return R.Risultato;
        }


    }
}