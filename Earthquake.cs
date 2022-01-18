using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EarthQuakeLog
{
    public class Earthquake
    {
        public double Magnitude { get; set; }
        public string Place { get; set; }
        public long Time { get; set; }
        public string Alert { get; set; }
        public string Url { get; set; } //kneza gej
        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static List<Earthquake> listEarthquakes = new List<Earthquake>();

        public Earthquake(double magnitude, string place, long time, string alert, string url, string title, double x, double y, double z){
            Magnitude = magnitude;
            Place = place;
            Time = time;
            Alert = alert;
            Url = url;
            Title = title;
            X = x;
            Y = y;
            Z = z;
        }

        public static void ShowQuakes(){
            dynamic Quakes = GetQuakes(); // prizivanje getquakes() za dobivanje podataka o potresima 
            foreach (var item in Quakes.features) // za svaki potres u JSONu
            {
                Earthquake e = new Earthquake(
                    Math.Round((Convert.ToDouble((item.properties.mag).ToString())), 2), //double
                    (item.properties.place).ToString(), //string
                    long.Parse((item.properties.time).ToString()), //long
                    item.properties.alert.ToString(), //string
                    item.properties.url.ToString(), //string
                    item.properties.title.ToString(), //string
                    Convert.ToDouble((item.geometry.coordinates[0]).ToString()), //double
                    Convert.ToDouble((item.geometry.coordinates[1]).ToString()), //double
                    Convert.ToDouble((item.geometry.coordinates[2]).ToString())  //double
                );
                listEarthquakes.Add(e);
                // pravljenje objekta sa navedenim atributima i spremanje u listu za kasniju korist
            }
            foreach (var item in listEarthquakes)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine(Quakes);
        } 

        public static dynamic GetQuakes(){
            var json = "";
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson");
            }   
            dynamic dobj = JToken.Parse(json);
            return dobj;
        } // uzimanje vrijednosti potresa od API (earthquake.usgs.gov)

        public DateTime UnixTimeToDateTime(long unixtime){
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        } // metoda za pretvaranje unix u norm vrijeme

        public override string ToString() 
        {
            return $"{Magnitude} \t| {Place} \t\t\t| {UnixTimeToDateTime(Time)}";
        }
    }
}