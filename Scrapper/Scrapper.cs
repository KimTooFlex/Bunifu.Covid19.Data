using ktf;
using System;
using System.Collections.Generic;
 

namespace Bunifu.Covid19.Scrapper
{
   public static class Scrapper
    {
        static string HTML;
        public static void Init(string html)
        {
            HTML = html;
        }
        public static List<CovidRecord>  GetCountries()
        {

            Kuto kuto = new Kuto(HTML);

            kuto = kuto.Extract("<tr style=\"\">", "</tbody>");

            List<CovidRecord> data = new List<CovidRecord>();

            while (kuto.Contains("<tr style=\"\">"))
            {
                var r = new CovidRecord();

                r.name = kuto.Extract("/\">", "<").StripTags().ToString();
                kuto = kuto.Extract("/\">", "");

                r.reported_cases = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.new_cases = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.deaths = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.new_deaths = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.recovered = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.active_cases = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                kuto = kuto.Extract("\">", "");

                r.serious_cases = kuto.Extract("\">", "<").StripTags().ToString().Trim();
                

                data.Add(r);
                kuto = kuto.Extract("<tr style=\"\">", "");
            }



            return data;
        }
       
        static int clean(string item)
        {
            int i = 0;

            if (item!=null && item.Trim().Length>0)
            {
                try
                {
                    i = int.Parse(item.Trim().Replace("+", "").Replace(",", ""));
                }
                catch (Exception)
                {

                } 
            }

            return i;
        }
        public static CovidRecord GetWorldSummary()
        {
            CovidRecord r = new CovidRecord();
            r.name = "World";
            foreach (var item in GetCountries())
            {
                r.active_cases = (clean(r.active_cases) + clean(item.active_cases)).ToString();
                r.new_cases = (clean(r.new_cases) + clean(item.new_cases)).ToString();
                r.deaths = (clean(r.deaths) + clean(item.deaths)).ToString();
                r.new_deaths = (clean(r.new_deaths) + clean(item.new_deaths)).ToString();
                r.recovered = (clean(r.recovered) + clean(item.recovered)).ToString();
                r.reported_cases = (clean(r.reported_cases) + clean(item.reported_cases)).ToString();
                r.serious_cases = (clean(r.serious_cases) + clean(item.serious_cases)).ToString();
          
            }


            r.active_cases = (clean(r.active_cases)).ToString("N0");
            r.new_cases = (clean(r.new_cases)).ToString("N0");
            r.deaths = (clean(r.deaths)).ToString("N0");
            r.new_deaths = (clean(r.new_deaths)).ToString("N0");
            r.recovered = (clean(r.recovered)).ToString("N0");
            r.reported_cases = (clean(r.reported_cases)).ToString("N0");
            r.serious_cases = (clean(r.serious_cases)).ToString("N0");

            return r;
        }
    }
}
