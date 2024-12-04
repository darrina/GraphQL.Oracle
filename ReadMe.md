# GraphQL using Oracle Cloud Infrastructure (OCI)

## Notes

```shell
# Add local IP to OCI > Autonomous Database > Network > Edit access control list

# Update appsettings.json > ConnectionString > OracleConnection > Data Source

# OCI > Autonomous Database > Database connection > Download Wallet
export TNS_ADMIN=./Wallet

#   Use connection string in OCI > Autonomous Database > Datbase connection > Connection strings
export ORACLE_PASSWORD=super-secret-password

dotnet tool install --global dotnet-ef
dotnet-ef database update

dotnet run

dotnet-ef database drop
```