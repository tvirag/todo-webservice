docker stop web
docker rm web
docker run --name web -p 5000:5000 web 
