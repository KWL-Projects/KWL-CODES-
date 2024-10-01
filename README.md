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




**2. Setup and Installation**
   
**2.1 Prerequisites**

Before proceeding with the installation, ensure that the following software is installed:

•	Visual Studio 2022 (for both mobile and web application development)

•	.NET SDK 6.0 or higher

•	Node.js (required for Angular development)

•	Git (for version control)

•	Docker (optional, for containerization)

**2.2 Mobile Application Setup**

1.	Clone the Repository: Clone the project repository from GitHub to your local machine.
   
2.	Open in Visual Studio: Open the .NET MAUI solution in Visual Studio 2022.
   
3.	Restore Dependencies: Use the NuGet package manager to restore the required packages.
   
4.	Configure the Backend URL: In the app settings file, make sure to update the API base URL so that the mobile app can communicate with the backend.
   
5.	Run the App: Choose the appropriate platform (Android, iOS, Windows) and run the mobile application.

**2.3 Web Application Setup**

1.	Clone the Repository: Pull the web app code from the GitHub repository.
   
2.	Install Node.js Packages: Install all required dependencies and packages.
	
3.	Build the Web App: Use the Angular CLI to build and run the Angular frontend.
	
4.	Run the Backend: Open the ASP.NET Core project in Visual Studio and start the backend services, ensuring the APIs are functional and accessible by the frontend.
   
5.	Access the Web Application: Once the app is running, open a browser and navigate to the appropriate URL to access the lecturer interface.



**3. Configuration**
   
**3.1 Database Configuration**

The system utilizes a relational database to store user data, video metadata, and feedback. Ensure that the connection strings for the database are appropriately set up in the configuration files of both the mobile and web applications. If using Entity Framework, apply the necessary database migrations to keep the database schema up to date by executing the appropriate commands prior to the initial use.

**3.2 API Configuration**

The API endpoints provided by the backend will manage authentication, video uploads, feedback processing, and data retrieval. Confirm that the API base URL is correctly configured in both the mobile and web application configuration files to facilitate seamless communication.




**4. Interactions with backend systems**
   
**4.1 Mobile Application Usage**

•	Login: Students will login using their credentials and if they don’t have an account sign up for one. This will securely authenticate the user via the backend API.

•	View Assignments: After logging in, students can view their list of assignments.

•	Record and Upload Videos: The app allows students to record exercise videos and upload them directly to the backend for evaluation.

•	Browse own submissions: Students can browse their own submissions.

•	View Feedback: Once feedback is provided by the lecturer, students can view the feedback, and any marks assigned to them.

•	Download marks: Students will be able to download the marks provided by the lecturers.

**4.2 Web Application Usage**

•	Login: Lecturers log in via a secure login interface. The system authenticates them using the backend system.

•	Create Assignments: Lecturers can create assignments for the students to complete.

•	View Submissions: Lecturers can view the submissions of the students and browse their submissions.

•	Stream video submissions: Lecturers can stream the videos submitted by the students.

•	Download video submissions: Lecturers can download videos submitted by the students to view them.

•	Provide feedback on video: Lecturers will be able to provide feedback on the videos submitted by the students.

•	User Administration: Allows for High-level users to manage other users.



**5. Advanced Features**
   
**5.1 Video Compression and streaming**

•	The system supports video compression on the mobile app to reduce file sizes before uploading to the server, minimizing upload time and server storage requirements.

•	The backend system is designed to stream videos efficiently on the web application, ensuring lecturers can view videos without long loading times.

**5.2 Scalability**

•	The system is built with scalability in mind, allowing for an increasing number of users (both students and lecturers) and video uploads without sacrificing performance.

•	Tools like Docker can be used to containerize the backend services for easier deployment and scaling in cloud environments.



**6. Links and Additional information**
   
Repository Link: GitHub Link: https://github.com/KWL-Projects/KWL-CODES-

Google Drive: Google Drive: https://drive.google.com/drive/folders/1OHpWchosojG6GotLTqb8Yg6WkLB6co2i



**8. Troubleshooting**
   
To continuously improve the system, several future enhancements are planned:

•	Enhanced User Interface: Ongoing improvements to the UI/UX for both mobile and web applications to enhance user experience and accessibility.

•	Integration of AI: Potential integration of AI-driven features for personalized feedback based on students' video submissions, facilitating targeted learning.

•	Extended Reporting Features: Development of advanced reporting features for lecturers to track student performance over time and identify trends.

•	Push Notifications: Implementation of push notifications within the mobile application to alert students about assignment updates or new feedback.

By following the steps outlined above, you should be able to set up, configure, and use the system for both mobile and web applications. For additional help or troubleshooting, consult the project’s README or issue tracker on GitHub.


**Authors:**

Waldo (WM) Nieman, 37943278

Kayla Venter, 37440136

Luhan Pieterse, 37773364

