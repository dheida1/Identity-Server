# Identity Server
To get this started...

https://readthedocs.org/projects/identitymodel/downloads/pdf/latest/

# Identity Resource:

Represent claims about a user like user ID, display name, email address etc. that can be requested using the scope parameter.
These could be retrieved from an identity provider whether internal (e.g. IdentityServer) or external (e.g. ADFS, Google)

# Api Resources:
Represent functionality a client wants to access. Typically, they are HTTP-based endpoints (aka APIs), but could be also message queuing endpoints or similar.

# Scopes: 

Scopes are an abstract concept that you can use how you see fit to subdivide (or not) your API resources. For example, you might have:

One scope that covers multiple physically different APIs ("OurEnterpriseScope")
One scope per physical API ("OurDataAccessApiScope", "OurFinanceApiScope")
Multiple scopes per physical API, at whatever granularity you see fit. Maybe two: one for read and one for write. Maybe one for normal users and one for admin users. Maybe a scope for every single endpoint (but don't ...)
In IdentityServer you specify which scopes your client(s) have access to, using AllowedScopes on the Client object. So you might have a client that can only use the 'MyApiUser' scope and a different client that can use both the 'MyApiUser' scope and the 'MyApiAdmin' scope.

Scopes are not a function of the user, only the client the user is using.

It is entirely up to you how you want to break up your resources.

In your API you need to define the AllowedScopes within your UseIdentityServerAuthentication, so that your API knows which scopes it will accept within an access token. If you have an API that accepts multiple scopes and you want to control access to your endpoints based on what scope the access token has (because your API supports multiple clients, each with different scopes) then you can define Policies based on Claims (the scope is just wrapped up as a claim).

# Clients:
Clients represent applications that can request tokens from your identityserver.

The details vary, but you typically define the following common settings for a client:

* a unique client ID
* a secret if needed
* the allowed interactions with the token service (called a grant type)
* a network location where identity and/or access token gets sent to (called a redirect URI)
* a list of scopes (aka resources) the client is allowed to access

## To get started....

## 1- Add-Migrations (via Package Manager)

### Important- Please make sure:
* That the Package Manager default project is set to "IdentityServer.Infrastructure".
* That "IdentityServer.Api" is set as the startup project.
* Run these commands: 

```        
PM> Add-Migration Initial -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb

PM> Add-Migration Initial -c ApplicationDbContext -o Data/Migrations/ApplicationDb

PM> Add-Migration Initial -c ConfigurationDbContext -o Data/Migrations/ConfigurationDbContext
```

## 2 - Set up databases (via Package Manager)
 Use these commands (below) to set up database. 

### Important- Please make sure:
* That the startup project is set to "IdentityServer.Api" 
* That the Package Manager default project is set to "IdentityServer.Infrastructure".
* That you have a DefaultConnection setup in your appsettings for a database

```        
PM> update-database -Context PersistedGrantDbContext

PM> update-database -Context ApplicationDbContext

PM> update-database -Context ConfigurationDbContext
```

## 4- To run the project
1- Select the solution, right click and select 'Set Startup Projects'.

2- Select 'multiple startup projects'.

3- Select and set in this order 
>a- Authorization Server: IdentityServer.Api

>b- Client: MvcPkceClient.Web

>c- Protected Resource: Api1 (e.g Inventory)

>d- Protected Resource: Api2 (e.g Invoices)
 
