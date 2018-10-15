# VR Club Universe's Website

Get the code to run VR Universe with `git clone https://github.com/UCSDVR/VRUniverse.git` 

Get to the `website` branch

Get to the Website code with `cd Website` from your VRUniverse home

Make sure you have `npm` and `node` working on your system. Installing `node` should install `npm`.

You will need the data folder from Google Drive. Ask someone from the VRUniverse dev team for access.

Place the data folder in `./data/VRClubUniverseData` The folder on drive is called VRClubUniverse_Data, be sure to rename it (I'm sorry :( )

VRUniverse uses Github OAuth for login. Ask Panda for a .env file

Get website dependencies with `npm install` or `npm i` if you're cool 

Run website with `npm start`


or 

## Docker (unsupported for now)

If you have Docker, you can run 

docker build -t <username>/universe-website .

docker run --rm -it -p 8080:8080 <username>/universe-website

Go to `http://localhost:3000` in your favourite browser



