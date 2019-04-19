/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System;
using System.Linq;

namespace HypogeumDBW.DB.DataWrapper.Tabelle.Base
{
    public abstract class BaseDBObject
    {

        public Tuple<string, dynamic> Campo(string nome, dynamic default_)
        {
            var t = Campo(nome);

            if (t.Item2 != null)
            {
                if (t.Item2 is int)
                    if (t.Item2 < 0)
                        return new Tuple<string, dynamic>(nome, default_);
            }

            return t;
        }

        public Tuple<string, dynamic> Campo(string nome)
        {
            var f = this.GetType().GetProperty(nome);

            if (f == null)
                throw new Exception("Il campo " + nome + " non esiste nell'entità " + this.GetType().Name);

            var v = f.GetValue(this, null);

            return new Tuple<string, dynamic>(nome, v);
        }

        public bool Equals(TabellaBase ot)
        {
            if (ot == null)
                return false;

            var me_fields = this.GetType().GetProperties();
            var ot_fields = ot.GetType().GetProperties();

            var TupleUguali =
                from m in me_fields
                join o in ot_fields on m.Name equals o.Name
                select new { m, o };

            foreach (var e in TupleUguali)
            {
                var mv = e.m.GetValue(this, null);
                var ov = e.o.GetValue(ot, null);

                if (mv == null && ov == null)
                    continue;

                if (mv == null && "".Equals(ov))
                    continue;

                if ("".Equals(mv) && ov == null)
                    continue;

                if (mv == null && ov != null)
                    return false;

                if (mv != null && ov == null)
                    return false;

                if (!mv.Equals(ov))
                    return false;
            }

            return true;
        }

        private System.Reflection.PropertyInfo PrimaryKeyProperties
        {
            get
            {
                var properties = this.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(false);

                    foreach (var attribute in attributes)
                        if (attribute is PrimaryKeyAttribute)
                            return property;
                }

                throw new Exception("La chiave primaria non è stata settata per la classe " + this.GetType().Name);
            }
        }

        public bool PrimaryKeyIsAutoInc
        {
            get
            {
                var properties = this.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(false);

                    foreach (var attribute in attributes)
                        if (attribute is PrimaryAutoIncKeyAttribute)
                            return true;
                }

                return false;
            }
        }

        public object PrimaryKey
        {
            get
            {
                return PrimaryKeyProperties.GetValue(this, null);
            }
        }

        public string PrimaryKeyName
        {
            get
            {
                return PrimaryKeyProperties.Name;
            }
        }

        protected bool EqualsOrNull(string a, string b)
        {
            if (a == b)
                return true;

            if (a == null && b == "")
                return true;
            if (b == null && a == "")
                return true;

            return false;
        }


    }
}