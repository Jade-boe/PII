using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HtmlAgilityPack;

namespace NextStep
{
    public class Experience
    {
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Categorie { get; set; }
        public string Pays { get; set; }
        public string Ville { get; set; }
        public DataType dateDeDebut { get; set; }
        public DataType dateDeFin { get; set; }
        
        //public int NiveauSportifRequis { get; set; }
        //public int AgeRequis { get; set; }
    }
}