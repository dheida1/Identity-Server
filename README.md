# Identity Server
To get this started...

# 1- Add-Migrations (via Package Manager)

### Important- Please make sure:
* That the startup project is set to "IdentityServer.Api" 
* That the Package Manager default project is set to "IdentityServer.Infrastructure".
* Run these commands: 

`
PM> Add-Migration Initial -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb

PM> Add-Migration Initial -c ApplicationDbContext -o Data/Migrations/ApplicationDb

PM> Add-Migration Initial -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb

`

# 2 - Set up databases (via Package Manager)
 Use these commands (below) to set up database. 

### Important- Please make sure:
* That the startup project is set to "IdentityServer.Api" 
* That the Package Manager default project is set to "IdentityServer.Infrastructure".
* That you have a DefaultConnection setup in your appsettings for a database

`

PM> update-database -Context PersistedGrantDbContext

PM> update-database -Context ApplicationDbContext

PM> update-database -Context ConfigurationDbContext

`

# 4- To run the project
1- Select the solution, right click and select 'Set Startup Projects'.

2- Select 'multiple startup projects'.

3- Select and set in this order 
>a- IdentityServer.Api

>b- MvcClient.Web

>c- Api1

>d- Api2 
 

# 3- The Architecture

Protecting an API using Client Credentials  