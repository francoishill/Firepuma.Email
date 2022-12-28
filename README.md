# Firepuma.Email

A repository containing code for a microservice to send emails.

## Introduction

This solution was generated with [francoishill/Firepuma.Email](https://github.com/francoishill/Firepuma.Email).

The following projects were generated as part of the solution:

* Firepuma.Email.Domain project contains the domain logic (not tightly coupled to Mongo or other infrastructure specifics)
* Firepuma.Email.Infrastructure contains infrastructure code, like mongo repositories inheriting from `MongoDbRepository<T>`
* Firepuma.Email.Tests contains unit tests
* Firepuma.Email.Worker project contains the service that will get deployed to Google Cloud Run

---

## Deploying

When using github, the deployment will happen automatically due to the folder containing workflow yaml files in the `.github/workflows` folder.

To test locally whether the Dockerfile can build, run the following command:

```shell
docker build --tag tmp-test-email-service --file Firepuma.Email.Worker/Dockerfile .
```