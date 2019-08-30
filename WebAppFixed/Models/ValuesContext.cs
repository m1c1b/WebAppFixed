using System.Data.Entity;

namespace WebAppFixed.Models
{
    public class ValuesContext:DbContext
    {
        public ValuesContext():base("AllValues")
        {}
        public DbSet<Value> Values { get; set; }
    }
}