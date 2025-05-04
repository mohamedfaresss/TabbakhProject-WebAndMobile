using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models.Domain
{

    public partial class Nutrition
    {
        public int Recipe_Id { get; set; }

        public double Calories_100g { get; set; }

        public double Fat_100g { get; set; }

        public double Sugar_100g { get; set; }

        public double Protein_100g { get; set; }

        public double Carb_100 { get; set; }

        public string Type { get; set; } = null!;

        public virtual Recipe? Recipe { get; set; }
    }
}
