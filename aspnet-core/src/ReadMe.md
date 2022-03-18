The project is dependent on redis for caching so in order for that there is a docker-compose.yml  file 
that will spin up a redis server also it will spin up an sql serve and the app also, 
if you have those servers running you can just set them in appsettings.json or you can change the environments in yml file to match
the ones you want.

To build it run docker-compose build
to run it run docker-compose up --remove-orphans

