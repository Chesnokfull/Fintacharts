#### Description
Demo web app that connects to third party API and Web Socket and then exposes it's own. 
App is deployed to Docker Hub. 
#### How to run 
To run application run “docker compose up” command in the same directory with docker-compose.yml file that’s attached. This command will start 2 containers: application and mssql to store data. DB and tables creation happens on container start. 

After containers start, visit http://localhost:5000/swagger/index.html page to see details about available methods.

![Рисунок1](https://github.com/user-attachments/assets/91579bbb-0de8-4605-832f-5323eb73a681)

![Рисунок2](https://github.com/user-attachments/assets/cdc44d44-be90-4f5a-b9f8-e8cbebd0c17e)

![Рисунок3](https://github.com/user-attachments/assets/290367cb-413e-4c92-86e0-c38652ea0548)

#### Execution notes
All the data is stored in db after being received. Since there are no data migration, corresponding tables are truncated each time new data is requested. 

To set up WebSocket connection /WebSocket/RequestConnection endpoint should be triggered. /WebSocket/Get will return data received by WebSocket. 
