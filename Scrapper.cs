using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper; //dotnet add package CsvHelper
using HtmlAgilityPack; //dotnet add package ScrapySharp

namespace NextStep
{
    public class Scrapper
    {
        // static void Main(string[] args)
        // {
        //     //ScrappingMethod();
        // }

        public static List<Experience> ScrappingMethod(DateTime dateDebut, DateTime dateFin, string ville, string pays) //ajouter tous les paramètres genre datedebut datefin et ville
        {
            //gérer l'appel de chaque lien en fonction des paramètres pays et dates
            //if (pays == "France") A REMETTRE SI JE MET L'ANGLETERRE
            //{
            var periodLinks = new List<String>();
            //ne prend pas en compte si période de plus de 12 mois, ni date dans plus de 12 mois
            if (dateDebut.Month <= dateFin.Month) //si dans la même année
            {
                periodLinks = GetPeriodLinks("https://www.france-voyage.com/evenements/", dateDebut.Month, dateFin.Month);
            }
            else //si déborde sur l'année suivante
            {
                periodLinks = GetPeriodLinks("https://www.france-voyage.com/evenements/", dateDebut.Month, 12);
                periodLinks.AddRange(GetPeriodLinks("https://www.france-voyage.com/evenements/", 1, dateFin.Month));
            }
            Console.WriteLine("Found {0} links", periodLinks.Count);
            var allEventLinks = new List<String>();
            foreach (string periodLink in periodLinks)
            {
                var eventLinks = GetEventLinks(periodLink);
                allEventLinks.AddRange(eventLinks);
            }
            var experiences = GetEventDetails(allEventLinks);
            return experiences;
            //}
            //else{
            //    return 
            //}
            ///// ce qu'il y a dessous était un test, à voir si on garde, seulement si on fait recherche par catégorie
            //var eventLinks = GetEventLinks("https://www.france-voyage.com/evenements/cat-fetestraditionnelles.htm");
            //Console.WriteLine("Found {0} links", eventLinks.Count);
            //var experiences = GetEventDetails(eventLinks);
            //return experiences;
        }

        // Parses the URL and returns HtmlDocument object
        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        //Gets all the links from specified period, between two months
        static List<string> GetPeriodLinks(string url, int dateDebutMonth, int dateFinMonth)
        {
            var periodLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            for (int i = dateDebutMonth; i < dateFinMonth; i++)
            {
                string xpath = "//section/div[4]/a[" + i + "]";
                HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes(xpath);
                var baseUri = new Uri(url);
                foreach (var link in linkNodes)
                {
                    string href = link.Attributes["href"].Value;
                    periodLinks.Add(new Uri(baseUri, href).AbsoluteUri);
                }
            }
            return periodLinks;
        }
        // Gets all the links from individual events linked to a specified category
        static List<string> GetEventLinks(string url)
        {
            var eventLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            //HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//section/div/div/div/a");
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//section/div/div/div/a");
            var baseUri = new Uri(url);
            foreach (var link in linkNodes)
            {
                string href = link.Attributes["href"].Value;
                eventLinks.Add(new Uri(baseUri, href).AbsoluteUri);
            }
            return eventLinks;
        }

        // Gets all the details of the events for the Experience class (in Models)
        static List<Experience> GetEventDetails(List<string> urls)
        {
            var experiences = new List<Experience>();
            foreach (var url in urls)
            {
                HtmlDocument document = GetDocument(url);
                var LibelleXPath = "//h1";
                var VilleXPath = "//tr[3]/td/a";
                var experience = new Experience();
                experience.Libelle = document.DocumentNode.SelectSingleNode(LibelleXPath).InnerText;
                if (document.DocumentNode.SelectSingleNode(VilleXPath) != null)
                {
                    experience.Ville = document.DocumentNode.SelectSingleNode(VilleXPath).InnerText;
                    Console.WriteLine(experience.Ville);
                }
                else
                {
                    experience.Ville = "Partout en France";
                    Console.WriteLine("Partout en France");
                }
                experiences.Add(experience);
            }
            return experiences;
        }

        // SUREMENT PAS UTILE
        // static void exportToCSV(List<Experience> experiences)
        // {
        //     using (var writer = new StreamWriter("./experiences.csv"))
        //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //     {
        //         csv.WriteRecords(experiences);
        //     }
        // }
    }
}
