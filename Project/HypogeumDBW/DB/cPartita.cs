/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System;
using System.Data.Common;
using HypogeumDBW.DB.Tabelle;

namespace HypogeumDBW.DB
{
    public sealed class cPartita : DataWrapper.Base.cBaseEntity<Partita>
    {

        protected override Partita Carica_Record(ref DbDataReader dr)
        {
            return new Partita()
            {
                codice_unet = DrTo<string>(dr, "codice_unet"),
                inizio = DrTo<DateTime>(dr, "inizio"),
                fine = DrTo<DateTime>(dr, "fine"),
                abortita = DrTo<bool>(dr, "abortita"),
            };
        }

        protected override DbParameter[] Inserisci_Parametri(Partita entita)
        {
            return new DbParameter[] {
                cDB.NewPar("codice_unet", entita.codice_unet),
                cDB.NewPar("inizio", entita.inizio),
                cDB.NewPar("fine", entita.fine),
                cDB.NewPar("abortita", entita.abortita),
            };
        }

        protected override DbParameter[] Modifica_Parametri(Partita entita)
        {
            return new DbParameter[] {
                cDB.NewPar("inizio", entita.inizio),
                cDB.NewPar("fine", entita.fine),
                cDB.NewPar("abortita", entita.abortita),
            };
        }

        protected override DbParameter[] Ricerca_Parametri(Partita entita)
        {
            return new DbParameter[] {
                cDB.NewPar("codice_unet", entita.codice_unet),
                cDB.NewPar("inizio", entita.inizio),
                cDB.NewPar("fine", entita.fine),
                cDB.NewPar("abortita", entita.abortita),
            };
        }

    }
}