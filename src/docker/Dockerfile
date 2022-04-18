# Use an official Python runtime as a base image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1

# Set the working directory to /app
WORKDIR /CoffeeMachine.WebApi

# Copy the current directory contents into the container at /app
ADD CoffeeMachine.WebApi .

# Make port 80 available to the world outside this container
EXPOSE 8081

# Define environment variable
ENV PORT 8081

# Run app.py when the container launches
#CMD ["./build.sh"]
CMD ["dotnet", "CoffeeMachine.WebApi.dll"]


