# http3-server

This is a skeleton implementation for HTTP/3 server, which also supports HTTP/1, HTTP/2 clients. It was tested in Ubtunyu 22.04 LTS environment with WSL 2. This is currently works only development only environment besides ignroing non tursted certificates in TLS layer

# Build and run

## Install MSQUIC
```bash
apt install libmsquic
```

## Install .NET 8.0
Open the terminal with following commands

```bash
# Get Ubuntu version
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '"'; fi)

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
sudo dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb

# Update packages
sudo apt update

# install the dotnet 8
apt install dotnet-runtime-8.0
```

### Check the install .NET version
```bash
 dotnet --version
```
A result should look like
```bash
8.0.100
```

The guide was created based on [Microsoft installation guide](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu).

## Running the application
For able to run the application, than needs a certificate for TLS. It can be created dev certificate with this command 
```bash
dotnet dev-certs https --trust
```

On the client the validation callback should ignore non trusted certifcate with this example code with httpclient.
```C#
var handler = new HttpClientHandler();
handler.ClientCertificateOptions = ClientCertificateOption.Manual;
handler.ServerCertificateCustomValidationCallback = 
    (httpRequestMessage, cert, cetChain, policyErrors) =>
{
    return true;
};

var client = new HttpClient(handler);
```
This is not suggested to use in it production environment.

To run the application in repository folder
```bash
dotnet run
```
