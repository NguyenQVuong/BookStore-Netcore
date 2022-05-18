using System;


namespace BookShop.Data.Entities
{
    public class Cronjobs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cronjob_Express { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public string Function { get; set; }
        public DateTime LastedRun { get; set; }
    }
}
