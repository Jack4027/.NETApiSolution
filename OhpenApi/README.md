# Hi I'm Jack Boston and this is my API Solution for Ohpen. 

# Purpose of the Application 

This is a small scale API Service which enables 3 endpoints.

The goal of the Service is to take in Customer Transactional Data and then process this data through the system performing validation.

The first endpoint is the alows a user to POST a data request to /Job, the data request contains a Job Type and a list of Customer Transactional Data. This Data is then Processed in the Service Class and a Job ID is added to it.

Requests with the data type specified as Bulk will run to completion even if errors are discovered in any of the transactions.

Requests with the data type specified as Batch will stop if any error is detected on the customer transactions, this means any further transactions will not be processed. 

Parameters: StartJobRequest(string JobType, List<CustomerTransactionModel>(string Id, string AccountId, string DestinationAccountId, int Amount))
Returns: StartJobResponse(Guid JobId, string message)

The Second endpoint is a GET method that allows a user to retrieve the response to the POST rqeuest in the first endpoint, this Response contains the JobId and a message if any problems occurred.
Available at /Job/{Job ID} 

Parameters: Guid JobId
Returns: StartJobResponse(Guid JobId, string message)

The Third end point is a GET that allows a user to retrieve the Job logs for a given Job ID this will the Job Log Response will contain the Job ID and a list of Job Items where each Job Item contains
an Item ID, a Success value and a Description detailing any errors on the item.

Parameters: Guid JobId
Returns: JobLogResponse(Guid JobId, List<ItemLog>JobItems(string ItemId, boolean Success, List<string>Description))


# Requirements to Run

# This Appliation was built upon .NET 6.0 in Visual Studio 2022

Additional Packages that have been installed
- Swashbuckle.AspNetCore v6.5.0: In order to enable Swagger API Testing
- ErrorOr v1.3.0, Author: Amichai Mantinband: This provides a user friendly and simple means of error catching
- NUnit v3.13.3: Enables Test Suite
- NUnit.Analyzers v3.3.0
- NUnit3TestAdapter v4.2.1
- Microsoft.NET.Test.Sdk v17.1.0

New Packages can be added via Tools > NuGet Package Manager

# Folder Architecture 

The main web app is run from the OhpenAPi Web Api, this should be set as the start up project
Response and Request objects are stored in the OhpenApi.Contracts Class Library
Data Models are stored in the OhpenApi.Entities Class Library
The Test Suite is stored in the OhpenApi.Tests NUnit Library


# Running the Application

No frontend has been built for the application, it exists solely to view the API requests on the Swagger UI, in order to access this run the Service with OhpenApi as the startup project.
Using this launch profile the startup url will be https://localhost:7261/swagger or http://localhost:5186/swagger if port 7261 is unavailable, you can specify different ports in the launchSettings.json file
under the OhpenApi profile and applicationUrls.

If running in Visual Studio Code use the dotnet run command from the OhpenApi/OhpenApi directory (The Web Api Project) then select either valid port address http://localhost:5186 or https://localhost:7261 and add /swagger onto the end

There you will see the 3 HTTP Request options, the Post Request will ask you to supply a request body In the given format. Launching the post request with an invalid job type returns a bad request.
Aslong as the request contains bulk or batch as the job type the request will run and return a 201 created with the Job Id and a short message detailing if any errors were found. 

The 2 get requests for Job Logs and the Job Response Message will ask you to supply the Job ID in Guid form, this can be obtained as long as the Post request returned a 201 created by copying the JobId returned from the POST and pasting it in as the parameter.

# Running the Test Suite

The Test Suite has been configured using NUnitFramework and Microsoft.Net.Test.Sdk and can be run via Test > Run All, via Test > Test Explorer or by going into a Test Class, right clicking on the class or an individual test that you wish to run then clicking run tests. 

You can run the Test Suite in Visual Studio Code by going into the OhpenApi.Tests directory and running the dotnet test command in the Command Line