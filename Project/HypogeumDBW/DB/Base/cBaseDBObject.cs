/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections.Generic;
using System.Data.Common;

namespace HypogeumDBW.DB.DataWrapper.Base
{
    public abstract class cBaseDBObject<TableEntity> where TableEntity : Tabelle.Base.BaseDBObject, new()
    {
        protected abstract DbParameter[] Ricerca_Parametri(TableEntity entita);
        protected abstract TableEntity Carica_RecordSenzaAudit(ref DbDataReader dr);


        protected bool ColumnExists(DbDataReader dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
                if (dr.GetName(i).Equals(columnName, System.StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        protected string getQuery(string NomeEvento)
        {
            return cDB.LeggiQuery(typeof(TableEntity).Name + "." + NomeEvento);
        }

        protected string getQuery(cDB.eTipoEvento NomeEvento, string PrimaryKeyName = "", bool PrimaryKeyIsAutoInc = false, DbParameter[] campiDaAggiornare = null)
        {
            var z = "";
            var p = "";

            try
            {
                p = typeof(TableEntity).Name + "." + NomeEvento;
                z = cDB.LeggiQuery(p);
            }
            catch (System.IO.FileNotFoundException)
            {
                z = cDB.ComponiQuery(p, NomeEvento, typeof(TableEntity).Name, PrimaryKeyName, PrimaryKeyIsAutoInc, campiDaAggiornare);
            }

            return z;
        }

        public cRisultatoSQL<List<TableEntity>> Ricerca(TableEntity entita)
        {
            var r = new List<TableEntity>();

            try
            {
                var dr = cDB.EseguiSQLDataReader(getQuery(cDB.eTipoEvento.Ricerca), Ricerca_Parametri(entita));

                try
                {
                    if (dr.HasRows)
                        while (dr.Read())
                            r.Add(Carica_RecordSenzaAudit(ref dr));
                }
                catch (System.Exception ex1)
                {
                    dr.Close();

                    return new cRisultatoSQL<List<TableEntity>>(ex1);
                }

                dr.Close();
            }
            catch (System.Exception ex2)
            {
                return new cRisultatoSQL<List<TableEntity>>(ex2);
            }

            return new cRisultatoSQL<List<TableEntity>>(r);
        }

        public cRisultatoSQL<System.Data.DataTable> Ricerca(int MaxRows, TableEntity entita)
        {
            try
            {
                return new cRisultatoSQL<System.Data.DataTable>(cDB.EseguiSQLDataTable(getQuery(cDB.eTipoEvento.Ricerca), Ricerca_Parametri(entita), MaxRows));
            }
            catch (System.Exception ex)
            {
                return new cRisultatoSQL<System.Data.DataTable>(ex);
            }
        }

        public cRisultatoSQL<System.Data.DataTable> Ricerca(object key)
        {
            try
            {
                var e = new TableEntity();

                DbParameter[] p =  {
                    cDB.NewPar(e.PrimaryKeyName, key)
                };

                return new cRisultatoSQL<System.Data.DataTable>(cDB.EseguiSQLDataTable(getQuery(cDB.eTipoEvento.Carica, e.PrimaryKeyName), p, 1));
            }
            catch (System.Exception ex)
            {
                return new cRisultatoSQL<System.Data.DataTable>(ex);
            }
        }

        public cRisultatoSQL<TableEntity> Carica(object key)
        {
            try
            {
                var ok = false;
                var e = new TableEntity();

                DbParameter[] p =  {
                    cDB.NewPar(e.PrimaryKeyName, key)
                };

                var dr = cDB.EseguiSQLDataReader(getQuery(cDB.eTipoEvento.Carica, e.PrimaryKeyName), p);

                try
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        e = Carica_RecordSenzaAudit(ref dr);

                        ok = true;
                    }
                }
                catch (System.Exception ex1)
                {
                    dr.Close();

                    return new cRisultatoSQL<TableEntity>(ex1);
                }

                dr.Close();

                if (ok)
                    return new cRisultatoSQL<TableEntity>(e);
                else
                    throw new System.Exception("Record non trovato!");
            }
            catch (System.Exception ex2)
            {
                return new cRisultatoSQL<TableEntity>(ex2);
            }
        }


    }
}