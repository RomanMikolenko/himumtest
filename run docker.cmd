if not exist Certificates mkdir Certificates
dotnet dev-certs https -ep Certificates\aspnetapp.pfx -p password
dotnet dev-certs https --trust

docker-compose up --build