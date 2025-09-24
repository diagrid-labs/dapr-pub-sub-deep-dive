# Install Dapr
wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
dapr uninstall
dapr init

# Install xdg-utils to open browser
sudo apt update
sudo apt install xdg-utils -y

# Install Diagrid CLI
curl -o- https://downloads.diagrid.io/cli/install.sh | bash
sudo mv ./diagrid /usr/local/bin 