/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HypogeumDBW.DB.DataWrapper.Base
{
    public abstract class cBaseEntity<TableEntity> : cBaseDBObject<TableEntity> where TableEntity : Tabelle.TabellaBase, new()
    {
        protected abstract DbParameter[] Inserisci_Parametri(TableEntity entita);
        protected abstract DbParameter[] Modifica_Parametri(TableEntity entita);
        protected abstract TableEntity Carica_Record(ref DbDataReader dr);

        public cRisultatoSQL<int> Modifica(TableEntity entita, string PrimaryKeyName)
        {
            var modP = Modifica_Parametri(entita);

            //DbParameter[] auditP = {
            //    cDB.NewPar("DataModifica", DateTime.Now),
            //    cDB.NewPar("CFModifica", cMemDB.UtenteConnesso.Persona_CF)
            //};

            var p = new List<DbParameter>();
            p.AddRange(modP);
            //p.AddRange(auditP);

            return cDB.EseguiSQLNoQuery(getQuery(cDB.eTipoEvento.Modifica, PrimaryKeyName, false, p.ToArray()), p);
        }

        public virtual cRisultatoSQL<Tuple<int, int>> Inserisci(TableEntity entita, string PrimaryKeyName, bool PrimaryKeyIsAutoInc)
        {
            DbParameter outp = null;
            var insP = Inserisci_Parametri(entita);

            //DbParameter[] auditP = {
            //    cDB.NewPar("CFCreazione", cMemDB.UtenteConnesso.Persona_CF),
            //    cDB.NewPar("CFModifica", cMemDB.UtenteConnesso.Persona_CF)
            //};

            var p = new List<DbParameter>();
            p.AddRange(insP);
            //p.AddRange(auditP);

            if (PrimaryKeyIsAutoInc)
            {
                outp = cDB.NewPar(PrimaryKeyName, null, System.Data.ParameterDirection.Output);
                p.Add(outp);
            }

            var r = cDB.EseguiSQLNoQuery(getQuery(cDB.eTipoEvento.Inserisci, PrimaryKeyName, PrimaryKeyIsAutoInc, p.ToArray()), p);

            if (r.Errore)
                return new cRisultatoSQL<Tuple<int, int>>(r.Eccezione);
            else
            {
                if (PrimaryKeyIsAutoInc)
                    return new cRisultatoSQL<Tuple<int, int>>(new Tuple<int, int>(r.Risultato, Convert.ToInt32(outp.Value)));
                else
                    return new cRisultatoSQL<Tuple<int, int>>(new Tuple<int, int>(r.Risultato, -1));
            }
        }

        public cRisultatoSQL<int> Elimina(object key)
        {
            var e = Carica(key);

            if (e.Errore)
            {
                return new cRisultatoSQL<int>(e.Eccezione);
            }
            else
            {
                var entita = e.Risultato;
                var CreatoDaMe = true; //entita.CFCreazione.Equals(cMemDB.UtenteConnesso.Persona_CF);

                if (CreatoDaMe)
                {
                    DbParameter[] p =  {
                        cDB.NewPar(entita.PrimaryKeyName, key)
                    };

                    return cDB.EseguiSQLNoQuery(getQuery(cDB.eTipoEvento.Elimina, entita.PrimaryKeyName), p);
                }
                else
                {
                    return new cRisultatoSQL<int>(new Exception("Non puoi eliminare un elemento che non hai creato tu!"));
                }
            }
        }

        protected override TableEntity Carica_RecordSenzaAudit(ref DbDataReader dr)
        {
            var e = Carica_Record(ref dr);
            Carica_Audit(ref e, dr);

            return e;
        }

        private void Carica_Audit(ref TableEntity entita, DbDataReader dr)
        {
            //entita.DataCreazione = cGB.ObjectToDateTime(dr["DataCreazione"]);
            //entita.DataModifica = cGB.ObjectToDateTime(dr["DataModifica"]);
            //entita.CFCreazione = cGB.ObjectToString(dr["CFCreazione"]);
            //entita.CFModifica = cGB.ObjectToString(dr["CFModifica"]);
        }

        protected J DrTo<J>(DbDataReader dr, string campo)
        {
            return (J)dr[campo];
        }

    }
}