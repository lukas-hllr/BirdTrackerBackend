# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY BirdTrackerProject/*.csproj ./BirdTrackerProject/
RUN dotnet restore

# copy everything else and build app
COPY BirdTrackerDB/. ./BirdTrackerDB/
COPY BirdTrackerProject/. ./BirdTrackerProject/
WORKDIR /source/BirdTrackerProject
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "BirdTrackerProject.dll"]

EXPOSE 80
ENV BT_DATABASE ""
#BUILD:
#docker build -t birdtrackerbackend .

#RUN:
#docker run -it --rm -p 5000:80 --name birdtrackerbackend_sample -e "STRING" birdtrackerbackend
