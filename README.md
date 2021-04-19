# EHI Contact Mangement System

gRPC implementation of server and client in C#.

This is a little sample implementation of "Contact book" register that shows some principles:
* proto files
* auto generated code from proto files
* server implementation
* implementation of simple singular request/response
* implementation of stream response

## Contents

* `EHContactGrpcService` - gRPC server implementation in C#
* `EHContactClient` - gRPC client implementation as a Blazer App

## Prerequisites
* .net core 5.0
* Visual Studio 2019

## How to open and run

### Preparation in Visual Studio
Simply download the code and open `sln` in Visual Studio.
https://drive.google.com/drive/folders/1lqO7reDkeGGCMysqTpqdCg0tSi1usClI?usp=sharing

Right click on solution and go to `Properties` - navigate to `Common properties - Startup Project`.

Make sure that `Multiple startup projects` is selected, both projects (`EHContactGrpcService` and `EHContactClient`) are set to `Start`.

Finally make sure that `EHContactGrpcService` is the first in order to be run.

### Run

Now you can just click on `Start` (or F5)

## What will happen on a startup

* gRPC server implementation in C#
* gRPC client implementation in C# as a blazor App
* Contact repository is a List that will on runtime auto generate 20 contact entries with random 1,2, or 3 phone numbers on each.

## Data structure

* collection of `ContactModel` objects
* on every `ContactModel` there is collection of `PhoneNumberModel` objects

## .proto files

`.proto` files are located in `Protos` folders in both solutions

