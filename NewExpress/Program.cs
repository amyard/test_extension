using System;
using System.Linq;

namespace NewExpress
{
    class Program
    {
        private static readonly Person[] persons = new Person[]
        {
            new() {Name="max", SiteID = 55},
            new() {Name="dina", SiteID = 52},
            new() {Name="john", SiteID = 53},
            new() {Name="alex", SiteID = 55},
            new() {Name="and", SiteID = 55},
            new() {Name="max", SiteID = 55},
            new() {Name="max", SiteID = 54},
        };
        
        static void Main(string[] args)
        {
            int cnt = 55;
            
            var res = persons.Where(x => x.Name == "max").AsQueryable();
            
            res = res.WhereLike("SiteID", cnt);
            
            foreach (var item in res.ToList())
            {
                Console.WriteLine($"Name - {item.Name}, siteID - {item.SiteID}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}