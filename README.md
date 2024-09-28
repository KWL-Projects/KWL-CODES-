# KWL-CODES-
CMPG 323 GROUP PROJECT

**Project Overview**

This project is designed to assist the Human Movement Sciences Faculty at North-West University by creating a system that allows lecturers to provide feedback to students based on exercise videos submitted via a mobile application. The project involves the development of a mobile application using .NET MAUI for students to upload exercise videos and a web application using ASP.NET Core and Angular for lecturers to view, provide feedback, and assign marks. A robust backend infrastructure supports the system with features such as secure login, video compression, storage, feedback management, and database integration.

Architectural Overview
The system is designed with a multi-tier architecture that separates the frontend, backend, and database layers to ensure scalability, maintainability, and security. The system follows a microservices approach where different components can interact with each other through RESTful APIs. This modular architecture allows us to expand the system without altering core functionalities significantly.

Key Components:

●	Mobile Application: Built using .NET MAUI, designed for students to record and upload videos.

●	Web Application: Built using Angular for the frontend and ASP.NET Core for the backend, aimed at lecturers to review videos and provide feedback.

●	Backend System: A REST API that manages authentication, video storage, and feedback.

●	Database: Managed by SQL Server, stores user details, video metadata, feedback, and assignment-related information.

●	File Storage: Azure Blob Storage (Blob Storage Container)

●	Cloud Provider: Azure

●	Host: Azure  

**Technology Stack**

Mobile Application:

●	Framework: .NET MAUI (Multi-platform App UI)

●	Target Platforms: Android, iOS, Windows

●	Programming Language: C#

Web Application:

●	Frontend Framework: Angular

●	Backend Framework: ASP.NET Core Web API

●	Programming Languages: TypeScript (Frontend), C# (Backend), HTML (Frontend), JavaScript (Backend)

Backend System:

●	API Style: RESTful APIs using ASP.NET Core

●	Database: SQL Server for relational data

●	File Storage: Azure Blob Storage Containers

●	Authentication: OAuth2 for secure login and access management

DevOps Tools:

●	Source Control: GitHub for version control

●	CI/CD: GitHub Actions for automated testing and deployment

●	Containerization: Docker for containerized environments

**Authors:**

Waldo (WM) Nieman, 37943278

Kayla Venter, 37440136

Luhan Pieterse, 37773364

