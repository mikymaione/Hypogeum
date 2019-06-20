/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System;
using System.Threading;

namespace Hypogeum
{
    public static class MyUpdater
    {
        private struct VersioneInfo
        {
            public string Programma;
            public DateTime Versione;
            public string NomeZip;
        }

        public static DateTime _Versione;
        private static VersioneInfo MioV;
        private static string CartellaLocale, productName, Mio, PathFileVersioni;


        private static bool DeZippaEdEsegui(string s)
        {
            var resu = false;
            var k = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(s), "SetupFiles");

            try
            {
                if (System.IO.Directory.Exists(k))
                    System.IO.Directory.Delete(k, true);
            }
            catch
            {
                //some error
            }
            finally
            {
                try
                {
                    var z = new ICSharpCode.SharpZipLib.Zip.FastZip();
                    z.ExtractZip(s, k, "");

                    var exex = System.IO.Directory.GetFiles(k, "*.exe", System.IO.SearchOption.AllDirectories);
                    var msi = System.IO.Directory.GetFiles(k, "*.msi", System.IO.SearchOption.AllDirectories);
                    var trov = false;
                    var il_setup = "";

                    //setup.exe
                    if (!trov)
                    {
                        var maxExex = (exex == null ? 0 : exex.Length);

                        for (var j = 0; j < maxExex; j++)
                        {
                            var nj = System.IO.Path.GetFileName(exex[j]);

                            if (nj.Equals("setup.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                il_setup = exex[j];
                                trov = true;
                                break;
                            }
                        }
                    }

                    //*.msi
                    if (!trov)
                        if (msi != null && msi.Length > 0)
                        {
                            il_setup = msi[0];
                            trov = true;
                        }

                    if (trov && System.IO.File.Exists(il_setup))
                        System.Diagnostics.Process.Start(il_setup);
                }
                catch (Exception ex)
                {
                    var errore = ex.Message;
                }
            }

            return resu;
        }

        private static bool DownloadFileFromInternet(string http_, string path_salvataggio)
        {
            if (System.IO.File.Exists(path_salvataggio))
                try
                {
                    System.IO.File.Delete(path_salvataggio);
                }
                catch
                {
                    //cannot delete
                }

            try
            {
                using (var c = new System.Net.WebClient())
                    c.DownloadFile(http_, path_salvataggio);

                return true;
            }
            catch
            {
                //error                
                return false;
            }
        }

        public static void AggiornaQuestoProgramma(string _productName, string _cartellaLocale, string _version)
        {
            CartellaLocale = _cartellaLocale;
            PathFileVersioni = System.IO.Path.Combine(_cartellaLocale, "versioni.txt");
            productName = _productName;
            Mio = _productName + ".exe";

            var s = _version;
            s = s.Replace(".", "");

            //0123 45 67
            //2016 12 04
            var a = Convert.ToInt32(s.Substring(0, 4));
            var m = Convert.ToInt32(s.Substring(4, 2));
            var g = Convert.ToInt32(s.Substring(6, 2));

            _Versione = new DateTime(a, m, g);

            var workerThread = new Thread(new ThreadStart(_AggiornaQuestoProgramma));
            workerThread.Start();
        }

        private static void _AggiornaQuestoProgramma()
        {
            try
            {
                DownloadFileFromInternet("http://www.maionemiky.it/public/programmi/versioni.txt", PathFileVersioni);

                if (ControllaVersione(PathFileVersioni))
                {
                    var MioV_NomeZip = System.IO.Path.Combine(CartellaLocale, MioV.NomeZip);

                    if (System.IO.File.Exists(PathFileVersioni))
                        try
                        {
                            System.IO.File.Delete(PathFileVersioni);
                        }
                        catch
                        {
                            //cannot delete
                        }

                    try
                    {
                        if (System.IO.File.Exists(MioV_NomeZip))
                            try
                            {
                                System.IO.File.Delete(MioV_NomeZip);
                            }
                            catch
                            {
                                return;
                            }

                        DownloadFileFromInternet("http://www.maionemiky.it/public/programmi/" + MioV.NomeZip, MioV_NomeZip);

                        if (System.IO.File.Exists(MioV_NomeZip))
                            DeZippaEdEsegui(MioV_NomeZip);
                    }
                    catch
                    {
                        return;
                    }
                }

                return;
            }
            catch
            {
                return;
            }
        }

        private static VersioneInfo LeggiRigaVersione(string s)
        {
            //22=23
            //31=32
            //XXXXXXXXXXXXXXXXXXXXXX0123456789
            //RationesCurare_Six.exe=21/11/07;RCV_Small.zip
            var c = default(VersioneInfo);

            var i = s.IndexOf("=");
            c.Programma = s.Substring(0, i);

            var m = s.IndexOf(";");
            c.NomeZip = s.Substring(m, s.Length - m);
            c.NomeZip = c.NomeZip.Replace(";", "");

            var d = s.Substring(i, 9);
            d = d.Replace("=", "");
            d = d.Replace(";", "");

            try
            {
                c.Versione = DateTime.ParseExact(d, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                //cannot convert to date
            }

            return c;
        }

        private static bool ControllaVersione(string p)
        {
            var ok = false;

            try
            {
                if (System.IO.File.Exists(p))
                    using (var f = new System.IO.StreamReader(p))
                    {
                        while (!f.EndOfStream)
                        {
                            var g = f.ReadLine();
                            var v = LeggiRigaVersione(g);

                            if (v.Programma.Equals(Mio, StringComparison.OrdinalIgnoreCase))
                                if (v.Versione > _Versione)
                                {
                                    MioV = v;
                                    ok = true;

                                    break;
                                }
                        }

                        f.Close();
                    }
            }
            catch
            {
                //error
            }

            return ok;
        }


    }
}