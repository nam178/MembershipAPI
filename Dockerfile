FROM microsoft/dotnet
ADD . /src
WORKDIR /src
RUN mkdir /build
RUN dotnet test Membership.Core.Testing
RUN dotnet publish --configuration=Release --output=/build
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "/build/Membership.Api.dll"]
