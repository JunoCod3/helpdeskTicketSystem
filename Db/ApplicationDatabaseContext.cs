using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FullstackDevTS.Jwt;
using FullstackDevTS.Models.Entities;
using FullstackDevTS.Models.Internal;

namespace FullstackDevTS.Db;

public class ApplicationDatabaseContext : IdentityDbContext<JwtIdentity>
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options) {}
    
    //Models Registration....
    public DbSet<TestModel> TestModels { get; set; }
    public DbSet<CategoryModel> CategoryModels { get; set; }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<UsersInformation> UsersInformationModels { get; set; }
    public DbSet<UsersCredential>  UsersCredentialModels { get; set; } 
    
}