FROM python:3.11-slim as python-build

# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

#/usr/local/lib/libpython3.so
#/usr/local/lib/python3.11/site-packages
# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Inventario.Backend/Inventario.Backend.csproj", "Inventario.Backend/"]
RUN dotnet publish --disable-parallel "Inventario.Backend/Inventario.Backend.csproj"

COPY . .
WORKDIR "/src/Susciptor_GPS_Retransmision"
RUN dotnet build "Susciptor_GPS_Retransmision.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "Susciptor_GPS_Retransmision.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy Python installation and pandas from the python-build image
COPY --from=python-build /usr/local/lib /usr/local/lib
COPY --from=python-build /usr/local/bin /usr/local/bin
COPY --from=python-build /usr/local/include /usr/local/include
COPY --from=python-build /usr/local/share /usr/local/share
RUN cp /app/relate_datasets.py /usr/local/lib/python3.11/site-packages

# Ensure Python environment is set up
ENV LD_LIBRARY_PATH=/usr/local/lib:$LD_LIBRARY_PATH

ENTRYPOINT ["dotnet", "Susciptor_GPS_Retransmision.dll"]

ENV ASPNETCORE_URLS=http://+:5000
