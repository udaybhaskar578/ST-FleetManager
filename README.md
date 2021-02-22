# FMS

Fleet Manager is an fleet managment system which allows the following activities

- Register as user
- Login to the application
- Update the user profile
- Create, Read, Delete and Update a Bus(Vehicle Information)
- Create, Read and Update a Garage entry
- Create, Read and Update a Maintenance Request Type
- Create, Read and Update a Maintenance Request on a given bus


**User Roles:**

- Admin
- Employee
- Technican


    **Admin:**

      Administrator has the complete access to the system. He has the ability to perform the following actions 
      - Create a user
      - Update the user profile
      - Lock a user out of the system
      - Create, Read, Delete and Update a Bus(Vehicle Information)
      - Create, Read and Update a Garage entry
      - Create, Read and Update a Maintenance Request Type (Master Data)
      - Create, Read and Update a Maintenance Request on a given bus


    **Employee:**

      Employee has a limited access in making changes to the master data in the system. He has the ability to perform the following actions 
      - Create a user
      - Update the user profile
      - Create, Read, Delete and Update a Bus(Vehicle Information)
      - Create, Read and Update a Garage entry
      - Create, Read and Update a Maintenance Request on a given bus


    **Technician:**

      Technican only has access to the maintenance requests generated in the system. He has the ability to perform the following actions 
      - Create, Read and Update a Maintenance Request on a given bus


**Usecases:**

- Bus can be assigned to a garage
- Bus can be checked out of a garage
    - A bus can only be checked out of the garage if it doesn't have any pending/In progress maintenance requests
- Maintenance request can be raised on a bus
    - Maintenance request status will be "Waiting for Technican" upon creation and the bus's current status will be updated to "Scheduled for maintenance"
- Maintenance Request status can be updated
    - If maintenance request status is updated "In Progress" by a user, the corresponding bus's current status will be updated to "Undergoing repairs"
    - If maintenance request status is updated "Complete" by a user and there are no other pending requests, the corresponding bus's current status will be updated to "Ready for       Use"
- Maintenance Request Type status can be updated
    - Only active maintenance request types will be displayed while creating a maintenance request


**Technical Aspects:**

- Back end: C#, ASP.Net Core 5.0 MVC
- ORM Framework: EF Core 
- JS Libraries: JQuery, Datatables 
- CSS: Bootstrap, Datatables 
- Database: SQL Server
- Code first approach for building the database using EF Core


**Future enhancements/updates:**

- Assigning Technician to a garage
- Show maintenance requests only related to technician
- Allow multiple closest garages assignment to a garage
- Only allow Admin to delete bus records
- Create a Maintenance header entity to envolope all the maintenance requests done at a time 


**Assumptions:**

- A garage is only allowed to have one close garage


**Database creation/migration:**

For running the database migration locally, please follow the below steps

1. Update the connection string in FleetManagementSystem => appsettings.json with the appropriate details of your server
2. Run the project
3. You should now be able to see the database in the SQL Server

Note:
Please do not delelte any content from the FleetManagementSystem.DataAccess.Migration folder
