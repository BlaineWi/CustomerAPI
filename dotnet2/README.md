# ASP.NET Core WebAPI w. Swashbuckle/Swagger #

This little demo app is to show how to:

* Implement an ASP.NET Core Web API project
* Add logging
* Add health checks
* Customize it with a global error handler
* Add and customize swagger
* Build a docker container
* Run it locally
* Deploy it to Azure Kubernetes Service (AKS)

## Libraries ##

* <a href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore" target="_blank">https://github.com/domaindrivendev/Swashbuckle.AspNetCore</a>
* <a href="https://www.nuget.org/packages/Faker.Data/" target="_blank">https://www.nuget.org/packages/Faker.Data/</a> (Converted to DotNet Core 2.0.3) [Binaries are in the ```lib/``` folder]

## Visual Studio  ##

Hitting Debug-Start will start the app on a randomly supplied port navigate to 
```bash
http://localhost:[port]/swagger
```

To see the swagger UI page.

## Dockerize ##

To dockerize the app, we supply a docker file, and a build

### Selecting a Port ###

In this example, port ```5000``` is configured. To change that, you will need to search all the source code for that port and replace it, here is a list of places to change:

Visual Studio:
* .\Properties\launchSettings.json
* .\Program.cs

Docker Files:
* .\docker-compose.override.yml
* .\Dockerfile

Kubernetes Files:
* .\deployment.yaml
* .\service.yaml

### Docker ###

Build Bash Script:

```bash
#!/bin/bash
set -x
CT={your registry name}/customerapi
dotnet restore ./customerAPI.sln && dotnet publish ./customerAPI.sln -c Release -o ./obj/Docker/publish
docker build -t $CT .
docker push $CT
```

Run Locally on Docker for Windows:

```bash
#!/bin/bash
CT={your registry name}/customerapi
winpty docker run -d -p 5000:5000 $CT --name customerapi
start http://localhost:5000/swagger
```

This will run it locally, and then browse to the swagger UI page

## Deploy to Azure Kubernetes Service (AKS) ##

```bash
kubectl create -f deployment.yaml
kubectl create -f service.yaml
```

Test:

Go to your Azure subscription, find your agents external ip address, and browse to it, on your port (default: 5000) and go to the swagger page:

```bash
http://{external ip address}:{port}/swagger
```

Clean up:

To delete your assets from your AKS:

```bash
kubectl delete service customerapi
kubectl delete deployment customerapi-deployment
```

Pro-Tip:

Shutdown your AKS Agent VMs when not in use to save $$$

## PCF with Linux Stemcell ##

Use the following script to make a linux image:

```dos
linux-publish.cmd
```

Then (assuming you are logged into your PCF instance, with the correct ORG and SPACE) using the `Manifest.yml` file:

```dos
pcf-push-it.cmd
```

Like most container systems, the app will run on its own internal port (usually 8080), but the load balancer in this case the GO Router, will map this to port 80, so you can browse to the app `/swagger` on the route PCF creates for you and use the API.

## About ##

Stuart Williams
Cloud/DevOps Practice Lead
 
Magenic Technologies Inc.
Office of the CTO
 
<a href="mailto:stuartw@magenic.com" target="_blank">stuartw@magenic.com</a> (e-mail)
 
Blog: <a href="http://blitzkriegsoftware.net/Blog" target="_blank">http://blitzkriegsoftware.net/Blog</a> 
LinkedIn: <a href="http://lnkd.in/P35kVT" target="_blank">http://lnkd.in/P35kVT</a> 
YouTube: <a href="https://www.youtube.com/channel/UCO88zFRJMTrAZZbYzhvAlMg" target="_blank">https://www.youtube.com/channel/UCO88zFRJMTrAZZbYzhvAlMg</a> 
