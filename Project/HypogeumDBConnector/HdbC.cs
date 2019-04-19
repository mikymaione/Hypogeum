/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System;

namespace HypogeumDBConnector
{
    public static class HdbC
    {
        private static HypogeumWS.Hypogeum H = new HypogeumWS.Hypogeum();

        public static HypogeumWS.Partita StatoPartita(string codice_unet) => H.StatoPartita(codice_unet);
        public static HypogeumWS.Partita[] ListaPartite(DateTime dal, DateTime al) => H.ListaPartite(dal, al);

        public static bool IniziaPartita(string codice_unet) => H.IniziaPartita(codice_unet);
        public static bool FinePartita(string codice_unet) => H.FinePartita(codice_unet);

        public static bool Partecipa(string codice_unet, int id_utente) => H.Partecipa(codice_unet, id_utente);
        public static bool Muori(string codice_unet, int id_utente, int punti, int posizione) => H.Muori(codice_unet, id_utente, punti, posizione);

        public static int LoginUtente(string facebook_key) => H.LoginUtente(facebook_key);

    }
}