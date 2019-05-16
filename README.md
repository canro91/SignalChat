# SignalChat

SignalChat is an small chat that uses SignalR to broadcast message to clients, Restsharp to make request, CsvHelper to parse CSV files, Foundatio to queue and process messages, InsightDatabase to query the database and SimpleInjector to inject dependencies into controller

## Installation

Update in the SignalChat web.config the database connection string. Make sure to create all database objects included in the Database project.