# Identity Server
To get this started...

https://readthedocs.org/projects/identitymodel/downloads/pdf/latest/

# Identity Resource:

Represent claims about a user like user ID, display name, email address etc. that can be requested using the scope parameter.
These could be retrieved from an identity provider whether internal (e.g. IdentityServer) or external (e.g. ADFS, Google)

# API Resources:
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


# Types of tokens:

## Access token: This is a client based token. It contains the client information 

```
//HEADER:ALGORITHM & TOKEN TYPE
{
  "alg": "RS256",
  "kid": "1B866B3C1ABC5A5DCA6B6593A15D377C",
  "typ": "at+jwt"
}
//PAYLOAD:DATA
{
  "nbf": 1599145056,
  "exp": 1599148656,
  "iss": "https://localhost:4300",
  "aud": [
    "inventory",
    "invoices",
    "https://localhost:4300/resources"
  ],
  "client_id": "mvcClient.pkce",
  "sub": "8dc9bd64-70ca-45f4-a704-7bf333a32988",
  "auth_time": 1599143508,
  "idp": "adfs",
  "name": "Dina.Heidar@LA.GOV",
  "email": "Dina.Heidar@LA.GOV",
  "jti": "A41051704010CCA73D8F0DFC589566FE",
  "sid": "12D96389094CDABF0700E042FF3BC1E0",
  "iat": 1599145056,
  "scope": [
    "openid",
    "profile",
    "inventory.read",
    "invoices.write",
    "offline_access"
  ],
  "amr": [
    "external"
  ]
}
//VERIFY SIGNATURE
{
}
```

## Reference Token (this is instead of using Access Tokens)

```

```

## Id Token: This contains user information

```
//HEADER:ALGORITHM & TOKEN TYPE
{
  "alg": "RS256",
  "kid": "1B866B3C1ABC5A5DCA6B6593A15D377C",
  "typ": "JWT"
}
//PAYLOAD:DATA
{
  "nbf": 1599145056,
  "exp": 1599145356,
  "iss": "https://localhost:4300",
  "aud": "mvcClient.pkce",
  "nonce": "637347418547986890.NzU0MjA0NmItNGVkMi00MGQ1LWEwMDQtMjYyNzAxMDkxZGIzZDM4ZjVmZGMtM2MyYS00ZmMzLWEzMmUtN2ZlNzFkMDAzZWMw",
  "iat": 1599145056,
  "at_hash": "KlR0SeIoTB8PtsN06ZQ7sQ",
  "s_hash": "9B3Xxyx6Akekf76u8PTWFg",
  "sid": "12D96389094CDABF0700E042FF3BC1E0",
  "sub": "8dc9bd64-70ca-45f4-a704-7bf333a32988",
  "auth_time": 1599143508,
  "idp": "adfs",
  "name": "Dina.Heidar@LA.GOV",
  "preferred_username": "5f783dd7-1014-401e-8ee7-3d5d4c4e4301",
  "amr": [
    "external"
  ]
}
//VERIFY SIGNATURE
{
}
```
# Authentication Methods and Secrets:
https://medium.com/@darutk/oauth-2-0-client-authentication-4b5f929305d4 
Identity Server Token API supports these methods:
* Basic Authentication (client_secret_basic) 
* Client Secret JWT Authentication (client_secret_jwt)
* Private Key JWT Client Authentication (private_key_jwt): can be used with X509Certficates using the client_assertion as a secret. In the context of FAPI Read-and-Write, other algorithms than ES256 (ECDSA using P-256 and SHA-256) and PS256 (RSASSA-PSS using SHA-256 and MGF1 with SHA-256) are not permitted. By default, IdentityServer4 uses RS256 to sign identity tokens and JWT access tokens; however, it does also support Elliptical Curve Cryptography (ECC). Using Elliptical Curve Digital Signing Algorithms (ECDSA) such as ES256 does have some benefits over RSA, such as shorter signature and smaller keys while providing the same level of security.
* Mutual TLS Authentication (tls_client_auth) :In a normal TLS connection, only the server presents its certificate. On the other hand, in a mutual TLS connection, the client presents its certificate, too. As a result, the server receives a client certificate.
* Self signed (self_signed_tls_client_auth) same as above.

# Secrets
Clients (if not using PKCE) and API's can be configured with secrets. There are a few types of secrets:

* client_secret: which is basically a string value. this would be comparable to a password. Please do not use!
* client_assertion: because passwords suck. Instead of comparing the client_secret against a value stored in the database, the authorization server must now instead validate a signed JWT. 

# Client Assertion Signature Algorithm
Financial-grade API (FAPI) requires higher security than traditional OAuth 2.0 and OpenID Connect. The specification puts restrictions on client authentication methods. Client authentication methods permitted by FAPI Part 2 are as follows. Note that client_secret_jwt is excluded.
* private_key_jwt
* tls_client_auth
* self_signed_tls_client_auth

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
 
