# Console Jwt app
This is to demonstrate how Gateway can send a JWT to identity server to recieve a client access_token that can be used to access an api.

To get this started...

# 1- Install the certificate in the Certificates folder
For some reason the console app has issues opening a realtive path *.pfx during runtime.
Please remember to either : 
>-change out the 'filename' to your correct absolute path where the certificate is located. 
>>OR 
>>
>-install in the certficate store as 'Local machine' under 'Personal' and refer to that cert.

# 3- To run the project
1- Select the solution, right click and select 'Set Startup Projects'.

2- Select 'multiple startup projects'.

3- Select and set in this order 
>a- IdentityServer.Api

>b- ConsoleJwt.Api

>c- Api2 

