############################################################
# Dockerfile to build Customer API 
# container image, based on aspnetcore image
############################################################
FROM microsoft/aspnetcore:2.0

# Labels
LABEL Author="Stuart Williams <spookdejur@hotmail.com>"
LABEL Version="1.1"

# Variables
ENV PORT 5000
ENV WDIR /app

ARG source

# Create app directory
RUN mkdir -p ${WDIR}
WORKDIR ${WDIR}

# Bundle app source
COPY ${source:-obj/Docker/publish} .

# Verify files are there
RUN find ${WDIR} -type f -follow -print

# Port for Web
EXPOSE ${PORT}

HEALTHCHECK --interval=5m --timeout=3s \
  CMD curl -f http://localhost:${PORT} || exit 1

# Start the app
ENTRYPOINT ["dotnet", "customerAPI.dll"]