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
using System.Web.Services;

namespace HypogeumWS
{
    [WebService(Namespace = "http://www.maionemiky.it/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Hypogeum : WebService
    {

        [WebMethod]
        public int LoginUtenteFB(string facebook_key)
        {
            return -1;
        }

        [WebMethod]
        public cRisultatoSQL<Utente> LoginUtenteEmail(string email)
        {
            var classeUtente = new cUtente();
            var utente = classeUtente.Carica(email);

            return utente;
        }

        [WebMethod]
        public bool Partecipa(string codice_unet, int id_utente)
        {
            var classePartecipanti = new cPartecipanti();
            var classePartita = new cPartita();

            var lista_partecipanti = classePartecipanti.Ricerca(new Partecipanti()
            {
                codice_unet = codice_unet,
                id_utente = id_utente
            });

            if (lista_partecipanti.Risultato.Count > 0)
            {
                //già presente
                return true;
            }
            else
            {
                var risultatoInserimento = classePartecipanti.Inserisci(new Partecipanti()
                {
                    codice_unet = codice_unet,
                    id_utente = id_utente
                }, null, false);

                return (risultatoInserimento.Risultato.Item1 > 0);
            }
        }

        [WebMethod]
        public bool Muori(string codice_unet, int id_utente, int punti, int posizione)
        {
            return true;
        }


        [WebMethod]
        public bool IniziaPartita(string codice_unet)
        {
            return true;
        }

        [WebMethod]
        public bool FinePartita(string codice_unet)
        {
            return true;
        }

        [WebMethod]
        public Partita StatoPartita(string codice_unet)
        {
            return new Partita();
        }

        [WebMethod]
        public Partita[] ListaPartite(DateTime dal, DateTime al)
        {
            return new Partita[] {
                new Partita(),
                new Partita(),
                new Partita(),
                new Partita()
            };
        }


    }
}