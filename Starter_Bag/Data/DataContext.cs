using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Starter_Bag.Entities; // main library for ef core

namespace Starter_Bag.Data; // folder ya group name to keep code organised

// our class "DataContest" tells ef core how to talk to the database
// ye ek bridge ki trh hai C# code aur database tables ke bich

public sealed class DataContext : DbContext
{
    // Constructor: this passes "options" to the base DbContest class
    // agr DbContext humara chakki hai to options ab hum usme kya kya pis skte hai vo hua
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    // this method lets us customise how our database tables should look and entities map to table
    // example: we can say "First name is required" or "email must be unique"
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // always call base.OnModelCreating first so ef core can do this default setup
        base.OnModelCreating(modelBuilder);
        
        // this line says: go look inside this project assembly and apply all the entity configurations you can find
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).GetTypeInfo().Assembly);
    }
    
    // this line creates a "Users" table in the database from our "Users" class.
    // Dbset<User> means: we want to keep a collection of users in the databse
    // if this line is missing ef will not create users table
    
    public DbSet<User> Users { get; set; } 
    
}